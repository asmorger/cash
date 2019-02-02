var registry = new CacheKeyRegistry();
registry.Register<UserModel>(x => x.Id.ToString());