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
	public class SceneMainPanel : EditorWindow, IEditorPreferenceSection
	{
		private const float UPDATE_POINT = 1.0f;
		private const string PANEL_NAME = "SceneMainPanel";

		private SceneDatabaseProvider _provider;
		private TextureDatabaseProvider _textureProvider;
		private ColorStack _colorStack;
		private FolderContainer _folders;
		private ScrollableContainer _scrolls;
		private string _search;
		private float _deltaBetweenUpdates = 0;
		private SceneHistory _history;
		private bool _justLoaded = true;
		private EditorPreferenceHandlerChannel _channel;

		// Drawers
		private SceneEntityDrawer _drawer;
		private GameplayControlsDrawer _controlsDrawer;

		/// <summary>
		/// Gets the type of the implementation.
		/// </summary>
		/// <value>The type of the implementation.</value>
		public System.Type ImplementationType {
			get {
				return typeof(SceneMainPanel);
			}
		}

		/// <summary>
		/// Gets the name of the section.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return PANEL_NAME;
			}
		}

		private void CheckProvider()
		{
			if(_provider == null)
				_provider = new SceneDatabaseProvider ();
			if (_textureProvider == null)
				_textureProvider = new TextureDatabaseProvider ();
			if (_history == null) {
				_history = new SceneHistory ();
				_history.Load ();
				_justLoaded = true;
			}
			if (_channel == null) {
				_channel = EditorPreferenceHandler.GetChannel (this);
				_restoreOnStop = _channel.GetBool ("restoreScene");
			}
		}

		private void CheckGUIElements()
		{
			if (_colorStack == null)
				_colorStack = new ColorStack ();
			if (_folders == null)
				_folders = new FolderContainer ("SceneMainPanel", true);
			if (_scrolls == null)
				_scrolls = new ScrollableContainer ("SceneMainPanel", true);
			if (_drawer == null)
				_drawer = new SceneEntityDrawer ();
			if (_controlsDrawer == null)
				_controlsDrawer = new GameplayControlsDrawer ();

			this.titleContent.text = "Scene Panel";
			this.titleContent.tooltip = "List of the scenes in the project.";
		}

		private void UpdateCurrent()
		{
			_provider.SetAsActive (EditorApplication.currentScene);
			_controlsDrawer.UpdateFirstScene (_provider.FirstScene);
			
			SaveCurrentHistory ();
		}

		private void SaveCurrentHistory()
		{
			// Add to history only if we are in Edit mode
			if (_controlsDrawer.IsPlaying)
				return;
			
			_history.Push (_provider.CurrentActive);
		}

		private void GetBackFromPlayMode()
		{
			// Restore only if nothing is moving
			if (EditorApplication.isPlaying || Application.isPlaying)
				return;
			if (!_justLoaded)
				return;
			if (!_restoreOnStop)
				return;

			var item = _history.CurrentScene;
			SceneMainPanelUtility.OpenScene (item);
			_justLoaded = false;
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
			if(_controlsDrawer != null)
				_controlsDrawer.OnInspectorUpdate ();
		}

		private void OnGUI()
		{
			_deltaBetweenUpdates = 0;
			CheckProvider ();
			CheckGUIElements ();
			GetBackFromPlayMode ();
			UpdateCurrent ();

			_colorStack.Reset ();
			DrawTitle ();
			_controlsDrawer.DrawGeneralControls ();
			EditorGUILayout.Space ();
			DrawSearch ();
			_folders.DrawFoldable ("History", DrawHistory);
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
			_colorStack.Reset ();
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


		private int _backSelected = 0;
		private int _forwardSelected = 0;
		private bool _restoreOnStop = true;
		private void DrawHistory()
		{
			_restoreOnStop = EditorGUILayout.Toggle ("Restore Scene On Stop", _restoreOnStop);
			_channel.SetValue ("restoreScene", _restoreOnStop);
			EditorGUILayout.BeginHorizontal ();
			{
				GUILayout.Space (20);
				EditorGUILayout.BeginVertical (GUILayout.Width(90));
				{
					EditorGUILayout.BeginHorizontal ();
					{
						_colorStack.Push ((_history.BackCount > 1) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						var arrowback = _textureProvider.GetRelativeTexture (IconSet.ARROW_BACK_ICON);
						if (GUILayout.Button (arrowback, GUILayout.Width(42))) {
							var item = _history.Back ();
							SceneMainPanelUtility.OpenScene (item);
						}
						_colorStack.Pop ();

						_colorStack.Push ((_history.FowardCount > 0) ? ColorPalette.HistoryArrowButton_ON : ColorPalette.HistoryArrowButton_OFF);
						var arrowForward = _textureProvider.GetRelativeTexture (IconSet.ARROW_FORWARD_ICON);
						if (GUILayout.Button (arrowForward, GUILayout.Width(42))) {
							var item = _history.Forward ();
							SceneMainPanelUtility.OpenScene (item);
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
					_backSelected = EditorGUILayout.Popup ("Back History: ", _backSelected, _history.GetBackStack ());
					_forwardSelected = EditorGUILayout.Popup ("Forward History: ", _forwardSelected, _history.GetForwardStack ());
				}
				EditorGUILayout.EndVertical ();

				// Check Selection
				if (_backSelected != 0) {
					for (int i = 0; i < _backSelected; ++i) {
						_history.Back ();
					}
					SceneMainPanelUtility.OpenScene (_history.CurrentScene);
					_backSelected = 0;
				}
				if (_forwardSelected != 0) {
					for (int i = 0; i <= _forwardSelected; ++i) {
						_history.Forward ();
					}
					SceneMainPanelUtility.OpenScene (_history.CurrentScene);
					_forwardSelected = 0;
				}
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
