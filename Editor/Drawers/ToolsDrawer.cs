/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using TuxedoBerries.ScenePanel.Constants;
using TuxedoBerries.ScenePanel.Controllers;
using TuxedoBerries.ScenePanel.Provider;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Tools drawer.
	/// Draws general tools.
	/// </summary>
	public class ToolsDrawer : BaseDrawer
	{
		private SceneDatabase _database;

		public ToolsDrawer () : base ()
		{
			_database = SceneDatabaseProvider.GetDatabase (this);
		}

		public void DrawUtils ()
		{
			if (_database == null)
				return;

			DrawSceneDatabaseExport ();
			EditorGUILayout.Space ();
		}

		public void DrawSceneDatabaseExport ()
		{
			EditorGUILayout.LabelField ("Scenes List Export", GUILayout.Width (120));
			EditorGUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button (GetContent ("Generate JSON", TooltipSet.GENERATE_JSON_BUTTON_TOOLTIP))) {
					_database.Refresh ();
					Debug.Log (_database.GenerateJSON ());
				}
				if (GUILayout.Button (GetContent ("Save to JSON File", TooltipSet.SAVE_JSON_BUTTON_TOOLTIP))) {
					var path = EditorUtility.SaveFilePanel ("Save scene list", "", "scenes.json", "json");
					if (!string.IsNullOrEmpty (path))
						_database.Refresh ();
					SceneMainPanelUtility.SaveText (_database.GenerateJSON (), path);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		public override void Dispose ()
		{
			SceneDatabaseProvider.ReturnDatabase (this);
			TextureDatabaseProvider.ReturnDatabase (this);
		}

		#region Helpers
		private GUIContent GetContent (string label, string tooltip)
		{
			return _contentCache.GetContent (label, tooltip);
		}
		#endregion
	}
}

