using UnityEngine;
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	public class SceneEntityDrawer
	{
		private ColorStack _colorStack;
		private ButtonContainer _buttonContainer;
		private TextureDatabaseProvider _textureProvider;
		private int _screenShotScale = 1;

		public SceneEntityDrawer()
		{
			_colorStack = new ColorStack ();
			_buttonContainer = new ButtonContainer ("SceneEntityDrawer", true);
		}

		public void SetTextureProvider(TextureDatabaseProvider provider)
		{
			_textureProvider = provider;
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
					if (GUILayout.Button (entity.Name) && !entity.IsActive) {
						SceneMainPanelUtility.OpenScene (entity);
					}
					_colorStack.Pop ();

					// Fav
					_colorStack.Push (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
					var stopTexture = _textureProvider.GetRelativeTexture (".icons/icon_star.png");
					if (GUILayout.Button (stopTexture, GUILayout.Width (30))) {
						entity.IsFavorite = !entity.IsFavorite;
					}
					_colorStack.Pop ();

					if (_buttonContainer != null) {
						_buttonContainer.DrawButton (string.Format ("{0} Details", entity.Name), "Details", GUILayout.Width (50));
					}

					// Select
					if (GUILayout.Button ("Select", GUILayout.Width (50))) {
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
					var cameraTexture = _textureProvider.GetRelativeTexture (".icons/icon_camera.png");
					if (GUILayout.Button (new GUIContent(cameraTexture, "Take a screenshot of the current scene. The size is the same as the Game View Panel"), GUILayout.MaxWidth(128))) {
						if (entity.IsActive) {
							SceneMainPanelUtility.TakeSnapshot (entity, _screenShotScale);
						}
					}
					_colorStack.Pop ();

					// Refresh
					_colorStack.Push ((texture != null) ? ColorPalette.SnapshotRefreshButton_ON : ColorPalette.SnapshotRefreshButton_OFF);
					if (GUILayout.Button ("Refresh", GUILayout.MaxWidth(128))) {
						if(texture != null)
							texture = GetTexture (entity, true);
					}
					_colorStack.Pop ();

					// Open
					_colorStack.Push ((texture != null) ? ColorPalette.SnapshotOpenButton_ON : ColorPalette.SnapshotOpenButton_OFF);
					if (GUILayout.Button ("Open Folder", GUILayout.MaxWidth(128))) {
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
		private Texture GetTexture(ISceneEntity entity, bool refresh)
		{
			if (_textureProvider == null)
				return null;
			return _textureProvider.GetTexture (entity.ScreenshotPath, refresh);
		}
		#endregion
	}
}

