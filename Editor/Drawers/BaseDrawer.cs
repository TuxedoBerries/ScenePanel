/// ------------------------------------------------
/// <summary>
/// Base Drawer
/// Purpose: 	Base Drawer.
/// Author:		Juan Silva
/// Date: 		December 5, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using TuxedoBerries.ScenePanel.Controllers;
using TuxedoBerries.ScenePanel.Provider;

namespace TuxedoBerries.ScenePanel.Drawers
{
	public abstract class BaseDrawer : IDisposable
	{
		protected ColorStack _colorStack;
		protected TextureDatabase _textureDatabase;
		protected GUIContentCache _contentCache;

		public BaseDrawer()
		{
			_colorStack = new ColorStack ();
			_textureDatabase = TextureDatabaseProvider.GetDatabase (this);
			_contentCache = new GUIContentCache ();
		}

		public virtual void Dispose()
		{
			TextureDatabaseProvider.ReturnDatabase (this);
		}
	}
}

