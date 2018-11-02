using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  internal class Parameters
  {
    /// <summary>
    /// The action that is to be performed.
    /// </summary>
    [JsonIgnore]
    public StreamAction Action;

    [JsonProperty(PropertyName = "action")]
    private string action => Action?.Value;

    /// <summary>
    /// The audio format (MIME type) of the audio.
    /// Where indicated, the format that you specify must include the sampling rate and can optionally include the number of channels and the endianness of the audio.
    /// </summary>
    [JsonIgnore]
    public StreamContentType ContentType;

    [JsonProperty(PropertyName = "content-type")]
    private string contentType => ContentType?.Value;

    /// <summary>
    /// If you specify a customization ID when you open the connection, you can use the customization weight to tell the service how much weight to give to words from the custom language model compared to those from the base model for the current request.
    ///
    /// Specify a value between 0.0 and 1.0.
    /// Unless a different customization weight was specified for the custom model when it was trained, the default value is 0.3. A customization weight that you specify overrides a weight that was specified when the custom model was trained.
    ///
    /// The default value yields the best performance in general.
    /// Assign a higher value if your audio makes frequent use of OOV words from the custom model.
    /// Use caution when setting the weight: a higher value can improve the accuracy of phrases from the custom model's domain, but it can negatively affect performance on non-domain phrases.
    ///
    /// See https://console.bluemix.net/docs/services/speech-to-text/input.html#custom
    /// </summary>
    [JsonProperty(PropertyName = "customization_weight", NullValueHandling = NullValueHandling.Ignore)]
    public double? CustomizationWeight = null;

    /// <summary>
    /// The time in seconds after which, if only silence (no speech) is detected in submitted audio, the connection is closed.
    /// The default is 30 seconds.
    /// The parameter is useful for stopping audio submission from a live microphone when a user simply walks away.
    /// Use -1 for infinity.
    /// See https://console.bluemix.net/docs/services/speech-to-text/input.html#timeouts
    /// </summary>
    [JsonProperty(PropertyName = "inactivity_timeout")]
    public int InactivityTimeout = 30;

    /// <summary>
    /// If true, the service returns interim results as a stream of JSON SpeechRecognitionResults objects.
    /// If false, the service returns a single SpeechRecognitionResults object with final results only.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#interim
    /// </summary>
    [JsonProperty(PropertyName = "interim_results")]
    public bool InterimResults = false;

    /// <summary>
    /// An array of keyword strings to spot in the audio.
    /// Each keyword string can include one or more string tokens.
    /// Keywords are spotted only in the final results, not in interim hypotheses.
    /// If you specify any keywords, you must also specify a keywords threshold.
    /// You can spot a maximum of 1000 keywords.
    /// Omit the parameter or specify an empty array if you do not need to spot keywords.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#keyword_spotting
    /// </summary>
    [JsonProperty(PropertyName = "keywords", NullValueHandling = NullValueHandling.Ignore)]
    public string[] Keywords;

    /// <summary>
    /// A confidence value that is the lower bound for spotting a keyword.
    /// A word is considered to match a keyword if its confidence is greater than or equal to the threshold.
    /// Specify a probability between 0.0 and 1.0.
    /// No keyword spotting is performed if you omit the parameter.
    /// If you specify a threshold, you must also specify one or more keywords.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#keyword_spotting
    /// </summary>
    [JsonProperty(PropertyName = "keywords_threshold", NullValueHandling = NullValueHandling.Ignore)]
    public float? KeywordsThreshold = null;

    /// <summary>
    /// The maximum number of alternative transcripts that the service is to return.
    /// By default, a single transcription is returned.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#max_alternatives
    /// </summary>
    [JsonProperty(PropertyName = "max_alternatives")]
    public uint MaxAlternatives = 1;

    /// <summary>
    /// A confidence value that is the lower bound for identifying a hypothesis as a possible word alternative (also known as "Confusion Networks").
    /// An alternative word is considered if its confidence is greater than or equal to the threshold.
    /// Specify a probability between 0.0 and 1.0.
    /// No alternative words are computed if you omit the parameter.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#word_alternatives
    /// </summary>
    [JsonProperty(PropertyName = "word_alternatives_threshold", NullValueHandling = NullValueHandling.Ignore)]
    public float? WordAlternativesThreshold = null;

    /// <summary>
    /// If true, the service returns a confidence measure in the range of 0.0 to 1.0 for each word.
    /// By default, no word confidence measures are returned.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#word_confidence
    /// </summary>
    [JsonProperty(PropertyName = "word_confidence")]
    public bool WordConfidence = false;

    /// <summary>
    /// If true, the service returns time alignment for each word.
    /// By default, no timestamps are returned.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#word_timestamps
    /// </summary>
    [JsonProperty(PropertyName = "timestamps")]
    public bool Timestamps = false;

    /// <summary>
    /// If true, the service filters profanity from all output except for keyword results by replacing inappropriate words with a series of asterisks.
    /// Set the parameter to false to return results with no censoring.
    /// Applies to US English transcription only.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#profanity_filter
    /// </summary>
    [JsonProperty(PropertyName = "profanity_filter")]
    public bool ProfanityFilter = true;

    /// <summary>
    /// If true, the service converts dates, times, series of digits and numbers, phone numbers, currency values, and internet addresses into more readable, conventional representations in the final transcript of a recognition request.
    /// For US English, the service also converts certain keyword strings to punctuation symbols.
    /// By default, no smart formatting is performed.
    /// Applies to US English and Spanish transcription only.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#smart_formatting
    /// </summary>
    [JsonProperty(PropertyName = "smart_formatting")]
    public bool SmartFormatting = false;

    /// <summary>
    /// If true, the response includes labels that identify which words were spoken by which participants in a multi-person exchange.
    /// By default, no speaker labels are returned.
    /// Specifying true forces the timestamps parameter to be true, regardless of whether you specify false for that parameter.
    /// To determine whether a language model supports speaker labels, use the Get models method and check that the attribute speaker_labels is set to true.
    /// See https://console.bluemix.net/docs/services/speech-to-text/output.html#speaker_labels
    /// </summary>
    [JsonProperty(PropertyName = "speaker_labels")]
    public bool SpeakerLabels = false;

    public override string ToString()
    {
      return JsonConvert.SerializeObject(this);
    }
  }
}
