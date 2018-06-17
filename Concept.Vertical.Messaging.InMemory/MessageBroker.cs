using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging.InMemory
{
  internal class MessageBroker
  {
    private static readonly ConcurrentDictionary<string, List<Func<byte[], IBasicProperties, CancellationToken, Task>>> _messageHandlers =
      new ConcurrentDictionary<string, List<Func<byte[], IBasicProperties, CancellationToken, Task>>>();

    private static List<Func<byte[], IBasicProperties, CancellationToken, Task>> _takeAllHandlers =
      new List<Func<byte[], IBasicProperties, CancellationToken, Task>>();

    public static void BasicPublish(string routingKey, IBasicProperties properties, byte[] body)
    {
      if (!_messageHandlers.TryGetValue(routingKey, out var specificHandlers))
      {
        return;
      }

      var relevantHandlers = specificHandlers.Concat(_takeAllHandlers);

      var exeuctionTask = Task.WhenAll(relevantHandlers.Select(h => h.Invoke(body, properties, CancellationToken.None)));
    }

    public static void BasicConsume(string routingKey, Func<byte[], IBasicProperties, CancellationToken, Task> consumeFunc)
    {
      if (string.Equals(routingKey, "*"))
      {
        _takeAllHandlers.Add(consumeFunc);
        return;
      }

      _messageHandlers.AddOrUpdate(
        routingKey,
        type => new List<Func<byte[], IBasicProperties, CancellationToken, Task>> { consumeFunc },
        (type, list) =>
        {
          list.Add(consumeFunc);
          return list;
        });
    }
  }
}
