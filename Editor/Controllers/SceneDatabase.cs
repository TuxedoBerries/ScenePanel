/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel.Controllers
{
	/// <summary>
	/// Scene database.
	/// Databse of the scenes in the project.
	/// </summary>
	public class SceneDatabase
	{
		private SortedDictionary<string, SceneEntity> _dict;
		private List<SceneEntity> _buildListByIndex;
		private Stack<SceneEntity> _pool;
		private SceneEntity _firstScene;
		private SceneEntity _activeScene;
		private int _assetCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="TuxedoBerries.ScenePanel.SceneDatabase"/> class.
		/// </summary>
		public SceneDatabase ()
		{
			_dict = new SortedDictionary<string, SceneEntity> ();
			_pool = new Stack<SceneEntity> ();
			_buildListByIndex = new List<SceneEntity> ();
			Refresh ();
		}

		/// <summary>
		/// Refresh this instance.
		/// </summary>
		public void Refresh ()
		{
			_buildListByIndex.Clear ();
			RefreshDictionary ();
			AddBuildData ();
			_buildListByIndex.Sort (SortByIndex);
		}

		#region Update Entities
		/// <summary>
		/// Sets as active.
		/// </summary>
		/// <returns><c>true</c>, if as active was set, <c>false</c> otherwise.</returns>
		/// <param name="scenePath">Scene path.</param>
		public bool SetAsActive (string scenePath)
		{
			if (string.IsNullOrEmpty (scenePath)) {
				_activeScene = SceneEntity.Empty;
				return true;
			}

			// Same Scene
			if (_activeScene != null && string.Equals (_activeScene.FullPath, scenePath)) {
				_activeScene.IsActive = true;
				return false;
			}

			// Not exist
			if (!_dict.ContainsKey (scenePath))
				return false;

			// Deactivate
			if (_activeScene != null)
				_activeScene.IsActive = false;

			// Set new active scene
			_activeScene = _dict [scenePath];
			_activeScene.IsActive = true;
			return true;
		}

		/// <summary>
		/// Updates the entity.
		/// </summary>
		/// <returns><c>true</c>, if entity was updated, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public bool UpdateEntity (ISceneEntity entity)
		{
			bool retval = true;
			if (entity.InBuild) {
				retval = AddToBuild (entity);
				retval = retval && UpdateIndex (entity);
			} else {
				retval = retval && RemoveToBuild (entity);
			}
			retval = retval && UpdateEnable (entity);
			return retval;
		}

		/// <summary>
		/// Updates the enable.
		/// </summary>
		/// <returns><c>true</c>, if enable was updated, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public bool UpdateEnable (ISceneEntity entity)
		{
			if (!_dict.ContainsKey (entity.FullPath))
				return false;

			if (entity.BuildIndex < 0)
				return false;
			if (entity.BuildIndex >= EditorBuildSettings.scenes.Length)
				return false;

			var current = EditorBuildSettings.scenes [entity.BuildIndex];
			if (current.enabled == entity.IsEnabled)
				return true;

			RefreshBuildSettings ();
			return true;
		}

		/// <summary>
		/// Adds to build.
		/// </summary>
		/// <returns><c>true</c>, if to build was added, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public bool AddToBuild (ISceneEntity entity)
		{
			if (!entity.InBuild)
				return false;

			if (!_dict.ContainsKey (entity.FullPath))
				return false;

			var scene = _dict [entity.FullPath];
			if (_buildListByIndex.Contains (scene))
				return true;

			// Add last
			scene.BuildIndex = _buildListByIndex.Count;
			_buildListByIndex.Add (scene);

			RefreshBuildSettings ();
			return true;
		}

		/// <summary>
		/// Updates the index.
		/// </summary>
		/// <returns><c>true</c>, if index was updated, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public bool UpdateIndex (ISceneEntity entity)
		{
			if (!entity.InBuild)
				return false;

			if (!_dict.ContainsKey (entity.FullPath))
				return false;

			var scene = _dict [entity.FullPath];
			if (_buildListByIndex.IndexOf (scene) == scene.BuildIndex)
				return true;

			RefreshBuildSettings ();
			return true;
		}

		/// <summary>
		/// Removes to build.
		/// </summary>
		/// <returns><c>true</c>, if to build was removed, <c>false</c> otherwise.</returns>
		/// <param name="entity">Entity.</param>
		public bool RemoveToBuild (ISceneEntity entity)
		{
			if (entity.InBuild)
				return false;

			if (!_dict.ContainsKey (entity.FullPath))
				return false;

			var scene = _dict [entity.FullPath];
			if (!_buildListByIndex.Contains (scene))
				return true;

			RefreshBuildSettings ();
			return true;
		}
		#endregion

		#region Access
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
		public bool Contains (string scenePath)
		{
			return _dict.ContainsKey (scenePath);
		}

		/// <summary>
		/// Gets the SceneEntity with the specified scenePath.
		/// </summary>
		/// <param name="scenePath">Scene path.</param>
		public ISceneEntity this [string scenePath] {
			get {
				return _dict [scenePath];
			}
		}

		/// <summary>
		/// Gets the build scenes.
		/// </summary>
		/// <returns>The build scenes.</returns>
		public IEnumerator<ISceneEntity> GetBuildScenes ()
		{
			for (int i = 0; i < _buildListByIndex.Count; ++i) {
				yield return _buildListByIndex [i];
			}
			yield break;
		}

		/// <summary>
		/// Gets all scenes.
		/// </summary>
		/// <returns>The all scenes.</returns>
		public IEnumerator<ISceneEntity> GetAllScenes ()
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
		public IEnumerator<ISceneEntity> GetFavorites ()
		{
			foreach (var data in _dict.Values) {
				if (!data.IsFavorite)
					continue;

				yield return data;
			}
			yield break;
		}
		#endregion

		#region Export
		/// <summary>
		/// Generates a JSON representation of the scenes.
		/// </summary>
		/// <returns>The JSON string.</returns>
		public string GenerateJSON ()
		{
			var builder = new StringBuilder ();
			builder.Append ("{");

			int total = _dict.Count;
			int current = 0;
			foreach (var data in _dict.Values) {
				builder.Append ("\"");
				builder.Append (data.Name);
				builder.Append ("\":");
				builder.Append (data.ToString ());

				++current;
				if (current < total)
					builder.Append (",");
			}
			builder.Append ("}");
			return builder.ToString ();
		}
		#endregion

		#region Helpers
		private void RefreshBuildSettings ()
		{
			// Debug.Log ("Refreshing Editor Build Settings");
			_buildListByIndex.Sort (SceneEntity.CompareByIndex);

			int total = 0;
			foreach (var scene in _buildListByIndex) {
				if (!scene.InBuild)
					continue;
				++total;
			}

			// Create new array
			var newArray = new EditorBuildSettingsScene [total];

			// Update Data
			int count = 0;
			for (int i = 0; i < _buildListByIndex.Count; ++i) {
				var scene = _buildListByIndex [i];
				if (!scene.InBuild)
					continue;
				newArray [count++] = new EditorBuildSettingsScene (scene.FullPath, scene.IsEnabled);
			}

			// Reassign
			EditorBuildSettings.scenes = newArray;
			Refresh ();
		}

		/// <summary>
		/// Generates the dictionary of scenes.
		/// </summary>
		private void RefreshDictionary ()
		{
			var assets = AssetDatabase.GetAllAssetPaths ();
			if (_assetCount == assets.Length)
				return;

			_assetCount = assets.Length;
			// Copy old keys
			var oldKeys = new HashSet<string> ();
			foreach (string key in _dict.Keys) {
				oldKeys.Add (key);
			}

			// Update Dictionary
			for (int i = 0; i < assets.Length; ++i) {
				var asset = assets [i];
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
				_dict.Remove (key);
			}
		}

		/// <summary>
		/// Adds the build data.
		/// </summary>
		private void AddBuildData ()
		{
			var scenes = EditorBuildSettings.scenes;
			for (int i = 0; i < scenes.Length; ++i) {
				var scene = scenes [i];
				if (!_dict.ContainsKey (scene.path))
					continue;

				var entity = _dict [scene.path];
				entity.Scene = scene;
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

		private int SortByIndex (SceneEntity entityA, SceneEntity entityB)
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
		private SceneEntity GenerateEntity (string assetPath)
		{
			var entity = GetFromPool ();
			entity.Name = Path.GetFileNameWithoutExtension (assetPath);
			entity.FullPath = assetPath;
			entity.GUID = AssetDatabase.AssetPathToGUID (assetPath);
			entity.IsEnabled = false;
			entity.InBuild = false;

			return entity;
		}

		/// <summary>
		/// Returns to pool.
		/// </summary>
		/// <param name="entity">Entity.</param>
		private void ReturnToPool (SceneEntity entity)
		{
			entity.Clear ();
			_pool.Push (entity);
		}

		private SceneEntity GetFromPool ()
		{
			if (_pool.Count <= 0) {
				return new SceneEntity ();
			}

			return _pool.Pop ();
		}
		#endregion
	}
}

