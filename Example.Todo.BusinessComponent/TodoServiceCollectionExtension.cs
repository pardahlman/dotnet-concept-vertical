using Example.Todo.BusinessComponent.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Todo.BusinessComponent
{
  public static class TodoServiceCollectionExtension
  {
    public static IServiceCollection AddTodoServcies(this IServiceCollection collection)
    {
      collection.AddSingleton<ITodoRepository, TodoRepository>();
      return collection;
    }
  }
}
