using System;
using System.Collections.Generic;
using System.Linq;

namespace Example.Todo
{
  class WebComponentConfiguration
  {
    public bool Active { get; set; }
    public Uri BaseUrl { get; set; }
    public List<string> ScriptResources { get; set; }
    public string DomElement { get; set; }
  }

  static class WebComponentConfigurationExtension
  {
    public static Uri[] GetScriptUris(this WebComponentConfiguration config)
      => config.ScriptResources.Select(r => new Uri(config.BaseUrl, r)).ToArray();
  }
}
