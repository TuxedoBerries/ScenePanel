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
	/// Gameplay control panel.
	/// </summary>
	public class GameplayControlPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Gameplay";
		private const string PANEL_TOOLTIP = "Custom Gameplay Controls";
		private GameplayControlsDrawer _drawer;

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
				_drawer = new GameplayControlsDrawer ();
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			EditorGUILayout.Space ();
			_drawer.DrawGeneralControls ();
		}

		private void OnDestroy ()
		{
			if (_drawer != null)
				_drawer.Dispose ();
		}
	}
}

