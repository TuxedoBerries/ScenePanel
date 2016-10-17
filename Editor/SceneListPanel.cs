/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.Drawers;
using TuxedoBerries.ScenePanel.Controllers;
using TuxedoBerries.ScenePanel.Provider;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene list panel.
	/// </summary>
	public class SceneListPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Scene List";
		private const string PANEL_TOOLTIP = "List all the scenes in the project";
		private SceneEntityDrawer _drawer;
		private SceneEntityDrawer _favDrawer;
		private ScreenshotDrawer _screenshotDrawer;
		private ScrollableContainer _scrolls;
		private FolderContainer _folders;
		private SceneDatabase _database;
		private string _search;

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
			if (_drawer == null)
				_drawer = new SceneEntityDrawer (PANEL_TITLE);
			if (_favDrawer == null)
				_favDrawer = new SceneEntityDrawer (PANEL_TITLE);
			if (_screenshotDrawer == null)
				_screenshotDrawer = new ScreenshotDrawer ();
			if (_scrolls == null)
				_scrolls = new ScrollableContainer (PANEL_TITLE, true);
			if (_folders == null)
				_folders = new FolderContainer (PANEL_TITLE, true);
			if (_database == null)
				_database = SceneDatabaseProvider.GetDatabase (this);
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			UpdateCurrentScene ();
			EditorGUILayout.Space ();
			_scrolls.DrawScrollable ("main", DrawMainScroll);
			EditorGUILayout.Space ();
		}

		private void UpdateCurrentScene ()
		{
			_database.SetAsActive (SceneMainPanelUtility.CurrentActiveScene);
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

		private void OnDestroy ()
		{
			if (_drawer != null)
				_drawer.Dispose ();
			if (_favDrawer != null)
				_favDrawer.Dispose ();
			if (_screenshotDrawer != null)
				_screenshotDrawer.Dispose ();
			SceneDatabaseProvider.ReturnDatabase (this);
		}

		#region Filter
		protected override void DrawToolbarContent ()
		{
			EditorGUILayout.LabelField ("Filter", GUILayout.Width (50));
			_search = EditorGUILayout.TextField (_search, GUI.skin.FindStyle ("ToolbarSeachTextField"));
			if (GUILayout.Button ("", GUI.skin.FindStyle ("ToolbarSeachCancelButton"))) {
				_search = "";
			}

			_drawer.EnableEditing = GUILayout.Toggle (_drawer.EnableEditing, "Edit", EditorStyles.toolbarButton, GUILayout.Width (40));
			_favDrawer.EnableEditing = _drawer.EnableEditing;
		}

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
			DrawIenum (_favDrawer, _database.GetFavorites ());
		}

		private void DrawAll ()
		{
			DrawIenum (_drawer, _database.GetAllScenes ());
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
	}
}

