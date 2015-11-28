/// ------------------------------------------------
/// <summary>
/// Scene Main Panel Utility
/// Purpose: 	Utility class.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using UnityEngine;
using System.IO;

namespace TuxedoBerries.ScenePanel
{
	public class SceneMainPanelUtility
	{
		/// <summary>
		/// Opens the scene.
		/// </summary>
		/// <returns><c>true</c>, if scene was opened, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public static bool OpenScene(ISceneFileEntity entity)
		{
			if (entity == null)
				return false;

			return OpenScene(entity.FullPath);
		}

		/// <summary>
		/// Opens the scene in editor.
		/// </summary>
		/// <returns><c>true</c>, if scene was opened, <c>false</c> otherwise.</returns>
		/// <param name="scene">Scene.</param>
		public static bool OpenScene(string scene)
		{
			if (string.IsNullOrEmpty(scene))
				return false;

			bool saved = EditorApplication.SaveCurrentSceneIfUserWantsTo ();
			if (saved) {
				EditorApplication.OpenScene (scene);
			}
			return saved;
		}

		/// <summary>
		/// Takes a snapshot of the current visible screen and save it.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public static void TakeSnapshot(ISceneEntity entity, int scale)
		{
			Directory.CreateDirectory (System.IO.Path.GetDirectoryName(entity.ScreenshotPath));
			Application.CaptureScreenshot (entity.ScreenshotPath, scale);
			EditorApplication.ExecuteMenuItem ("Window/Game");
			EditorUtility.DisplayDialog ("Snapshot", "Snapshot successfully saved", "ok");
		}

		/// <summary>
		/// Gets the color representation of the given entity.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="entity">Entity.</param>
		public static Color GetColor(ISceneEntity entity)
		{
			// Active Color
			if (entity.IsActive)
				return ColorPalette.SceneOpenButton_Active;

			// Build Color
			if (entity.InBuild) {
				if (entity.IsEnabled)
					return ColorPalette.SceneOpenButton_InBuild_Enabled;
				else
					return ColorPalette.SceneOpenButton_InBuild_Disabled;
			}

			return ColorPalette.SceneOpenButton_Regular;
		}

		/// <summary>
		/// Gets the size of the Game View screen.
		/// </summary>
		/// <returns>The game view size.</returns>
		public static Vector2 GetGameViewSize()
		{
			return Handles.GetMainGameViewSize ();
		}
	}
}

