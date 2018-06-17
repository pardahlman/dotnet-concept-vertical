using System;

namespace Example.Todo.Domain.Events
{
  public class TodoMarkedAsActive
  {
    public Guid TodoId { get; set; }
  }
}