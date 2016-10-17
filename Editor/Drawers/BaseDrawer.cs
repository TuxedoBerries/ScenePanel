/**
 * Author:		Juan Silva <juanssl@gmail.com>
 * Date: 		December 5, 2015
 * Copyright (c) Tuxedo Berries All rights reserved.
 **/
using System;
using TuxedoBerries.ScenePanel.Controllers;
using TuxedoBerries.ScenePanel.Provider;

namespace TuxedoBerries.ScenePanel.Drawers
{
	/// <summary>
	/// Base drawer.
	/// </summary>
	public abstract class BaseDrawer : IDisposable
	{
		protected ColorStack _colorStack;
		protected TextureDatabase _textureDatabase;
		protected GUIContentCache _contentCache;

		public BaseDrawer ()
		{
			_colorStack = new ColorStack ();
			_textureDatabase = TextureDatabaseProvider.GetDatabase (this);
			_contentCache = new GUIContentCache ();
		}

		public virtual void Dispose ()
		{
			TextureDatabaseProvider.ReturnDatabase (this);
		}
	}
}

