using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging
{
  public interface IMessageSubscriber
  {
    Task SubscribeAsync<TMessage>(IMessageHandler<TMessage> handler, CancellationToken token);
  }
}
