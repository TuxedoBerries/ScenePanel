/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using TuxedoBerries.ScenePanel.Drawers;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene database panel.
	/// </summary>
	public class SceneDatabasePanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Database";
		private const string PANEL_TOOLTIP = "Options for exporting the current scene database.";
		private ToolsDrawer _drawer;

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

		private void OnDestroy ()
		{
			if (_drawer != null)
				_drawer.Dispose ();
		}
	}
}

