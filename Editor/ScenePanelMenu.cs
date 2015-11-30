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

		[MenuItem("TuxedoBerries/Scene Panel/Gameplay Control Panel", false, 100)]
		private static void InitGameplayControls()
		{
			var window = EditorWindow.GetWindow<GameplayControlPanel> ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/History Panel", false, 101)]
		private static void InitHistory()
		{
			var window = EditorWindow.GetWindow<HistoryPanel> ();
			window.ClearHistory ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/Scene List Panel", false, 102)]
		private static void InitSceneList()
		{
			var window = EditorWindow.GetWindow<SceneListPanel> ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/Snapshot Panel", false, 103)]
		private static void InitSnapshot()
		{
			var window = EditorWindow.GetWindow<QuickScreenshotPanel> ();
			window.Show ();
		}
	}
}

