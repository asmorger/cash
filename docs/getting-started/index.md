---
uid: getting-started-index
---

# Getting Started

## Nuget Installation

# [Autofac](#tab/autofac)

Install-Package Cash.Autofac

***

## Integration

After installing the NuGet package, integrating the solution is incredibly simple.

**Step One:** Create a **Registry** and update your Autofac registrations

# [Autofac](#tab/autofac)

***

[!code-cs[Main](samples/autofac/integration-with-container-builder.cs)]

**Step Two:** Update your service implementation method with the **Cache** attribute

[!code-cs[Main](samples/autofac/integration-with-service.cs)]

**Step Three:** Profit!

## Registering Keys

Cash provides built-in support for all native .Net primitive types, but it is inevitable that you will need to support your own custom, complex types within your caching schemes.  This is where the **Registry** comes into play.  Consider the following example:

[!code-cs[Main](samples/registry/complex-type-registrations-models.cs)]

How do you instruct Cash to convert the **UserModel** into a cache key?  Simple!

[!code-cs[Main](samples/registry/complex-type-registrations.cs)]

Once this registration is in place, every time Cash sees the type **UserModel** as a method parameter, it will use this logic to convert that type into it's cache key representation.