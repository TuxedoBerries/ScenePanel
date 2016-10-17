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
	/// Base persistant stack.
	/// Keeps the record of the specific data.
	/// </summary>
	public abstract class BasePersistantStack<T>
		where T : class
	{
		private Stack<T> _backHistory;
		private IPreferenceChannel _channel;
		private bool _autoSave = true;

		public BasePersistantStack ()
		{
			_backHistory = new Stack<T> ();
			_channel = EditorPreferenceHandler.GetChannel (this);
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear ()
		{
			_backHistory.Clear ();
		}

		/// <summary>
		/// Enables or disable the auto save.
		/// </summary>
		/// <value><c>true</c> if auto save; otherwise, <c>false</c>.</value>
		public bool AutoSave {
			get {
				return _autoSave;
			}
			set {
				_autoSave = false;
			}
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count {
			get {
				return _backHistory.Count;
			}
		}

		/// <summary>
		/// Gets the current Element.
		/// </summary>
		/// <value>The current scene.</value>
		public T Current {
			get {
				if (_backHistory.Count <= 0)
					return null;

				return _backHistory.Peek ();
			}
		}

		public IEnumerator<T> History {
			get {
				var ienum = _backHistory.GetEnumerator ();
				while (ienum.MoveNext ()) {
					yield return ienum.Current;
				}
			}
		}

		/// <summary>
		/// Push the specified scene.
		/// </summary>
		/// <param name="scene">Scene.</param>
		public void Push (T element)
		{
			if (!IsValid (element))
				return;

			// Add if empty
			if (_backHistory.Count <= 0) {
				_backHistory.Push (element);
				ExecuteAutoSave ();
				return;
			}

			// Discard if the same element
			var peek = _backHistory.Peek ();
			if (AreEquals (peek, element)) {
				return;
			}

			// Add if not the same
			_backHistory.Push (element);
			ExecuteAutoSave ();
		}

		/// <summary>
		/// Pop the last element in the stack
		/// </summary>
		/// <param name="restack">If set to <c>true</c> restack.</param>
		public T Pop ()
		{
			if (_backHistory.Count <= 0)
				return null;

			var item = _backHistory.Pop ();
			ExecuteAutoSave ();
			return item;
		}

		private void ExecuteAutoSave ()
		{
			if (_autoSave)
				Save ();
		}

		public void Save ()
		{
			SaveHistory ("_backHistory", _backHistory);
		}

		private void SaveHistory (string name, Stack<T> stack)
		{
			// Generate Back History
			var builder = new StringBuilder ();
			var ienum = stack.GetEnumerator ();
			int total = stack.Count;
			int currentCount = 0;

			while (ienum.MoveNext ()) {
				var current = ienum.Current;
				builder.Append (GetSerializedElement (current));
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
		}

		private void LoadHistory (string name, Stack<T> stack)
		{
			var data = _channel.GetString (name);
			if (string.IsNullOrEmpty (data)) {
				return;
			}

			stack.Clear ();

			var tempStack = new Stack<T> ();
			var array = data.Split (';');
			foreach (var element in array) {
				var sceneElement = GetDeserializedElement (element);
				tempStack.Push (sceneElement);
			}

			foreach (var item in tempStack) {
				stack.Push (item);
			}
		}

		#region Abstract
		/// <summary>
		/// Checks if the two given elements are the same for the stack purpose.
		/// </summary>
		/// <returns><c>true</c>, if equals was ared, <c>false</c> otherwise.</returns>
		/// <param name="elementA">Element a.</param>
		/// <param name="elementB">Element b.</param>
		protected abstract bool AreEquals (T elementA, T elementB);

		/// <summary>
		/// Determines whether the given element is valid for the stack.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid the specified element; otherwise, <c>false</c>.</returns>
		/// <param name="element">Element.</param>
		protected abstract bool IsValid (T element);

		/// <summary>
		/// Gets the serialized element for saving purposes.
		/// </summary>
		/// <returns>The serialized element.</returns>
		/// <param name="element">Element.</param>
		protected abstract string GetSerializedElement (T element);

		/// <summary>
		/// Gets the deserialized element for loading purposes.
		/// </summary>
		/// <returns>The deserialized element.</returns>
		/// <param name="element">Element.</param>
		protected abstract T GetDeserializedElement (string element);
		#endregion
	}
}

