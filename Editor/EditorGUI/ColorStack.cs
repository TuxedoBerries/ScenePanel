/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 22, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using UnityEngine;
using System.Collections.Generic;

namespace TuxedoBerries.ScenePanel
{
	/// <summary>
	/// Color stack.
	/// Controls the color of the GUI.
	/// </summary>
	public class ColorStack
	{
		private Stack<Color> _stackColor;
		private Color _startingColor;

		public ColorStack ()
		{
			_stackColor = new Stack<Color> ();
			_startingColor = Color.white;
		}

		#region Main Color
		/// <summary>
		/// Reset the colors.
		/// </summary>
		public void Reset ()
		{
			_stackColor.Clear ();
			ApplyCurrentColor ();
		}

		/// <summary>
		/// Push the specified color to GUI.
		/// </summary>
		/// <param name="color">Color.</param>
		public void Push (Color color)
		{
			_stackColor.Push (color);
			ApplyCurrentColor ();
		}

		/// <summary>
		/// Pop this the current color.
		/// </summary>
		public void Pop ()
		{
			if (_stackColor.Count > 0)
				_stackColor.Pop ();
			ApplyCurrentColor ();
		}
		#endregion

		private void ApplyCurrentColor ()
		{
			if (_stackColor.Count <= 0) {
				GUI.color = _startingColor;
				return;
			}

			var color = _stackColor.Peek ();
			GUI.color = color;
		}
	}
}

