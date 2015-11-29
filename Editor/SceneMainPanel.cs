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
using TuxedoBerries.ScenePanel.PreferenceHandler;
using TuxedoBerries.ScenePanel.Drawers;
using TuxedoBerries.ScenePanel.Constants;

namespace TuxedoBerries.ScenePanel
{
	public class SceneMainPanel : EditorWindow
	{
		private const float UPDATE_POINT = 1.0f;
		private const string PANEL_NAME = "SceneMainPanel";

		private SceneDatabaseProvider _provider;
		private FolderContainer _folders;
		private ScrollableContainer _scrolls;
		private string _search;
		private float _deltaBetweenUpdates = 0;

		// Drawers
		private SceneEntityDrawer _drawer;
		private GameplayControlsDrawer _controlsDrawer;
		private SceneHistoryDrawer _historyDrawer;

		private void CheckComponents()
		{
			this.titleContent.text = "Scene Panel";
			this.titleContent.tooltip = "List of the scenes in the project.";

			// Database
			if(_provider == null)
				_provider = new SceneDatabaseProvider ();

			// GUI
			if (_folders == null)
				_folders = new FolderContainer ("SceneMainPanel", true);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer ("SceneMainPanel", true);

			// Drawers
			if (_drawer == null)
				_drawer = new SceneEntityDrawer ();
			if (_controlsDrawer == null)
				_controlsDrawer = new GameplayControlsDrawer ();
			if (_historyDrawer == null)
				_historyDrawer = new SceneHistoryDrawer ();
		}

		private void UpdateCurrent()
		{
			_provider.SetAsActive (EditorApplication.currentScene);

			if (_controlsDrawer.IsPlaying)
				return;

			_historyDrawer.RestoreFromPlay ();
			_controlsDrawer.UpdateFirstScene (_provider.FirstScene);
			_historyDrawer.UpdateCurrentHistory (_provider.CurrentActive);
		}

		private void OnInspectorUpdate()
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
			_deltaBetweenUpdates = 0;
			CheckComponents ();
			UpdateCurrent ();

			DrawTitle ();
			_controlsDrawer.DrawGeneralControls ();
			EditorGUILayout.Space ();
			DrawSearch ();
			_folders.DrawFoldable ("History", _historyDrawer.DrawHistory);
			_folders.DrawFoldable ("Tools", DrawScrollableUtils);
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Scenes");
			_scrolls.DrawScrollable ("main", DrawMainScroll);
		}

		private void DrawTitle()
		{
			EditorGUILayout.LabelField ("All the scenes in the project are displayed here.");
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

		private void DrawScrollableUtils()
		{
			_scrolls.DrawScrollable ("tools", DrawUtils);
		}

		private void DrawUtils()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.BeginVertical (GUILayout.Width(120));
				{
					EditorGUILayout.LabelField ("Scenes List", GUILayout.Width(80));
					EditorGUILayout.BeginHorizontal ();
					{
						GUILayout.Space (20);
						EditorGUILayout.BeginVertical (GUILayout.Width(120));
						{
							if (GUILayout.Button ("Generate JSON", GUILayout.Width(120))) {
								Debug.Log (_provider.GenerateJSON ());
							}
							if (GUILayout.Button ("Save to JSON File", GUILayout.Width(120))) {
								var path = EditorUtility.SaveFilePanel ("Save scene list", "", "scenes.json", "json");
								if(!string.IsNullOrEmpty(path))
									SceneMainPanelUtility.SaveText (_provider.GenerateJSON (), path);
							}
						}
						EditorGUILayout.EndVertical ();
					}
					EditorGUILayout.EndHorizontal ();
				}
				EditorGUILayout.EndVertical ();

				EditorGUILayout.BeginVertical ();
				{
					_drawer.DrawDetailEntity (_provider.CurrentActive);
				}
				EditorGUILayout.EndVertical ();
			}
			EditorGUILayout.EndHorizontal ();
		}

		#region Lists
		private void DrawMainScroll()
		{
			_folders.DrawFoldable ("Favorites", DrawAllFavorites);
			_folders.DrawFoldable ("All Scenes In Build", DrawAllInBuild);
			_folders.DrawFoldable ("All Scenes", DrawAll);
		}

		private void DrawAllFavorites()
		{
			EditorGUILayout.HelpBox ("All the favorites scenes are displayed here\nDisplay order is: Alphabetical", MessageType.Info);
			DrawIenum (_provider.GetFavorites ());
		}

		private void DrawAllInBuild()
		{
			EditorGUILayout.HelpBox ("All the scenes included in the build are here.\nDisplay order is: By Build Index", MessageType.Info);
			DrawIenum (_provider.GetBuildScenes ());
		}

		private void DrawAll()
		{
			EditorGUILayout.HelpBox ("All the scenes in the project are here.\nDisplay order is: Alphabetical", MessageType.Info);
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
