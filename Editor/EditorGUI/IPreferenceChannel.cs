/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 28, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
namespace TuxedoBerries.ScenePanel.PreferenceHandler
{
	/// <summary>
	/// IPreference channel.
	/// Interface for a Editor Preference Channel.
	/// </summary>
	public interface IPreferenceChannel
	{

		#region Get Values
		/// <summary>
		/// Gets a bool value for a field.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		bool GetBool (string name);

		/// <summary>
		/// Gets the int value for a field.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="name">Name.</param>
		int GetInt (string name);

		/// <summary>
		/// Gets the float value for a field.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="name">Name.</param>
		float GetFloat (string name);

		/// <summary>
		/// Gets the string value for a field.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="name">Name.</param>
		string GetString (string name);
		#endregion

		#region Set Values
		/// <summary>
		/// Sets a bool value for a field.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">If set to <c>true</c> value.</param>
		void SetValue (string name, bool value);

		/// <summary>
		/// Sets a int value for a field.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		void SetValue (string name, int value);

		/// <summary>
		/// Sets a float value for a field.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		void SetValue (string name, float value);

		/// <summary>
		/// Sets a string value for a field.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		void SetValue (string name, string value);
		#endregion
	}
}

