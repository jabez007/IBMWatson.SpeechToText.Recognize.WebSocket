namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  internal class RegionalHost
  {
    /// <summary>
    /// for US South and UK
    /// </summary>
    public static readonly RegionalHost UsSouthUk = new RegionalHost("stream.watsonplatform.net");

    /// <summary>
    ///  for Germany
    /// </summary>
    public static readonly RegionalHost Germany = new RegionalHost("stream-fra.watsonplatform.net");

    /// <summary>
    /// for Sydney and AP North
    /// </summary>
    public static readonly RegionalHost SydneyApNorth = new RegionalHost("gateway-syd.watsonplatform.net");

    /// <summary>
    /// for US East
    /// </summary>
    public static readonly RegionalHost UsEast = new RegionalHost("gateway-wdc.watsonplatform.net");

    public readonly string Value;

    private RegionalHost(string value)
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}
