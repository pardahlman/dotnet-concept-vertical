using System.Threading.Tasks;
using Concept.Vertical.SpaComponent;
using Concept.Vertical.Tests.Framework;
using Xunit;

namespace Concept.Vertical.Tests
{
  public class UnitTest1 : IClassFixture<WebhostFixture<Startup>>
  {
    private readonly WebhostFixture<Startup> _webhostComponent;

    public UnitTest1(WebhostFixture<Startup> webhostComponent)
    {
      _webhostComponent = webhostComponent;
    }

    [Fact]
    public async Task Test1()
    {
      
    }
  }
}
