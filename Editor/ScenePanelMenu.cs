/// ------------------------------------------------
/// <summary>
/// Scene Panel Menu
/// Purpose: 	Menu item for Scene Panel.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	public class ScenePanelMenu
	{

		[MenuItem("TuxedoBerries/Scene Panel/Main Panel")]
		private static void Init()
		{
			var window = EditorWindow.GetWindow<SceneMainPanel> ();
			window.Show ();
		}
	}
}

