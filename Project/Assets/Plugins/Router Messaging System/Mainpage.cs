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

	\page notes Notes
	\note Router internally maintains routing tables for all registered Routes.\n
	\note These tables do not exists until the first Route has been registered.\n
	\note If all Routes are removed then Router will destroy these tables and recreate them again when they are needed.

	\page todo Todo List
	\details General changes:
		- Propagate changes from base to RX routers.
		- Redocument everything.
		.
	___
	\details Specific changes:
*/