using System;

namespace Crif.Api
{
    public class Link
    {
        public Link(string href)
        {
            if (string.IsNullOrEmpty(href)) throw new ArgumentNullException(nameof(href));
            Href = href;
        }
        
        public string Href { get; }
    }
}