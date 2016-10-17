/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 28, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
namespace TuxedoBerries.ScenePanel.Constants
{
	/// <summary>
	/// Tooltip set.
	/// Set of tooltip texts used in Scene Panel.
	/// </summary>
	public static class TooltipSet
	{
		// Play buttons
		public const string PLAY_START_TOOLTIP = "Play from the first scene in the build list.";
		public const string PLAY_TOOLTIP = "Play the current scene.";
		public const string STOP_TOOLTIP = "Stop the gameplay.";
		public const string PAUSE_TOOLTIP = "Pause the current gameplay.";
		public const string STEP_TOOLTIP = "Execute a single frame of the gameplay.";

		// Scene Entity
		public const string SCENE_BUTTON_TOOLTIP = "Opens the {0} Scene.";
		public const string SELECT_BUTTON_TOOLTIP = "Selects the scene in the project panel.";
		public const string DETAIL_BUTTON_TOOLTIP = "Open/Close the detail section of the scene.";
		public const string FAVORITE_BUTTON_TOOLTIP = "Toggle set this scene as favorite.";
		public const string SCREENSHOT_BUTTON_TOOLTIP = "Take a screenshot of the current scene. The size is the same as the Game View Panel.";
		public const string SCREENSHOT_REFRESH_BUTTON_TOOLTIP = "Refresh the current screenshot on display.";
		public const string SCREENSHOT_OPEN_FOLDER_BUTTON_TOOLTIP = "Open the folder containing all the screenshots.";

		// Data
		public const string GENERATE_JSON_BUTTON_TOOLTIP = "Generate a JSON representation of all the scenes in the project and print it in the console.";
		public const string SAVE_JSON_BUTTON_TOOLTIP = "Generate and save a JSON representation of all the scenes in the project.";
	}
}

