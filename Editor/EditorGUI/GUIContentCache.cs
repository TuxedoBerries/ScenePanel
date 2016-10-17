/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 28, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// GUIContent Cache
	/// </summary>
	public class GUIContentCache
	{
		private Dictionary<string, GUIContent> _contentDict;
		private Dictionary<Texture, GUIContent> _contentByTextureDict;

		public GUIContentCache ()
		{
			_contentDict = new Dictionary<string, GUIContent> ();
			_contentByTextureDict = new Dictionary<Texture, GUIContent> ();
		}

		public bool Contains (string key)
		{
			return _contentDict.ContainsKey (key);
		}

		public bool Contains (Texture key)
		{
			return _contentByTextureDict.ContainsKey (key);
		}

		public void Clear ()
		{
			_contentDict.Clear ();
			_contentByTextureDict.Clear ();
		}

		public GUIContent this [string key] {
			get {
				if (!_contentDict.ContainsKey (key))
					return null;
				return _contentDict [key];
			}
			set {
				if (!_contentDict.ContainsKey (key)) {
					_contentDict.Add (key, value);
				} else {
					_contentDict [key] = value;
				}
			}
		}

		public GUIContent this [Texture key] {
			get {
				if (!_contentByTextureDict.ContainsKey (key))
					return null;
				return _contentByTextureDict [key];
			}
			set {
				if (!_contentByTextureDict.ContainsKey (key)) {
					_contentByTextureDict.Add (key, value);
				} else {
					_contentByTextureDict [key] = value;
				}
			}
		}

		public GUIContent GetContent (Texture texture, string tooltip)
		{
			if (!_contentByTextureDict.ContainsKey (texture)) {
				_contentByTextureDict.Add (texture, new GUIContent (texture, tooltip));
			}
			return _contentByTextureDict [texture];
		}

		public GUIContent GetContent (string text, string tooltip)
		{
			if (!_contentDict.ContainsKey (text)) {
				_contentDict.Add (text, new GUIContent (text, tooltip));
			}

			return _contentDict [text];
		}

		public GUIContent GetContent (string text, Texture texture)
		{
			if (!_contentDict.ContainsKey (text)) {
				_contentDict.Add (text, new GUIContent (text, texture));
			}

			return _contentDict [text];
		}

		public GUIContent GetContent (string text, Texture texture, string tooltip)
		{
			if (!_contentDict.ContainsKey (text)) {
				_contentDict.Add (text, new GUIContent (text, texture, tooltip));
			}

			return _contentDict [text];
		}
	}
}

