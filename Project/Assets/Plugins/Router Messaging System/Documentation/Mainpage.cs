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
		Routers are the base system in RMS as they are used be to pass messages between objects.
		Routers internally maintain routing tables for all registered routes.\n
		These tables do not exists until the first route has been registered.\n
		If all routes are removed then the router will destroy these tables and recreate them again when they are needed.

	\page routes Routes
		Routes are the structs used by Router to register and access subscribers.\n
		Router cannot operate without valid Routes being registered first.\n
		A valid Route is composed of a reference to a component, a delegate to a function and a routing event.\n
		A Route is considered valid as long as all three of this fields are non-null.
		Components can create any number of Routes to subscribe multiple functions to multiple events; however, when constructing a Route if you pass a multicast delegate then the Route will only use the delegate at index 0 in the invocation list.

	\page hub Hubs
		Hubs are lightweight routers designed to handle message passing within objects.\n
		A hub component is attached to a GameObject and that GameObject's components can then use the hub to pass messages between each other.

	\page todo Todo List
	\details General changes:
		- Propagate changes from R0 to RX routers.
		- Implement table destruction for when the sole route in a router dies and nothing calls CleanDeadRoutes.
		- Redocument everything.
		.
	___
	\details Specific changes:
*/
