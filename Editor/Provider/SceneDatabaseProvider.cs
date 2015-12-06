/// ------------------------------------------------
/// <summary>
/// Scene Database Provider
/// Purpose: 	Provide a databse of the scenes in the project.
/// Author:		Juan Silva
/// Date: 		December 5, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel.Provider
{
	public class SceneDatabaseProvider
	{
		private static ReferenceCounter<SceneDatabase> _instance;

		#region Public Static
		/// <summary>
		/// Gets the database.
		/// </summary>
		/// <returns>The database.</returns>
		/// <param name="source">Source.</param>
		public static SceneDatabase GetDatabase(object source)
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
		public static void ReturnDatabase(object source)
		{
			if (_instance == null)
				return;

			_instance.RemoveSource(source);
			if (_instance.Count <= 0) {
				_instance = null;
				UnityEngine.Debug.Log ("Destroying Scene Database");
			}
		}
		#endregion
	}
}

