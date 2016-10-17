/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		November 29, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
namespace TuxedoBerries.ScenePanel.Controllers
{
	/// <summary>
	/// Screenshot history.
	/// Keeps the record of the screenshots.
	/// </summary>
	public class ScreenshotHistory : BasePersistantStack<string>
	{
		public ScreenshotHistory () : base ()
		{
		}

		#region Abstract
		/// <summary>
		/// Checks if the two given elements are the same for the stack purpose.
		/// </summary>
		/// <returns><c>true</c>, if equals was ared, <c>false</c> otherwise.</returns>
		/// <param name="elementA">Element a.</param>
		/// <param name="elementB">Element b.</param>
		protected override bool AreEquals (string elementA, string elementB)
		{
			return string.Equals (elementA, elementB);
		}

		/// <summary>
		/// Determines whether the given element is valid for the stack.
		/// </summary>
		/// <returns><c>true</c> if this instance is valid the specified element; otherwise, <c>false</c>.</returns>
		/// <param name="element">Element.</param>
		protected override bool IsValid (string element)
		{
			return !string.IsNullOrEmpty (element);
		}

		/// <summary>
		/// Gets the serialized element for saving purposes.
		/// </summary>
		/// <returns>The serialized element.</returns>
		/// <param name="element">Element.</param>
		protected override string GetSerializedElement (string element)
		{
			return element;
		}

		/// <summary>
		/// Gets the deserialized element for loading purposes.
		/// </summary>
		/// <returns>The deserialized element.</returns>
		/// <param name="element">Element.</param>
		protected override string GetDeserializedElement (string element)
		{
			return element;
		}
		#endregion
	}
}

