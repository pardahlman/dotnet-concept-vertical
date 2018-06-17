using System;

namespace Example.Todo.Domain.Commands
{
  public class ChangeTodoTitle
  {
    public Guid TodoId { get; set; }
    public Guid Title { get; set; }
  }
}
