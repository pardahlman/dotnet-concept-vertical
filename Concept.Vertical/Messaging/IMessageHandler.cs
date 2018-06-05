using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging
{
  public interface IMessageHandler<in TMessage>
  {
    Task HandleAsync(TMessage message, CancellationToken token);
  }
}
