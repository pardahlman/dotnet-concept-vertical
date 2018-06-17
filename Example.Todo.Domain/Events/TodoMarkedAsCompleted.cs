using System;

namespace Example.Todo.Domain.Events
{
  public class TodoMarkedAsCompleted
  {
    public Guid TodoId { get; set; }
  }
}
