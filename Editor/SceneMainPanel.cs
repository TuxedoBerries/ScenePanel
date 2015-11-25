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
		private SceneEntityDrawer _drawer;

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
			if (_drawer == null) {
				_drawer = new SceneEntityDrawer ();
				_drawer.SetColorStack (_colorStack);
				_drawer.SetFolderContainer (_folders);
				_drawer.SetDataProvider (_provider);
			}

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
					if (first != null && SceneEntityDrawer.OpenScene (first)) {
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
					_drawer.DrawEntity (entity);
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
		#endregion

	}
}
