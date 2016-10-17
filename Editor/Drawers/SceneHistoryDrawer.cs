/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 28, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.PreferenceHandler;
using TuxedoBerries.ScenePanel.Constants;
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Scene history drawer.
	/// Draws the history of the scenes opened.
	/// </summary>
	public class SceneHistoryDrawer : BaseDrawer
	{
		private const string CLASS_NAME = "SceneHistoryDrawer";
		private const string RESTORE_VAR = "restoreScene";
		private SceneHistory _history;
		private IPreferenceChannel _channel;

		private int _backSelected = 0;
		private int _forwardSelected = 0;
		private bool _restoreOnStop = true;
		private bool _justLoaded = true;
		private GUILayoutOption _textCol1;

		public SceneHistoryDrawer () : base ()
		{
			_history = new SceneHistory ();
			_history.Load ();

			_channel = EditorPreferenceHandler.GetChannel (this, CLASS_NAME);
			_restoreOnStop = _channel.GetBool (RESTORE_VAR);
			_textCol1 = GUILayout.Width (100);
		}

		/// <summary>
		/// Updates the current history.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void UpdateCurrentHistory ()
		{
			_history.Push (SceneFileEntity.GetCurrent ());
		}

		/// <summary>
		/// Restores from play.
		/// </summary>
		public void RestoreFromPlay ()
		{
			if (!_justLoaded)
				return;
			if (!_restoreOnStop)
				return;

			var item = _history.CurrentScene;
			SceneMainPanelUtility.OpenScene (item);
			_justLoaded = false;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TuxedoBerries.ScenePanel.Drawers.SceneHistoryDrawer"/>
		/// restore on stop.
		/// </summary>
		/// <value><c>true</c> if restore on stop; otherwise, <c>false</c>.</value>
		public bool RestoreOnStop {
			get {
				return _restoreOnStop;
			}
			set {
				_restoreOnStop = value;
				_channel.SetValue (RESTORE_VAR, _restoreOnStop);
			}
		}

		/// <summary>
		/// Draws the history.
		/// </summary>
		public void DrawHistory ()
		{
			_colorStack.Reset ();
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.BeginVertical (GUILayout.Width (90));
				{
					GUILayout.Space (5);
					EditorGUILayout.BeginHorizontal ();
					{
						// Back button
						_colorStack.Push (GetColor (_history.BackCount > 1));
						if (GUILayout.Button (GetTexture (IconSet.ARROW_BACK_ICON), GUILayout.Width (42))) {
							BackButtonAction ();
						}
						_colorStack.Pop ();

						// Forward button
						_colorStack.Push (GetColor (_history.FowardCount > 0));
						if (GUILayout.Button (GetTexture (IconSet.ARROW_FORWARD_ICON), GUILayout.Width (42))) {
							ForwardButtonAction ();
						}
						_colorStack.Pop ();
					}

					_colorStack.Push (GetColor (_history.Count > 1));
					EditorGUILayout.EndHorizontal ();
					if (GUILayout.Button ("Clear History", GUILayout.Width (90))) {
						ClearHistory ();
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndVertical ();
				EditorGUILayout.BeginVertical ();
				{
					// Current Scene
					EditorGUILayout.BeginHorizontal ();
					{
						EditorGUILayout.LabelField ("Current Scene: ", _textCol1);
						EditorGUILayout.SelectableLabel (_history.CurrentSceneName, GUILayout.Height (16));
					}
					EditorGUILayout.EndHorizontal ();

					// Back list
					EditorGUILayout.BeginHorizontal ();
					{
						_colorStack.Push (GetColor (_history.BackCount > 1));
						EditorGUILayout.LabelField ("Back History: ", _textCol1);
						_backSelected = EditorGUILayout.Popup (_backSelected, _history.GetBackStack ());
						_colorStack.Pop ();
					}
					EditorGUILayout.EndHorizontal ();

					// Forward list
					EditorGUILayout.BeginHorizontal ();
					{
						_colorStack.Push (GetColor (_history.FowardCount > 0));
						EditorGUILayout.LabelField ("Forward History: ", _textCol1);
						_forwardSelected = EditorGUILayout.Popup (_forwardSelected, _history.GetForwardStack ());
						_colorStack.Pop ();
					}
					EditorGUILayout.EndHorizontal ();
				}
				EditorGUILayout.EndVertical ();
			}
			EditorGUILayout.EndHorizontal ();

			// Check Selection
			GoToPast ();
			GoToFuture ();
		}

		#region Actions
		private void BackButtonAction ()
		{
			var item = _history.Back ();
			SceneMainPanelUtility.OpenScene (item);
		}

		private void ForwardButtonAction ()
		{
			var item = _history.Forward ();
			SceneMainPanelUtility.OpenScene (item);
		}

		private void GoToPast ()
		{
			if (_backSelected != 0 && !SceneMainPanelUtility.IsPlaying) {
				for (int i = 0; i < _backSelected; ++i) {
					_history.Back ();
				}
				SceneMainPanelUtility.OpenScene (_history.CurrentScene);
				_backSelected = 0;
			}
		}

		private void GoToFuture ()
		{
			if (_forwardSelected != 0 && !SceneMainPanelUtility.IsPlaying) {
				for (int i = 0; i <= _forwardSelected; ++i) {
					_history.Forward ();
				}
				SceneMainPanelUtility.OpenScene (_history.CurrentScene);
				_forwardSelected = 0;
			}
		}

		public void ClearHistory ()
		{
			_history.Clear ();
		}
		#endregion

		private Texture GetTexture (string path)
		{
			return _textureDatabase.GetRelativeTexture (path);
		}

		private Color GetColor (bool enabled)
		{
			if (enabled && !SceneMainPanelUtility.IsPlaying)
				return ColorPalette.HistoryArrowButton_ON;
			else
				return ColorPalette.HistoryArrowButton_OFF;
		}
	}
}

