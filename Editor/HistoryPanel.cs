/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.Drawers;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// History panel.
	/// </summary>
	public class HistoryPanel : BaseUpdateablePanel
	{
		private const string PANEL_TITLE = "History";
		private const string PANEL_TOOLTIP = "History control for the working scenes";
		private SceneHistoryDrawer _drawer;

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
			if (_drawer == null)
				_drawer = new SceneHistoryDrawer ();
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected override void DrawContent ()
		{
			UpdateHistory ();
			EditorGUILayout.Space ();
			_drawer.DrawHistory ();
		}

		/// <summary>
		/// Draws the content of the toolbar.
		/// </summary>
		protected override void DrawToolbarContent ()
		{
			EditorGUILayout.LabelField ("");
			_drawer.RestoreOnStop = GUILayout.Toggle (_drawer.RestoreOnStop, "Restore On Stop", EditorStyles.toolbarButton, GUILayout.Width (100));
		}

		/// <summary>
		/// Updates the history.
		/// </summary>
		private void UpdateHistory ()
		{
			if (SceneMainPanelUtility.IsPlaying)
				return;

			_drawer.RestoreFromPlay ();
			_drawer.UpdateCurrentHistory ();
		}

		/// <summary>
		/// Clears the history.
		/// </summary>
		public void ClearHistory ()
		{
			CheckComponents ();
			_drawer.ClearHistory ();
		}

		private void OnDestroy ()
		{
			if (_drawer != null)
				_drawer.Dispose ();
		}
	}
}

