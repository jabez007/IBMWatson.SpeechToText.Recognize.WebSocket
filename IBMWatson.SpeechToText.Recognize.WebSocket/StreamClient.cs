using IBM.WatsonDeveloperCloud.SpeechToText.v1.Model;
using IBM.WatsonDeveloperCloud.Util;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  /// <summary>
  /// https://developer.ibm.com/answers/questions/477708/c-speech-to-text-get-token-using-apikey-and-websoc/
  /// https://github.com/watson-developer-cloud/dotnet-standard-sdk/tree/development/src/IBM.WatsonDeveloperCloud.SpeechToText.v1
  /// Somehow, that 'official' SDK from IBM doesn't support their WebSockets interface.
  /// </summary>
  internal class StreamClient : IDisposable
  {
    protected static TokenManager tokenManager;

    protected ClientWebSocket watson;

    protected readonly Uri endpoint;

    protected readonly CancellationToken cancellationToken = new CancellationToken();

    private Thread ReceiveThread;

    public StreamClient(RegionalHost regionalHost, string apiKey)
    {
      if (tokenManager == null)
      {
        tokenManager = new TokenManager(new TokenOptions()
        {
          IamApiKey = apiKey,
        });
      }

      endpoint = new Uri($"wss://{regionalHost}/speech-to-text/api/v1/recognize");
    }

    public async Task ConnectAsync(QueryParameters queryParameters, Parameters parameters)
    {
      watson = new ClientWebSocket();
      watson.Options.SetRequestHeader("Authorization", $"Bearer {tokenManager.GetToken()}");
    
      UriBuilder builder = new UriBuilder(endpoint)
      {
        Query = queryParameters.ToQueryString()
      };

      await watson.ConnectAsync(builder.Uri, cancellationToken);
      await SendAsync(parameters.ToString());
      
      ReceiveThread = new Thread(ReceiveTranscription);
      ReceiveThread.Start();
    }

    public async Task SendAsync(string data)
    {
      if (watson.State == WebSocketState.Open)
      {
        await watson.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, cancellationToken);
      }
    }

    public async Task SendAsync(byte[] data)
    {
      if (watson.State == WebSocketState.Open)
      {
        await watson.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Binary, true, cancellationToken);
      }
    }

    public event EventHandler<TranscriptionEventArgs> TranscriptionReceived;

    protected async void ReceiveTranscription()
    {
      byte[] buffer = new byte[1024];
      
      // Valid states are: 'Open, CloseSent'
      while (watson.State == WebSocketState.Open || watson.State == WebSocketState.CloseSent)
      {
        var segment = new ArraySegment<byte>(buffer);
        var result = await watson.ReceiveAsync(segment, cancellationToken);

        if (result.MessageType == WebSocketMessageType.Close)
        {
          return;
        }

        int count = result.Count;
        while (!result.EndOfMessage)
        {
          if (count >= buffer.Length)
          {
            await watson.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Received transcription too large", cancellationToken);
            return;
          }

          segment = new ArraySegment<byte>(buffer, count, buffer.Length - count);
          result = await watson.ReceiveAsync(segment, cancellationToken);
          count += result.Count;
        }

        var transription = Encoding.UTF8.GetString(buffer, 0, count);

        TranscriptionReceived?.Invoke(this, new TranscriptionEventArgs(transription));
      }
    }

    public async Task CloseAsync()
    {
      await SendAsync(new Parameters() { Action = StreamAction.Stop }.ToString());
      await watson.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", cancellationToken);
      // wait for the read thread to realize the socket it closed
      ReceiveThread.Join(30000);  // 30 second timeout
      watson.Dispose()
    }

    public void Dispose()
    {
      CloseAsync().Wait();
      ((IDisposable)watson).Dispose();
    }
  }

  internal class TranscriptionEventArgs : EventArgs
  {
    public readonly RecognitionResponse Response;

    public TranscriptionEventArgs(string results)
    {
      Response = JsonConvert.DeserializeObject<RecognitionResponse>(results);
    }
  }
}
