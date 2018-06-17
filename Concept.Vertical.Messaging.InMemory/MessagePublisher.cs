using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using Newtonsoft.Json;

namespace Concept.Vertical.Messaging.InMemory
{
  public class MessagePublisher : IMessagePublisher
  {
    private readonly JsonSerializer _jsonSerializer;
    private const string _jsonContent = "application/json";

    public MessagePublisher(JsonSerializer jsonSerializer)
    {
      _jsonSerializer = jsonSerializer;
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken token)
    {
      var routingKey = typeof(TMessage).Name;
      var body = SerializeMessage(message);
      var props = new BasicProperties
      {
        Type = routingKey,
        ContentType = _jsonContent,
        Headers = new Dictionary<string, string>(),
        MessageId = Guid.NewGuid().ToString()
      };
      MessageBroker.BasicPublish(routingKey, props, body);
      return Task.CompletedTask;
    }

    private byte[] SerializeMessage(object message)
    {
      using (var writer = new StringWriter())
      using (var jsonWriter = new JsonTextWriter(writer))
      {
        _jsonSerializer.Serialize(jsonWriter, message);
        var json = writer.ToString();
        return Encoding.UTF8.GetBytes(json);
      }
    }
  }
}
