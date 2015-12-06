/// ------------------------------------------------
/// <summary>
/// Scene Database Panel
/// Purpose: 	Display some options for the database.
/// Author:		Juan Silva
/// Date: 		November 29, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using UnityEngine;
using TuxedoBerries.ScenePanel.Drawers;
using TuxedoBerries.ScenePanel.Controllers;
using TuxedoBerries.ScenePanel.Provider;

namespace TuxedoBerries.ScenePanel
{
	public class SceneDatabasePanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Database";
		private const string PANEL_TOOLTIP = "Options for exporting the current scene database.";
		private ToolsDrawer _drawer;

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
				_drawer = new ToolsDrawer ();
		}
		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ();
			{
				_drawer.DrawUtils ();
			}
			EditorGUILayout.EndVertical ();
			EditorGUILayout.Space ();
		}

		private void OnDestroy()
		{
			if(_drawer != null)
				_drawer.Dispose ();
		}
	}
}

