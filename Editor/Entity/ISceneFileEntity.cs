/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 27, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// IScene file entity.
	/// </summary>
	public interface ISceneFileEntity
	{
		/// <summary>
		/// Gets the name of the scene.
		/// </summary>
		/// <value>The name.</value>
		string Name {
			get;
		}

		/// <summary>
		/// Gets the full path of the scene.
		/// </summary>
		/// <value>The full path.</value>
		string FullPath {
			get;
		}

		/// <summary>
		/// Gets the GUID.
		/// </summary>
		/// <value>The GUID.</value>
		string GUID {
			get;
		}
	}
}