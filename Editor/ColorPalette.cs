/// ------------------------------------------------
/// <summary>
/// Color Palette
/// Purpose: 	Color Palette for the Scene Panel.
/// Author:		Juan Silva
/// Date: 		November 24, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEngine;

namespace TuxedoBerries.ScenePanel
{
	public static class ColorPalette
	{
		// Play button
		public static Color PlayButton_ON = new Color(155f / 255f, 202f / 255f, 60f / 255f, 1);
		public static Color PlayButton_OFF = new Color(155f / 255f, 202f / 255f, 60f / 255f, 0.25f);

		// Stop button
		public static Color StopButton_ON = new Color(192f / 255f, 46f / 255f, 29f / 255f, 1);
		public static Color StopButton_OFF = new Color(192f / 255f, 46f / 255f, 29f / 255f, 0.25f);

		// Favorite Button
		public static Color FavoriteButton_ON = new Color(1f, 213f / 255f, 4f / 255f, 1f);
		public static Color FavoriteButton_OFF = Color.white;

		// Scene Open Button
		public static Color SceneOpenButton_Active = new Color (155f / 255f, 202f / 255f, 60f / 255f, 1);
		public static Color SceneOpenButton_InBuild_Enabled = new Color (98f / 255f, 192f / 255f, 220f / 255f, 1f);
		public static Color SceneOpenButton_InBuild_Disabled = new Color (98f / 255f, 192f / 255f, 220f / 255f, 0.5f);
		public static Color SceneOpenButton_Regular = Color.white;

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
	}
}

