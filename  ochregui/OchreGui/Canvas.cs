//Copyright (c) 2010 Shane Baker
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
using System.Text;

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

            defaultPigment = new Pigment(0xffffff, 0x000000);
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
        public void SetDefaultPigment(Pigment pigment)
        {
            if (pigment == null)
                throw new ArgumentNullException("pigment");

            defaultPigment = pigment;
            SetPigment(pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the pigment of a single character at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="pigment"/>
        /// is null</exception>
        public void SetCharPigment(int x, int y, Pigment pigment)
        {
            if (pigment == null)
                throw new ArgumentNullException("pigment");

            Console.setCharBackground(x, y, pigment.Background.GetTCODColor());
            Console.setCharForeground(x, y, pigment.Foreground.GetTCODColor());
        }

        /// <summary>
        /// Sets the pigment of a single character at the given coordinates.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when pigment is null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="position"/> is outside of this Canvas region</exception>
        public void SetCharPigment(Point position, Pigment pigment)
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

            SetCharPigment(position.X, position.Y, pigment);
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
                SetPigment(pigment);

            Console.putChar(x, y, character);

            if (pigment != null)
                SetPigment(defaultPigment);
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
                SetPigment(pigment);

            //Console.print(x, y, text);
            Print(x, y, text);

            if (pigment != null)
                SetPigment(defaultPigment);
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
        /// If the text length is larger than the field length, then the text will be trimmed to fit.
        /// The field length must be equal to or greater than 1, or an exception will be thrown.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified field length is less than 1</exception>
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

            if (fieldLength < 1)
            {
                throw new ArgumentOutOfRangeException("fieldLength",
                    "The field length must equal to or greater than 1");
            }

            Point pos = GetHorAlign(new Point(x, y), text, alignment, fieldLength);

            if (fieldLength < TextLength(text))
            {
                text = TrimText(text, fieldLength);
            }

            PrintString(pos, text, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// horizontally with the specified alignment and within the specified field length.
        /// If the text length is larger than the field length, then the text will be trimmed to fit.
        /// The field length must be equal to or greater than 1, or an exception will be thrown.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified field length is less than 1</exception>
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

            if (fieldLength < 1)
            {
                throw new ArgumentOutOfRangeException("fieldLength",
                    "The field length must equal to or greater than 1");
            }

            PrintStringAligned(lPos.X, lPos.Y, text, alignment, fieldLength, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Prints the specified string at the given coordinates.  The text is aligned
        /// both horizontally and vertically with the specified alignments, and within
        /// the specified size of the field.
        /// If the text length is larger than the field width, then the text will be trimmed to fit.
        /// The field width and height must be equal to or greater than 1, or an exception will
        /// be thrown.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified position is outside of this Canvas region</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the width or height of the field size is less than 1</exception>
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

            if (fieldSize.Width < 1)
            {
                throw new ArgumentOutOfRangeException("fieldSize", "The specified width of fieldSize is less than 1");
            }

            if (fieldSize.Height < 1)
            {
                throw new ArgumentOutOfRangeException("fieldSize", "The specified height of fieldSize is less than 1");
            }
            
            if (fieldSize.Width < TextLength(text))
            {
                text = TrimText(text, fieldSize.Width);
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
        /// If the text length is larger than the field width, then the text will be trimmed to fit.
        /// The field width and height must be equal to or greater than 1, or an exception will
        /// be thrown.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="lPos"/> is outside of this Canvas region</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the width or height of the field size is less than 1</exception>
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

            if (fieldSize.Width < 1)
            {
                throw new ArgumentOutOfRangeException("fieldSize", "The specified width of fieldSize is less than 1");
            }

            if (fieldSize.Height < 1)
            {
                throw new ArgumentOutOfRangeException("fieldSize", "The specified height of fieldSize is less than 1");
            }

            PrintStringAligned(lPos.X, lPos.Y, text, hAlign, vAlign, fieldSize, pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Prints the specified string within the given Rect.  The text is aligned
        /// both horizontally and vertically with the specified alignments.
        /// If the text length is larger than the field width, then the text will be trimmed to fit.
        /// The field width and height must be equal to or greater than 1, or an exception will
        /// be thrown.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> is
        /// null</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified <paramref name="rect"/> is outside of this Canvas region</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the width or height of the rect is less than 1</exception>
        public void PrintStringAligned(Rect rect, string text, HorizontalAlignment hAlign,
            VerticalAlignment vAlign, Pigment pigment = null)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            if (rect.UpperLeft.X < 0 || rect.UpperLeft.X >= this.Size.Width)
            {
                throw new ArgumentOutOfRangeException("rect", "The specified x coordinate is invalid.");
            }

            if (rect.UpperLeft.Y < 0 || rect.UpperLeft.Y >= this.Size.Height)
            {
                throw new ArgumentOutOfRangeException("rect", "The specified y coordinate is invalid.");
            }

            if (rect.Size.Width < 1)
            {
                throw new ArgumentOutOfRangeException("rect", "The specified width of rect is less than 1");
            }

            if (rect.Size.Height < 1)
            {
                throw new ArgumentOutOfRangeException("rect", "The specified height of rect is less than 1");
            }

            PrintStringAligned(rect.UpperLeft.X, rect.UpperLeft.Y, 
                text, 
                hAlign, vAlign,
                rect.Size, 
                pigment);
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
                SetPigment(pigment);

            Console.hline(startX, startY, length);

            if (pigment != null)
                SetPigment(defaultPigment);
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
                SetPigment(pigment);

            Console.vline(x, y, length);

            if (pigment != null)
                SetPigment(defaultPigment);
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
                SetPigment(pigment);

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
                SetPigment(defaultPigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Scrolls this Canvas by the given delta x and y amounts.  
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public void Scroll(int deltaX,int deltaY)
        {
            Size srcSize = new Size(this.Size.Width - Math.Abs(deltaX), 
                this.Size.Height - Math.Abs(deltaY));

            using (Canvas tmpCanvas = new Canvas(srcSize))
            {
                int srcX = 0;
                int srcY = 0;
                int destX = 0;
                int destY = 0;

                if (deltaX < 0)
                {
                    srcX = -deltaX;
                }

                if (deltaX > 0)
                {
                    destX = deltaX;
                }

                if (deltaY < 0)
                {
                    srcY = -deltaY;
                }

                if (deltaY > 0)
                {
                    destY = deltaY;
                }

                TCODConsole.blit(this.Console,
                    srcX, srcY,
                    srcSize.Width, srcSize.Height,
                    tmpCanvas.Console,
                    destX, destY);

                this.Clear();
                this.Blit(tmpCanvas, 0, 0);
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
        private void SetPigment(Pigment pigment)
        {
            Console.setBackgroundColor(pigment.Background.GetTCODColor());
            Console.setBackgroundFlag(pigment.BackgroundFlag);
            Console.setForegroundColor(pigment.Foreground.GetTCODColor());
        }

        private void Print(int x, int y, string str)
        {
            int cx = x;
            TCODColor bg = Console.getBackgroundColor();
            TCODColor fg = Console.getForegroundColor();
            int i = 0;

            while(i < str.Length)
            {
                char c = str[i];
                if (c == Color.CodeForeground[0])
                {
                    int r = str[i + 1];
                    int g = str[i + 2];
                    int b = str[i + 3];
                    Console.setForegroundColor(new TCODColor(r, g, b));
                    i += 4;
                }
                else if (c == Color.CodeBackground[0])
                {
                    int r = str[i + 1];
                    int g = str[i + 2];
                    int b = str[i + 3];
                    Console.setBackgroundColor(new TCODColor(r, g, b));
                    i += 4;
                }
                else if (c == Color.StopColorCode[0])
                {
                    Console.setForegroundColor(fg);
                    Console.setBackgroundColor(bg);
                    i++;
                }
                else
                {
                    Console.putChar(cx, y, c);
                    i++;
                    cx++;

                    if (cx >= this.Size.Width)
                        return;
                }
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////
        private Pigment defaultPigment;
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Returns the length (width) of the given text string when printed, taking into account
        /// embedded color codes.  Use this method instead of string.Length for strings
        /// with embedded color codes.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int TextLength(string str)
        {
            int len = str.Length;

            foreach (char c in str)
            {
                if (c == Color.CodeBackground[0] || c == Color.CodeForeground[0])
                {
                    len = len - 4;
                }
                else if (c == Color.StopColorCode[0])
                {
                    len = len - 1;
                }
            }

            return len;
        }

        /// <summary>
        /// Trims the specified string to the specified width, ignoring
        /// color control codes.  Use this method instead of string.Substring()
        /// for strings with embedded color codes.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimText(string text,int width)
        {
            StringBuilder str = new StringBuilder();

            int i = 0;
            int w = 0;
            while (w < width)
            {
                char c = text[i];

                if (c == Color.CodeBackground[0] || c == Color.CodeForeground[0])
                {
                    str.Append(c);
                    str.Append(text[i + 1]);
                    str.Append(text[i + 2]);
                    str.Append(text[i + 3]);

                    i += 4;
                }
                else if (c == Color.StopColorCode[0])
                {
                    str.Append(c);
                    i++;
                }
                else
                {
                    str.Append(c);
                    i++;
                    w++;
                }
            }

            return str.ToString();
        }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the position of a text string within the given field with the specified
        /// horizontal alignment.
        /// </summary>
        /// <param name="lPos">The position of the left side of the field</param>
        /// <param name="str"></param>
        /// <param name="align"></param>
        /// <param name="fieldLength">The length (width) of the field</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="str"/> is
        /// null</exception>
        public static Point GetHorAlign(Point lPos, string str, HorizontalAlignment align, int fieldLength)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            int startX = 0;

            switch (align)
            {
                case HorizontalAlignment.Center:
                    startX = (fieldLength - Canvas.TextLength(str)) / 2;
                    break;

                case HorizontalAlignment.Right:
                    startX = fieldLength - Canvas.TextLength(str);
                    break;
            }

            Point pos = new Point(lPos.X + startX, lPos.Y);

            return pos;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the position of the given text aligned both horizontally and vertically
        /// within the field defined by the upper left position and size.
        /// </summary>
        /// <param name="lPos"></param>
        /// <param name="str"></param>
        /// <param name="hAlign"></param>
        /// <param name="vAlign"></param>
        /// <param name="fieldSize"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="str"/> is
        /// null</exception>
        public static Point GetHVAlign(Point lPos, string str, HorizontalAlignment hAlign, VerticalAlignment vAlign, Size fieldSize)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            int startX = 0;
            int startY = 0;

            switch (hAlign)
            {
                case HorizontalAlignment.Center:
                    startX = (fieldSize.Width - Canvas.TextLength(str)) / 2;
                    break;

                case HorizontalAlignment.Right:
                    startX = (fieldSize.Width - Canvas.TextLength(str));
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
