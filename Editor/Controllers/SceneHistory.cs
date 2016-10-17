/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 25, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using System.Text;
using System.Collections.Generic;
using TuxedoBerries.ScenePanel.PreferenceHandler;

namespace TuxedoBerries.ScenePanel.Controllers
{
	/// <summary>
	/// Scene history.
	/// Keeps the record of the scenes.
	/// </summary>
	public class SceneHistory
	{
		private Stack<ISceneFileEntity> _backHistory;
		private string [] _backCache;
		private Stack<ISceneFileEntity> _forwardHistory;
		private string [] _forwardCache;
		private IPreferenceChannel _channel;

		public SceneHistory ()
		{
			_backHistory = new Stack<ISceneFileEntity> ();
			_forwardHistory = new Stack<ISceneFileEntity> ();
			_channel = EditorPreferenceHandler.GetChannel (this);
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
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

				return _backHistory.Peek ().FullPath;
			}
		}

		public string CurrentSceneName {
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
		public string [] GetBackStack ()
		{
			if (_backCache == null) {
				_backCache = new string [_backHistory.Count];
				var ienum = _backHistory.GetEnumerator ();
				int index = 0;
				while (ienum.MoveNext ()) {
					_backCache [index] = string.Format ("[{0}]{1}", index, ienum.Current.Name);
					++index;
				}
			}
			return _backCache;
		}

		/// <summary>
		/// Gets the forward stack as an array.
		/// </summary>
		/// <returns>The forward stack.</returns>
		public string [] GetForwardStack ()
		{
			if (_forwardCache == null) {
				_forwardCache = new string [_forwardHistory.Count];
				var ienum = _forwardHistory.GetEnumerator ();
				int index = 0;
				while (ienum.MoveNext ()) {
					_forwardCache [index] = string.Format ("[{0}]{1}", index, ienum.Current.Name);
					++index;
				}
			}
			return _forwardCache;
		}

		/// <summary>
		/// Push the specified scene.
		/// </summary>
		/// <param name="scene">Scene.</param>
		public void Push (ISceneFileEntity scene)
		{
			// Add if empty
			if (_backHistory.Count <= 0) {
				_backHistory.Push (scene);
				_forwardHistory.Clear ();
				ClearStringCache ();
				Save ();
				return;
			}

			// Discard if the same scene
			var peek = _backHistory.Peek ();
			if (string.Equals (peek.Name, scene.Name)) {
				return;
			}

			// Add if not the same
			_backHistory.Push (scene);
			ClearStringCache ();
			_forwardHistory.Clear ();
			Save ();
		}

		/// <summary>
		/// Go back in the stack and restack the element in the Forward histoy.
		/// </summary>
		public ISceneFileEntity Back ()
		{
			return Back (true);
		}

		/// <summary>
		/// Go back in the stack.
		/// If restack is tryu, restack the element in the Forward histoy.
		/// </summary>
		/// <param name="restack">If set to <c>true</c> restack.</param>
		public ISceneFileEntity Back (bool restack)
		{
			if (_backHistory.Count <= 1)
				return null;

			var item = _backHistory.Pop ();
			if (restack) {
				_forwardHistory.Push (item);
			}
			ClearStringCache ();

			if (_backHistory.Count <= 0)
				return null;

			Save ();
			return _backHistory.Peek ();
		}

		/// <summary>
		/// Go Gorward.
		/// </summary>
		public ISceneFileEntity Forward ()
		{
			if (_forwardHistory.Count <= 0)
				return null;

			var item = _forwardHistory.Pop ();
			_backHistory.Push (item);
			ClearStringCache ();

			if (_backHistory.Count <= 0)
				return null;

			Save ();
			return _backHistory.Peek ();
		}

		private void ClearStringCache ()
		{
			_backCache = null;
			_forwardCache = null;
		}

		public void Save ()
		{
			SaveHistory ("_backHistory", _backHistory);
			SaveHistory ("_forwardHistory", _forwardHistory);
		}

		private void SaveHistory (string name, Stack<ISceneFileEntity> stack)
		{
			// Generate Back History
			var builder = new StringBuilder ();
			var ienum = stack.GetEnumerator ();
			int total = stack.Count;
			int currentCount = 0;

			while (ienum.MoveNext ()) {
				var current = ienum.Current;
				builder.Append (current.Name);
				builder.Append (",");
				builder.Append (current.FullPath);
				if (++currentCount < total) {
					builder.Append (";");
				}
			}

			var data = builder.ToString ();
			_channel.SetValue (name, data);
		}

		public void Load ()
		{
			LoadHistory ("_backHistory", _backHistory);
			LoadHistory ("_forwardHistory", _forwardHistory);
			ClearStringCache ();
		}

		private void LoadHistory (string name, Stack<ISceneFileEntity> stack)
		{
			var data = _channel.GetString (name);
			if (string.IsNullOrEmpty (data)) {
				return;
			}

			stack.Clear ();

			var tempStack = new Stack<ISceneFileEntity> ();
			var array = data.Split (';');
			foreach (var element in array) {
				var arrayElement = element.Split (',');

				var sceneElement = new SceneFileEntity (arrayElement [0], arrayElement [1]);
				tempStack.Push (sceneElement);
			}

			foreach (var item in tempStack) {
				stack.Push (item);
			}
		}
	}
}

