﻿//Copyright (c) 2010 Shane Baker
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using OchreGui.Utility;
using libtcod;

namespace OchreGui
{
    #region Enums
    /// <summary>
    /// Text alignment in the horizontal direction
    /// </summary>
    public enum HorizontalAlignment
    {
        /// <summary>
        /// Text is placed on the far left side.
        /// </summary>
        Left,
        /// <summary>
        /// Text is placed halfway between the left and right sides.
        /// </summary>
        Center,
        /// <summary>
        /// Text is placed on the far right side.
        /// </summary>
        Right
    };

    /// <summary>
    /// Text alignment in the vertical direction.
    /// </summary>
    public enum VerticalAlignment
    {
        /// <summary>
        /// Text is placed at the very top.
        /// </summary>
        Top,
        /// <summary>
        /// Text is placed halfway between the top and bottom.
        /// </summary>
        Center,
        /// <summary>
        /// Text is placed at the very bottom.
        /// </summary>
        Bottom
    }
    #endregion

    #region Canvas Class
    /// <summary>
    /// A canvas is basically a wrapper around an off-screen TCODConsole.  Every window
    /// exposes a Canvas property to provide drawing functionality.  Drawing can be performed
    /// using the provided Canvas methods, or by exposing the underlying TCODConsole object
    /// through the Console property.
    /// </summary>
    public class Canvas : IDisposable
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Canvas object of the given size.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the specified
        /// <paramref name="size"/> is larger than the screen size</exception>
        public Canvas(Size size)
        {
            if (size.Width > Application.ScreenSize.Width ||
                size.Height > Application.ScreenSize.Height)
            {
                throw new ArgumentOutOfRangeException("size", "The specified size must be equal to or smaller than the screen size");
            }

            Console = new TCODConsole(size.Width, size.Height);
            this.Size = size;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Canvas object with the given dimensions.
        /// </summary>
        public Canvas(int width, int height)
            : this(new Size(width, height))
        {
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the size of the canvas
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// Get the TCODConsole associated with this Canvas
        /// </summary>
        public TCODConsole Console { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the default pigment for this Canvas.  If no other pigment is specified
        /// for drawing operations, this pigment is used.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="pigment"/> 
        /// is null</exception>
        public void SetDefaultColors(Pigment pigment)
        {
            if (pigment == null)
                throw new ArgumentNullException("pigment");

            defaultColors = pigment;
            SetColors(pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the colors of a single character at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="pigment"/>
        /// is null</exception>
        public void SetCharColors(int x, int y, Pigment pigment)
        {
            if (pigment == null)
                throw new ArgumentNullException("pigment");

            Console.setCharBackground(x, y, pigment.Background.GetTCODColor());
            Console.setCharForeground(x, y, pigment.Foreground.GetTCODColor());
        }

        /// <summary>
        /// Sets the colors of a single character at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when pigment is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="position"/> is outside of this Canvas region</exception>
        public void SetCharColors(Point position, Pigment pigment)
        {
            if (pigment == null)
                throw new ArgumentNullException("pigment");

            if (position.X < 0 || position.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("position", "The specified x coordinate is invalid.");
            }

            if (position.Y < 0 || position.Y >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("position", "The specified y coordinate is invalid.");
            }

            SetCharColors(position.X, position.Y, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clear the Canvas using the default pigment
        /// </summary>
        public void Clear()
        {
            Console.clear();
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints a single character at the specified coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void PrintChar(int x, int y, int character, Pigment pigment = null)
        {
            if (x < 0 || x >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("x", "The specified x coordinate is invalid.");
            }

            if (y < 0 || y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("y", "The specified y coordinate is invalid.");
            }

            if (pigment != null)
                SetColors(pigment);

            Console.putChar(x, y, character);

            if (pigment != null)
                SetColors(defaultColors);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints a single character at the specified coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        public void PrintChar(Point lPos, int character, Pigment pigment = null)
        {
            if (lPos.X < 0 || lPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified x coordinate is invalid.");
            }

            if (lPos.Y < 0 || lPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified y coordinate is invalid.");
            }

            PrintChar(lPos.X, lPos.Y, character,pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void PrintString(int x, int y, string text, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (x < 0 || x >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("x", "The specified x coordinate is invalid.");
            }

            if (y < 0 || y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("y", "The specified y coordinate is invalid.");
            }

            if (pigment != null)
                SetColors(pigment);

            Console.print(x, y, text);

            if (pigment != null)
                SetColors(defaultColors);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        public void PrintString(Point lPos, string text, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (lPos.X < 0 || lPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified x coordinate is invalid.");
            }

            if (lPos.Y < 0 || lPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified y coordinate is invalid.");
            }

            PrintString(lPos.X, lPos.Y, text, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// horizontally with the specified alignment and within the specified field length.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void PrintStringAligned(int x, int y, string text,
            HorizontalAlignment alignment, int fieldLength, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (x < 0 || x >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("x", "The specified x coordinate is invalid.");
            }

            if (y < 0 || y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("y", "The specified y coordinate is invalid.");
            }

            Point pos = GetHorAlign(new Point(x, y), text, alignment, fieldLength);

            PrintString(pos, text, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// horizontally with the specified alignment and within the specified field length.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        public void PrintStringAligned(Point lPos, string text,
            HorizontalAlignment alignment, int fieldLength, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (lPos.X < 0 || lPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified x coordinate is invalid.");
            }

            if (lPos.Y < 0 || lPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified y coordinate is invalid.");
            }

            PrintStringAligned(lPos.X, lPos.Y, text, alignment, fieldLength, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// both horizontally and vertically with the specified alignments, and within
        /// the specified size of the field.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void PrintStringAligned(int x, int y, string text, HorizontalAlignment hAlign,
            VerticalAlignment vAlign, Size fieldSize, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (x < 0 || x >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("x", "The specified x coordinate is invalid.");
            }

            if (y < 0 || y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("y", "The specified y coordinate is invalid.");
            }

            Point pos = GetHVAlign(new Point(x, y), text, hAlign, vAlign, fieldSize);

            PrintString(pos, text, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// both horizontally and vertically with the specified alignments, and within
        /// the specified size of the field.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        public void PrintStringAligned(Point lPos, string text, HorizontalAlignment hAlign,
            VerticalAlignment vAlign, Size fieldSize, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (lPos.X < 0 || lPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified x coordinate is invalid.");
            }

            if (lPos.Y < 0 || lPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("lPos", "The specified y coordinate is invalid.");
            }

            PrintStringAligned(lPos.X, lPos.Y, text, hAlign, vAlign, fieldSize, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Draws a horizontal line of the given length and starting coordinates
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void DrawHLine(int startX, int startY, int length, Pigment pigment = null)
        {
            if (startX < 0 || startX >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("startX", "The specified x coordinate is invalid.");
            }

            if (startY < 0 || startY >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("startY", "The specified y coordinate is invalid.");
            }

            if (pigment != null)
                SetColors(pigment);

            Console.hline(startX, startY, length);

            if (pigment != null)
                SetColors(defaultColors);
        }

        /// <summary>
        /// Draws a horizontal line of the given length and starting coordinates
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="startPos"/> is outside of this Canvas region</exception>
        public void DrawHLine(Point startPos, int length, Pigment pigment = null)
        {
            if (startPos.X < 0 || startPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("startPos", "The specified x coordinate is invalid.");
            }

            if (startPos.Y < 0 || startPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("startPos", "The specified y coordinate is invalid.");
            }

            DrawHLine(startPos.X, startPos.Y, length, pigment);
        }

        /// <summary>
        /// Draws a vertical line of the given length and starting coordinates
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        public void DrawVLine(int x, int y, int length, Pigment pigment = null)
        {
            if (x < 0 || x >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("x", "The specified x coordinate is invalid.");
            }

            if (y < 0 || y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("y", "The specified y coordinate is invalid.");
            }

            if (pigment != null)
                SetColors(pigment);

            Console.vline(x, y, length);

            if (pigment != null)
                SetColors(defaultColors);
        }

        /// <summary>
        /// Draws a vertical line of the given length and starting coordinates
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="startPos"/> is outside of this Canvas region</exception>
        public void DrawVLine(Point startPos, int length, Pigment pigment = null)
        {
            if (startPos.X < 0 || startPos.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("startPos", "The specified x coordinate is invalid.");
            }

            if (startPos.Y < 0 || startPos.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("startPos", "The specified y coordinate is invalid.");
            }

            DrawVLine(startPos.X, startPos.Y, length, pigment);
        }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits this Canvas to the screen at the given coordinates in screen space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ToScreen(int x, int y)
        {
            int maxWidth = Application.ScreenSize.Width - x;
            int maxHeight = Application.ScreenSize.Height - y;

            if (maxWidth < 1 || maxHeight < 1)
            {
                return;
            }

            int finalWidth = Size.Width;
            int finalHeight = Size.Height;

            if (finalWidth > maxWidth)
            {
                finalWidth = maxWidth;
            }
            if (finalHeight > maxHeight)
            {
                finalHeight = maxHeight;
            }

            Size finalSize = new Size(finalWidth, finalHeight);
            
            TCODConsole.blit(Console,
                0, 0,
                finalSize.Width,
                finalSize.Height,
                TCODConsole.root,
                x,
                y);
        }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits this Canvas to the screen at the given coordinates.
        /// </summary>
        /// <param name="sPos">Position given in screen coordinate space</param>
        public void ToScreen(Point sPos)
        {
            ToScreen(sPos.X, sPos.Y);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits this Canvas to the screen at the given coordinates and using the provided
        /// foreground and background alpha values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="fgAlpha"></param>
        /// <param name="bgAlpha"></param>
        public void ToScreenAlpha(int x, int y, float fgAlpha, float bgAlpha)
        {
            int maxWidth = Application.ScreenSize.Width - x;
            int maxHeight = Application.ScreenSize.Height - y;

            if (maxWidth < 1 || maxHeight < 1)
            {
                return;
            }

            int finalWidth = Size.Width;
            int finalHeight = Size.Height;

            if (finalWidth > maxWidth)
            {
                finalWidth = maxWidth;
            }
            if (finalHeight > maxHeight)
            {
                finalHeight = maxHeight;
            }

            Size finalSize = new Size(finalWidth, finalHeight);

            TCODConsole.blit(Console,
                0, 0,
                finalSize.Width,
                finalSize.Height,
                TCODConsole.root,
                x,
                y,
                fgAlpha,
                bgAlpha);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits this Canvas to the screen at the given coordinates and using the provided
        /// foreground and background alpha values.
        /// </summary>
        /// <param name="sPos"></param>
        /// <param name="fgAlpha"></param>
        /// <param name="bgAlpha"></param>
        public void ToScreenAlpha(Point sPos, float fgAlpha, float bgAlpha)
        {
            ToScreenAlpha(sPos.X, sPos.Y, fgAlpha, bgAlpha);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits the provided Canvas to this Canvas at the specified position
        /// </summary>
        /// <param name="source">The destination position in local (to this) coordinate space</param>
        /// <param name="destPos"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="source"/> is null</exception>
        public void Blit(Canvas source, Point destPos)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            Blit(source, destPos.X, destPos.Y);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Blits the provided Canvas to this Canvas at the specified position 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="source"/> is null</exception>
        public void Blit(Canvas source, int x, int y)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int maxWidth = Application.ScreenSize.Width - x;
            int maxHeight = Application.ScreenSize.Height - y;

            if (maxWidth < 1 || maxHeight < 1)
            {
                return;
            }

            int finalWidth = source.Size.Width;
            int finalHeight = source.Size.Height;

            if (finalWidth > maxWidth)
            {
                finalWidth = maxWidth;
            }
            if (finalHeight > maxHeight)
            {
                finalHeight = maxHeight;
            }

            Size finalSize = new Size(finalWidth, finalHeight);

            TCODConsole.blit(source.Console,
                0, 0,
                finalSize.Width,
                finalSize.Height,
                this.Console,
                x,
                y);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints a frame border around the canvas, with an optional centered title.
        /// </summary>
        public void PrintFrame(string title, Pigment pigment = null)
        {
            if (pigment != null)
                SetColors(pigment);

            if (string.IsNullOrEmpty(title))
            {
                Console.printFrame(0, 0,
                    this.Size.Width, this.Size.Height,
                    false);
            }
            else
            {
                Console.printFrame(0, 0,
                    this.Size.Width, this.Size.Height,
                    false,
                    TCODBackgroundFlag.Set,
                    title);
            }

            if (pigment != null)
                SetColors(defaultColors);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        public void Scroll(int deltaX,int deltaY)
        {
            using (Canvas tmpCanvas = new Canvas(this.Size))
            {

            }
        }



        /// <summary>
        /// Get the size, in pixels, of a single character, as per TCODSystem.getCharSize().
        /// </summary>
        /// <returns></returns>
        static public Size GetCharSize()
        {
            int w, h;

            TCODSystem.getCharSize(out w, out h);

            return new Size(w, h);
        }
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private void SetColors(Pigment pigment)
        {
            Console.setBackgroundColor(pigment.Background.GetTCODColor());
            Console.setBackgroundFlag(pigment.BackgroundFlag);
            Console.setForegroundColor(pigment.Foreground.GetTCODColor());
        }
        // /////////////////////////////////////////////////////////////////////////////////
        private Pigment defaultColors;
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private static Point GetHorAlign(Point lPos, string str, HorizontalAlignment align, int fieldLength)
        {
            int startX = 0;

            switch (align)
            {
                case HorizontalAlignment.Center:
                    startX = (fieldLength - str.Length) / 2;
                    break;

                case HorizontalAlignment.Right:
                    startX = fieldLength - str.Length;
                    break;
            }

            Point pos = new Point(lPos.X + startX, lPos.Y);

            return pos;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private static Point GetHVAlign(Point lPos, string str, HorizontalAlignment hAlign, VerticalAlignment vAlign, Size fieldSize)
        {
            int startX = 0;
            int startY = 0;

            switch (hAlign)
            {
                case HorizontalAlignment.Center:
                    startX = (fieldSize.Width - str.Length) / 2;
                    break;

                case HorizontalAlignment.Right:
                    startX = (fieldSize.Width - str.Length);
                    break;
            }

            switch (vAlign)
            {
                case VerticalAlignment.Center:
                    startY = (fieldSize.Height - 1) / 2;
                    break;

                case VerticalAlignment.Bottom:
                    startY = fieldSize.Height - 1;
                    break;
            }

            Point pos = new Point(lPos.X + startX, lPos.Y + startY);
            return pos;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Dispose
        // /////////////////////////////////////////////////////////////////////////////////
        private bool _alreadyDisposed;

        /// <summary>
        /// Default finalizer calls Dispose.
        /// </summary>
        ~Canvas()
        {
            Dispose(false);
        }

        /// <summary>
        /// Safely dispose this object and all of its contents.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override to add custom disposing code.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                Console.Dispose();
            }
            _alreadyDisposed = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
    #endregion
}
