/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 5, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel.Provider
{
	/// <summary>
	/// Scene database provider.
	/// </summary>
	public class SceneDatabaseProvider
	{
		private static ReferenceCounter<SceneDatabase> _instance;

		#region Public Static
		/// <summary>
		/// Gets the database.
		/// </summary>
		/// <returns>The database.</returns>
		/// <param name="source">Source.</param>
		public static SceneDatabase GetDatabase (object source)
		{
			if (_instance == null)
				_instance = new ReferenceCounter<SceneDatabase> ();

			_instance.AddSource (source);
			return _instance.ClassInstance;
		}

		/// <summary>
		/// Returns the database.
		/// </summary>
		/// <param name="source">Source.</param>
		public static void ReturnDatabase (object source)
		{
			if (_instance == null)
				return;

			_instance.RemoveSource (source);
			if (_instance.Count <= 0) {
				_instance = null;
			}
		}
		#endregion
	}
}

