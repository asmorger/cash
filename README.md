# cash

## Build Status
| Branch | Status | Coverage |
| ------ | ------ | -------- |
| develop | [![Build status](https://ci.appveyor.com/api/projects/status/cunktf3184qjhr8j?svg=true)](https://ci.appveyor.com/project/asmorger/cash) | [![Coverage Status](https://coveralls.io/repos/github/asmorger/cash/badge.svg?branch=develop)](https://coveralls.io/github/asmorger/cash?branch=develop) |
| master | [![Build status](https://ci.appveyor.com/api/projects/status/cunktf3184qjhr8j/branch/master?svg=true)](https://ci.appveyor.com/project/asmorger/cash/branch/master) | [![Coverage Status](https://coveralls.io/repos/github/asmorger/cash/badge.svg?branch=master)](https://coveralls.io/github/asmorger/cash?branch=master) |

## Overview
Cash is perhaps the simplest way of adding caching to your .Net application.  Through the clever use of standard patterns/practices, sane defaults, and a simple interface for the more complicated stuff, Cash provides a drop-in solution that easily integrates with your application at the DI level.

## How does this Wizardry work?
It's time for some buzzwords!  Cash is an **Aspect Oriented Programming (AOP)** implementation of the **Decorator pattern** via **dynamic proxy objects** that integrates with your application through your **Dependency Injection container**.  

Let's unpack what that actually means, eh?

* _AOP_: AOP is a software development paradigm that seeks to increase software modularity by adding behaviors to existing code without modifying the code itself.
* _Decorator pattern_: The Decorator pattern is a design pattern that adds behaviors to an individual object through inheritance.
* _Dynamic proxy objects_: Dynamic proxies are a special type of object that are defined and generated on-demand at run time.  These proxy objects define hooks that allow application developers to define behaviors before and after methods are invoked.  The solution used in this project is [Dynamic Proxy](http://www.castleproject.org/projects/dynamicproxy/).
* _Dependency Injection_: You **ARE** using DI, right?  If not [here's an overview of SOLID](https://scotch.io/bar-talk/s-o-l-i-d-the-first-five-principles-of-object-oriented-design).

## Just how good is this Wizardry?

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

// By default Cash will handle enum, primitive, and null parameter values.

// within our Autofac registration logic:
var builder = new ContainerBuilder();

// this registers all of the necessary classes with Autofac
builder.AddCaching(MemoryCache.Default, registry)

// append Autofac registration option to include caching logic
builder.RegisterType<DataService>().As<IDataService>().WithCaching();
```

In the words of a former coworker: **Boom!  Done!**.



## TODO!
More to come.
- Samples
- Built-in type handlers
- Defining registrations via *ICacheKeyRegistrationService*
- Defining Issues around
  - Updating interface to better match netStandard usages
  - Potential updates to the *ICacheKeyRegistrationService* to make it easier to define DI needs of the solution
- Finish updating the nuspec files to be netStandard compliant