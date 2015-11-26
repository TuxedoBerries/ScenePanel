using UnityEngine;
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	public class SceneEntityDrawer
	{
		private ColorStack _colorStack;
		private ButtonContainer _buttonContainer;
		private FolderContainer _folders;
		private SceneDatabaseProvider _provider;

		public SceneEntityDrawer()
		{
		}

		public void SetColorStack(ColorStack colorStack)
		{
			_colorStack = colorStack;
		}

		public void SetButtonContainer(ButtonContainer container)
		{
			_buttonContainer = container;
		}

		public void SetFolderContainer(FolderContainer folders)
		{
			_folders = folders;
		}

		public void SetDataProvider(SceneDatabaseProvider provider)
		{
			_provider = provider;
		}

		/// <summary>
		/// Draws the entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void DrawEntity(ISceneEntity entity)
		{
			EditorGUILayout.BeginVertical ();
			{
				// Row 1
				EditorGUILayout.BeginHorizontal ();
				{
					// Open
					PushColor (SceneEntity.GetColor(entity));
					if (GUILayout.Button (entity.Name) && !entity.IsActive) {
						OpenScene (entity);
					}
					PopColor ();

					// Fav
					PushColor (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
					if (GUILayout.Button ("Fav", GUILayout.Width (30))) {
						entity.IsFavorite = !entity.IsFavorite;
					}
					PopColor ();

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
				} else if (_folders != null) {
					_folders.DrawFoldable<ISceneEntity> (string.Format ("{0} Details", entity.Name), DrawDetailEntity, entity);
				} else {
					DrawDetailEntity (entity);
				}
			}
			EditorGUILayout.EndVertical ();
		}

		private void DrawDetailEntity(ISceneEntity entity)
		{
			var col1Space = GUILayout.Width (128);
			EditorGUILayout.BeginVertical ();
			{
				// Name
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Name", col1Space);
					EditorGUILayout.SelectableLabel (entity.Name, GUILayout.Height(15));
				}
				EditorGUILayout.EndHorizontal ();
				// Path
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Path", col1Space);
					EditorGUILayout.SelectableLabel (entity.FullPath, GUILayout.Height(15));
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("In Build", col1Space);
					EditorGUILayout.Toggle (entity.InBuild);
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("In Build Enabled", col1Space);
					EditorGUILayout.Toggle (entity.IsEnabled);
				}
				EditorGUILayout.EndHorizontal ();
				// In Build Enabled Check
				EditorGUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Current Scene", col1Space);
					EditorGUILayout.Toggle (entity.IsActive);
				}
				EditorGUILayout.EndHorizontal ();

				// Snapshot
				DrawSnapshot(entity);
			}
			EditorGUILayout.EndVertical ();
		}

		private void DrawSnapshot(ISceneEntity entity)
		{
			var texture = GetTexture (entity, false);
			var buttonLabel = (texture == null) ? "Take Snapshot" : "Update Snapshot";

			var col1Space = GUILayout.Width (128);
			EditorGUILayout.LabelField ("Snapshot: ", col1Space);
			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Space (35);
				// Display
				EditorGUILayout.BeginVertical (col1Space);
				{
					// Take Snapshot
					PushColor (entity.IsActive ? ColorPalette.SnapshotButton_ON : ColorPalette.SnapshotButton_OFF);
					if (GUILayout.Button (buttonLabel, GUILayout.MaxWidth(128))) {
						if (entity.IsActive) {
							TakeSnapshot (entity);
							texture = GetTexture (entity, true);
						}
					}
					PopColor ();

					// Refresh
					PushColor ((texture != null) ? ColorPalette.SnapshotRefreshButton_ON : ColorPalette.SnapshotRefreshButton_OFF);
					if (GUILayout.Button ("Refresh", GUILayout.MaxWidth(128))) {
						if(texture != null)
							texture = GetTexture (entity, true);
					}
					PopColor ();

					// Open
					PushColor ((texture != null) ? ColorPalette.SnapshotOpenButton_ON : ColorPalette.SnapshotOpenButton_OFF);
					if (GUILayout.Button ("Open Folder", GUILayout.MaxWidth(128))) {
						if(texture != null)
							EditorUtility.RevealInFinder (entity.SnapshotPath);
					}
					PopColor ();
				}
				EditorGUILayout.EndVertical ();

				if (texture != null) {
					GUILayout.Label (texture, GUILayout.Height(128), GUILayout.MaxWidth(128));
				} else {
					EditorGUILayout.LabelField ("Empty Snapshot", col1Space);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Opens the scene in editor.
		/// </summary>
		/// <returns><c>true</c>, if scene was opened, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public static bool OpenScene(ISceneEntity entity)
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

		#region Helpers
		private void PushColor(Color color)
		{
			if (_colorStack == null)
				return;
			_colorStack.Push (color);
		}

		private void PopColor()
		{
			if (_colorStack == null)
				return;
			_colorStack.Pop ();
		}

		private Texture GetTexture(ISceneEntity entity, bool refresh)
		{
			if (_provider == null)
				return null;
			return _provider.GetTexture (entity, refresh);
		}

		private void TakeSnapshot(ISceneEntity entity)
		{
			EnsureSnapshotFolders (entity);
			Application.CaptureScreenshot (entity.SnapshotPath);
			EditorApplication.ExecuteMenuItem ("Window/Game");
		}

		private void EnsureSnapshotFolders(ISceneEntity entity)
		{
			System.IO.Directory.CreateDirectory (System.IO.Path.GetDirectoryName(entity.SnapshotPath));
		}
		#endregion
	}
}

