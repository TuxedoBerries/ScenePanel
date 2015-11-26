/// ------------------------------------------------
/// <summary>
/// Scene History
/// Purpose: 	Keeps the record of the scenes.
/// Author:		Juan Silva
/// Date: 		November 25, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class SceneHistory
	{
		private Stack<ISceneEntity> _backHistory;
		private string[] _backCache;
		private Stack<ISceneEntity> _forwardHistory;
		private string[] _forwardCache;

		public SceneHistory ()
		{
			_backHistory = new Stack<ISceneEntity> ();
			_forwardHistory = new Stack<ISceneEntity> ();
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			_backHistory.Clear ();
			_forwardHistory.Clear ();
			ClearStringCache ();
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return _backHistory.Count + _forwardHistory.Count;
			}
		}

		/// <summary>
		/// Gets the back count.
		/// </summary>
		/// <value>The back count.</value>
		public int BackCount {
			get {
				return _backHistory.Count;
			}
		}

		/// <summary>
		/// Gets the foward count.
		/// </summary>
		/// <value>The foward count.</value>
		public int FowardCount {
			get {
				return _forwardHistory.Count;
			}
		}

		/// <summary>
		/// Gets the current scene.
		/// </summary>
		/// <value>The current scene.</value>
		public string CurrentScene {
			get {
				if (_backHistory.Count <= 0)
					return null;

				return _backHistory.Peek ().Name;
			}
		}

		/// <summary>
		/// Gets the Back stack as an array.
		/// </summary>
		/// <returns>The stack.</returns>
		public string[] GetBackStack()
		{
			if (_backCache == null) {
				_backCache = new string[_backHistory.Count];
				var ienum = _backHistory.GetEnumerator ();
				int index = 0;
				while (ienum.MoveNext ()) {
					_backCache [index] = string.Format("[{0}]{1}", index, ienum.Current.Name);
					++index;
				}
			}
			return _backCache;
		}

		/// <summary>
		/// Gets the forward stack as an array.
		/// </summary>
		/// <returns>The forward stack.</returns>
		public string[] GetForwardStack()
		{
			if (_forwardCache == null) {
				_forwardCache = new string[_forwardHistory.Count];
				var ienum = _forwardHistory.GetEnumerator ();
				int index = 0;
				while (ienum.MoveNext ()) {
					_forwardCache [index] = string.Format("[{0}]{1}", index, ienum.Current.Name);
					++index;
				}
			}
			return _forwardCache;
		}

		/// <summary>
		/// Push the specified scene.
		/// </summary>
		/// <param name="scene">Scene.</param>
		public void Push(ISceneEntity scene)
		{
			// Add if empty
			if (_backHistory.Count <= 0) {
				_backHistory.Push (scene);
				_forwardHistory.Clear ();
				ClearStringCache ();
				return;
			}

			// Discard if the same scene
			var peek = _backHistory.Peek();
			if (string.Equals(peek.Name, scene.Name)) {
				return;
			}

			// Add if not the same
			_backHistory.Push (scene);
			ClearStringCache ();
			_forwardHistory.Clear ();
		}

		/// <summary>
		/// Go Back.
		/// </summary>
		public ISceneEntity Back()
		{
			if (_backHistory.Count <= 1)
				return null;
			
			var item = _backHistory.Pop ();
			_forwardHistory.Push (item);
			ClearStringCache ();

			if (_backHistory.Count <= 0)
				return null;
			return _backHistory.Peek ();
		}

		public ISceneEntity Forward()
		{
			if (_forwardHistory.Count <= 0)
				return null;

			var item = _forwardHistory.Pop ();
			_backHistory.Push (item);
			ClearStringCache ();

			if (_backHistory.Count <= 0)
				return null;
			return _backHistory.Peek ();
		}

		private void ClearStringCache()
		{
			_backCache = null;
			_forwardCache = null;
		}
	}
}

