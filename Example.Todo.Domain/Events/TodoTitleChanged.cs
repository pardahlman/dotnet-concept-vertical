using System;

namespace Example.Todo.Domain.Events
{
  public class TodoTitleChanged
  {
    public Guid TodoId { get; set; }
    public string Title { get; set; }
  }
}
