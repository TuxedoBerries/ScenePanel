/// ------------------------------------------------
/// <summary>
/// Scene Entity
/// Purpose: 	Represents a scene in the project.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using UnityEngine;
using UnityEditor;

namespace TuxedoBerries.ScenePanel
{
	public class SceneEntity : ISceneEntity
	{
		private string _name;
		private string _fullPath;
		private bool _isActive;
		// Only in build
		private bool _inBuild;
		private bool _isEnabled;
		private int _index;

		public SceneEntity ()
		{
			Clear ();
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			_name = "";
			_fullPath = "";

			_inBuild = false;
			_isActive = false;
			_isEnabled = false;
			_index = -1;
		}

		/// <summary>
		/// Gets the name of the scene.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		/// <summary>
		/// Gets the full path of the scene.
		/// </summary>
		/// <value>The full path.</value>
		public string FullPath {
			get {
				return _fullPath;
			}
			set {
				_fullPath = value;
			}
		}

		/// <summary>
		/// Determine if the scene is the current active scene.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsActive {
			get {
				return _isActive;
			}
			set {
				_isActive = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this scene is marked as favorite.
		/// </summary>
		/// <value><c>true</c> if this instance is favorite; otherwise, <c>false</c>.</value>
		public bool IsFavorite {
			get {
				if (!EditorPrefs.HasKey (string.Format ("SceneEntity:[{0}]", _fullPath)))
					return false;
				return EditorPrefs.GetBool (string.Format("SceneEntity:[{0}]", _fullPath));
			}
			set {
				EditorPrefs.SetBool (string.Format("SceneEntity:[{0}]", _fullPath), value);
			}
		}

		#region Only In Build
		/// <summary>
		/// Determine if the scene is in the build list or not.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool InBuild {
			get {
				return _inBuild;
			}
			set {
				_inBuild = value;
			}
		}

		/// <summary>
		/// Determine if the scene is in the build is enabled or not.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled {
			get {
				return _isEnabled;
			}
			set {
				_isEnabled = value;
			}
		}

		/// <summary>
		/// Gets the index in the build.
		/// </summary>
		/// <value>The index in the build.</value>
		public int BuildIndex {
			get {
				return _index;
			}
			set {
				_index = value;
			}
		}
		#endregion

		/// <summary>
		/// Gets the current color representation of the scene.
		/// </summary>
		/// <value>The color of the current.</value>
		public Color CurrentColor {
			get {
				// Active Color
				if (_isActive)
					return new Color (141f/255f, 216f/255f, 248f/255f, 1);

				// Build Color
				if (_inBuild) {
					if (_isEnabled)
						return new Color (0, 125f/255f, 143f/255f, 1f);
					else
						return new Color (0, 125f/255f, 143f/255f, 0.5f);
				}

				return GUI.color;
			}
		}
	}
}

