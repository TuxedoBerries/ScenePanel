/// ------------------------------------------------
/// <summary>
/// Scene Entity Drawer
/// Purpose: 	Draws everything related with Scene Entity.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.Constants;

namespace TuxedoBerries.ScenePanel.Drawers
{
	public class SceneEntityDrawer
	{
		private ColorStack _colorStack;
		private ButtonContainer _buttonContainer;
		private TextureDatabaseProvider _textureProvider;
		private GUIContentCache _contentCache;
		private int _screenShotScale = 1;

		public SceneEntityDrawer()
		{
			_colorStack = new ColorStack ();
			_buttonContainer = new ButtonContainer ("SceneEntityDrawer", true);
			_textureProvider = new TextureDatabaseProvider ();
			_contentCache = new GUIContentCache ();
		}

		/// <summary>
		/// Draws the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawEntity(ISceneEntity entity)
		{
			_colorStack.Reset ();
			EditorGUILayout.BeginVertical ();
			{
				// Row 1
				EditorGUILayout.BeginHorizontal ();
				{
					// Open
					_colorStack.Push (SceneMainPanelUtility.GetColor(entity));
					if (GUILayout.Button (GetContent(entity)) && !entity.IsActive) {
						SceneMainPanelUtility.OpenScene (entity);
					}
					_colorStack.Pop ();

					// Fav
					_colorStack.Push (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
					if (GUILayout.Button (GetContentIcon(IconSet.STAR_ICON, TooltipSet.FAVORITE_BUTTON_TOOLTIP), GUILayout.Width (30))) {
						entity.IsFavorite = !entity.IsFavorite;
					}
					_colorStack.Pop ();

					// Detail
					if (_buttonContainer != null) {
						_buttonContainer.DrawButton (string.Format ("{0} Details", entity.Name), GetContent("Details", TooltipSet.DETAIL_BUTTON_TOOLTIP), GUILayout.Width (50));
					}

					// Select
					if (GUILayout.Button (GetContent("Select", TooltipSet.SELECT_BUTTON_TOOLTIP), GUILayout.Width (50))) {
						Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (entity.FullPath);
						EditorGUIUtility.PingObject (Selection.activeObject);
					}
				}
				EditorGUILayout.EndHorizontal ();

				// Row 2 - More
				if (_buttonContainer != null) {
					_buttonContainer.DrawContent (string.Format ("{0} Details", entity.Name), DrawDetailEntity, entity);
				} else {
					DrawDetailEntity (entity);
				}
			}
			EditorGUILayout.EndVertical ();
		}

		/// <summary>
		/// Draws the detail of the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawDetailEntity(ISceneEntity entity)
		{
			var col1Space = GUILayout.Width (128);
			EditorGUILayout.BeginVertical ();
			{
				// Name
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Name:", col1Space);
					EditorGUILayout.SelectableLabel (entity.Name, GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// Path
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Path:", col1Space);
					EditorGUILayout.SelectableLabel (entity.FullPath, GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// Path
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("GUID:", col1Space);
					EditorGUILayout.SelectableLabel (entity.GUID.ToUpper(), GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("In Build:", col1Space);
					EditorGUILayout.Toggle (entity.InBuild);
				}
				EditorGUILayout.EndHorizontal ();

				_colorStack.Push (entity.InBuild ? ColorPalette.InBuildField_ON : ColorPalette.InBuildField_OFF);
				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Enabled:", col1Space);
					EditorGUILayout.Toggle (entity.IsEnabled);
				}
				EditorGUILayout.EndHorizontal ();

				// In Build index
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Index:", col1Space);
					EditorGUILayout.LabelField (entity.BuildIndex.ToString());
				}
				EditorGUILayout.EndHorizontal ();
				_colorStack.Pop ();

				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Current Scene:", col1Space);
					EditorGUILayout.Toggle (entity.IsActive);
				}
				EditorGUILayout.EndHorizontal ();

				// Snapshot
				DrawSnapshot(entity);
			}
			EditorGUILayout.EndVertical ();
		}

		/// <summary>
		/// Draws the snapshot section of the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawSnapshot(ISceneEntity entity)
		{
			_colorStack.Reset ();
			var texture = GetTexture (entity, false);

			var col1Space = GUILayout.Width (128);
			// In Build Enabled Check
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Screenshot: ", col1Space);
				string displayText;
				if (texture != null) {
					displayText = entity.ScreenshotPath;
				} else {
					displayText = "--";
				}
				EditorGUILayout.SelectableLabel (displayText, GUILayout.Height(16));
			}
			EditorGUILayout.EndHorizontal ();

