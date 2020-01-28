using Application.Core.Services;
using Application.Core.Services.CachedProxies;
using Umbraco.Core.Cache;
using Umbraco.Core.Composing;
using Umbraco.Web.Cache;

namespace Application.Web
{
    public class CachePurgingComposer : ComponentComposer<CachePurging>
    {
    }


    public class CachePurging : IComponent
    {
        public void Initialize()
        {
        }

        public void Terminate()
        {
        }

        public void Compose(Composition composition)
        {
            ContentCacheRefresher.CacheUpdated += ContentCacheRefresher_CacheUpdated;
        }

        private void ContentCacheRefresher_CacheUpdated(ContentCacheRefresher sender, CacheRefresherEventArgs e)
        {
            Cache.Instance.RemoveByPrefix(typeof(CmsServiceCachedProxy).ToString());
        }
    }
}