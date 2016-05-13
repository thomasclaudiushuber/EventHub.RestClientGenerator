using Microsoft.ServiceBus;
using System;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.EventHubLogic
{
  public class RestUriAndSharedAccessSignatureGenerator
  {
    private ConnectionDetails _connectionDetails;
    private string _publisherName;
    private TimeSpan _tokenLifeTime;

    public RestUriAndSharedAccessSignatureGenerator(ConnectionDetails connectionDetails, string publisherName, TimeSpan tokenLifeTime)
    {
      _connectionDetails = connectionDetails;
      _publisherName = publisherName;
      _tokenLifeTime = tokenLifeTime;
    }

    public string GetEventHubRestUri()
    {
      if (string.IsNullOrWhiteSpace(_publisherName))
      {
        return ServiceBusEnvironment.CreateServiceUri("https", _connectionDetails.EventHubNamespace, $"{_connectionDetails.EventHubName}/messages")
            .ToString()
            .Trim('/');
      }
      else
      {
        return ServiceBusEnvironment.CreateServiceUri("https", _connectionDetails.EventHubNamespace, $"{_connectionDetails.EventHubName}/publishers/{_publisherName}/messages")
             .ToString()
             .Trim('/');
      }
    }

    public string GenerateSharedAccessSignature()
    {
      var serviceUri = GetEventHubRestUri();
      return SharedAccessSignatureTokenProvider.GetSharedAccessSignature(_connectionDetails.SharedAccessKeyName, _connectionDetails.SharedAccessKey, serviceUri, _tokenLifeTime);
    }
  }
}
