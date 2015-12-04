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
		private GUILayoutOption _column1;

		public SceneEntityDrawer() : this("SceneEntityDrawer")
		{
		}

		public SceneEntityDrawer(string name)
		{
			_colorStack = new ColorStack ();
			_buttonContainer = new ButtonContainer (name, true);
			_contentCache = new GUIContentCache ();
			_textureProvider = new TextureDatabaseProvider ();
			_column1 = GUILayout.Width (128);
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
					if (entity.IsActive) {
						GUILayout.Label (GetContentIcon (IconSet.PLAY_ICON, "Current Scene"), GUILayout.Width (18), GUILayout.Height (18));
					} else {
						GUILayout.Space (26);
					}

					// Open
					_colorStack.Push (SceneMainPanelUtility.GetColor(entity));
					if (GUILayout.Button (GetContent(entity)) && !entity.IsActive) {
						SceneMainPanelUtility.OpenScene (entity);
					}
					_colorStack.Pop ();

					// Build Index

					// Fav
					_colorStack.Push (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
					if (GUILayout.Button (GetContentIcon(IconSet.STAR_ICON, TooltipSet.FAVORITE_BUTTON_TOOLTIP), GUILayout.Width (30), GUILayout.Height (18))) {
						entity.IsFavorite = !entity.IsFavorite;
					}
					_colorStack.Pop ();

					// Build
					_colorStack.Push (SceneMainPanelUtility.GetColor(entity));
					if (GUILayout.Button (GetContentIcon(IconSet.PACKAGE_ICON, TooltipSet.FAVORITE_BUTTON_TOOLTIP), GUILayout.Width (30), GUILayout.Height (18))) {
						entity.InBuild = !entity.InBuild;
					}
					_colorStack.Pop ();

					// Enable
					entity.IsEnabled = EditorGUILayout.Toggle (entity.IsEnabled, GUILayout.Width (15));

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
					EditorGUILayout.BeginHorizontal ();
					{
						GUILayout.Space (25);
						_buttonContainer.DrawContent (string.Format ("{0} Details", entity.Name), DrawDetailEntity, entity);
					}
					EditorGUILayout.EndHorizontal ();
				} else {
					DrawDetailEntity (entity);
				}
			}
			EditorGUILayout.EndVertical ();
		}

		public bool AreDetailsOpen(ISceneEntity entity)
		{
			return _buttonContainer.GetValue (string.Format ("{0} Details", entity.Name));
		}

		/// <summary>
		/// Draws the detail of the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawDetailEntity(ISceneEntity entity)
		{
			if (entity == null)
				entity = SceneEntity.Empty;
			
			EditorGUILayout.BeginVertical ();
			{
				// Name
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Name:", _column1);
					EditorGUILayout.SelectableLabel (entity.Name, GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// Path
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Path:", _column1);
					EditorGUILayout.SelectableLabel (entity.FullPath, GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// Path
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("GUID:", _column1);
					EditorGUILayout.SelectableLabel (entity.GUID.ToUpper(), GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("In Build:", _column1);
					entity.InBuild = EditorGUILayout.Toggle (entity.InBuild);
				}
				EditorGUILayout.EndHorizontal ();

				_colorStack.Push (entity.InBuild ? ColorPalette.InBuildField_ON : ColorPalette.InBuildField_OFF);
				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Enabled:", _column1);
					entity.IsEnabled = EditorGUILayout.Toggle (entity.IsEnabled);
				}
				EditorGUILayout.EndHorizontal ();

				// In Build index
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Index:", _column1);
					EditorGUILayout.LabelField (entity.BuildIndex.ToString());
				}
				EditorGUILayout.EndHorizontal ();
				_colorStack.Pop ();

				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Current Scene:", _column1);
					EditorGUILayout.Toggle (entity.IsActive);
				}
				EditorGUILayout.EndHorizontal ();

				// Screenshot
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Screenshot: ", _column1);
					EditorGUILayout.SelectableLabel (entity.ScreenshotPath, GUILayout.Height(16));
				}
				EditorGUILayout.EndHorizontal ();

			}
			EditorGUILayout.EndVertical ();
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
		#endregion
	}
}

