/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.PreferenceHandler;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Folder container.
	/// Manages the Foldout GUI elements.
	/// </summary>
	public class FolderContainer
	{
		private Dictionary<string, bool> _folders;
		private string _containerName;
		private bool _saveInPreferences;
		private IPreferenceChannel _channel;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.FolderContainer"/> class.
		/// </summary>
		public FolderContainer () : this("FolderContainer", false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.FolderContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		public FolderContainer (string containerName) : this(containerName, false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.FolderContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		/// <param name="saveInPreferences">If set to <c>true</c> save in preferences.</param>
		public FolderContainer(string containerName, bool saveInPreferences)
		{
			_containerName = containerName;
			_saveInPreferences = saveInPreferences;
			_folders = new Dictionary<string, bool> ();
			_channel = EditorPreferenceHandler.GetChannel (this, _containerName);
		}

		/// <summary>
		/// Gets the value for the given key name.
		/// </summary>
		/// <returns><c>true</c>, if value was gotten, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		public bool GetValue(string name)
		{
			if (!_folders.ContainsKey (name))
				return false;

			return _folders [name];
		}

		/// <summary>
		/// Draws a foldable element.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		public void DrawFoldable(string name, Action action)
		{
			CheckNew (name);
			var oldValue = _folders [name];
			_folders [name] = EditorGUILayout.Foldout (_folders [name], name);
			// Save if needed
			if (oldValue != _folders [name]) {
				SaveValue (name, _folders [name]);
			}
			if (_folders [name]) {
				EditorGUILayout.BeginHorizontal ();
				{
					GUILayout.Space (20);
					EditorGUILayout.BeginVertical ();
					{
						if (action != null)
							action.Invoke ();
					}
					EditorGUILayout.EndVertical ();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		/// <summary>
		/// Draws a foldable element with a parameter.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		/// <param name="param">Parameter.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void DrawFoldable<T>(string name, Action<T> action, T param)
		{
			CheckNew (name);
			var oldValue = _folders [name];
			_folders [name] = EditorGUILayout.Foldout (_folders [name], name);
			// Save if needed
			if (oldValue != _folders [name]) {
				SaveValue (name, _folders [name]);
			}
			if (_folders [name]) {
				EditorGUILayout.BeginHorizontal ();
				{
					GUILayout.Space (20);
					EditorGUILayout.BeginVertical ();
					{
						if (action != null)
							action.Invoke (param);
					}
					EditorGUILayout.EndVertical ();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		/// <summary>
		/// Checks the new.
		/// </summary>
		/// <param name="name">Name.</param>
		private void CheckNew(string name)
		{
			// Add New
			if (!_folders.ContainsKey (name)) {
				_folders.Add (name, GetDefaultValue(name));
			}
		}

		/// <summary>
		/// Saves the value.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">If set to <c>true</c> value.</param>
		private void SaveValue(string name, bool value)
		{
			if (_saveInPreferences) {
				_channel.SetValue (name, value);
			}
		}

		/// <summary>
		/// Gets the default value.
		/// </summary>
		/// <returns><c>true</c>, if default value was gotten, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		private bool GetDefaultValue(string name)
		{
			if (_saveInPreferences) {
				return _channel.GetBool (name);
			}

			return true;
		}
	}
}

