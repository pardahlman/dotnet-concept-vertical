using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Messaging.InMemory
{
  public class Connection : IConnection
  {
    private readonly IModel _onlyChannel;

    public Connection()
    {
      _onlyChannel = new Model();
    }

    public IModel CreateModel() => _onlyChannel;
  }

  internal class Model : IModel
  {
    public void BasicPublish(string routingKey, IBasicProperties properties, byte[] body)
    {
      MessageBroker.BasicPublish(routingKey, properties, body);
    }

    public void BasicConsume(string routingKey, Func<byte[], IBasicProperties, CancellationToken, Task> consumeFunc)
    {
      MessageBroker.BasicConsume(routingKey, consumeFunc);
    }
  }

  public interface IConnection
  {
    IModel CreateModel();
  }

  public interface IModel
  {
    void BasicPublish(string routingKey, IBasicProperties properties, byte[] body);
    void BasicConsume(string routingKey, Func<byte[], IBasicProperties, CancellationToken, Task> consumeFunc);
  }

  public interface IBasicProperties
  {
    string Type { get; }
    string MessageId { get; }
    string ContentType { get; }
    IDictionary<string, string> Headers { get; }
  }

  public class BasicProperties : IBasicProperties
  {
    public string Type { set; get; }
    public string MessageId { get; set; }
    public string ContentType { get; set; }
    public IDictionary<string, string> Headers { get; set; }
  }
}