using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Crif.Api
{
    public class HalResource
    {
        [JsonProperty("_links", Order = 100)]
        public Dictionary<string, Link> Links { get; } = new Dictionary<string, Link>();

        public void AddLink(string relType, string href)
        {
            if (string.IsNullOrEmpty(relType)) throw new ArgumentNullException(nameof(relType));
            Links.Add(relType, new Link(href));
        }
    }
}