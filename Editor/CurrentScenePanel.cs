/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using TuxedoBerries.ScenePanel.Drawers;
using TuxedoBerries.ScenePanel.Provider;
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Current scene panel.
	/// </summary>
	public class CurrentScenePanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Current Scene";
		private const string PANEL_TOOLTIP = "Display all the detials of the current scene";
		private ScrollableContainer _scrolls;
		private SceneEntityDrawer _drawer;
		private ScreenshotDrawer _screenshotDrawer;
		private SceneDatabase _database;

		/// <summary>
		/// Applies the title.
		/// </summary>
		protected override void ApplyTitle()
		{
			this.titleContent.text = PANEL_TITLE;
			this.titleContent.tooltip = PANEL_TOOLTIP;
		}

		/// <summary>
		/// Checks the components.
		/// </summary>
		protected override void CheckComponents()
		{
			if (_drawer == null)
				_drawer = new SceneEntityDrawer (PANEL_TITLE);
			if (_screenshotDrawer == null)
				_screenshotDrawer = new ScreenshotDrawer ();
			if (_database == null)
				_database = SceneDatabaseProvider.GetDatabase(this);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer (PANEL_TITLE, true);
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			UpdateCurrentScene ();
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Space (20);
				_scrolls.DrawScrollable ("main", Content);
			}
			EditorGUILayout.EndHorizontal ();
		}

		protected override void DrawToolbarContent()
		{
			EditorGUILayout.Space ();
			_drawer.EnableEditing = GUILayout.Toggle (_drawer.EnableEditing, "Edit", EditorStyles.toolbarButton, GUILayout.Width (40));
		}

		private void Content()
		{
			var currentScene = _database.CurrentActive;
			_drawer.DrawDetailEntity (currentScene);
			_screenshotDrawer.DrawSnapshot (currentScene);
			_database.UpdateEntity (currentScene);
		}

		private void UpdateCurrentScene()
		{
			_database.SetAsActive (SceneMainPanelUtility.CurrentActiveScene);
		}

		/// <summary>
		/// Execute the Before Update event
		/// </summary>
		protected override void BeforeUpdate()
		{
			if (_database == null)
				return;

			_database.Refresh ();
		}

		private void OnDestroy()
		{
			// Return the Database
			SceneDatabaseProvider.ReturnDatabase (this);
			if(_drawer != null)
				_drawer.Dispose ();
			if(_screenshotDrawer != null)
				_screenshotDrawer.Dispose ();
		}
	}
}

