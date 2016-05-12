using System.Linq;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.EventHubLogic
{
  public static class ConnectionDetailsExtractor
  {
    public static bool TryExtract(string connectionString, out ConnectionDetails details)
    {
      details = null;
      var connectionDetails = new ConnectionDetails();
      var items = connectionString.Split(';');

      if (ExtractEventHubNamespace(connectionDetails, items)
        && ExtractEventHubName(connectionDetails, items)
        && ExtractSharedAccessKey(connectionDetails, items)
        && ExtractSharedAccessKeyName(connectionDetails, items)
        )
      {
        details = connectionDetails;
        return true;
      }

      return false;
    }

    private static bool ExtractSharedAccessKey(ConnectionDetails connectionDetails, string[] allItems)
    {
      var items = allItems.Where(i => i.StartsWith("SharedAccessKey="));
      if (items.Count() != 1)
      {
        return false;
      }
      connectionDetails.SharedAccessKey = items.Single().Replace("SharedAccessKey=", "");
      return true;
    }

    private static bool ExtractSharedAccessKeyName(ConnectionDetails connectionDetails, string[] allItems)
    {
      var items = allItems.Where(i => i.StartsWith("SharedAccessKeyName="));
      if (items.Count() != 1)
      {
        return false;
      }
      connectionDetails.SharedAccessKeyName = items.Single().Replace("SharedAccessKeyName=", "");
      return true;
    }

    private static bool ExtractEventHubName(ConnectionDetails connectionDetails, string[] allItems)
    {
      var items = allItems.Where(i => i.StartsWith("EntityPath="));
      if (items.Count() != 1)
      {
        return false;
      }
      connectionDetails.EventHubName = items.Single().Replace("EntityPath=", "");
      return true;
    }

    private static bool ExtractEventHubNamespace(ConnectionDetails connectionDetails, string[] allItems)
    {
      var items = allItems.Where(i => i.StartsWith("Endpoint=sb://"));
      if (items.Count() != 1)
      {
        return false;
      }

      connectionDetails.EventHubNamespace = items.Single().Replace("Endpoint=sb://", "").Replace(".servicebus.windows.net/", "");
      return true;
    }
  }
}
