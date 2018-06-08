using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Concept.Vertical.Tests.Framework
{
  public static class ContentRootBasedOnProjectExtension
  {
    public static IWebHostBuilder UseProjectContentRoot<TTypeInProject>(this IWebHostBuilder builder)
    {
      var projectName = typeof(TTypeInProject).Assembly.GetName().Name;
      return builder.UseSolutionRelativeContentRoot(projectName);
    }
  }
}
