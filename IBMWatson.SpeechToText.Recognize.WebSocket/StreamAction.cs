namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  internal class StreamAction
  {
    /// <summary>
    /// initiates a recognition request.
    /// The message can also include any other optional parameters described in this table.
    /// After sending this text message, the client sends the data as a binary message (blob).
    ///
    /// Between recognition requests, the client can send new start messages to modify the parameters that are to be used for subsequent requests.
    /// By default, the service continues to use the parameters that were specified with the previous start message.
    /// </summary>
    public static readonly StreamAction Start = new StreamAction("start");

    /// <summary>
    /// indicates that all audio data for the request has been sent to the service.
    /// The client can send additional requests with the same or different parameters.
    /// </summary>
    public static readonly StreamAction Stop = new StreamAction("stop");

    public readonly string Value;

    private StreamAction(string value)
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}
