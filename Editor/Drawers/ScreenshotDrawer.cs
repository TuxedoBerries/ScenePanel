/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.Constants;
using TuxedoBerries.ScenePanel.Controllers;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Screenshot drawer.
	/// Draws the screenshot tool.
	/// </summary>
	public class ScreenshotDrawer : BaseDrawer
	{
		private GUILayoutOption _column1;
		private ScrollableContainer _scrolls;
		private int _screenShotScale = 1;
		private ScreenshotHistory _history;

		public ScreenshotDrawer () : base()
		{
			_column1 = GUILayout.Width (128);
			_history = new ScreenshotHistory ();
			_history.Load ();
			_scrolls = new ScrollableContainer ();
		}

		#region Last screenshot
		/// <summary>
		/// Draws the configuration, controls and previews for a screenshot using the last screenshot taken.
		/// </summary>
		/// <returns>The full.</returns>
		public void DrawFull()
		{
			bool refresh = false;
			bool deleteAll = false;
			EditorGUILayout.BeginVertical ();
			{
				DrawConfiguration ();
				EditorGUILayout.Space ();
				var givenName = string.Format ("screenshot_{0:000}.png", _history.Count);
				var givenPath = string.Format ("Screenshots/{0}",givenName);
				EditorGUILayout.LabelField (string.Format("Current Screenshot: {0}", givenPath));
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.BeginVertical (_column1);
					{
						// Take Snapshot
						DrawTakeScreenshotButton(givenPath, true, "Screenshots", givenName);

						// Refresh
						DrawRefreshScreenshotButton (_history.Current);

						// Open
						DrawOpenFolderButton(_history.Current);
					}
					EditorGUILayout.EndVertical ();

					EditorGUILayout.BeginVertical (_column1);
					{
						DrawPreview (_history.Current);
					}
					EditorGUILayout.EndVertical ();

					var exist = GetTexture (givenPath, false);
					if(exist != null)
						_history.Push (givenPath);
				}
				EditorGUILayout.EndHorizontal ();

				// History
				EditorGUILayout.LabelField (string.Format("Screenshots Taken: {0}", _history.Count));
				EditorGUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("Refresh")) {
						refresh = true;
					}
					if (GUILayout.Button ("Delete All")) {
						deleteAll = true;
					}
				}
				EditorGUILayout.EndHorizontal ();

				_scrolls.DrawScrollable ("Saved Screenshots", DrawScreenshots);
			}
			EditorGUILayout.EndVertical ();

			if (refresh)
				RefreshScreenshots ();
			if (deleteAll)
				DeleteAllGeneralScreenshots ();
		}

		private void DrawScreenshots()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				var ienum = _history.History;
				while(ienum.MoveNext()){
					DrawPreview (ienum.Current);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void RefreshScreenshots()
		{
			_history.AutoSave = false;
			// Add to a new stack
			var tempStack = new Stack<string> (_history.Count);
			while (_history.Count > 0) {
				tempStack.Push (_history.Pop ());
			}
			// Clear cache
			_textureDatabase.Clear ();

			// Add only existant
			var ienum = tempStack.GetEnumerator();
			while(ienum.MoveNext()){
				var exist = GetTexture (ienum.Current, true);
				if (exist != null) {
					_history.Push (ienum.Current);
				}
			}
			_history.AutoSave = true;
			_history.Save ();
		}

		private void DeleteAllGeneralScreenshots()
		{
			_history.AutoSave = false;
			while (_history.Count > 0) {
				var screenshot = _history.Pop ();
				SceneMainPanelUtility.DeleteFileIfExist (screenshot);
			}
			_history.Clear ();
			_textureDatabase.Clear ();
			_history.AutoSave = true;
			_history.Save ();
		}
		#endregion

		#region Specific screenshot
		/// <summary>
		/// Draws the snapshot section of the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawSnapshot(ISceneEntity entity)
		{
			EditorGUILayout.Space ();
			DrawConfiguration ();
			EditorGUILayout.BeginHorizontal ();
			{
				entity.ScreenshotPath = DrawControls (entity.ScreenshotPath, entity.IsActive || SceneMainPanelUtility.IsPlaying, "Screenshots", string.Format("{0}.png", entity.Name));
				DrawPreview (entity.ScreenshotPath);
			}
			EditorGUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Draws the configuration.
		/// </summary>
		public void DrawConfiguration()
		{
			// Show current view size
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Current View Size: ", _column1);
				var size = SceneMainPanelUtility.GetGameViewSize ();
				EditorGUILayout.LabelField (string.Format ("{0} x {1}", size.x, size.y));
			}
			EditorGUILayout.EndHorizontal ();

			// Show current scale
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Screenshot Scale: ", _column1);
				_screenShotScale = EditorGUILayout.IntSlider (_screenShotScale, 1, 10);
			}
			EditorGUILayout.EndHorizontal ();

			// Show estimated size
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Estimated Size: ", _column1);
				var size = SceneMainPanelUtility.GetGameViewSize ();
				EditorGUILayout.LabelField (string.Format ("{0} x {1}", size.x * _screenShotScale, size.y * _screenShotScale));
			}
			EditorGUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Draws the controls for a screenshot.
		/// </summary>
		/// <returns>The controls.</returns>
		/// <param name="dataPath">Data path.</param>
		/// <param name="enableShot">If set to <c>true</c> enable shot.</param>
		public string DrawControls(string dataPath, bool enableShot)
		{
			return DrawControls (dataPath, enableShot, "", "screenshot.png");
		}

		/// <summary>
		/// Draws the controls for a screenshot.
		/// </summary>
		/// <returns>The controls.</returns>
		/// <param name="dataPath">Data path.</param>
		/// <param name="enableShot">If set to <c>true</c> enable shot.</param>
		public string DrawControls(string dataPath, bool enableShot, string suggestedFolder, string suggestedName)
		{
			_colorStack.Reset ();
			
			// Display
			EditorGUILayout.BeginVertical (_column1);
			{
				// Take Snapshot
				dataPath = DrawTakeScreenshotButton(dataPath, enableShot, suggestedFolder, suggestedName);

				// Refresh
				dataPath = DrawRefreshScreenshotButton (dataPath);

				// Open
				DrawOpenFolderButton(dataPath);

			}
			EditorGUILayout.EndVertical ();
			return dataPath;
		}

		private string DrawTakeScreenshotButton(string dataPath, bool enableShot, string suggestedFolder, string suggestedName)
		{
			// Take Snapshot
			_colorStack.Push (enableShot ? ColorPalette.SnapshotButton_ON : ColorPalette.SnapshotButton_OFF);
			if (GUILayout.Button (GetContentIcon(IconSet.CAMERA_ICON, TooltipSet.SCREENSHOT_BUTTON_TOOLTIP), _column1)) {
				if (enableShot) {
					if (string.IsNullOrEmpty (dataPath)) {
						SceneMainPanelUtility.EnsureDirectory (suggestedFolder);
					}
					dataPath = SceneMainPanelUtility.TakeSnapshot (dataPath, _screenShotScale, suggestedFolder, suggestedName);
				}
			}
			_colorStack.Pop ();
			return dataPath;
		}

		private string DrawRefreshScreenshotButton(string dataPath)
		{
			var isCached = _textureDatabase.isCached (dataPath);
			// Refresh
			_colorStack.Push ((isCached) ? ColorPalette.SnapshotRefreshButton_ON : ColorPalette.SnapshotRefreshButton_OFF);
			if (GUILayout.Button (GetContent("Refresh", TooltipSet.SCREENSHOT_REFRESH_BUTTON_TOOLTIP), _column1)) {
				var exist = _textureDatabase.RefreshCache (dataPath);
				if (!exist)
					dataPath = "";
			}
			_colorStack.Pop ();

			return dataPath;
		}

		private void DrawOpenFolderButton(string dataPath)
		{
			bool validPath = SceneMainPanelUtility.ExistFile(dataPath);

			_colorStack.Push ((validPath) ? ColorPalette.SnapshotOpenButton_ON : ColorPalette.SnapshotOpenButton_OFF);
			if (GUILayout.Button (GetContent("Open Folder", TooltipSet.SCREENSHOT_OPEN_FOLDER_BUTTON_TOOLTIP), _column1)) {
				if (validPath) {
					EditorUtility.RevealInFinder (dataPath);
				}
			}
			_colorStack.Pop ();
		}

		/// <summary>
		/// Draws the preview screenshot given the image path.
		/// </summary>
		/// <param name="dataPath">Data path.</param>
		public bool DrawPreview(string dataPath)
		{
			var texture = GetTexture (dataPath, false);
			EditorGUILayout.BeginVertical ();
			{
				// Get Texture
				if (texture != null) {
					GUILayout.Label (texture, GUILayout.MaxWidth (MaxWidth (texture)), GUILayout.MaxHeight (MaxHeight (texture)));
				} else {
					EditorGUILayout.LabelField ("[ Empty Screenshot ]", _column1);
				}

				// In Build Enabled Check
				if (!string.IsNullOrEmpty (dataPath)) {
					EditorGUILayout.LabelField (System.IO.Path.GetFileName (dataPath), _column1);
				} else {
					EditorGUILayout.LabelField ("--");
				}
				EditorGUILayout.LabelField ("Screenshot Size: ", _column1);
				string displayText;
				if (texture != null) {
					displayText = string.Format ("{0} x {1}", texture.width, texture.height);
				} else {
					displayText = "--";
				}
				EditorGUILayout.LabelField (displayText, _column1);
			}
			EditorGUILayout.EndVertical ();
			return texture != null;
		}
		#endregion

		#region Helpers
		private GUIContent GetContent(string label, string tooltip)
		{
			return _contentCache.GetContent (label, tooltip);
		}

		private GUIContent GetContentIcon(string iconName, string tooltip)
		{
			if(!_contentCache.Contains(iconName)){
				var texture = _textureDatabase.GetRelativeTexture (iconName);
				_contentCache [iconName] = new GUIContent (texture, tooltip);
			}
			return _contentCache[iconName];
		}

		private Texture GetTexture(string path, bool refresh)
		{
			if (_textureDatabase == null)
				return null;
			if (string.IsNullOrEmpty (path))
				return null;
			return _textureDatabase.GetTexture (path, refresh);
		}

		private float MaxWidth(Texture texture)
		{
			if (texture == null)
				return 0;

			float scale = 128f / (Mathf.Max(texture.width, texture.height) * 1f);
			return scale * texture.width;
		}

		private float MaxHeight(Texture texture)
		{
			if (texture == null)
				return 0;

			float scale = 128f / (Mathf.Max(texture.width, texture.height) * 1f);
			return scale * texture.height;
		}
		#endregion
	}
}

