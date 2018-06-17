namespace Example.Todo.Domain.Events
{
  public class TodoCreated
  {
      public TodoCreated(Todo newTodo)
    {
      Todo = newTodo;
    }
    public Todo Todo { get; set; }
  }
}
