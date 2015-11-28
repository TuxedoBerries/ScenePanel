/// ------------------------------------------------
/// <summary>
/// Scene File Entity
/// Purpose: 	Scene file entity class.
/// Author:		Juan Silva
/// Date: 		November 27, 2015
/// Copyright (c) Tuxedo Berries All rights reserved.
/// </summary>
/// ------------------------------------------------
using System;
using System.Text;
using UnityEngine;

namespace TuxedoBerries.ScenePanel
{
	public class SceneFileEntity : ISceneFileEntity
	{

		private string _name;
		private string _fullPath;

		public SceneFileEntity() : this ("", "") { }

		public SceneFileEntity(string name) : this (name, "") { }

		public SceneFileEntity(string name, string fullPath)
		{
			_name = name;
			_fullPath = fullPath;
		}

		/// <summary>
		/// Gets the name of the scene.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return _name;
			}
			set {
				_name = value;
			}
		}

		/// <summary>
		/// Gets the full path of the scene.
		/// </summary>
		/// <value>The full path.</value>
		public string FullPath {
			get {
				return _fullPath;
			}
			set {
				_fullPath = value;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="TuxedoBerries.ScenePanel.SceneFileEntity"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="TuxedoBerries.ScenePanel.SceneFileEntity"/>.</returns>
		public override string ToString ()
		{
			var builder = new StringBuilder ();
			builder.Append ("{");
			// Name
			builder.Append ("\"name\":");
			builder.Append ("\"");
			builder.Append (Name);
			builder.Append ("\",");
			// Full Path
			builder.Append ("\"full_path\":");
			builder.Append ("\"");
			builder.Append (FullPath);
			builder.Append ("\"");

			builder.Append ("}");
			return builder.ToString ();
		}
	}
}