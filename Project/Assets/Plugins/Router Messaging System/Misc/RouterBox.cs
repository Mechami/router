using UnityEngine;
using System.Collections.Generic;
using RouterMessagingSystem;

namespace RouterMessagingSystem
{
	/// \brief Manages multiple routers
	public static class RouterBox
	{
		private static Dictionary<string, Router> SwitchTable = null;

		static RouterBox()
		{
			SwitchTable = new Dictionary<string, Router>();
			SwitchTable.Add("system", new Router("System Router"));
		}

		/** \brief Creates and returns a new router with the associated key */
		/// Creates a new router if none with the specified key exists
		/// otherwise returns the already existing router.
		public static Router AddRouter(string Key)
		{
			string Fixed = Key.ToLower();

			if (!SwitchTable.ContainsKey(Fixed))
			{
				Router NewRouter = new Router();
				SwitchTable.Add(Fixed, NewRouter);
			}
			else
			{
				Debug.LogWarning("[RouterBox] Router for " +
								 Key + " already exists!");
			}

			return SwitchTable[Fixed];
		}

		/// \brief Removes the router with the associated key
		public static bool RemoveRouter(string Key)
		{
			bool Result = false;
			string Fixed = Key.ToLower();

			if (Fixed != "system")
			{
				Result = SwitchTable.Remove(Fixed);
			}
			else
			{
				Debug.LogWarning("[RouterBox] Cannot remove system router!");
			}

			return Result;
		}

		/// \brief Returns true if the specified key is associated with a router
		public static bool HasRouter(string Key)
		{
			return SwitchTable.ContainsKey(Key.ToLower());
		}

		/// \brief Returns the default system-wide router
		public static Router GetRouter()
		{
			return SwitchTable["system"];
		}

		/** \brief Returns the router associated with the specified key */
		/// \returns The router associated with the specified key or null.
		public static Router GetRouter(string Key)
		{
			string Fixed = Key.ToLower();
			return SwitchTable.ContainsKey(Fixed)? SwitchTable[Fixed] : null;
		}
	}
}