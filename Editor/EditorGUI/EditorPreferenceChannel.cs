/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 27, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/

namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	/// <summary>
	/// Editor Preference Handler Channel
	/// Channel between EditorPreferenceHandler and source.
	/// </summary>
	public class EditorPreferenceChannel : IPreferenceChannel
	{
		private EditorPreferenceHandler _instance;
		private System.Type _className;
		private string _instanceName;

		public EditorPreferenceChannel (EditorPreferenceHandler instance, System.Type className, string instanceName)
		{
			_instance = instance;
			_className = className;
			_instanceName = instanceName;
		}

		#region Get Values
		public bool GetBool (string name)
		{
			return _instance.GetBool (_className, _instanceName, name);
		}

		public int GetInt (string name)
		{
			return _instance.GetInt (_className, _instanceName, name);
		}

		public float GetFloat (string name)
		{
			return _instance.GetFloat (_className, _instanceName, name);
		}

		public string GetString (string name)
		{
			return _instance.GetString (_className, _instanceName, name);
		}
		#endregion

		#region Set Values
		public void SetValue (string name, bool value)
		{
			_instance.SetValue (_className, _instanceName, name, value);
		}

		public void SetValue (string name, int value)
		{
			_instance.SetValue (_className, _instanceName, name, value);
		}

		public void SetValue (string name, float value)
		{
			_instance.SetValue (_className, _instanceName, name, value);
		}

		public void SetValue (string name, string value)
		{
			_instance.SetValue (_className, _instanceName, name, value);
		}
		#endregion
	}
}

