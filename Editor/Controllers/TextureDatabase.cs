/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 27, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel.Controllers
{
	/// <summary>
	/// Texture database.
	/// Provide a databse of the scenes snapshots.
	/// </summary>
	public class TextureDatabase
	{
		private Dictionary<string, Texture> _textureCache;
		private string _localFolder;

		public TextureDatabase ()
		{
			_textureCache = new Dictionary<string, Texture> ();
			LocateRelativePath ();
		}

		private void LocateRelativePath ()
		{
			var result = AssetDatabase.FindAssets ("ScenePanelMenu");
			if (result == null || result.Length <= 0) {
				Debug.LogError ("Could not find relative path");
				return;
			}

			// Assume only one
			var path = AssetDatabase.GUIDToAssetPath (result [0]);
			_localFolder = Path.GetDirectoryName (path);
		}

		#region Textures
		/// <summary>
		/// Gets the texture using the relative path fro this class file.
		/// </summary>
		/// <returns>The relative texture.</returns>
		/// <param name="path">Path.</param>
		public Texture GetRelativeTexture (string path)
		{
			return GetTexture (Path.Combine (_localFolder, path));
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			_textureCache.Clear ();
		}

		/// <summary>
		/// Check if the given texture path is cached
		/// </summary>
		/// <returns><c>true</c>, if cached was ised, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public bool isCached (string path)
		{
			if (string.IsNullOrEmpty (path))
				return false;
			return _textureCache.ContainsKey (path);
		}

		/// <summary>
		/// Gets the texture by path if any.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="path">Path.</param>
		public Texture GetTexture (string path)
		{
			return GetTexture (path, false);
		}

		/// <summary>
		/// Gets the texture by path if any.
		/// If refresh is true, it will force to update the texture.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="path">Path.</param>
		/// <param name="refresh">If set to <c>true</c> refresh.</param>
		public Texture GetTexture (string path, bool refresh)
		{
			// Force Refresh
			if (refresh)
				RefreshCache (path);

			// Refresh if not exist
			if (!_textureCache.ContainsKey (path))
				RefreshCache (path);

			// Not exist
			if (!_textureCache.ContainsKey (path))
				return null;

			// Refresh cache if null
			if (_textureCache [path] == null)
				RefreshCache (path);

			// Return cached
			return _textureCache [path];
		}

		/// <summary>
		/// Refreshs the cache.
		/// </summary>
		/// <returns><c>true</c>, if cache was refreshed, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		public bool RefreshCache (string path)
		{
			if (string.IsNullOrEmpty (path))
				return false;

			if (!System.IO.File.Exists (path))
				return false;

			var bytes = System.IO.File.ReadAllBytes (path);
			// Refresh Cache
			if (_textureCache.ContainsKey (path)) {
				var texture2D = _textureCache [path] as Texture2D;
				if (texture2D == null) {
					texture2D = new Texture2D (2, 2);
					texture2D.hideFlags = HideFlags.HideAndDontSave;
					_textureCache [path] = texture2D;
				}
				texture2D.LoadImage (bytes);
			} else {
				Texture2D texture = new Texture2D (2, 2);
				texture.LoadImage (bytes);
				texture.hideFlags = HideFlags.HideAndDontSave;
				_textureCache.Add (path, texture);
			}
			return true;
		}
		#endregion
	}
}

