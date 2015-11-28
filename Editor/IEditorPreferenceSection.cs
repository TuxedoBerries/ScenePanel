/// ------------------------------------------------
/// <summary>
/// IEditor Preference Section
/// Purpose: 	Interface for identify a Section in Editor Preference.
/// Author:		Juan Silva
/// Date: 		November 27, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;

namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	public interface IEditorPreferenceSection
	{
		/// <summary>
		/// Gets the type of the implementation.
		/// </summary>
		/// <value>The type of the implementation.</value>
		System.Type ImplementationType {
			get;
		}

		/// <summary>
		/// Gets the name of the section.
		/// </summary>
		/// <value>The name.</value>
		string Name {
			get;
		}
	}
}

