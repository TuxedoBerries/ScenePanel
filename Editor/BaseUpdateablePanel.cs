/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Base updateable panel.
	/// </summary>
	public abstract class BaseUpdateablePanel : EditorWindow
	{
		private const float UPDATE_POINT = 1.0f;
		private float _deltaBetweenUpdates = 0;
		private bool _allowRepaint = true;

		private void OnInspectorUpdate ()
		{
			if (EditorApplication.isPlaying)
				return;

			// Fixed Update
			_deltaBetweenUpdates += 0.1f;
			if (_deltaBetweenUpdates >= UpdatePoint) {
				if (_allowRepaint) {
					_deltaBetweenUpdates = 0;
					BeforeUpdate ();
					Repaint ();
				}
			}
		}

		/// <summary>
		/// Execute the OnGUI event.
		/// </summary>
		private void OnGUI ()
		{
			_allowRepaint = false;
			ApplyTitle ();
			CheckComponents ();

			DrawToolbar ();
			DrawContent ();
			_allowRepaint = true;
		}

		/// <summary>
		/// Draws the toolbar.
		/// </summary>
		private void DrawToolbar ()
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.toolbar);
			{
				DrawToolbarContent ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		#region Virtual
		/// <summary>
		/// Execute the Before Update event
		/// </summary>
		protected virtual void BeforeUpdate ()
		{
			// Nothing
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

		/// <summary>
		/// Draws the content of the toolbar.
		/// </summary>
		protected virtual void DrawToolbarContent ()
		{
			EditorGUILayout.Space ();
		}
		#endregion

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
