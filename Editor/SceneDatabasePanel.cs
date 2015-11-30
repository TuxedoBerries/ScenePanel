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

namespace TuxedoBerries.ScenePanel
{
	public class SceneDatabasePanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Database";
		private const string PANEL_TOOLTIP = "Options for exporting the current scene database.";
		private ToolsDrawer _drawer;
		private SceneDatabaseProvider _provider;

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
			if (_provider == null)
				_provider = new SceneDatabaseProvider ();
		}
		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ();
			{
				_drawer.SetDatabase (_provider);
				_drawer.DrawUtils ();
			}
			EditorGUILayout.EndVertical ();
			EditorGUILayout.Space ();
		}
	}
}

