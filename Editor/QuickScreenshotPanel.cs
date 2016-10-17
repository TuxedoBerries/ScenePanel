/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using TuxedoBerries.ScenePanel.Drawers;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Quick screenshot panel.
	/// </summary>
	public class QuickScreenshotPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "Snapshots";
		private const string PANEL_TOOLTIP = "Take quick snapshots of the game";
		private ScreenshotDrawer _drawer;

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
				_drawer = new ScreenshotDrawer ();
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			_drawer.DrawFull ();
		}

		private void OnDestroy()
		{
			if(_drawer != null)
				_drawer.Dispose ();
		}
	}
}

