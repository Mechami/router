/**
	\mainpage Router Messaging System
	\brief A statically-typed replacement messaging system for Unity Engine.
	\author Philip Muzzall
	\version 1.0.0
	\date 3/17/2014
	\copyright Property of Philip Muzzall

	\page usage Usage
		\details Each Component must register with the Router before it can receive messages.\n
		\details To do this create a Route and register it with the Router using Router.AddRoute(Route NewRoute).\n
		\details Components can register multiple Routes with the Router to subscribe to multiple events.

	\page routers Routers
		Routers are the core system for message passing in RMS.
		Each router internally maintains routing tables for all registered routes.\n
		As such these tables do not exist until the first route has been registered.\n
		If all routes are removed then the router will destroy these tables and recreate them again when they are needed.

	\page routes Routes
		Routes are the structs used by Router to register and access subscribers.\n
		Router cannot operate without valid Routes being registered first.\n
		A valid Route is composed of a reference to a component, a delegate to a function and a routing event.\n
		A Route is considered valid as long as all three of this fields are non-null.\n
		Multicast delegates are not supported; All but the first delegate in the invocation are discarded.

	\page hub Hubs
		Hubs are lightweight routers designed for intrahierarchy message passing.

	\page todo Todo List
	\details General changes:
		- Propagate changes from R0 to RX routers.
		- Implement table destruction for when the sole route in a router dies and nothing calls CleanDeadRoutes.
		- Redocument everything.
		.
	___
	\details Specific changes:
*/
