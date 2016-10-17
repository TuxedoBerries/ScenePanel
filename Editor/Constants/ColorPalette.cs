/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 24, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;

namespace TuxedoBerries.ScenePanel.Constants
{
	/// <summary>
	/// Color palette.
	/// Color Palette for the Scene Panel.
	/// </summary>
	public static class ColorPalette
	{
		#region Play Buttons
		// Play button
		public static Color PlayButton_ON = new Color (155f / 255f, 202f / 255f, 60f / 255f, 1);
		public static Color PlayButton_OFF = new Color (155f / 255f, 202f / 255f, 60f / 255f, 0.25f);

		// Pause button
		public static Color PauseButton_ON = new Color (155f / 255f, 202f / 255f, 60f / 255f, 1);
		public static Color PauseButton_HOLD = new Color (98f / 255f, 192f / 255f, 220f / 255f, 1);
		public static Color PauseButton_OFF = new Color (1f, 1f, 1f, 0.25f);

		// Stop button
		public static Color StopButton_ON = new Color (192f / 255f, 46f / 255f, 29f / 255f, 1);
		public static Color StopButton_OFF = new Color (192f / 255f, 46f / 255f, 29f / 255f, 0.25f);

		// Step button
		public static Color StepButton_ON = new Color (252f / 255f, 185f / 255f, 20f / 255f, 1);
		public static Color StepButton_OFF = new Color (252f / 255f, 185f / 255f, 20f / 255f, 0.25f);
		#endregion

		// Favorite Button
		public static Color FavoriteButton_ON = new Color (1f, 213f / 255f, 4f / 255f, 1f);
		public static Color FavoriteButton_OFF = Color.white;

		// Scene Open Button
		public static Color SceneOpenButton_InBuild_Enabled = new Color (98f / 255f, 192f / 255f, 220f / 255f, 1f);
		public static Color SceneOpenButton_InBuild_Disabled = new Color (98f / 255f, 192f / 255f, 220f / 255f, 0.5f);
		public static Color SceneOpenButton_Regular = Color.white;

		// In Build fields
		public static Color InBuildField_ON = Color.white;
		public static Color InBuildField_OFF = new Color (1f, 1f, 1f, 0.25f);

		// Take Snapshot Button
		public static Color SnapshotButton_ON = Color.white;
		public static Color SnapshotButton_OFF = new Color (1f, 1f, 1f, 0.25f);

		// Refresh Snapshot Folder Button
		public static Color SnapshotRefreshButton_ON = Color.white;
		public static Color SnapshotRefreshButton_OFF = new Color (1f, 1f, 1f, 0.25f);

		// Open Snapshot Folder Button
		public static Color SnapshotOpenButton_ON = Color.white;
		public static Color SnapshotOpenButton_OFF = new Color (1f, 1f, 1f, 0.25f);

		// History Arrow Button
		public static Color HistoryArrowButton_ON = Color.white;
		public static Color HistoryArrowButton_OFF = new Color (1f, 1f, 1f, 0.25f);

		#region Get Colors

		/// <summary>
		/// Gets the color representation of the given entity.
		/// </summary>
		/// <returns>The color.</returns>
		/// <param name="entity">Entity.</param>
		public static Color GetColor (ISceneEntity entity)
		{
			// Build Color
			if (entity.InBuild) {
				if (entity.IsEnabled)
					return SceneOpenButton_InBuild_Enabled;
				else
					return SceneOpenButton_InBuild_Disabled;
			}

			return SceneOpenButton_Regular;
		}

		/// <summary>
		/// Gets the color of the edit.
		/// </summary>
		/// <returns>The edit color.</returns>
		/// <param name="editMode">If set to <c>true</c> edit mode.</param>
		public static Color GetEditColor (bool editMode)
		{
			if (editMode)
				return InBuildField_ON;

			return InBuildField_OFF;
		}
		#endregion
	}
}

