var registry = new CacheKeyRegistry();
// eventually your own complex-type key registrations will go here.

var builder = new ContainerBuilder();

// this registers all of the necessary classes with Autofac
// by default it will use MemoryCache.Default, but it can be overridden
builder.AddCaching(registry);

// append Autofac registration option to include caching process
builder.RegisterType<HelloWorld>().As<IHelloWorld>().WithCaching();