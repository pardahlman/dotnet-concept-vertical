using System.Threading;
using System.Threading.Tasks;

namespace Concept.Vertical.Abstractions
{
  public interface ILogicalComponent
  {
    Task StartAsync(CancellationToken ct = default);
    Task StopAsync(CancellationToken ct = default);
  }
}
