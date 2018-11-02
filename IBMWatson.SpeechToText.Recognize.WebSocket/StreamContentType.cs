namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  /// <summary>
  /// For formats that are labeled Required, you must use the Content-Type header with the request to specify the format of the audio.
  /// See https://console.bluemix.net/docs/services/speech-to-text/audio-formats.html
  /// </summary>
  internal class StreamContentType
  {
    /// <summary>
    /// to have the service automatically detect the format of the audio.
    /// </summary>
    public static readonly StreamContentType OctetStream = new StreamContentType("application/octet-stream");

    /// <summary>
    /// Required.
    /// Use only with narrowband models.
    /// </summary>
    public static readonly StreamContentType Basic = new StreamContentType("audio/basic");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType Flac = new StreamContentType("audio/flac");

    /// <summary>
    /// Required.
    /// Specify the sampling rate (rate) and optionally the number of channels (channels) and endianness (endianness) of the audio.
    /// </summary>
    /// <param name="samplingRate"></param>
    /// <returns></returns>
    public static StreamContentType L16(uint samplingRate) => new StreamContentType(string.Format("audio/l16;rate={0}", samplingRate));

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType Mp3 = new StreamContentType("audio/mp3");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType Mpeg = new StreamContentType("audio/mpeg");

    /// <summary>
    /// Required.
    /// Specify the sampling rate (rate) of the audio.
    /// </summary>
    /// <param name="samplingRate"></param>
    /// <returns></returns>
    public static StreamContentType MuLaw(uint samplingRate) => new StreamContentType(string.Format("audio/mulaw;rate={0}", samplingRate));

    /// <summary>
    /// The service automatically detects the codec of the input audio.
    /// </summary>
    public static readonly StreamContentType Ogg = new StreamContentType("audio/ogg");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType OggOpus = new StreamContentType("audio/ogg;codecs=opus");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType OggVorbis = new StreamContentType("audio/ogg;codecs=vorbis");

    /// <summary>
    /// Provide audio with a maximum of nine channels.
    /// </summary>
    public static readonly StreamContentType Wav = new StreamContentType("audio/wav");

    /// <summary>
    /// The service automatically detects the codec of the input audio.
    /// </summary>
    public static readonly StreamContentType Webm = new StreamContentType("audio/webm");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType WebmOpus = new StreamContentType("audio/webm;codecs=opus");

    /// <summary>
    ///
    /// </summary>
    public static readonly StreamContentType WebmVorbis = new StreamContentType("audio/webm;codecs=vorbis");

    public readonly string Value;

    private StreamContentType(string value)
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}
