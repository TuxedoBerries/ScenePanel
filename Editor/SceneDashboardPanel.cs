/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		December 5, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.Drawers;
using TuxedoBerries.ScenePanel.Provider;
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene dashboard panel.
	/// </summary>
	public class SceneDashboardPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Scene Dashboard";
		private const string PANEL_TOOLTIP = "Scene Dashboard Panel";
		private SceneEntityDrawer _sceneDrawer;
		private SceneEntityDrawer _favSceneDrawer;
		private GameplayControlsDrawer _gameplayDrawer;
		private ScreenshotDrawer _screenshotDrawer;
		private SceneHistoryDrawer _historyDrawer;

		private FolderContainer _folders;
		private ScrollableContainer _scrolls;
		private SceneDatabase _database;
		private string _search;

		#region Update Flow
		/// <summary>
		/// Gets the update point.
		/// </summary>
		/// <value>The update point.</value>
		protected override float UpdatePoint {
			get {
				return 0.25f;
			}
		}

		/// <summary>
		/// Applies the title.
		/// </summary>
		protected override void ApplyTitle ()
		{
			this.titleContent.text = PANEL_TITLE;
			this.titleContent.tooltip = PANEL_TOOLTIP;
		}

		/// <summary>
		/// Checks the components.
		/// </summary>
		protected override void CheckComponents ()
		{
			if (_sceneDrawer == null)
				_sceneDrawer = new SceneEntityDrawer (PANEL_TITLE);
			if (_favSceneDrawer == null)
				_favSceneDrawer = new SceneEntityDrawer (PANEL_TITLE);
			if (_gameplayDrawer == null)
				_gameplayDrawer = new GameplayControlsDrawer ();
			if (_screenshotDrawer == null)
				_screenshotDrawer = new ScreenshotDrawer ();
			if (_historyDrawer == null)
				_historyDrawer = new SceneHistoryDrawer ();

			if (_database == null)
				_database = SceneDatabaseProvider.GetDatabase (this);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer (PANEL_TITLE, true);
			if (_folders == null)
				_folders = new FolderContainer (PANEL_TITLE, true);
		}

		/// <summary>
		/// Draws the content of the toolbar.
		/// </summary>
		protected override void DrawToolbarContent ()
		{
			EditorGUILayout.LabelField ("Filter", GUILayout.Width (50));
			_search = EditorGUILayout.TextField (_search, GUI.skin.FindStyle ("ToolbarSeachTextField"));
			if (GUILayout.Button ("", GUI.skin.FindStyle ("ToolbarSeachCancelButton"))) {
				_search = "";
			}

			_sceneDrawer.EnableEditing = GUILayout.Toggle (_sceneDrawer.EnableEditing, "Edit", EditorStyles.toolbarButton, GUILayout.Width (40));
			_favSceneDrawer.EnableEditing = _sceneDrawer.EnableEditing;
			_historyDrawer.RestoreOnStop = GUILayout.Toggle (_historyDrawer.RestoreOnStop, "Restore On Stop", EditorStyles.toolbarButton, GUILayout.Width (100));
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			UpdateCurrentScene ();
			UpdateHistory ();

			// Gameplay controls
			_gameplayDrawer.DrawGeneralControls ();
			EditorGUILayout.Space ();
			_folders.DrawFoldable ("History", _historyDrawer.DrawHistory);
			// Scene list
			EditorGUILayout.Space ();
			_scrolls.DrawScrollable ("main", DrawMainScroll);
			EditorGUILayout.Space ();
		}

		/// <summary>
		/// Execute the Before Update event
		/// </summary>
		protected override void BeforeUpdate ()
		{
			if (_database == null)
				return;

			_database.Refresh ();
		}

		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		private void OnDestroy ()
		{
			// Return the Database
			if (_sceneDrawer != null)
				_sceneDrawer.Dispose ();
			if (_favSceneDrawer != null)
				_favSceneDrawer.Dispose ();
			if (_gameplayDrawer != null)
				_gameplayDrawer.Dispose ();
			if (_screenshotDrawer != null)
				_screenshotDrawer.Dispose ();
			if (_historyDrawer != null)
				_historyDrawer.Dispose ();
			SceneDatabaseProvider.ReturnDatabase (this);
		}
		#endregion

		#region Filter
		private bool PassFilter (ISceneEntity entity)
		{
			if (string.IsNullOrEmpty (_search))
				return true;
			if (entity.Name.ToLower ().Contains (_search.ToLower ()))
				return true;

			return false;
		}
		#endregion

		#region Lists
		private void DrawMainScroll ()
		{
			_folders.DrawFoldable ("Favorites", DrawAllFavorites);
			_folders.DrawFoldable ("All Scenes", DrawAll);
		}

		private void DrawAllFavorites ()
		{
			DrawIenum (_favSceneDrawer, _database.GetFavorites ());
		}

		private void DrawAll ()
		{
			DrawIenum (_sceneDrawer, _database.GetAllScenes ());
		}

		private void DrawIenum (SceneEntityDrawer drawer, IEnumerator<ISceneEntity> ienum)
		{
			while (ienum.MoveNext ()) {
				var entity = ienum.Current;
				// Apply Search
				if (!PassFilter (entity))
					continue;

				EditorGUILayout.BeginHorizontal ();
				{
					drawer.DrawEntity (entity);
					_database.UpdateEntity (entity);
				}
				EditorGUILayout.EndHorizontal ();

				if (!drawer.AreDetailsOpen (entity))
					continue;
				EditorGUILayout.BeginHorizontal ();
				{
					GUILayout.Space (22);
					EditorGUILayout.BeginVertical ();
					{
						_screenshotDrawer.DrawSnapshot (entity);
						EditorGUILayout.Space ();
					}
					EditorGUILayout.EndVertical ();
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
		#endregion

		#region History
		/// <summary>
		/// Updates the current scene.
		/// </summary>
		private void UpdateCurrentScene ()
		{
			_database.SetAsActive (SceneMainPanelUtility.CurrentActiveScene);
		}

		/// <summary>
		/// Updates the history.
		/// </summary>
		private void UpdateHistory ()
		{
			if (_gameplayDrawer.IsPlaying)
				return;

			_historyDrawer.RestoreFromPlay ();
			_historyDrawer.UpdateCurrentHistory ();
		}

		/// <summary>
		/// Clears the history.
		/// </summary>
		public void ClearHistory ()
		{
			CheckComponents ();
			_historyDrawer.ClearHistory ();
		}
		#endregion
	}
}

