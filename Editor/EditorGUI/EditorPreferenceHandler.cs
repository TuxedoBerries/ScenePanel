/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 27, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEditor;

namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	/// <summary>
	/// Editor Preference Handler
	/// Handles all the preferences in editor.
	/// </summary>
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

		public static IPreferenceChannel GetChannel(object obj, string instanceName)
		{
			return new EditorPreferenceChannel (Instance, obj.GetType(), instanceName);
		}

		public static IPreferenceChannel GetChannel(object obj)
		{
			return new EditorPreferenceUniqueChannel (Instance, obj.GetType());
		}

		#region Get Values
		public bool GetBool(System.Type className, string instanceName, string name)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(className, instanceName), name);
			return EditorPrefs.GetBool (key);
		}

		public int GetInt(System.Type className, string instanceName, string name)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(className, instanceName), name);
			return EditorPrefs.GetInt (key);
		}

		public float GetFloat(System.Type className, string instanceName, string name)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(className, instanceName), name);
			return EditorPrefs.GetFloat (key);
		}

		public string GetString(System.Type className, string instanceName, string name)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(className, instanceName), name);
			return EditorPrefs.GetString (key);
		}
		#endregion

		#region Set Values
		public void SetValue(System.Type className, string instanceName, string name, bool value)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(className, instanceName), name);
			EditorPrefs.SetBool (key, value);
		}

		public void SetValue(System.Type className, string instanceName, string name, int value)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(className, instanceName), name);
			EditorPrefs.SetInt (key, value);
		}

		public void SetValue(System.Type className, string instanceName, string name, float value)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(className, instanceName), name);
			EditorPrefs.SetFloat (key, value);
		}

		public void SetValue(System.Type className, string instanceName, string name, string value)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(className, instanceName), name);
			EditorPrefs.SetString (key, value);
		}
		#endregion

		#region Get Values - No instance name
		public bool GetBool(System.Type className, string name)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(className), name);
			return EditorPrefs.GetBool (key);
		}

		public int GetInt(System.Type className, string name)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(className), name);
			return EditorPrefs.GetInt (key);
		}

		public float GetFloat(System.Type className, string name)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(className), name);
			return EditorPrefs.GetFloat (key);
		}

		public string GetString(System.Type className, string name)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(className), name);
			return EditorPrefs.GetString (key);
		}
		#endregion

		#region Set Values - No instance name
		public void SetValue(System.Type className, string name, bool value)
		{
			string key = string.Format ("{0}/bool:{1}", GetBaseName(className), name);
			EditorPrefs.SetBool (key, value);
		}

		public void SetValue(System.Type className, string name, int value)
		{
			string key = string.Format ("{0}/int:{1}", GetBaseName(className), name);
			EditorPrefs.SetInt (key, value);
		}

		public void SetValue(System.Type className, string name, float value)
		{
			string key = string.Format ("{0}/float:{1}", GetBaseName(className), name);
			EditorPrefs.SetFloat (key, value);
		}

		public void SetValue(System.Type className, string name, string value)
		{
			string key = string.Format ("{0}/string:{1}", GetBaseName(className), name);
			EditorPrefs.SetString (key, value);
		}
		#endregion

		private string GetBaseName(System.Type className)
		{
			return string.Format ("{0}//{1}", MAIN_KEY, className.Name);
		}

		private string GetBaseName(System.Type className, string instanceName)
		{
			return string.Format ("{0}//{1}:{2}", MAIN_KEY, className, instanceName);
		}
	}
}

