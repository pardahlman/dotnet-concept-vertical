using System;

namespace Example.Todo.Domain.Events
{
  public class TodoDeleted
  {
    public Guid TodoId { get; set; }
  }
}
