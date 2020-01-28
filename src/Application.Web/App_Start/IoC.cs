﻿using Application.Core.Configuration;
using Application.Core.Services;
using Application.Core.Services.CachedProxies;
using Umbraco.Core.Composing;

namespace Application.Web
{
    public class IoCComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            RegisterBuilders(composition);
            RegisterServices(composition);
            RegisterCachedServices(composition);
        }


        private static void RegisterBuilders(Composition composition)
        {
        }

        private static void RegisterServices(Composition composition)
        {
        }

        private static void RegisterCachedServices(Composition composition)
        {
            if (ConfigurationHelper.IsServiceCacheEnabled())
            {
                composition.Register(typeof(ICache), typeof(Cache));

                composition.Register(typeof(CmsService), typeof(CmsService));
                composition.Register(typeof(ICmsService), typeof(CmsServiceCachedProxy));
            }
            else
            {
                composition.Register(typeof(ICmsService), typeof(CmsService));
            }
        }
    }
}