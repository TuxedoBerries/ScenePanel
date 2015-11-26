/// ------------------------------------------------
/// <summary>
/// Button Container
/// Purpose: 	Manages a Button with a foldable GUI elements.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class ButtonContainer
	{
		private Dictionary<string, bool> _folders;
		private string _containerName;
		private bool _saveInPreferences;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ButtonContainer"/> class.
		/// </summary>
		public ButtonContainer () : this("", false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ButtonContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		public ButtonContainer (string containerName) : this("ButtonContainer", false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.ButtonContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		/// <param name="saveInPreferences">If set to <c>true</c> save in preferences.</param>
		public ButtonContainer (string containerName, bool saveInPreferences)
		{
			_containerName = containerName;
			_saveInPreferences = saveInPreferences;
			_folders = new Dictionary<string, bool> ();
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
		/// Draws a button that shows or hide the content.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="label">Label.</param>
		public void DrawButton (string name, string label, params GUILayoutOption[] options)
		{
			CheckNew (name);
			if (GUILayout.Button (label, options)) {
				_folders [name] = !_folders [name];
			}
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		public void DrawContent (string name, Action action)
		{
			if (!_folders [name])
				return;
			if (action == null)
				return;
			action.Invoke ();
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		/// <param name="data">Data.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void DrawContent<T> (string name, Action<T> action, T data)
		{
			if (!_folders [name])
				return;
			if (action == null)
				return;
			action.Invoke (data);
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
				EditorPrefs.SetBool (string.Format ("ButtonContainer/{0}/{1}", _containerName, name), value);
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
				return EditorPrefs.GetBool (string.Format ("ButtonContainer/{0}/{1}", _containerName, name));
			}

			return true;
		}
	}
}

