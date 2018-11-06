using IBM.WatsonDeveloperCloud.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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

    private readonly ConcurrentQueue<(ArraySegment<byte>, WebSocketMessageType)> sendQueue = new ConcurrentQueue<(ArraySegment<byte>, WebSocketMessageType)>();

    private bool closeRequested = false;

    private Thread sendThread;

    private Thread receiveThread;

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
      closeRequested = false;

      watson = new ClientWebSocket();
      watson.Options.SetRequestHeader("Authorization", $"Bearer {tokenManager.GetToken()}");

      UriBuilder builder = new UriBuilder(endpoint)
      {
        Query = queryParameters.ToQueryString()
      };

      await watson.ConnectAsync(builder.Uri, cancellationToken);
      StartAsync(parameters);

      sendThread = new Thread(Send);
      sendThread.Start();

      receiveThread = new Thread(Receive);
      receiveThread.Start();
    }

    protected void StartAsync(Parameters parameters)
    {
      SendAsync(parameters.ToString());
    }

    public void SendAsync(string data)
    {
      if (!closeRequested)
      {
        sendQueue.Enqueue((new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text));
      }
    }

    public void SendAsync(byte[] data)
    {
      if (!closeRequested)
      {
        sendQueue.Enqueue((new ArraySegment<byte>(data), WebSocketMessageType.Binary));
      }
    }

    protected void Send()
    {
      while (!closeRequested)
      {
        while (watson.State == WebSocketState.Open && sendQueue.TryDequeue(out (ArraySegment<byte>, WebSocketMessageType) result))
        {
          watson.SendAsync(result.Item1, result.Item2, true, cancellationToken).Wait();
        }
      }
    }

    public event EventHandler<TranscriptionEventArgs> TranscriptionReceived;

    protected async void Receive()
    {
      byte[] buffer = new byte[1048576];  // 1 MB

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
            closeRequested = true;
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

    protected void StopAsync()
    {
      SendAsync(new Parameters() { Action = StreamAction.Stop }.ToString());
    }

    public async Task CloseAsync()
    {
      StopAsync();
      closeRequested = true;
      // wait to finish up sending the queue
      sendThread.Join(30000); // 30 second timeout
      await watson.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", cancellationToken);
      // wait for the read thread to realize the socket it closed
      receiveThread.Join(30000);  // 30 second timeout
    }

    public void Dispose()
    {
      if (watson != null)
      {
        CloseAsync().Wait();
        ((IDisposable)watson).Dispose();
      }
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
