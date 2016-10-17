/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 27, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	/// <summary>
	/// Editor Preference Unique Channel
	/// Channel between EditorPreferenceHandler and source.
	/// </summary>
	public class EditorPreferenceUniqueChannel : IPreferenceChannel
	{
		private EditorPreferenceHandler _instance;
		private System.Type _className;

		public EditorPreferenceUniqueChannel (EditorPreferenceHandler instance, System.Type className)
		{
			_instance = instance;
			_className = className;
		}

		#region Get Values
		public bool GetBool (string name)
		{
			return _instance.GetBool (_className, name);
		}

		public int GetInt (string name)
		{
			return _instance.GetInt (_className, name);
		}

		public float GetFloat (string name)
		{
			return _instance.GetFloat (_className, name);
		}

		public string GetString (string name)
		{
			return _instance.GetString (_className, name);
		}
		#endregion

		#region Set Values
		public void SetValue (string name, bool value)
		{
			_instance.SetValue (_className, name, value);
		}

		public void SetValue (string name, int value)
		{
			_instance.SetValue (_className, name, value);
		}

		public void SetValue (string name, float value)
		{
			_instance.SetValue (_className, name, value);
		}

		public void SetValue (string name, string value)
		{
			_instance.SetValue (_className, name, value);
		}
		#endregion
	}
}

