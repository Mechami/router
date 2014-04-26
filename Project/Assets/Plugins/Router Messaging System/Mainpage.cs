/**
	\mainpage Router Messaging System
	\brief A statically-typed replacement messaging system for Unity Engine built around not using blasted strings.
	\author Philip Muzzall
	\version 1.0.0
	\date 3/17/2014
	\copyright Property of Philip Muzzall

	\page usage Usage
		\details Each Component must register with the Router before it can receive messages.\n
		\details To do this create a Route and register it with the Router using Router.AddRoute(Route NewRoute).\n
		\details Components can register multiple Routes with the Router to subscribe to multiple events.

	\page routers Routers
		Router internally maintains routing tables for all registered Routes.\n
		These tables do not exists until the first Route has been registered.\n
		If all Routes are removed then Router will destroy these tables and recreate them again when they are needed.

	\page routes Routes
		Routes are the structs used by Router to register and access subscribers.\n
		Router cannot operate without valid Routes being registered first.\n
		A valid Route is composed of a reference to a component, a delegate to a function and a routing event.\n
		A Route is considered valid as long as all three of this fields are non-null.
		Components can create any number of Routes to subscribe multiple functions to multiple events; however, when constructing a Route if you pass a multicast delegate then the Route will only use the delegate at index 0 in the invocation list.

	\page todo Todo List
	\details General changes:
		- Propagate changes from R0 to RX routers.
		- Redocument everything.
		.
	___
	\details Specific changes:
*/