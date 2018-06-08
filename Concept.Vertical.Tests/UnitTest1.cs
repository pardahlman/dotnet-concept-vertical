using System;
using System.Threading.Tasks;
using Concept.Vertical.ReadComponent.Domain;
using Concept.Vertical.SpaComponent;
using Concept.Vertical.Tests.Framework;
using Xunit;

namespace Concept.Vertical.Tests
{
  public class UnitTest1 : IClassFixture<WebhostFixture<SpaStartup>>
  {
    private readonly WebhostFixture<SpaStartup> _webhostComponent;

    public UnitTest1(WebhostFixture<SpaStartup> webhostComponent)
    {
      _webhostComponent = webhostComponent;
    }

    [Fact]
    public async Task Should_Post_Stop_Command_When_Clicking_Stop()
    {
      var stopRecieved = new TaskCompletionSource<StopCommand>();
      await _webhostComponent.Intercept<StopCommand>(stop => stopRecieved.TrySetResult(stop));

      // TODO: Setup Selenium

      await stopRecieved.Task;

      Assert.NotNull(stopRecieved.Task.Result);
    }
  }
}
