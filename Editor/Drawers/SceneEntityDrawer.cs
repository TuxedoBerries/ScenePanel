/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using TuxedoBerries.ScenePanel.Constants;
using TuxedoBerries.ScenePanel.PreferenceHandler;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Scene entity drawer.
	/// Draws everything related with Scene Entity.
	/// </summary>
	public class SceneEntityDrawer : BaseDrawer
	{
		private ButtonContainer _buttonContainer;
		private GUILayoutOption _column1;
		private IPreferenceChannel _channel;
		private bool _enableEditing = false;

		public SceneEntityDrawer() : this("SceneEntityDrawer")
		{
		}

		public SceneEntityDrawer(string name) : base()
		{
			_buttonContainer = new ButtonContainer (name, true);
			_column1 = GUILayout.Width (128);

			_channel = EditorPreferenceHandler.GetChannel (this, name);
			_enableEditing = _channel.GetBool ("edit");
		}

		public bool EnableEditing {
			get {
				return _enableEditing;
			}
			set {
				_enableEditing = value;
				_channel.SetValue ("edit", value);
			}
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
					DrawOpenButton (entity);

					// Fav
					DrawFavoriteButton (entity);

					// Build
					DrawBuildButton (entity);

					// Enable
					DrawEnable (entity);

					// Index
					DrawBuildNumber (entity);

					// Detail
					if (_buttonContainer != null) {
						_buttonContainer.DrawButton (string.Format ("{0} Details", entity.Name), GetContent("Details", TooltipSet.DETAIL_BUTTON_TOOLTIP), GUILayout.Width (50));
					}

					// Select
					DrawSelectButton(entity);
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

		/// <summary>
		/// Check if a scene is open for details
		/// </summary>
		/// <returns><c>true</c>, if details open was ared, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
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
				// Select
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Select in Project:", _column1);
					DrawSelectButton (entity);
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("In Build:", _column1);
					_colorStack.Push (ColorPalette.GetEditColor (_enableEditing));
					if (_enableEditing) {
						entity.InBuild = EditorGUILayout.Toggle (entity.InBuild);
					} else {
						EditorGUILayout.Toggle (entity.InBuild);
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndHorizontal ();

				_colorStack.Push (entity.InBuild ? ColorPalette.InBuildField_ON : ColorPalette.InBuildField_OFF);
				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Enabled:", _column1);
					_colorStack.Push (ColorPalette.GetEditColor (entity.InBuild && _enableEditing));
					if (entity.InBuild && _enableEditing) {
						entity.IsEnabled = EditorGUILayout.Toggle (entity.IsEnabled);
					} else {
						EditorGUILayout.Toggle (entity.IsEnabled);
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndHorizontal ();

				// In Build index
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Build Index:", _column1);
					if (entity.InBuild && _enableEditing) {
						entity.BuildIndex = EditorGUILayout.IntField (entity.BuildIndex);
					} else {
						EditorGUILayout.LabelField (entity.BuildIndex.ToString());
					}
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

		#region Buttons
		/// <summary>
		/// Draws the open button.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawOpenButton(ISceneEntity entity)
		{
			_colorStack.Push (ColorPalette.GetColor(entity));
			GUI.skin.button.alignment = TextAnchor.MiddleLeft;
			if (GUILayout.Button (GetContent(entity)) && !entity.IsActive) {
				SceneMainPanelUtility.OpenScene (entity);
			}
			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			_colorStack.Pop ();
		}

		/// <summary>
		/// Draws the select button.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawSelectButton(ISceneEntity entity)
		{
			if (GUILayout.Button (GetContent("Select", TooltipSet.SELECT_BUTTON_TOOLTIP), GUILayout.Width (47))) {
				Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (entity.FullPath);
				EditorGUIUtility.PingObject (Selection.activeObject);
			}
		}

		/// <summary>
		/// Draws the favorite button.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawFavoriteButton(ISceneEntity entity)
		{
			_colorStack.Push (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
			if (GUILayout.Button (GetContentIcon(IconSet.STAR_ICON, TooltipSet.FAVORITE_BUTTON_TOOLTIP), GUILayout.Width (25), GUILayout.Height (18))) {
				entity.IsFavorite = !entity.IsFavorite;
			}
			_colorStack.Pop ();
		}

		/// <summary>
		/// Draws the build button.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawBuildButton(ISceneEntity entity)
		{
			if (!_enableEditing)
				return;
			
			_colorStack.Push (ColorPalette.GetColor(entity));
			// Build
			if (GUILayout.Button (GetContentIcon(IconSet.PACKAGE_ICON, TooltipSet.FAVORITE_BUTTON_TOOLTIP), GUILayout.Width (30), GUILayout.Height (18))) {
				entity.InBuild = !entity.InBuild;
			}
			_colorStack.Pop ();
		}

		/// <summary>
		/// Draws the enable.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawEnable(ISceneEntity entity)
		{
			if (!_enableEditing)
				return;

			if (entity.InBuild) {
				entity.IsEnabled = EditorGUILayout.Toggle (entity.IsEnabled, GUILayout.Width (15));
			} else {
				EditorGUILayout.LabelField ("", GUILayout.Width (15));
			}
		}

		/// <summary>
		/// Draws the build number.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void DrawBuildNumber(ISceneEntity entity)
		{
			if (!_enableEditing)
				return;

			if (entity.InBuild) {
				entity.BuildIndex = EditorGUILayout.IntField (entity.BuildIndex, GUILayout.Width (25), GUILayout.Height (20));
			} else {
				EditorGUILayout.LabelField ("", GUILayout.Width (25));
			}
		}
		#endregion

		#region Helpers
		private GUIContent GetContent(ISceneEntity scene)
		{
			string keyName = "";
			if (scene.InBuild) {
				keyName = string.Format ("[{0:00}] {1}", scene.BuildIndex, scene.Name);
			} else {
				keyName = string.Format ("        {0}", scene.Name);
			}

			if(!_contentCache.Contains(keyName)){
				var tooltip = string.Format(TooltipSet.SCENE_BUTTON_TOOLTIP, scene.Name);
				return _contentCache.GetContent (keyName, tooltip);
			}
			return _contentCache[keyName];
		}

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
		#endregion
	}
}

