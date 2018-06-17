using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using Newtonsoft.Json;

namespace Concept.Vertical.Messaging.InMemory
{
  public class MessageSubscriber : IMessageSubscriber
  {
    private readonly JsonSerializer _jsonSerializer;

    public MessageSubscriber(JsonSerializer jsonSerializer)
    {
      _jsonSerializer = jsonSerializer;
    }

    public Task SubscribeAsync<TMessage>(IMessageHandler<TMessage> handler, CancellationToken token)
    {
      token.ThrowIfCancellationRequested();
      var routingKey = typeof(TMessage).Name;
      MessageBroker.BasicConsume(routingKey, (body, basicProps, ct) =>
      {
        var message = Deserialize<TMessage>(body);
        return handler.HandleAsync(message, ct);
      });

      token.ThrowIfCancellationRequested();
      return Task.CompletedTask;
    }

    private TMessage Deserialize<TMessage>(byte[] body)
    {
      var json = Encoding.UTF8.GetString(body);
      using (var stringReader = new StringReader(json))
      using (var jsonTextReader = new JsonTextReader(stringReader))
      {
        return _jsonSerializer.Deserialize<TMessage>(jsonTextReader);
      }
    }
  }
}
