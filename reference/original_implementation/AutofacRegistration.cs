       private static void RegisterCacheInfrastructure(ContainerBuilder builder)
        {
            // register all the cache workers
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IParameterBroker)))
                .Where(t => typeof(IParameterBroker).IsAssignableFrom(t))
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();
            // register cache service
            builder.RegisterType<MemoryCacheService>().As<ICacheService>().InstancePerHttpRequest();
            //register the interceptors
            builder.Register(c => new CacheAttributeInterceptor(c.Resolve<ICacheService>(), c.Resolve<IEnumerable<IParameterBroker>>())).InstancePerHttpRequest();
        }