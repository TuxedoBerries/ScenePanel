/// ------------------------------------------------
/// <summary>
/// Folder Container
/// Purpose: 	Manages the Foldout GUI elements.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using System;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class FolderContainer
	{
		private Dictionary<string, bool> _folders;
		private string _containerName;
		private bool _saveInPreferences;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.FolderContainer"/> class.
		/// </summary>
		public FolderContainer () : this("", false)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.FolderContainer"/> class.
		/// </summary>
		/// <param name="containerName">Container name.</param>
		public FolderContainer (string containerName) : this("FolderContainer", false)
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
		}

		/// <summary>
		/// Draws a foldable element.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="action">Action.</param>
		public void DrawFoldable(string name, Action action)
		{
			CheckNew (name);
			_folders [name] = EditorGUILayout.Foldout (_folders [name], name);
			SaveValue (name, _folders [name]);
			if (_folders [name]) {
				EditorGUI.indentLevel++;
				if (action != null)
					action.Invoke ();
				EditorGUI.indentLevel--;
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
			_folders [name] = EditorGUILayout.Foldout (_folders [name], name);
			SaveValue (name, _folders [name]);
			if (_folders [name]) {
				EditorGUI.indentLevel++;
				if (action != null)
					action.Invoke (param);
				EditorGUI.indentLevel--;
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
				EditorPrefs.SetBool (string.Format ("FolderContainer/{0}/{1}", _containerName, name), value);
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
				return EditorPrefs.GetBool (string.Format ("FolderContainer/{0}/{1}", _containerName, name));
			}

			return true;
		}
	}
}

