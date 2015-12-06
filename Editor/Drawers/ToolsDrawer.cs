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
		private SceneDatabase _database;
		private GUIContentCache _contentCache;

		public ToolsDrawer ()
		{
			_contentCache = new GUIContentCache ();
		}

		public void SetDatabase(SceneDatabase provider)
		{
			_database = provider;
		}

		public void DrawUtils()
		{
			if (_database == null)
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
					_database.Refresh ();
					Debug.Log (_database.GenerateJSON ());
				}
				if (GUILayout.Button ( GetContent("Save to JSON File", TooltipSet.SAVE_JSON_BUTTON_TOOLTIP))) {
					var path = EditorUtility.SaveFilePanel ("Save scene list", "", "scenes.json", "json");
					if(!string.IsNullOrEmpty(path))
						_database.Refresh ();
						SceneMainPanelUtility.SaveText (_database.GenerateJSON (), path);
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

