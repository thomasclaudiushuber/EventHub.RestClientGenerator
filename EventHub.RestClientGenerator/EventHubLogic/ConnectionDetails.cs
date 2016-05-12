namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.EventHubLogic
{
  public class ConnectionDetails
  {
    public string EventHubNamespace { get; set; }
    public string EventHubName { get; set; }
    public string SharedAccessKeyName { get; set; }
    public string SharedAccessKey { get; set; }
  }
}
