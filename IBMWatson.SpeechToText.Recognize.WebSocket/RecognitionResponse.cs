using IBM.WatsonDeveloperCloud.SpeechToText.v1.Model;
using Newtonsoft.Json;

namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  internal class RecognitionResponse : SpeechRecognitionResults
  {
    [JsonProperty(PropertyName = "state")]
    public string State { get; set; }

    [JsonProperty(PropertyName = "error")]
    public string Error { get; set; }
  }
}
