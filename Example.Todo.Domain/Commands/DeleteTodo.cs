using System;

namespace Example.Todo.Domain.Commands
{
  public class DeleteTodo
  {
    public Guid TodoId { get; set; }
  }
}
