/// ------------------------------------------------
/// <summary>
/// Scene Database Provider
/// Purpose: 	Provide a databse of the scenes in the project.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class SceneDatabaseProvider
	{
		private SortedDictionary<string, SceneEntity> _dict;
		private List<SceneEntity> _buildListByIndex;
		private Dictionary<string, Texture> _textureCache;
		private Stack<SceneEntity> _pool;
		private SceneEntity _firstScene;
		private SceneEntity _activeScene;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.SceneDatabaseProvider"/> class.
		/// </summary>
		public SceneDatabaseProvider()
		{
			_dict = new SortedDictionary<string, SceneEntity> ();
			_pool = new Stack<SceneEntity> ();
			_buildListByIndex = new List<SceneEntity> ();
			_textureCache = new Dictionary<string, Texture> ();
			Refresh ();
		}

		/// <summary>
		/// Refresh this instance.
		/// </summary>
		public void Refresh()
		{
			_buildListByIndex.Clear ();
			RefreshDictionary ();
			AddBuildData ();
			_buildListByIndex.Sort (SortByIndex);
		}

		/// <summary>
		/// Gets the first scene in build, if any.
		/// </summary>
		/// <value>The first scene.</value>
		public ISceneEntity FirstScene {
			get {
				return _firstScene;
			}
		}

		/// <summary>
		/// Gets the current active.
		/// </summary>
		/// <value>The current active.</value>
		public ISceneEntity CurrentActive {
			get {
				return _activeScene;
			}
		}

		/// <summary>
		/// Determine if the given scenePath exist.
		/// </summary>
		/// <param name="scenePath">Scene path.</param>
		public bool Contains(string scenePath)
		{
			return _dict.ContainsKey (scenePath);
		}

		/// <summary>
		/// Sets as active.
		/// </summary>
		/// <returns><c>true</c>, if as active was set, <c>false</c> otherwise.</returns>
		/// <param name="scenePath">Scene path.</param>
		public bool SetAsActive(string scenePath)
		{
			if (!_dict.ContainsKey (scenePath))
				return false;

			// Deactivate
			if (_activeScene != null)
				_activeScene.IsActive = false;

			_activeScene = _dict [scenePath];
			_activeScene.IsActive = true;
			return true;
		}

		/// <summary>
		/// Gets the SceneEntity with the specified scenePath.
		/// </summary>
		/// <param name="scenePath">Scene path.</param>
		public ISceneEntity this[string scenePath] {
			get {
				return _dict [scenePath];
			}
		}

		/// <summary>
		/// Gets the build scenes.
		/// </summary>
		/// <returns>The build scenes.</returns>
		public IEnumerator<ISceneEntity> GetBuildScenes()
		{
			foreach (var data in _buildListByIndex) {
				yield return data;
			}
			yield break;
		}

		/// <summary>
		/// Gets all scenes.
		/// </summary>
		/// <returns>The all scenes.</returns>
		public IEnumerator<ISceneEntity> GetAllScenes()
		{
			foreach (var data in _dict.Values) {
				yield return data;
			}
			yield break;
		}

		/// <summary>
		/// Gets the favorites.
		/// </summary>
		/// <returns>The favorites.</returns>
		public IEnumerator<ISceneEntity> GetFavorites()
		{
			foreach (var data in _dict.Values) {
				if (!data.IsFavorite)
					continue;
				
				yield return data;
			}
			yield break;
		}

		/// <summary>
		/// Generates a JSON representation of the scenes.
		/// </summary>
		/// <returns>The JSON string.</returns>
		public string GenerateJSON()
		{
			var builder = new StringBuilder ();
			builder.Append ("{");

			int total = _dict.Count;
			int current = 0;
			foreach (var data in _dict.Values) {
				builder.Append ("\"");
				builder.Append (data.Name);
				builder.Append ("\":");
				builder.Append (data.ToString());

				++current;
				if(current < total)
					builder.Append (",");
			}
			builder.Append ("}");
			return builder.ToString ();
		}

		#region Snapshots
		/// <summary>
		/// Gets the texture of the snapshot of the scene if any.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="entity">Entity.</param>
		public Texture GetTexture(ISceneEntity entity)
		{
			return GetTexture (entity, false);
		}

		/// <summary>
		/// Gets the texture of the snapshot of the scene if any.
		/// If refresh is true, it will force to update the texture.
		/// </summary>
		/// <returns>The texture.</returns>
		/// <param name="entity">Entity.</param>
		/// <param name="refresh">If set to <c>true</c> refresh.</param>
		public Texture GetTexture(ISceneEntity entity, bool refresh)
		{
			// Force Refresh
			if(refresh)
				RefreshCache (entity);

			// Refresh if not exist
			if (!_textureCache.ContainsKey (entity.SnapshotPath))
				RefreshCache (entity);

			// Not exist
			if (!_textureCache.ContainsKey (entity.SnapshotPath))
				return null;

			// Refresh cache if null
			if(_textureCache [entity.SnapshotPath] == null)
				RefreshCache (entity);

			// Return cached
			return _textureCache [entity.SnapshotPath];
		}

		private void RefreshCache(ISceneEntity entity)
		{
			if (!System.IO.File.Exists (entity.SnapshotPath))
				return;
			
			var bytes = System.IO.File.ReadAllBytes (entity.SnapshotPath);
			Texture2D texture = new Texture2D (2, 2);
			texture.LoadImage (bytes);

			// Refresh Cache
			if (_textureCache.ContainsKey (entity.SnapshotPath)) {
				_textureCache[entity.SnapshotPath] = texture;
			} else {
				_textureCache.Add (entity.SnapshotPath, texture);
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Generates the dictionary of scenes.
		/// </summary>
		private void RefreshDictionary()
		{
			var assets = AssetDatabase.GetAllAssetPaths ();
			// Copy old keys
			var oldKeys = new HashSet<string> ();
			foreach (string key in _dict.Keys) {
				oldKeys.Add (key);
			}

			// Update Dictionary
			foreach (var asset in assets) {
				if (asset.EndsWith (".unity")) {
					var entity = GenerateEntity (asset);
					if (_dict.ContainsKey (asset)) {
						_dict [asset].Copy (entity);
						oldKeys.Remove (asset);
						ReturnToPool (entity);
					} else {
						_dict.Add (asset, entity);
					}
				}
			}

			// Check removed
			foreach (string key in oldKeys) {
				Debug.LogFormat ("Scene [{0}] was removed", key);
				_dict.Remove (key);
			}
		}

		/// <summary>
		/// Adds the build data.
		/// </summary>
		private void AddBuildData()
		{
			var scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; ++i) {

				var scene = scenes [i];
				if (!_dict.ContainsKey (scene.path))
					continue;

				var entity = _dict [scene.path];
				entity.InBuild = true;
				entity.IsEnabled = scene.enabled;
				entity.BuildIndex = i;
				if (i == 0) {
					_firstScene = entity;
				}
				// Add to list
				if (entity.InBuild)
					_buildListByIndex.Add (entity);
			}
		}

		private int SortByIndex(SceneEntity entityA, SceneEntity entityB)
		{
			if (entityA == null && entityB == null)
				return 0;
			if (entityA == null)
				return -1;
			if (entityB == null)
				return 1;

			return entityA.BuildIndex - entityB.BuildIndex;
		}

		/// <summary>
		/// Generates the scene entity based on the asset path.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="assetPath">Asset path.</param>
		private SceneEntity GenerateEntity(string assetPath)
		{
			var entity = GetFromPool ();
			entity.FullPath = assetPath;
			entity.Name = Path.GetFileNameWithoutExtension (assetPath);
			entity.IsEnabled = false;
			entity.InBuild = false;

			return entity;
		}

		/// <summary>
		/// Returns to pool.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void ReturnToPool(SceneEntity entity)
		{
			entity.Clear ();
			_pool.Push (entity);
		}

		private SceneEntity GetFromPool()
		{
			if (_pool.Count <= 0) {
				return new SceneEntity ();
			}

			return _pool.Pop ();
		}
		#endregion
	}
}

