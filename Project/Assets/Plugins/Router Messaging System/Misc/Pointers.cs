namespace RouterMessagingSystem
{
	/** \brief Function that doesn't accept or return any parameters. */
	public delegate void RoutePointer();

	/** \brief Function that returns an object of type R. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R>();

	/** \brief Function that returns an object and accepts one parameter. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1>(T1 P1 /**< Input parameter of type T. */);

	/** \brief Function that returns an object and accepts two parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */);

	/** \brief Function that returns an object and accepts three parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2, in T3>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */, T3 P3 /**< Input parameter of type T3. */);
}