# cash

## Build Status
| Branch | Status | Coverage |
| ------ | ------ | -------- |
| develop | [![Build status](https://ci.appveyor.com/api/projects/status/cunktf3184qjhr8j?svg=true)](https://ci.appveyor.com/project/asmorger/cash) | [![Coverage Status](https://coveralls.io/repos/github/asmorger/cash/badge.svg?branch=develop)](https://coveralls.io/github/asmorger/cash?branch=develop) |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/cunktf3184qjhr8j/branch/master?svg=true)](https://ci.appveyor.com/project/asmorger/cash/branch/master) | [![Coverage Status](https://coveralls.io/repos/github/asmorger/cash/badge.svg?branch=master)](https://coveralls.io/github/asmorger/cash?branch=master) |

## Overview
Cash is perhaps the simplest way of adding caching to your .Net application.  Through the clever use of standard patterns/practices, sane defaults, and a simple interface for the more complicated stuff, Cash provides a drop-in solution that easily integrates with your application at the DI level.

## How does this wizardry work?
It's time for some buzzwords!  Cash is an **Aspect Oriented Programming (AOP)** implementation of the **Decorator pattern** via **dynamic proxy objects** that integrates with your application through your **Inversion of Control (dependency injection) container**.  

Let's unpack what that actually means, eh?

* _AOP_: AOP is a software development paradigm that seeks to increase software modularity by adding behaviors to existing code without modifying the code itself.
* _Decorator pattern_: The Decorator pattern is a design pattern that adds behaviors to an individual object through inheritance.
* _Dynamic proxy objects_: Dynamic proxies are a special type of object that are defined and generated on-demand at run time.  These proxy objects define hooks that allow application developers to define behaviors before and after methods are invoked.  The solution used in this project is [Dynamic Proxy](http://www.castleproject.org/projects/dynamicproxy/).
* _Inversion of Control_: You **ARE** using DI, right?  If not, this solution is **not for you** and [here's an overview of SOLID](https://scotch.io/bar-talk/s-o-l-i-d-the-first-five-principles-of-object-oriented-design).

## Just how good is this wizardry?

### Standard caching paradigm

Let's take a look as to how this would normally be implemented:

```csharp
public class DataService
{
    private readonly ObjectCache _cache;

    public DataService()
    {
        _cache = MemoryCache.Default;
    }

    public int GenerateRandomData(int maxValue)
    {
        var cacheKey = $"GenerateRandomData_max:{maxValue}";

        if(_cache.Contains(cacheKey))
        {
            var cachedValue = (int)_cache.Get(cacheKey);
            return cachedValue;
        }

        // don't actually do this - it won't work the way you think.  It's an exmaple, yo!
        var random = new Random(0, maxValue);
        var value = random.Next();

        _cache.Set(value, cacheKey);

        return value;
    }
}

```
There's a lot of boilerplate code in the example above.  There is the generation of the method/argument specific **cache key**, **checking** the cache for existing data, and then **adding** the result of the method operation to the cache itself.  In fact, **7 out of 10 LOC** of that method are dedicated toward caching alone!

For a single method, this isn't bad a bad pattern. In fact this is pretty straight-forward, to the point, and easy to maintain.

But what happens if you have 10 methods that need cached?  Or 100?  More?  At what point do you not want to write the boilerplate every single time?  What is your change management process if you ever want to move to **Redis**?

### The Cash paradigm

```csharp
public interface IDataService
{
    int GenerateRandomData(int maxValue);
}

public class DataService : IDataService
{
    [Cache]
    public int GenerateRandomData(int maxValue)
    {
        // don't actually do this - it won't work the way you think.  It's an exmaple, yo!
        var random = new Random(1, maxValue);
        var value = random.Next();

        return value;
    }
}

```

That is all we need to do from a class declaration standpoint.  We do, however, need to modify our DI registration to tell it about our caching solution:

**Autofac:**
```csharp

var registry = new CacheKeyRegistry();
// In this particular example we don't need to add anything to the registry due to the fact that 
// the "maxValue" method parameter is a primitive type.

// By default Cash will handle enum, primitive, and null parameter values (see below).

// within our Autofac registration logic:
var builder = new ContainerBuilder();

// this registers all of the necessary classes with Autofac
// by default it will use MemoryCache.Default, but it can be overridden
builder.AddCaching(registry);

// append Autofac registration option to include caching logic
builder.RegisterType<DataService>().As<IDataService>().WithCaching();
```

To quote an old friend: **Boom!  Done!**.

## Ok, I'm interested.  How does it actually work?

### Cache key generation

One crucial aspect of a caching system is unique cache keys.  If your keys aren't appropriately unique you'll run into a heap of trouble when your keys collide and your caching returns unpredictable items.

Cache keys are generally defined on a per-method instance.  Consider the following method signature

```csharp
public class UserService
{
    public User GetUserByUserId(Guid userId)
    {
       // ... goes forth and gets the user   
    }
}

```

If caching were to be added to this method I would expect a cache key to be something like:

> GetUserById_{userId_guid_here}

However, what if that particular key is used elsewhere?  We now have a collision problem.  To make it more unique we might do something like this:

> UserService.GetUserByUserId_{userId_guid_here}

With that specific of a cache key, it is unlikely to collide and you can have a reasonably high degree of confidence that it will behave as intended.

## The Cash solution

How can Cash solve this problem in an automatic way?  Well, we can get a long way using standard .Net reflection, but there are some areas where we **need** help from you.

Using reflection, we can easily pull out the class and method names at runtime via our proxy.  That part is easy.  The hard part is dealing with the method parameters and pulling out a stable, unique representation of that object.  

Enter type handlers.

### Type handlers

Type handlers are the way that we translate both simple and complex method parameters into a string representation so we can incorporate that into our cache key.

A number of built-in .Net types are handled automatically for you:

* Enums
* Primitive types (string, int, bool, etc.)
* Null

This means that if your method parameters use only these types, Cash will work out-of-the-box with no further help from you.

However, it is unlikely that you will _never_ use a complex type as a parameter.  How do we configure these types to integrate with Cash?

The answer is the **CacheKeyRegistry**

### Cache Key Registry

The cache key registry is how we, as developers, can map complex types to simple string representations for incorporation into the cache key generation process.

Consider the following example:

```csharp
public class UserModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
}

public class AddressModel
{
    public int Id { get; set }
    public string Address1 { get; set; }
}

public interface IUserService
{
    AddressModel GetAddress(UserModel user);
}

public class UserService : IUserService
{
    [Cache]
    public AddressModel GetAddress(UserModel user)
    {
        // .. goes forth and gets the address
    }
}
```

If we tried to run execute this method with Cash it would result in an exception because the system doesn't know how to handle the **UserModel**.

How do we register that?  Great question.

```csharp
var registry = new CacheKeyRegistry();
registry.Register<UserModel>(x => x.Id.ToString());

// autofac example
var builder = new ContainerBuilder();
builder.AddCaching(registry);
builder.RegisterType<UserService>().As<IUserService>().WithCaching();
```

That's it!  Behind the scenes this method is transforming that UserModel Expression into another version of the TypeHandler and integrating it into the Cash system.

The cache key generated for the the example would be
> UserService.AddressModel(UserModel::{value of the Id property})

## But wait!  You didn't think about ...

Oh, you think I missed something?  Let's go through some scenarios taken from the unit tests 

### Enumerable parameters

Method signature:
```csharp
public void TestMethod_EnumerableParameter(IEnumerable<int> heights)
{
}
```

Cache Key: 
> TestMethod_EnumerableParameter(Int32::10||Int32::20)

### No arguments

Method signature:
```csharp
public void TestMethod_NoParameters()
{
}
```

Cache Key: 
> TestMethod_NoParameters(<no_arguments>)

### Multiple Args when 1 is null

Method signature:
```csharp
public void TestMethod_Args(object[] args)
{
    // called with valeus 5, null
}
```

Cache Key: 
> TestMethod_Args(Int32::5||[UnknownType]::[NULL])


## TODO!
More to come.
- Defining Issues around
  - Updating interface to better match netStandard usages
  - Potential updates to the *ICacheKeyRegistrationService* to make it easier to define DI needs of the solution
- Finish updating the nuspec files to be netStandard compliant