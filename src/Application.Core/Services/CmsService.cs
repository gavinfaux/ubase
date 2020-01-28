using System;
using System.Linq;
using System.Runtime.Caching;
using Application.Models.Models.CmsModels;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace Application.Core.Services
{
    public class Cache : ICache
    {
        public static Cache Instance { get; } = new Cache();

        public bool Add(string key, object value, TimeSpan? cacheDuration = null)
        {
            var memoryCache = MemoryCache.Default;
            var dateTime = cacheDuration.HasValue ? DateTime.UtcNow.Add(cacheDuration.Value) : DateTime.MaxValue;
            var key1 = key;
            var obj = value;
            var absoluteExpiration = dateTime;
            return memoryCache.Add(key1, obj, absoluteExpiration);
        }

        public T Get<T>(string key)
        {
            return (T) MemoryCache.Default.Get(key);
        }

        public T Get<T>(string cacheKey, Func<T> hydrationFunction, TimeSpan? timeSpan = null) where T : class
        {
            T obj1;
            if ((obj1 = Get<T>(cacheKey)) != null)
                return obj1;
            var obj2 = hydrationFunction();
            if (obj2 == null)
                return default;
            Add(cacheKey, obj2, timeSpan);
            return obj2;
        }

        public bool Remove(string key)
        {
            var source = MemoryCache.Default;
            if (source.Any(x => x.Key.Equals(key)))
                return source.Remove(key) != null;
            return false;
        }

        public void RemoveByPrefix(string cachePrefix)
        {
            var source = MemoryCache.Default;
            foreach (var keyValuePair in source.Where(x => x.Key.StartsWith(cachePrefix, StringComparison.Ordinal)))
                source.Remove(keyValuePair.Key);
        }
    }

    public class CacheKey
    {
        public static string Build<TCallingClass, TReturnedType>(string value)
        {
            return $"{typeof(TCallingClass)}_{typeof(TReturnedType)}_{value}";
        }
    }

    public interface ICache
    {
        bool Add(string key, object value, TimeSpan? cacheDuration = null);

        T Get<T>(string key);

        T Get<T>(string cacheKey, Func<T> hydrationFunction, TimeSpan? cacheDuration = null) where T : class;

        bool Remove(string key);

        void RemoveByPrefix(string cachePrefix);
    }

    public interface ICmsService
    {
        SiteRoot GetSiteNode(int nodeId);

        HomePage GetHomeNode(int nodeId);

        Error404 GetError404Node(int nodeId);

        //TagContainer GetTagContainer(int currentNodeId);
    }

    public class CmsService : ICmsService
    {
        private readonly ILogger _logger;
        private readonly UmbracoHelper _umbracoHelper;

        public CmsService(UmbracoHelper umbracoHelper, ILogger logger)
        {
            _umbracoHelper = umbracoHelper;
            _logger = logger;
        }

        public SiteRoot GetSiteNode(int currentNodeId)
        {
            var node = _umbracoHelper.Content(currentNodeId);
            if (node == null)
            {
                _logger.Warn<CmsService>($"1.Node with id {currentNodeId} is null");
                return null;
            }

            var siteNode =
                node.AncestorsOrSelf().SingleOrDefault(x => x.ContentType.Alias == SiteRoot.ModelTypeAlias) as SiteRoot;

            if (siteNode == null) _logger.Warn<CmsService>("siteNode is null");

            return siteNode;
        }

        public HomePage GetHomeNode(int currentNodeId)
        {
            var siteNode = GetSiteNode(currentNodeId);
            return siteNode.Children<HomePage>().FirstOrDefault();
        }

        public Error404 GetError404Node(int currentNodeId)
        {
            var homeNode = GetSiteNode(currentNodeId);
            var notFoundNode = homeNode.Children<Error404>().FirstOrDefault();

            return notFoundNode;
        }
    }
}