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
		private float _deltaBetweenUpdates = 0;

		// Drawers
		private SceneEntityDrawer _drawer;
		private ToolsDrawer _toolsDrawer;

		private void CheckComponents()
		{
			this.titleContent.text = "Scene Panel";
			this.titleContent.tooltip = "List of the scenes in the project.";
		}

		private void OnInspectorUpdate()
		{
			// Fixed Update
			_deltaBetweenUpdates += 0.1f;
			if (_deltaBetweenUpdates >= UPDATE_POINT) {
				_deltaBetweenUpdates = 0;
				Repaint ();
			}
		}

		private void OnGUI()
		{
			_deltaBetweenUpdates = 0;
			CheckComponents ();

			DrawTitle ();
			EditorGUILayout.Space ();
		}

		private void DrawTitle()
		{
			EditorGUILayout.LabelField ("All the scenes in the project are displayed here.");
		}

	}
}
