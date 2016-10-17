/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene panel menu.
	/// </summary>
	public class ScenePanelMenu
	{

		[MenuItem("TuxedoBerries/Scene Panel/Gameplay Control Panel", false, 10)]
		private static void InitGameplayControls()
		{
			var window = EditorWindow.GetWindow<GameplayControlPanel> ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/Dashboard Panel", false, 100)]
		private static void InitDashboard()
		{
			var window = EditorWindow.GetWindow<SceneDashboardPanel> ();
			window.ClearHistory ();
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

		[MenuItem("TuxedoBerries/Scene Panel/Current Scene Panel", false, 103)]
		private static void InitCurrentScene()
		{
			var window = EditorWindow.GetWindow<CurrentScenePanel> ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/Snapshot Panel", false, 1000)]
		private static void InitSnapshot()
		{
			var window = EditorWindow.GetWindow<QuickScreenshotPanel> ();
			window.Show ();
		}

		[MenuItem("TuxedoBerries/Scene Panel/Database Panel", false, 1001)]
		private static void InitDatabase()
		{
			var window = EditorWindow.GetWindow<SceneDatabasePanel> ();
			window.Show ();
		}
	}
}

