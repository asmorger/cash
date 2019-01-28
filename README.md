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
* _Dynamic proxy objects_: Dynamic proxies are a special type of object that are defined and generated on demand at run time.  These proxy objects define hooks that allow application developers to define behaviors before and after methods are invoked.  The solution used in this project is [Dynamic Proxy](http://www.castleproject.org/projects/dynamicproxy/).
* _Dependency Injection_: You **ARE** using DI, right?  If not [here's an overview of SOLID](https://scotch.io/bar-talk/s-o-l-i-d-the-first-five-principles-of-object-oriented-design).

## TODO!
More to come.
- Samples
- Built-in type handlers
- Defining registrations via *ICacheKeyRegistrationService*
- Defining Issues around
  - Updating interface to better match netStandard usages
  - Potential updates to the *ICacheKeyRegistrationService* to make it easier to define DI needs of the solution
- Finish updating the nuspec files to be netStandard compliant