/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif
using System.IO;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene main panel utility.
	/// </summary>
	public class SceneMainPanelUtility
	{
		/// <summary>
		/// Opens the scene.
		/// </summary>
		/// <returns><c>true</c>, if scene was opened, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public static bool OpenScene (ISceneFileEntity entity)
		{
			if (entity == null)
				return false;

			return OpenScene (entity.FullPath);
		}

		/// <summary>
		/// Opens the first scene.
		/// </summary>
		/// <returns><c>true</c>, if first scene was opened, <c>false</c> otherwise.</returns>
		public static bool OpenFirstScene ()
		{
			var scenes = EditorBuildSettings.scenes;
			if (scenes == null)
				return false;
			if (scenes.Length <= 0)
				return false;
			return OpenScene (scenes [0].path);
		}

		/// <summary>
		/// Opens the scene in editor.
		/// </summary>
		/// <returns><c>true</c>, if scene was opened, <c>false</c> otherwise.</returns>
		/// <param name="scene">Scene.</param>
		public static bool OpenScene (string scene)
		{
			if (string.IsNullOrEmpty (scene))
				return false;

			var currentScene = CurrentActiveScene;
			if (string.Equals (scene, currentScene))
				return true;

			#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			bool saved = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
			#else
			bool saved = EditorApplication.SaveCurrentSceneIfUserWantsTo ();
			#endif
			if (saved) {
				#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				EditorSceneManager.OpenScene (scene);
				#else
				EditorApplication.OpenScene (scene);
				#endif
			}
			return saved;
		}

		public static string CurrentActiveScene {
			get {
				#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				var currentScene = EditorSceneManager.GetActiveScene ().path;
				#else
				var currentScene = EditorApplication.currentScene;
				#endif
				return currentScene;
			}
		}

		public static string TakeSnapshot (string path, int scale)
		{
			return TakeSnapshot (path, scale, "", "screenshot.png");
		}

		/// <summary>
		/// Takes a snapshot of the current visible screen and save it.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public static string TakeSnapshot (string path, int scale, string suggestedFolder, string suggestedName)
		{
			// Ask for path
			string givenPath = path;
			if (string.IsNullOrEmpty (givenPath)) {
				givenPath = EditorUtility.SaveFilePanel ("Save screenshot", suggestedFolder, suggestedName, "png");
			}
			// Still null
			if (string.IsNullOrEmpty (givenPath))
				return givenPath;

			EnsureDirectory (System.IO.Path.GetDirectoryName (givenPath));
			Application.CaptureScreenshot (givenPath, scale);
			EditorApplication.ExecuteMenuItem ("Window/Game");
			return givenPath;
		}

		/// <summary>
		/// Ensures the existance of a directory.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void EnsureDirectory (string path)
		{
			if (string.IsNullOrEmpty (path))
				return;
			Directory.CreateDirectory (path);
		}

		/// <summary>
		/// Check if a file Exists.
		/// </summary>
		/// <returns><c>true</c>, if file was existed, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public static bool ExistFile (string path)
		{
			return System.IO.File.Exists (path);
		}

		/// <summary>
		/// Deletes the file if exist.
		/// </summary>
		/// <returns><c>true</c>, if file if exist was deleted, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public static bool DeleteFileIfExist (string path)
		{
			var exist = System.IO.File.Exists (path);
			if (!exist)
				return false;
			System.IO.File.Delete (path);
			return true;
		}

		/// <summary>
		/// Saves a text into a file.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="path">Path.</param>
		public static void SaveText (string text, string path)
		{
			if (string.IsNullOrEmpty (path))
				return;
			if (string.IsNullOrEmpty (text)) {
				Debug.LogWarning ("Data to save is null or empty");
				return;
			}

			bool saved = false;
			try {
				System.IO.File.WriteAllText (path, text);
				saved = true;
			} catch (System.Exception e) {
				Debug.LogErrorFormat ("Exception trying to write file: {0}", e.Message);
			}
			if (saved) {
				EditorUtility.DisplayDialog ("Scene list", "File successfully saved", "ok");
			}
		}

		public static bool IsPlaying {
			get {
				return EditorApplication.isPlaying || Application.isPlaying;
			}
		}

		/// <summary>
		/// Gets the size of the Game View screen.
		/// </summary>
		/// <returns>The game view size.</returns>
		public static Vector2 GetGameViewSize ()
		{
			return Handles.GetMainGameViewSize ();
		}
	}
}

