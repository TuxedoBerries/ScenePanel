/// ------------------------------------------------
/// <summary>
/// Tools Drawer
/// Purpose: 	Draws general tools.
/// Author:		Juan Silva
/// Date: 		November 29, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using UnityEngine;
using TuxedoBerries.ScenePanel.Constants;

namespace TuxedoBerries.ScenePanel.Drawers
{
	public class ToolsDrawer
	{
		private SceneDatabaseProvider _provider;
		private GUIContentCache _contentCache;

		public ToolsDrawer ()
		{
			_contentCache = new GUIContentCache ();
		}

		public void SetDatabase(SceneDatabaseProvider provider)
		{
			_provider = provider;
		}

		public void DrawUtils()
		{
			if (_provider == null)
				return;
			
			DrawSceneDatabaseExport ();
			EditorGUILayout.Space ();
		}

		public void DrawSceneDatabaseExport()
		{
			EditorGUILayout.LabelField ("Scenes List Export", GUILayout.Width(120));
			EditorGUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button ( GetContent("Generate JSON", TooltipSet.GENERATE_JSON_BUTTON_TOOLTIP))) {
					_provider.Refresh ();
					Debug.Log (_provider.GenerateJSON ());
				}
				if (GUILayout.Button ( GetContent("Save to JSON File", TooltipSet.SAVE_JSON_BUTTON_TOOLTIP))) {
					var path = EditorUtility.SaveFilePanel ("Save scene list", "", "scenes.json", "json");
					if(!string.IsNullOrEmpty(path))
						_provider.Refresh ();
						SceneMainPanelUtility.SaveText (_provider.GenerateJSON (), path);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		#region Helpers
		private GUIContent GetContent(string label, string tooltip)
		{
			return _contentCache.GetContent (label, tooltip);
		}
		#endregion
	}
}

