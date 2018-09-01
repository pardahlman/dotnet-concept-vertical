namespace Example.Todo.Domain.Queries
{
  public class TodoListQuery
  {
    public string ClientId { get; set; }
    public uint Start { get; set; }
    public uint ItemCount { get; set; }
  }
}
