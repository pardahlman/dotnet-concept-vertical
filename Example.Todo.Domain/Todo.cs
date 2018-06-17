using System;

namespace Example.Todo.Domain
{
  public class Todo
  {
    public bool IsCompleted { get; set; }
    public string Title { get; set; }
    public Guid Id { get; set; }
  }
}
