using System;

namespace Example.Todo.Domain.Commands
{
  public class MarkTodoAsActive
  {
    public Guid TodoId { get; set; }
  }
}
