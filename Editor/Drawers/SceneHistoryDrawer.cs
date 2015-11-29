/// ------------------------------------------------
/// <summary>
/// Scene History Drawer
/// Purpose: 	Draws the history of the scenes opened.
/// Author:		Juan Silva
/// Date: 		November 28, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.PreferenceHandler;
using TuxedoBerries.ScenePanel.Constants;

namespace TuxedoBerries.ScenePanel.Drawers
{
	public class SceneHistoryDrawer
	{
		private const string CLASS_NAME = "SceneHistoryDrawer";
		private const string RESTORE_VAR = "restoreScene";
		private ColorStack _colorStack;
		private SceneHistory _history;
		private IPreferenceChannel _channel;
		private TextureDatabaseProvider _textureProvider;

		private int _backSelected = 0;
		private int _forwardSelected = 0;
		private bool _restoreOnStop = true;
		private bool _justLoaded = true;

		public SceneHistoryDrawer ()
		{
			_history = new SceneHistory ();
			_history.Load ();

			_channel = EditorPreferenceHandler.GetChannel (this, CLASS_NAME);
			_restoreOnStop = _channel.GetBool (RESTORE_VAR);

			_colorStack = new ColorStack ();
			_textureProvider = new TextureDatabaseProvider ();
		}

		/// <summary>
		/// Updates the current history.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void UpdateCurrentHistory(ISceneEntity entity)
		{
			_history.Push (entity);
		}

		/// <summary>
		/// Restores from play.
		/// </summary>
		public void RestoreFromPlay()
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
		/// Draws the history.
		/// </summary>
		public void DrawHistory()
		{
			_colorStack.Reset ();

			_restoreOnStop = EditorGUILayout.Toggle ("Restore Scene On Stop", _restoreOnStop);
			_channel.SetValue (RESTORE_VAR, _restoreOnStop);

			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Space (20);
				EditorGUILayout.BeginVertical (GUILayout.Width(90));
				{
					EditorGUILayout.BeginHorizontal ();
					{
						_colorStack.Push ((_history.BackCount > 1) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						var arrowback = _textureProvider.GetRelativeTexture (IconSet.ARROW_BACK_ICON);
						if (GUILayout.Button (arrowback, GUILayout.Width(42))) {
							var item = _history.Back ();
							SceneMainPanelUtility.OpenScene (item);
						}
						_colorStack.Pop ();

						_colorStack.Push ((_history.FowardCount > 0) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						var arrowForward = _textureProvider.GetRelativeTexture (IconSet.ARROW_FORWARD_ICON);
						if (GUILayout.Button (arrowForward, GUILayout.Width(42))) {
							var item = _history.Forward ();
							SceneMainPanelUtility.OpenScene (item);
						}
						_colorStack.Pop ();
					}

					_colorStack.Push ((_history.Count > 1) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
					EditorGUILayout.EndHorizontal ();
					if (GUILayout.Button ("Clear History", GUILayout.Width(90))) {
						_history.Clear ();
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndVertical ();
				EditorGUILayout.BeginVertical ();
				{
					_backSelected = EditorGUILayout.Popup ("Back History: ", _backSelected, _history.GetBackStack ());
					_forwardSelected = EditorGUILayout.Popup ("Forward History: ", _forwardSelected, _history.GetForwardStack ());
				}
				EditorGUILayout.EndVertical ();

				// Check Selection
				if (_backSelected != 0) {
					for (int i = 0; i < _backSelected; ++i) {
						_history.Back ();
					}
					SceneMainPanelUtility.OpenScene (_history.CurrentScene);
					_backSelected = 0;
				}
				if (_forwardSelected != 0) {
					for (int i = 0; i <= _forwardSelected; ++i) {
						_history.Forward ();
					}
					SceneMainPanelUtility.OpenScene (_history.CurrentScene);
					_forwardSelected = 0;
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
}

