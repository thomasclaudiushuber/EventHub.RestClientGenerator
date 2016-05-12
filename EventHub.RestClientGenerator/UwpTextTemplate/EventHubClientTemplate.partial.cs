using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThomasClaudiusHuber.Azure.EventHub.RestClientGenerator.UwpTextTemplate
{
public partial  class EventHubClientTemplate
  {
    public EventHubClientTemplate(string sharedAccessSignature,string eventHubRestUri)
    {
      SharedAccessSignature = sharedAccessSignature;
      EventHubRestUri = eventHubRestUri;
    }

    public string SharedAccessSignature { get; }
    public string EventHubRestUri { get; }
  }
}
