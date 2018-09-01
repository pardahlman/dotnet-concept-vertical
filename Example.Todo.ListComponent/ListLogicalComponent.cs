using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Concept.Vertical.Abstractions;
using Concept.Vertical.Messaging;
using Concept.Vertical.Messaging.Abstractions;
using Example.Todo.Domain.Commands;
using Example.Todo.Domain.Events;
using Example.Todo.Domain.Queries;

namespace Example.Todo.ListComponent
{
  public class ListLogicalComponent : ILogicalComponent
  {
    private readonly IMessageSubscriber _subscriber;
    private readonly IMessagePublisher _publisher;
    private readonly ListViewModel _todoList;
    private readonly List<string> _clientIds;

    public ListLogicalComponent(IMessageSubscriber subscriber, IMessagePublisher publisher)
    {
      _subscriber = subscriber;
      _publisher = publisher;
      _todoList = new ListViewModel{ NumberOfPages = 1, PageIndex = 0 };
      _clientIds = new List<string>();
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
      await _subscriber.SubscribeAsync<TodoListQuery>(OnListQuery, ct);
      await _subscriber.SubscribeAsync<TodoCreated>(OnTodoCreated, ct);
      await _subscriber.SubscribeAsync<TodoDeleted>(OnTodoDeleted, ct);
    }

    public Task StopAsync(CancellationToken ct = default) => Task.CompletedTask;

    private Task OnListQuery(TodoListQuery query, CancellationToken ct)
    {
      _clientIds.Add(query.ClientId);
      return _publisher.PublishAsync(new ClientMessage
      {
        ClientIds = new []{ query.ClientId },
        Payload = _todoList,
        Type = nameof(ListViewModel)
      }, ct);
    }

    private Task OnTodoCreated(TodoCreated created, CancellationToken ct)
    {
      _todoList.ItemsLeft++;
      _todoList.Todos.Add(new ListViewModel.TodoViewModel
      {
        Id = created.Todo.Id,
        IsCompleted = created.Todo.IsCompleted,
        Title = created.Todo.Title,
      });

      return _publisher.PublishAsync(new ClientMessage
      {
        ClientIds = _clientIds,
        Payload = _todoList,
        Type = nameof(ListViewModel)
      }, ct);
    }

    private async Task OnTodoDeleted(TodoDeleted todo, CancellationToken ct)
    {
      var found = _todoList.Todos.FirstOrDefault(t => t.Id == todo.Todo.Id);
      if (found == default)
      {
        return;
      }

      if (!todo.Todo.IsCompleted)
      {
        _todoList.ItemsLeft--;
      }

      _todoList.Todos.Remove(found);

      await _publisher.PublishAsync(new ClientMessage
      {
        ClientIds = _clientIds,
        Payload = _todoList,
        Type = nameof(ListViewModel)
      }, ct);
    }
  }
}
