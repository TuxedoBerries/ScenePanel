/// ------------------------------------------------
/// <summary>
/// Editor Preference Handler Channel
/// Purpose: 	Channel between EditorPreferenceHandler and source.
/// Author:		Juan Silva
/// Date: 		November 27, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;

namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	public class EditorPreferenceHandlerChannel
	{
		private EditorPreferenceHandler _instance;
		private IEditorPreferenceSection _source;

		public EditorPreferenceHandlerChannel (EditorPreferenceHandler instance, IEditorPreferenceSection source)
		{
			_instance = instance;
			_source = source;
		}

		#region Get Values
		public bool GetBool(string name)
		{
			return _instance.GetBool (_source, name);
		}

		public int GetInt(string name)
		{
			return _instance.GetInt (_source, name);
		}

		public float GetFloat(string name)
		{
			return _instance.GetFloat (_source, name);
		}

		public string GetString(string name)
		{
			return _instance.GetString (_source, name);
		}
		#endregion

		#region Set Values
		public void SetValue(string name, bool value)
		{
			_instance.SetValue (_source, name, value);
		}

		public void SetValue(string name, int value)
		{
			_instance.SetValue (_source, name, value);
		}

		public void SetValue(string name, float value)
		{
			_instance.SetValue (_source, name, value);
		}

		public void SetValue(string name, string value)
		{
			_instance.SetValue (_source, name, value);
		}
		#endregion
	}
}

