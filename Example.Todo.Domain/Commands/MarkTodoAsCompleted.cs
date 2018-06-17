using System;

namespace Example.Todo.Domain.Commands
{
  public class MarkTodoAsCompleted
  {
    public Guid TodoId { get; set; }
  }
}
