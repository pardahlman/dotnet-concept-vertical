using System;
using System.Collections.Generic;

namespace Example.Todo.ListComponent
{
  public class ListViewModel
  {
    public List<TodoViewModel> Todos { get; set; }
    public ushort ItemsLeft { get; set; }
    public uint PageIndex { get; set; }
    public uint NumberOfPages { get; set; }

    public ListViewModel()
    {
      Todos = new List<TodoViewModel>();
    }
    public class TodoViewModel
    {
      public bool IsCompleted { get; set; }
      public string Title { get; set; }
      public Guid Id { get; set; }
    }
  }
}
