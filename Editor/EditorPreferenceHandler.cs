/// ------------------------------------------------
/// <summary>
/// Editor Preference Handler
/// Purpose: 	Handles all the preferences in editor.
/// Author:		Juan Silva
/// Date: 		November 27, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEditor;

namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	public class EditorPreferenceHandler
	{
		private const string MAIN_KEY = "EditorPreferenceHandler";
		private static EditorPreferenceHandler _instance;

		private EditorPreferenceHandler ()
		{
			// Empty Constructor... For now

		}

		public static EditorPreferenceHandler Instance {
			get {
				if (_instance == null) {
					_instance = new EditorPreferenceHandler ();
				}
				return _instance;
			}
		}

		public static EditorPreferenceHandlerChannel GetChannel(IEditorPreferenceSection section)
		{
			return new EditorPreferenceHandlerChannel (Instance, section);
		}

		#region Get Values
		public bool GetBool(IEditorPreferenceSection sectionFrom, string name)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(sectionFrom), name);
			return EditorPrefs.GetBool (key);
		}

		public int GetInt(IEditorPreferenceSection sectionFrom, string name)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(sectionFrom), name);
			return EditorPrefs.GetInt (key);
		}

		public float GetFloat(IEditorPreferenceSection sectionFrom, string name)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(sectionFrom), name);
			return EditorPrefs.GetFloat (key);
		}

		public string GetString(IEditorPreferenceSection sectionFrom, string name)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(sectionFrom), name);
			return EditorPrefs.GetString (key);
		}
		#endregion

		#region Set Values
		public void SetValue(IEditorPreferenceSection sectionFrom, string name, bool value)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(sectionFrom), name);
			EditorPrefs.SetBool (key, value);
		}

		public void SetValue(IEditorPreferenceSection sectionFrom, string name, int value)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(sectionFrom), name);
			EditorPrefs.SetInt (key, value);
		}

		public void SetValue(IEditorPreferenceSection sectionFrom, string name, float value)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(sectionFrom), name);
			EditorPrefs.SetFloat (key, value);
		}

		public void SetValue(IEditorPreferenceSection sectionFrom, string name, string value)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(sectionFrom), name);
			EditorPrefs.SetString (key, value);
		}
		#endregion

		private string GetBaseName(IEditorPreferenceSection sectionFrom)
		{
			return string.Format ("{0}//{1}:{2}", MAIN_KEY, sectionFrom.ImplementationType.Name, sectionFrom.Name);
		}
	}
}

