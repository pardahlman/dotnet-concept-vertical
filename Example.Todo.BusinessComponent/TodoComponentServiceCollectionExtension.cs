using Example.Todo.BusinessComponent.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Todo.BusinessComponent
{
  public static class TodoComponentServiceCollectionExtension
  {
    public static IServiceCollection AddTodoServcies(this IServiceCollection collection)
    {
      collection.AddSingleton<ITodoRepository, TodoRepository>();
      return collection;
    }
  }
}
