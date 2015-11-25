/// ------------------------------------------------
/// <summary>
/// Scene Main Panel
/// Purpose: 	Manages scenes in the project.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class SceneMainPanel : EditorWindow
	{
		private const float UPDATE_POINT = 1.0f;
		private SceneDatabaseProvider _provider;
		private ColorStack _colorStack;
		private FolderContainer _folders;
		private ScrollableContainer _scrolls;
		private string _search;
		private float _deltaBetweenUpdates = 0;

		private void CheckProvider()
		{
			if(_provider == null)
				_provider = new SceneDatabaseProvider ();
		}

		private void CheckGUIElements()
		{
			if (_colorStack == null)
				_colorStack = new ColorStack ();
			if (_folders == null)
				_folders = new FolderContainer ("SceneMainPanel", true);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer ("SceneMainPanel", true);

			this.titleContent.text = "Scene Panel";
			this.titleContent.tooltip = "List of the scenes in the project.";
		}

		private void UpdateCurrent()
		{
			_provider.SetAsActive (EditorApplication.currentScene);
		}

		private void Update()
		{
			// Fixed Update
			_deltaBetweenUpdates += 0.1f;
			if (_deltaBetweenUpdates >= UPDATE_POINT) {
				_deltaBetweenUpdates = 0;
				if(_provider != null)
					_provider.Refresh ();
				Repaint ();
			}
		}

		private void OnGUI()
		{
			CheckProvider ();
			CheckGUIElements ();
			UpdateCurrent ();

			_colorStack.Reset ();
			DrawTitle ();
			DrawGeneralControls ();
			DrawSearch ();
			EditorGUILayout.Space ();
			_scrolls.DrawScrollable ("main", DrawMainScroll);
		}

		private void DrawMainScroll()
		{
			_folders.DrawFoldable ("Favorites", DrawAllFavorites);
			_folders.DrawFoldable ("All Scenes In Build", DrawAllInBuild);
			_folders.DrawFoldable ("All Scenes", DrawAll);
		}

		private void DrawTitle()
		{
			EditorGUILayout.LabelField ("All the scenes in the project are displayed here.");
		}

		private void DrawGeneralControls()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				// Play
				var playColor = !EditorApplication.isPlaying ? ColorPalette.PlayButton_ON : ColorPalette.PlayButton_OFF;
				_colorStack.Push (playColor);
				if (GUILayout.Button ("Play") && !EditorApplication.isPlaying) {
					var first = _provider.FirstScene;
					if (first != null && OpenScene (first)) {
						EditorApplication.isPlaying = true;
					}
				}
				_colorStack.Pop ();

				// Stop
				var stopColor = EditorApplication.isPlaying ? ColorPalette.StopButton_ON : ColorPalette.StopButton_OFF;
				_colorStack.Push (stopColor);
				if (GUILayout.Button ("Stop") && EditorApplication.isPlaying) {
					EditorApplication.isPlaying = false;
				}
				_colorStack.Pop ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawSearch()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Filter", GUILayout.Width (50));
				_search = EditorGUILayout.TextField (_search);
				if (GUILayout.Button ("Clear")) {
					_search = "";
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		private bool PassFilter(ISceneEntity entity)
		{
			if (string.IsNullOrEmpty (_search))
				return true;
			if (entity.Name.ToLower().Contains(_search.ToLower()))
				return true;
			
			return false;
		}

		#region Lists
		private void DrawAllFavorites()
		{
			DrawIenum (_provider.GetFavorites ());
		}

		private void DrawAllInBuild()
		{
			DrawIenum (_provider.GetBuildScenes ());
		}

		private void DrawAll()
		{
			DrawIenum (_provider.GetAllScenes ());
		}

		private void DrawIenum(IEnumerator<ISceneEntity> ienum)
		{
			while (ienum.MoveNext ()) {
				var entity = ienum.Current;
				// Apply Search
				if (!PassFilter(entity))
					continue;

				EditorGUILayout.BeginHorizontal ();
				{
					// Space
					GUILayout.Space (20);
					DrawEntity (entity);
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
		#endregion

		#region Single Entity
		private void DrawEntity(ISceneEntity entity)
		{
			EditorGUILayout.BeginVertical ();
			{
				// Row 1
				EditorGUILayout.BeginHorizontal ();
				{
					// Open
					_colorStack.Push (entity.CurrentColor);
					if (GUILayout.Button (entity.Name) && !entity.IsActive) {
						OpenScene (entity);
					}
					_colorStack.Pop ();

					// Fav
					_colorStack.Push (entity.IsFavorite ? ColorPalette.FavoriteButton_ON : ColorPalette.FavoriteButton_OFF);
					if (GUILayout.Button ("Fav", GUILayout.Width (30))) {
						_provider.SetAsFavorite (entity.FullPath, !entity.IsFavorite);
					}
					_colorStack.Pop ();

					// Select
					if (GUILayout.Button ("Select", GUILayout.Width (50))) {
						Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (entity.FullPath);
						EditorGUIUtility.PingObject (Selection.activeObject);
					}
				}
				EditorGUILayout.EndHorizontal ();

				// Row 2 - More
				_folders.DrawFoldable<ISceneEntity> (string.Format ("{0} Details", entity.Name), DrawDetailEntity, entity);
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
					EditorGUILayout.LabelField (entity.Name);
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
			var texture = _provider.GetTexture (entity);
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
					_colorStack.Push(entity.IsActive ? ColorPalette.SnapshotButton_ON : ColorPalette.SnapshotButton_OFF);
					if (GUILayout.Button (buttonLabel, GUILayout.MaxWidth(128))) {
						if (entity.IsActive) {
							TakeSnapshot (entity);
							texture = _provider.GetTexture (entity, true);
						}
					}
					_colorStack.Pop ();

					// Refresh
					_colorStack.Push((texture != null) ? ColorPalette.SnapshotRefreshButton_ON : ColorPalette.SnapshotRefreshButton_OFF);
					if (GUILayout.Button ("Refresh", GUILayout.MaxWidth(128))) {
						if(texture != null)
							texture = _provider.GetTexture (entity, true);
					}
					_colorStack.Pop ();

					// Open
					_colorStack.Push((texture != null) ? ColorPalette.SnapshotOpenButton_ON : ColorPalette.SnapshotOpenButton_OFF);
					if (GUILayout.Button ("Open Folder", GUILayout.MaxWidth(128))) {
						if(texture != null)
							EditorUtility.RevealInFinder (entity.SnapshotPath);
					}
					_colorStack.Pop ();
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

		private bool OpenScene(ISceneEntity entity)
		{
			bool saved = EditorApplication.SaveCurrentSceneIfUserWantsTo ();
			if (saved) {
				EditorApplication.OpenScene (entity.FullPath);
			}
			return saved;
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
