using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace Application.Core.Extensions
{
    public static class PublishedElementExtensions
    {
        public static IPublishedElement GetElement(this IEnumerable<IPublishedElement> items, string alias)
        {
            var element = items.FirstOrDefault(x => x.ContentType.Alias == alias);

            return element;
        }
    }
}