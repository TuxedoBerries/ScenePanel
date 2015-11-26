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
		private ButtonContainer _buttonFolders;
		private FolderContainer _folders;
		private ScrollableContainer _scrolls;
		private string _search;
		private float _deltaBetweenUpdates = 0;
		private SceneEntityDrawer _drawer;
		private SceneHistory _history;

		private void CheckProvider()
		{
			if(_provider == null)
				_provider = new SceneDatabaseProvider ();
			if (_history == null)
				_history = new SceneHistory ();
		}

		private void CheckGUIElements()
		{
			if (_colorStack == null)
				_colorStack = new ColorStack ();
			if (_folders == null)
				_folders = new FolderContainer ("SceneMainPanel", true);
			if (_buttonFolders == null)
				_buttonFolders = new ButtonContainer ("SceneMainPanel", true);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer ("SceneMainPanel", true);
			if (_drawer == null) {
				_drawer = new SceneEntityDrawer ();
				_drawer.SetColorStack (_colorStack);
				_drawer.SetButtonContainer (_buttonFolders);
				_drawer.SetDataProvider (_provider);
			}

			this.titleContent.text = "Scene Panel";
			this.titleContent.tooltip = "List of the scenes in the project.";
		}

		private void UpdateCurrent()
		{
			_provider.SetAsActive (EditorApplication.currentScene);
			_history.Push (_provider.CurrentActive);
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
			_folders.DrawFoldable ("History", DrawHistory);
			_folders.DrawFoldable ("Tools", DrawUtils);
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Scenes");
			_scrolls.DrawScrollable ("main", DrawMainScroll);
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

		private void DrawUtils()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button ("Generate JSON")) {
					Debug.Log (_provider.GenerateJSON ());
				}
				if (GUILayout.Button ("Save to JSON File")) {
					var path = EditorUtility.SaveFilePanel ("Save scene list", "", "scenes.json", "json");
					if (!string.IsNullOrEmpty (path)) {
						bool saved = false;
						try{
							System.IO.File.WriteAllText (path, _provider.GenerateJSON ());
							saved = true;
						}catch(System.Exception e){
							Debug.LogErrorFormat ("Exception trying to write file: {0}", e.Message);
						}
						if (saved) {
							EditorUtility.DisplayDialog ("Scene list", "File successfully saved", "ok");
						}
					}
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawHistory()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.BeginVertical (GUILayout.Width(90));
				{
					EditorGUILayout.BeginHorizontal ();
					{
						_colorStack.Push ((_history.BackCount > 1) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						if (GUILayout.Button ("<=", GUILayout.Width(42))) {
							var item = _history.Back ();
							SceneEntityDrawer.OpenScene (item);
						}
						_colorStack.Pop ();

						_colorStack.Push ((_history.FowardCount > 0) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						if (GUILayout.Button ("=>", GUILayout.Width(42))) {
							var item = _history.Forward ();
							SceneEntityDrawer.OpenScene (item);
						}
						_colorStack.Pop ();
					}

					_colorStack.Push ((_history.Count > 1) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
					EditorGUILayout.EndHorizontal ();
					if (GUILayout.Button ("Clear History", GUILayout.Width(90))) {
						_history.Clear ();
					}
					_colorStack.Pop ();
				}
				EditorGUILayout.EndVertical ();
				EditorGUILayout.BeginVertical ();
				{
					EditorGUILayout.Popup ("Back History: ", 0, _history.GetBackStack ());
					EditorGUILayout.Popup ("Forward History: ", 0, _history.GetForwardStack ());
				}
				EditorGUILayout.EndVertical ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		private void DrawMainScroll()
		{
			_folders.DrawFoldable ("Favorites", DrawAllFavorites);
			_folders.DrawFoldable ("All Scenes In Build", DrawAllInBuild);
			_folders.DrawFoldable ("All Scenes", DrawAll);
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
