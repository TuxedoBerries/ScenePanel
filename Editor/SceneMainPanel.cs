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
			var color = !EditorApplication.isPlaying ?
				new Color (155f / 255f, 202f / 255f, 60f / 255f, 1) :
				new Color (155f / 255f, 202f / 255f, 60f / 255f, 0.25f);

			_colorStack.Push (color);
			if (GUILayout.Button ("Play") && !EditorApplication.isPlaying) {
				var first = _provider.FirstScene;
				if (first != null && OpenScene (first)) {
					EditorApplication.isPlaying = true;
				}
			}
			_colorStack.Pop ();
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
			var ienum = _provider.GetFavorites ();
			while (ienum.MoveNext ()) {
				DrawEntity (ienum.Current);
			}
		}

		private void DrawAllInBuild()
		{
			var ienum = _provider.GetBuildScenes ();
			while (ienum.MoveNext ()) {
				DrawEntity (ienum.Current);
			}
		}

		private void DrawAll()
		{
			var ienum = _provider.GetAllScenes ();
			while (ienum.MoveNext ()) {
				DrawEntity (ienum.Current);
			}
		}
		#endregion

		#region Single Entity
		private void DrawEntity(ISceneEntity entity)
		{
			// Apply Search
			if (!PassFilter(entity))
				return;

			EditorGUILayout.BeginHorizontal ();
			{
				// Open
				_colorStack.Push (entity.CurrentColor);
				if (GUILayout.Button (entity.Name) && !entity.IsActive) {
					OpenScene (entity);
				}
				_colorStack.Pop ();

				// Fav
				_colorStack.Push (entity.IsFavorite ? new Color(1, 213f/255f, 4f/255f) : GUI.color);
				if (GUILayout.Button ("Fav", GUILayout.Width(30))) {
					_provider.SetAsFavorite (entity.FullPath, !entity.IsFavorite);
				}
				_colorStack.Pop ();

				// Select
				if (GUILayout.Button ("Select", GUILayout.Width(50))) {
					Selection.activeObject = AssetDatabase.LoadMainAssetAtPath (entity.FullPath);
					EditorGUIUtility.PingObject (Selection.activeObject);
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
		#endregion
	}
}
