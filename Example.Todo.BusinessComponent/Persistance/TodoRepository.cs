using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Todo.BusinessComponent.Persistance
{
  public interface ITodoRepository
  {
    Task RemoveAsync(Guid todoId, CancellationToken ct);
    Task<Domain.Todo> GetAsync(Guid todoId, CancellationToken ct);
    Task AddOrUpdateAsync(Domain.Todo todo);
    Task<Domain.Todo> AddAsync(string title);
  }

  public class TodoRepository : ITodoRepository
  {
    private readonly ConcurrentDictionary<Guid, Domain.Todo> _todos;

    public TodoRepository()
    {
      _todos = new ConcurrentDictionary<Guid, Domain.Todo>();
    }

    public Task RemoveAsync(Guid todoId, CancellationToken ct)
    {
      _todos.TryRemove(todoId, out _);
      return Task.CompletedTask;
    }

    public Task<Domain.Todo> GetAsync(Guid todoId, CancellationToken ct)
    {
      _todos.TryGetValue(todoId, out var todo);
      return Task.FromResult(todo);
    }

    public Task<Domain.Todo> AddAsync(string title)
    {
      var todo = new Domain.Todo
      {
        Id = Guid.NewGuid(),
        IsCompleted = false,
        Title = title
      };

      _todos.TryAdd(todo.Id, todo);
      return Task.FromResult(todo);
    }

    public Task AddOrUpdateAsync(Domain.Todo todo)
    {
      _todos.AddOrUpdate(todo.Id, id => todo, (id, existing) => todo);
      return Task.CompletedTask;
    }
  }
}
