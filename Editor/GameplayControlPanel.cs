/// ------------------------------------------------
/// <summary>
/// Gameplay Controls Panel
/// Purpose: 	Custom Gameplay Controls.
/// Author:		Juan Silva
/// Date: 		November 29, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using TuxedoBerries.ScenePanel.Drawers;

namespace TuxedoBerries.ScenePanel
{
	public class GameplayControlPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Gameplay";
		private const string PANEL_TOOLTIP = "Custom Gameplay Controls";
		private GameplayControlsDrawer _drawer;

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

		private void OnDestroy()
		{
			if(_drawer != null)
				_drawer.Dispose ();
		}
	}
}

