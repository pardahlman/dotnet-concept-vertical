using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging.Abstractions
{
  public interface IMessageHandler<in TMessage>
  {
    Task HandleAsync(TMessage message, CancellationToken token);
  }
}
