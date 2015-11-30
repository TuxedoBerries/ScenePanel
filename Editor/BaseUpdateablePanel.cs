/// ------------------------------------------------
/// <summary>
/// Base Updateable Panel
/// Purpose: 	Panel that refreshes every one second.
/// Author:		Juan Silva
/// Date: 		November 29, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	public abstract class BaseUpdateablePanel : EditorWindow
	{
		private const float UPDATE_POINT = 1.0f;
		private float _deltaBetweenUpdates = 0;
		private bool _allowRepaint = true;

		private void OnInspectorUpdate()
		{
			// Fixed Update
			_deltaBetweenUpdates += 0.1f;
			if (_deltaBetweenUpdates >= UpdatePoint) {
				if (_allowRepaint) {
					_deltaBetweenUpdates = 0;
					Repaint ();
				}
			}
		}

		/// <summary>
		/// Gets the update point.
		/// </summary>
		/// <value>The update point.</value>
		protected virtual float UpdatePoint {
			get {
				return UPDATE_POINT;
			}
		}

		private void OnGUI()
		{
			_allowRepaint = false;
			ApplyTitle ();
			CheckComponents ();
			DrawContent ();
			_allowRepaint = true;
		}

		#region Abstract
		/// <summary>
		/// Applies the title.
		/// </summary>
		protected abstract void ApplyTitle ();

		/// <summary>
		/// Checks the components.
		/// </summary>
		protected abstract void CheckComponents ();

		/// <summary>
		/// Draws the content.
		/// </summary>
		protected abstract void DrawContent ();
		#endregion
	}
}

