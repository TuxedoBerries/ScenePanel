/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 24, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.PreferenceHandler;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scrollable container.
	/// Manages the ScrollView GUI elements.
	/// </summary>
	public class ScrollableContainer
	{
		private Dictionary<string, Vector2> _areas;
		private string _containerName;
		private bool _saveInPreferences;
		private IPreferenceChannel _channel;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ScrollableContainer"/> class.
		/// </summary>
		public ScrollableContainer () : this ("", false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ScrollableContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		public ScrollableContainer (string containerName) : this (containerName, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ScrollableContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		/// <param name="saveInPreferences">If set to <c>true</c> save in preferences.</param>
		public ScrollableContainer (string containerName, bool saveInPreferences)
		{
			_areas = new Dictionary<string, Vector2> ();
			_containerName = containerName;
			_saveInPreferences = saveInPreferences;
			_channel = EditorPreferenceHandler.GetChannel (this, _containerName);
		}

		/// <summary>
		/// Draws a scrollable area.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		public void DrawScrollable(string name, Action action)
		{
			CheckNew (name);
			var oldValue = _areas [name];
			_areas[name] = EditorGUILayout.BeginScrollView (_areas[name]);
			// Save if needed
			if (oldValue != _areas [name]) {
				SaveValue (name, _areas[name]);
			}
			{
				if (action != null)
					action.Invoke ();
			}
			EditorGUILayout.EndScrollView ();
		}

		/// <summary>
		/// Draws the scrollable.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		/// <param name="param">Parameter.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void DrawScrollable<T>(string name, Action<T> action, T param)
		{
			CheckNew (name);
			var oldValue = _areas [name];
			_areas[name] = EditorGUILayout.BeginScrollView (_areas[name]);
			// Save if needed
			if (oldValue != _areas [name]) {
				SaveValue (name, _areas[name]);
			}
			{
				if (action != null)
					action.Invoke (param);
			}
			EditorGUILayout.EndScrollView ();
		}

		/// <summary>
		/// Checks the new.
		/// </summary>
		/// <param name="name">Name.</param>
		private void CheckNew(string name)
		{
			// Add New
			if (!_areas.ContainsKey (name)) {
				_areas.Add (name, GetDefaultValue(name));
			}
		}

		/// <summary>
		/// Saves the value.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		private void SaveValue(string name, Vector2 value)
		{
			if (_saveInPreferences) {
				_channel.SetValue (name, string.Format ("{0};{1}", value.x, value.y));
			}
		}

		/// <summary>
		/// Gets the default value.
		/// </summary>
		/// <returns>The default value.</returns>
		/// <param name="name">Name.</param>
		private Vector2 GetDefaultValue(string name)
		{
			if (_saveInPreferences) {
				var pair = _channel.GetString (name);
				if (string.IsNullOrEmpty (pair))
					return Vector2.zero;
				
				var array = pair.Split(';');
				return new Vector2 (float.Parse (array [0]), float.Parse (array [1]));
			}

			return Vector2.zero;
		}
	}
}

