using System.Collections.Generic;

namespace Concept.Vertical.Messaging
{
  public class ClientMessage
  {
    public object Payload { get; set; }
    public string Type { get; set; }
    public IReadOnlyList<string> ClientIds { get; set; }
  }
}
