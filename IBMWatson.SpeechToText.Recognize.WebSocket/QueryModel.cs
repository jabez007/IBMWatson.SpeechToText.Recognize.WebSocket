namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  /// <summary>
  /// https://console.bluemix.net/docs/services/speech-to-text/input.html#models
  /// </summary>
  internal class QueryModel
  {
    public static readonly QueryModel arAR_BroadbandModel = new QueryModel("ar-AR_BroadbandModel");

    public static readonly QueryModel deDE_BroadbandModel = new QueryModel("de-DE_BroadbandModel");

    public static readonly QueryModel enGB_BroadbandModel = new QueryModel("en-GB_BroadbandModel");

    public static readonly QueryModel enGB_NarrowbandModel = new QueryModel("en-GB_NarrowbandModel");

    public static readonly QueryModel enUS_BroadbandModel = new QueryModel("en-US_BroadbandModel");

    public static readonly QueryModel enUS_NarrowbandModel = new QueryModel("en-US_NarrowbandModel");

    public static readonly QueryModel esES_BroadbandModel = new QueryModel("es-ES_BroadbandModel");

    public static readonly QueryModel esES_NarrowbandModel = new QueryModel("es-ES_NarrowbandModel");

    public static readonly QueryModel frFR_BroadbandModel = new QueryModel("fr-FR_BroadbandModel");

    public static readonly QueryModel jaJP_BroadbandModel = new QueryModel("ja-JP_BroadbandModel");

    public static readonly QueryModel jaJP_NarrowbandModel = new QueryModel("ja-JP_NarrowbandModel");

    public static readonly QueryModel koKR_BroadbandModel = new QueryModel("ko-KR_BroadbandModel");

    public static readonly QueryModel koKR_NarrowbandModel = new QueryModel("ko-KR_NarrowbandModel");

    public static readonly QueryModel ptBR_BroadbandModel = new QueryModel("pt-BR_BroadbandModel");

    public static readonly QueryModel ptBR_NarrowbandModel = new QueryModel("pt-BR_NarrowbandModel");

    public static readonly QueryModel zhCN_BroadbandModel = new QueryModel("zh-CN_BroadbandModel");

    public static readonly QueryModel zhCN_NarrowbandModel = new QueryModel("zh-CN_NarrowbandModel");

    public readonly string Value;

    private QueryModel(string value)
    {
      Value = value;
    }

    public override string ToString()
    {
      return Value;
    }
  }
}
