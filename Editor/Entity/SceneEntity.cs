/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using System.Text;
using UnityEditor;
using TuxedoBerries.ScenePanel.PreferenceHandler;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Scene entity.
	/// </summary>
	public class SceneEntity : ISceneEntity, ISceneFileEntity
	{
		private const string FAVORITE_KEY = "SceneEntity:Favorite:[{0}]";
		public static SceneEntity Empty = new SceneEntity ();

		private EditorBuildSettingsScene _scene;
		private string _name;
		private string _fullPath;
		private string _guid;
		private bool _isActive;
		// Only in build
		private bool _inBuild;
		private bool _isEnabled;
		private int _index;
		// Cached
		private IPreferenceChannel _channel;

		public SceneEntity ()
		{
			Clear ();
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			_name = "";
			_fullPath = "";
			_guid = "";

			_inBuild = false;
			_isActive = false;
			_isEnabled = false;
			_index = -1;
		}

		/// <summary>
		/// Copy the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void Copy (SceneEntity entity)
		{
			_name = entity.Name;
			_fullPath = entity.FullPath;
			_guid = entity.GUID;

			_inBuild = entity.InBuild;
			_isActive = entity.IsActive;
			_isEnabled = entity.IsEnabled;
			_index = entity.BuildIndex;
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
				_channel = EditorPreferenceHandler.GetChannel (this, _name);
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
		/// Gets the GUID.
		/// </summary>
		/// <value>The GUID.</value>
		public string GUID {
			get {
				return _guid;
			}
			set {
				_guid = value;
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
				if (_channel == null)
					return false;
				return _channel.GetBool ("Favorite");
			}
			set {
				if (_channel == null)
					return;
				_channel.SetValue ("Favorite", value);
			}
		}

		/// <summary>
		/// Gets the snapshot path.
		/// </summary>
		/// <value>The snapshot path.</value>
		public string ScreenshotPath {
			get {
				if (_channel == null)
					return "";
				return _channel.GetString ("Screenshot");
			}
			set {
				if (_channel == null)
					return;
				_channel.SetValue ("Screenshot", value);
			}
		}

		#region Only In Build
		/// <summary>
		/// Gets the scene in the build if any.
		/// </summary>
		/// <value>The scene.</value>
		public EditorBuildSettingsScene Scene {
			get {
				return _scene;
			}
			set {
				_scene = value;
			}
		}

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
		/// Returns a <see cref="System.String"/> that represents the current <see cref="TuxedoBerries.ScenePanel.SceneEntity"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="TuxedoBerries.ScenePanel.SceneEntity"/>.</returns>
		public override string ToString ()
		{
			var builder = new StringBuilder ();
			builder.Append ("{");
			// Name
			builder.Append ("\"name\":");
			builder.Append ("\"");
			builder.Append (Name);
			builder.Append ("\",");
			// Full Path
			builder.Append ("\"full_path\":");
			builder.Append ("\"");
			builder.Append (FullPath);
			builder.Append ("\",");
			// GUID
			builder.Append ("\"guid\":");
			builder.Append ("\"");
			builder.Append (GUID);
			builder.Append ("\",");
			// SnapshotPath
			builder.Append ("\"screenshot_path\":");
			builder.Append ("\"");
			builder.Append (ScreenshotPath);
			builder.Append ("\",");
			// IsFavorite
			builder.Append ("\"is_favorite\":");
			builder.Append (IsFavorite.ToString ().ToLower ());
			builder.Append (",");
			// InBuild
			builder.Append ("\"in_build\":");
			builder.Append (InBuild.ToString ().ToLower ());
			builder.Append (",");
			// IsEnabled
			builder.Append ("\"is_enabled\":");
			builder.Append (IsEnabled.ToString ().ToLower ());
			builder.Append (",");
			// BuildIndex
			builder.Append ("\"build_index\":");
			builder.Append (BuildIndex);

			builder.Append ("}");
			return builder.ToString ();
		}

		public static int CompareByIndex (SceneEntity sceneA, SceneEntity sceneB)
		{
			if (sceneA == null && sceneB == null)
				return 0;
			if (sceneA == null)
				return -1;
			if (sceneB == null)
				return 1;

			return sceneA.BuildIndex - sceneB.BuildIndex;
		}
	}
}

