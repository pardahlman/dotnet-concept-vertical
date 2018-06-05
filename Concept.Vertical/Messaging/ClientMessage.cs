using System.Collections.Generic;

namespace Concept.Vertical.Messaging
{
  public class ClientMessage
  {
    public string RoutingKey { get; set; }
    public object Payload { get; set; }
    public IReadOnlyList<string> ClientIds { get; set; }
  }
}
