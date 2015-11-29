/// ------------------------------------------------
/// <summary>
/// Color Stack
/// Purpose: 	Controls the color of the GUI.
/// Author:		Juan Silva
/// Date: 		November 22, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	public class ColorStack
	{
		private Stack<Color> _stackColor;

		public ColorStack ()
		{
			_stackColor = new Stack<Color> ();
		}

		/// <summary>
		/// Reset the colors.
		/// </summary>
		public void Reset()
		{
			_stackColor.Clear ();
			_stackColor.Push (Color.white);
			ApplyCurrentColor ();
		}

		#region Main Color
		/// <summary>
		/// Push the specified color to GUI.
		/// </summary>
		/// <param name="color">Color.</param>
		public void Push(Color color)
		{
			_stackColor.Push (color);
			ApplyCurrentColor ();
		}

		/// <summary>
		/// Pop this the current color.
		/// </summary>
		public void Pop()
		{
			if(_stackColor.Count > 0)
				_stackColor.Pop ();
			ApplyCurrentColor ();
		}

		private void ApplyCurrentColor()
		{
			if (_stackColor.Count <= 0) {
				GUI.color = Color.gray;
				return;
			}

			var color = _stackColor.Peek ();
			GUI.color = color;
		}
		#endregion
	}
}

