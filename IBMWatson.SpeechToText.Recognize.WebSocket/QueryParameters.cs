using System.Collections.Specialized;
using System.Web;

namespace IBMWatson.SpeechToText.Recognize.WebSocket
{
  internal class QueryParameters
  {
    /// <summary>
    /// Provides an authentication token for the service.
    /// Use a token that is based on your service credentials.
    /// Do not pass an IAM access token with this parameter.
    /// See https://console.bluemix.net/docs/services/watson/getting-started-tokens.html#tokens-for-authentication
    /// </summary>
    public string WatsonToken;

    /// <summary>
    /// The identifier of the model that is to be used for all recognition requests sent over the connection.
    /// </summary>
    public QueryModel Model = QueryModel.enUS_BroadbandModel;

    /// <summary>
    /// The customization ID (GUID) of a custom language model that is to be used for all requests sent over the connection.
    /// The base model of the specified custom language model must match the model specified with the model parameter.
    /// You must make the request with service credentials created for the instance of the service that owns the custom model.
    /// Omit the parameter to use the specified model with no custom language model.
    /// See https://console.bluemix.net/docs/services/speech-to-text/input.html#custom
    /// </summary>
    public string LanguageCustomizationId;

    /// <summary>
    /// The customization ID (GUID) of a custom acoustic model that is to be used for all requests sent over the connection.
    /// The base model of the specified custom acoustic model must match the model specified with the model parameter.
    /// You must make the request with service credentials created for the instance of the service that owns the custom model.
    /// Omit the parameter to use the specified model with no custom acoustic model.
    /// See https://console.bluemix.net/docs/services/speech-to-text/input.html#custom
    /// </summary>
    public string AcousticCustomizationId;

    /// <summary>
    /// The version of the specified base model that is to be used for all requests sent over the connection.
    /// Multiple versions of a base model can exist when a model is updated for internal improvements.
    /// The parameter is intended primarily for use with custom models that have been upgraded for a new base model.
    /// The default value depends on whether the parameter is used with or without a custom model.
    /// See https://console.bluemix.net/docs/services/speech-to-text/input.html#version
    /// </summary>
    public string BaseModelVersion;

    /// <summary>
    /// Indicates whether IBM can use data that is sent over the connection to improve the service for future users.
    /// Specify true to prevent IBM from accessing the logged data.
    /// Specify false to allow IBM to access the logged data.
    /// </summary>
    public bool XWatsonLearningOptOut = false;

    /// <summary>
    /// Associates a customer ID with all data that is passed over the connection.
    /// The parameter accepts the argument customer_id={id}, where {id} is a random or generic string that is to be associated with the data.
    /// You must URL-encode the argument to the parameter, for example, customer_id%3dmy_ID.
    /// By default, no customer ID is associated with the data.
    /// </summary>
    public string XWatsonMetadata;

    public override string ToString()
    {
      return ToQueryString();
    }

    public string ToQueryString()
    {
      NameValueCollection queryParams = HttpUtility.ParseQueryString(string.Empty);

      if (!string.IsNullOrEmpty(WatsonToken))
      {
        queryParams.Add("watson-token", WatsonToken);
      }

      queryParams.Add("model", Model.Value);

      if (!string.IsNullOrEmpty(LanguageCustomizationId))
      {
        queryParams.Add("language_customization_id", LanguageCustomizationId);
      }

      if (!string.IsNullOrEmpty(AcousticCustomizationId))
      {
        queryParams.Add("acoustic_customization_id", AcousticCustomizationId);
      }

      if (!string.IsNullOrEmpty(BaseModelVersion))
      {
        queryParams.Add("base_model_version", BaseModelVersion);
      }

      if (XWatsonLearningOptOut)
      {
        queryParams.Add("x-watson-learning-opt-out", "true");
      }

      if (!string.IsNullOrEmpty(XWatsonMetadata))
      {
        queryParams.Add("x-watson-metadata", XWatsonMetadata);
      }

      return queryParams.ToString();
    }
  }
}