			// In Build Enabled Check
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Screenshot Size: ", col1Space);
				string displayText;
				if (texture != null) {
					displayText = string.Format ("{0} x {1}", texture.width, texture.height);
				} else {
					displayText = "--";
				}
				EditorGUILayout.LabelField (displayText);
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			if (entity.IsActive) {
				// Show current view size
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Current View Size: ", col1Space);
					var size = SceneMainPanelUtility.GetGameViewSize ();
					EditorGUILayout.LabelField (string.Format ("{0} x {1}", size.x, size.y));
				}
				EditorGUILayout.EndHorizontal ();

				// Show current scale
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Screenshot Scale: ", col1Space);
					_screenShotScale = EditorGUILayout.IntSlider (_screenShotScale, 1, 10);
				}
				EditorGUILayout.EndHorizontal ();

				// Show current scale
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Estimated Size: ", col1Space);
					var size = SceneMainPanelUtility.GetGameViewSize ();
					EditorGUILayout.LabelField (string.Format ("{0} x {1}", size.x * _screenShotScale, size.y * _screenShotScale));
				}
				EditorGUILayout.EndHorizontal ();
			}

			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Space (20);
				// Display
				EditorGUILayout.BeginVertical (col1Space);
				{
					// Take Snapshot
					_colorStack.Push (entity.IsActive ? ColorPalette.SnapshotButton_ON : ColorPalette.SnapshotButton_OFF);
					if (GUILayout.Button (GetContentIcon(IconSet.CAMERA_ICON, TooltipSet.SCREENSHOT_BUTTON_TOOLTIP), GUILayout.MaxWidth(128))) {
						if (entity.IsActive) {
							SceneMainPanelUtility.TakeSnapshot (entity, _screenShotScale);
						}
					}
					_colorStack.Pop ();

					// Refresh
					_colorStack.Push ((texture != null) ? ColorPalette.SnapshotRefreshButton_ON : ColorPalette.SnapshotRefreshButton_OFF);
					if (GUILayout.Button (GetContent("Refresh", TooltipSet.SCREENSHOT_REFRESH_BUTTON_TOOLTIP), GUILayout.MaxWidth(128))) {
						if(texture != null)
							texture = GetTexture (entity, true);
					}
					_colorStack.Pop ();

					// Open
					_colorStack.Push ((texture != null) ? ColorPalette.SnapshotOpenButton_ON : ColorPalette.SnapshotOpenButton_OFF);
					if (GUILayout.Button (GetContent("Open Folder", TooltipSet.SCREENSHOT_OPEN_FOLDER_BUTTON_TOOLTIP), GUILayout.MaxWidth(128))) {
						if(texture != null)
							EditorUtility.RevealInFinder (entity.ScreenshotPath);
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndVertical ();

				EditorGUILayout.BeginVertical (col1Space);
				{
					if (texture != null) {
						GUILayout.Label (texture, GUILayout.Height(128), GUILayout.MaxWidth(128));
					} else {
						EditorGUILayout.LabelField ("Empty Screenshot", col1Space);
					}
				}
				EditorGUILayout.EndVertical ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		#region Helpers
		private GUIContent GetContent(ISceneEntity scene)
		{
			if(!_contentCache.Contains(scene.Name)){
				var tooltip = string.Format(TooltipSet.SCENE_BUTTON_TOOLTIP, scene.Name);
				return _contentCache.GetContent (scene.Name, tooltip);
			}
			return _contentCache[scene.Name];
		}

		private GUIContent GetContent(string label, string tooltip)
		{
			return _contentCache.GetContent (label, tooltip);
		}

		private GUIContent GetContentIcon(string iconName, string tooltip)
		{
			if(!_contentCache.Contains(iconName)){
				var texture = _textureProvider.GetRelativeTexture (iconName);
				_contentCache [iconName] = new GUIContent (texture, tooltip);
			}
			return _contentCache[iconName];
		}

		private Texture GetTexture(ISceneEntity entity, bool refresh)
		{
			if (_textureProvider == null)
				return null;
			return _textureProvider.GetTexture (entity.ScreenshotPath, refresh);
		}
		#endregion
	}
}

