using System;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Abstractions;
using Concept.Vertical.Messaging;
using Concept.Vertical.Messaging.Abstractions;
using Example.Todo.BusinessComponent.Persistance;
using Example.Todo.Domain.Commands;
using Example.Todo.Domain.Events;

namespace Example.Todo.BusinessComponent
{
  public class TodoLogicalComponent : ILogicalComponent
  {
    private readonly IMessagePublisher _publisher;
    private readonly IMessageSubscriber _subscriber;
    private readonly ITodoRepository _todoRepo;

    public TodoLogicalComponent(IMessagePublisher publisher, IMessageSubscriber subscriber, ITodoRepository todoRepo)
    {
      _publisher = publisher;
      _subscriber = subscriber;
      _todoRepo = todoRepo;
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
      await _subscriber.SubscribeAsync<CreateTodo>(OnCreateTodo, ct);
    }

    public Task StopAsync(CancellationToken ct = default)
    {
      throw new NotImplementedException();
    }

    private async Task OnCreateTodo(CreateTodo todo, CancellationToken ct)
    {
      var newTodo = await _todoRepo.AddAsync(todo.Title);
      await _publisher.PublishAsync(new TodoCreated(newTodo), ct);
    }
  }
}
