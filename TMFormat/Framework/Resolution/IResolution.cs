﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMFormat.Framework.Resolution
{
	public interface IResolution
	{
		/// <summary>
		/// The rectangle of the title safe area (game coords)
		/// </summary>
		Rectangle TitleSafeArea { get; }

		/// <summary>
		/// The rectangle of the entire screen (game coords)
		/// </summary>
		Rectangle ScreenArea { get; }

		/// <summary>
		/// Matrix to convert screen coordinates to game coordinates. Used for mouse clicks, touches, etc.
		/// </summary>
		Matrix ScreenMatrix { get; }

		Point VirtualResolution { get; set; }

		Point ScreenResolution { get; set; }

		/// <summary>
		/// Matrix to convert game coordinates to screen coordinates. Pass into Spritebatch.BeginDraw or this thing won't work
		/// </summary>
		/// <returns></returns>
		Matrix TransformationMatrix();

		/// <summary>
		/// Given a screen coordinate, convert it to the cooresponding game coord
		/// </summary>
		/// <param name="screenCoord"></param>
		/// <returns></returns>
		Vector2 ScreenToGameCoord(Vector2 screenCoord);

		void ResetViewport();
	}
}
