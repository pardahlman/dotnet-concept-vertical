using System;
using System.Threading.Tasks;
using Concept.Vertical.Messaging.Abstractions;
using Concept.Vertical.Tests.Framework;
using Example.Todo.Domain.Commands;
using Example.Todo.Domain.Events;
using Example.Todo.ListComponent;
using Example.Todo.ListView;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Example.Todo.Tests
{
  public class SeleniumTests : IClassFixture<WebhostFixture<ListViewStartup>>
  {
    private readonly WebhostFixture<ListViewStartup> _webhostComponent;

    public SeleniumTests(WebhostFixture<ListViewStartup> webhosetFixture)
    {
      _webhostComponent = webhosetFixture;
      
    }

    [Fact]
    public async Task Foo()
    {
      /* Setup */
      var readModleComponent = new ListLogicalComponent(
        _webhostComponent.GetService<IMessageSubscriber>(),
        _webhostComponent.GetService<IMessagePublisher>()
      );

      await Task.Delay(TimeSpan.FromSeconds(10));
      await readModleComponent.StartAsync();
      var todo = new Domain.Todo
      {
        Id = Guid.NewGuid(),
        IsCompleted = false,
        Title = "Run test"
      };

      await _webhostComponent.PublishAsync(new TodoCreated(todo));
      await Task.Delay(TimeSpan.FromHours(1));
    }
  }
}
