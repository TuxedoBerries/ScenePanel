/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 28, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.Constants;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Gameplay controls drawer.
	/// Draws the gameplay controls.
	/// </summary>
	public class GameplayControlsDrawer : BaseDrawer
	{
		private bool _hittedPlay = false;
		private bool _performStep = false;

		public GameplayControlsDrawer () : base ()
		{
			_hittedPlay = false;
		}

		/// <summary>
		/// Draws the general controls.
		/// - Play from start
		/// - Play
		/// - Pause
		/// - Stop
		/// </summary>
		public void DrawGeneralControls ()
		{
			EditorGUILayout.BeginVertical ();
			{
				EditorGUILayout.BeginHorizontal ();
				{
					// Play From Start
					_colorStack.Push (GetPlayFromStartButtonColor ());
					if (GUILayout.Button (GetContent (IconSet.PLAY_START_ICON, TooltipSet.PLAY_START_TOOLTIP))) {
						if (!IsPlaying && SceneMainPanelUtility.OpenFirstScene ()) {
							_hittedPlay = true;
							EditorApplication.isPlaying = true;
						}
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.BeginHorizontal ();
				{
					if (!IsPlaying) {
						// Play Current
						_colorStack.Push (GetPlayButtonColor ());
						if (GUILayout.Button (GetContent (IconSet.PLAY_ICON, TooltipSet.PLAY_TOOLTIP))) {
							_hittedPlay = true;
							EditorApplication.isPlaying = true;
						}
						_colorStack.Pop ();
					} else {
						// Stop
						_colorStack.Push (GetStopButtonColor ());
						if (GUILayout.Button (GetContent (IconSet.STOP_ICON, TooltipSet.STOP_TOOLTIP))) {
							EditorApplication.isPlaying = false;
						}
						_colorStack.Pop ();
					}

					// Pause
					_colorStack.Push (GetPauseButtonColor ());
					if (GUILayout.Button (GetContent (IconSet.PAUSE_ICON, TooltipSet.PAUSE_TOOLTIP))) {
						EditorApplication.isPaused = !EditorApplication.isPaused;
					}
					_colorStack.Pop ();

					// Step
					_colorStack.Push (GetStepButtonColor ());
					if (GUILayout.Button (GetContent (IconSet.STEP_ICON, TooltipSet.STEP_TOOLTIP))) {
						if (IsPlaying)
							_performStep = true;
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical ();

			// Perform at the end
			if (_performStep) {
				EditorApplication.Step ();
				EditorApplication.isPaused = true;
				_performStep = false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the editor is in play mode.
		/// </summary>
		/// <value><c>true</c> if the game is playing; otherwise, <c>false</c>.</value>
		public bool IsPlaying {
			get {
				return _hittedPlay || EditorApplication.isPlaying || Application.isPlaying;
			}
		}

		#region helpers
		private GUIContent GetContent (string texture, string tooltip)
		{
			if (!_contentCache.Contains (texture)) {
				_contentCache [texture] = new GUIContent (_textureDatabase.GetRelativeTexture (texture), tooltip);
			}

			return _contentCache [texture];
		}

		private Color GetPlayFromStartButtonColor ()
		{
			return !IsPlaying ? ColorPalette.PlayButton_ON : ColorPalette.PlayButton_OFF;
		}

		private Color GetPlayButtonColor ()
		{
			return !IsPlaying ? ColorPalette.PlayButton_ON : ColorPalette.PlayButton_OFF;
		}

		private Color GetPauseButtonColor ()
		{
			if (!IsPlaying)
				return ColorPalette.PauseButton_OFF;

			return EditorApplication.isPaused ? ColorPalette.PauseButton_HOLD : ColorPalette.PauseButton_ON;
		}

		private Color GetStopButtonColor ()
		{
			return IsPlaying ? ColorPalette.StopButton_ON : ColorPalette.StopButton_OFF;
		}

		private Color GetStepButtonColor ()
		{
			return IsPlaying ? ColorPalette.StepButton_ON : ColorPalette.StepButton_OFF;
		}
		#endregion
	}
}

