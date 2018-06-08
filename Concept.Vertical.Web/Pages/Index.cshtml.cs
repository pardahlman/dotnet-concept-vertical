using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Concept.Vertical.Web
{
  public class IndexModel : PageModel
  {
    public IReadOnlyCollection<string> ComponentDomIds { get; private set; }
    public IReadOnlyCollection<string> ComponentScriptSrc { get; private set; }

    public void OnGet()
    {
      ComponentDomIds = new[] { "root" };
      ComponentScriptSrc = new[] {"http://localhost:5000/static/js/bundle.js"};
    }
  }
}