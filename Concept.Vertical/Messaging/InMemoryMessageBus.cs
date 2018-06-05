using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging
{
  public class InMemoryMessageBus : IMessageSubscriber, IMessagePublisher
  {
    private ConcurrentDictionary<Type, List<Func<object, CancellationToken, Task>>> _messageHandlers = new ConcurrentDictionary<Type, List<Func<object, CancellationToken, Task>>>();

    private static InMemoryMessageBus _instance;

    public static InMemoryMessageBus Instance
    {
      get
      {
        _instance = _instance ?? new InMemoryMessageBus();
        return _instance;
      }
    }

    private InMemoryMessageBus()
    {
    }

    public Task SubscribeAsync<TMessage>(IMessageHandler<TMessage> messageHandler, CancellationToken token)
    {
      _messageHandlers.AddOrUpdate(
        typeof(TMessage),
        type => new List<Func<object, CancellationToken, Task>> {UntypedHandler},
        (type, list) =>
        {
          list.Add(UntypedHandler);
          return list;
        });

      return Task.CompletedTask;

      Task UntypedHandler(object msg, CancellationToken ct) => messageHandler.HandleAsync((TMessage) msg, ct);
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken token)
    {
      if(!_messageHandlers.TryGetValue(typeof(TMessage), out var handlers))
      {
        return Task.CompletedTask;
      }

      return Task.WhenAll(handlers.Select(h => h.Invoke(message, token)));
    }
  }
}
