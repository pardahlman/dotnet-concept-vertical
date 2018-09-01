using System;

namespace Example.Todo.Domain.Events
{
  public class TodoDeleted
  {
    public Todo Todo { get; set; }
  }
}
