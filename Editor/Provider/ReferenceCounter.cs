/// ------------------------------------------------
/// <summary>
/// Reference Counter
/// Purpose: 	Count reference for a specific class.
/// Author:		Juan Silva
/// Date: 		December 5, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel.Provider
{
	public class ReferenceCounter<T>
		where T : class, new()
	{
		private T _classInstance;
		private HashSet<object> _sources;
		private static SceneDatabaseProvider _instance;

		public ReferenceCounter()
		{
			_classInstance = new T ();
			_sources = new HashSet<object> ();
		}

		public T ClassInstance {
			get {
				return _classInstance;
			}
		}

		public void AddSource(object source)
		{
			if (_sources.Contains (source))
				return;
			_sources.Add (source);
		}

		public void RemoveSource(object source)
		{
			if (!_sources.Contains (source))
				return;

			_sources.Remove (source);
		}

		public int Count {
			get {
				return _sources.Count;
			}
		}
	}
}

