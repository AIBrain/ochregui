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
using OchreGui.Utility;

using libtcod;

namespace OchreGui
{
    #region Enums
    // /////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Represents which mouse button is pressed.
    /// </summary>
	public enum MouseButton
	{
        /// <summary>
        /// No mouse buttons
        /// </summary>
		None = 0,
        /// <summary>
        /// Left mouse button
        /// </summary>
		LeftButton = 1,
        /// <summary>
        /// Middle mouse button
        /// </summary>
		MiddleButton = 2,
        /// <summary>
        /// Right mouse button
        /// </summary>
		RightButton = 3
	}
    // /////////////////////////////////////////////////////////////////////////////////

    // /////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Control key flags
    /// </summary>
	[Flags]
	public enum ControlKeys
	{
        /// <summary>
        /// No control keys
        /// </summary>
		None = 0,
        /// <summary>
        /// Left ALT key
        /// </summary>
		LeftAlt = 1,
        /// <summary>
        /// Right ALT key
        /// </summary>
		RightAlt = 2,
        /// <summary>
        /// Left CTRL key
        /// </summary>
		LeftControl = 4,
        /// <summary>
        /// Right CTRL key
        /// </summary>
		RightControl = 8,
        /// <summary>
        /// Left or right SHIFT key
        /// </summary>
		Shift = 16
	}
    // /////////////////////////////////////////////////////////////////////////////////

    #endregion

    #region Event and Message data

    /// <summary>
	/// Holds data pertaining to a mouse event
    /// Immtable data type
	/// </summary>
	public class MouseData
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a MouseData with specified mouse button, position and pixel position
        /// </summary>
        /// <param name="button"></param>
        /// <param name="screenPos"></param>
        /// <param name="pixelPos"></param>
        public MouseData(MouseButton button, Point screenPos, Point pixelPos)
		{
			this.pixelPosition = pixelPos;
			this.position = screenPos;
			this.mouseButton = button;
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs MouseData from TCODMouseData
        /// </summary>
        /// <param name="tcodMouseData"></param>
		public MouseData(TCODMouseData tcodMouseData)
		{
            if (tcodMouseData == null)
            {
                throw new ArgumentNullException("tcodMouseData");
            }

			this.position = new Point(tcodMouseData.CellX, 
                tcodMouseData.CellY);
			
			this.pixelPosition = new Point(tcodMouseData.PixelX, 
                tcodMouseData.PixelY);
			
			mouseButton = MouseButton.None;
			if (tcodMouseData.LeftButton)
				mouseButton = MouseButton.LeftButton;
			if (tcodMouseData.MiddleButton)
				mouseButton = MouseButton.MiddleButton;
			if (tcodMouseData.RightButton)
				mouseButton = MouseButton.RightButton;
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns position of mouse pointer in character coordinates
        /// Coordinate space may either be screen or local, depending on
        /// source
        /// </summary>
		public Point Position {
			get { return position; }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get which mouse button, if any, is pressed
        /// </summary>
		public MouseButton MouseButton {
			get { return mouseButton; }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns mouse cursor position in pixel coordinates.  Pixel coordinates
        /// are always in screen space
        /// </summary>
		public Point PixelPosition {
			get { return pixelPosition; }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private Fields
        // /////////////////////////////////////////////////////////////////////////////////
        private readonly Point position;
        private readonly MouseButton mouseButton;
        private readonly Point pixelPosition;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }



	/// <summary>
	/// Holds data pertaining to a keyboard event
    /// Immutable data type
	/// </summary>
	public class KeyboardData
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a KeyboardData given all of the parameters seperately
        /// </summary>
        /// <param name="character"></param>
        /// <param name="keyCode"></param>
        /// <param name="isKeyDown"></param>
        /// <param name="controlKeys"></param>
        public KeyboardData(char character, TCODKeyCode keyCode, bool isKeyDown, 
            ControlKeys controlKeys)
		{
			this.character = character;
			this.isKeyPress = isKeyDown;
			this.keyCode = keyCode;
			this.controlKeys = controlKeys;
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructs a KeyboardData from specified TCODKey object
        /// </summary>
        /// <param name="tcodKeys"></param>
		public KeyboardData(TCODKey tcodKeys)
		{
            if (tcodKeys == null)
            {
                throw new ArgumentNullException("tcodKeys");
            }

			this.character = tcodKeys.Character;
			this.keyCode = tcodKeys.KeyCode;
			this.isKeyPress = tcodKeys.Pressed;
			
			int f = 0;
			if (tcodKeys.LeftAlt)
				f |= (int)ControlKeys.LeftAlt;
			if (tcodKeys.RightAlt)
				f |= (int)ControlKeys.RightAlt;
			if (tcodKeys.LeftControl)
				f |= (int)ControlKeys.LeftControl;
			if (tcodKeys.RightControl)
				f |= (int)ControlKeys.RightControl;
			if (tcodKeys.Shift)
				f |= (int)ControlKeys.Shift;
			
			this.controlKeys = (ControlKeys)f;
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// An ASCII character representation of the pressed key, or 0 if none.
        /// </summary>
        public char Character {
			get { return character; }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A TCODKeyCode value representing a key press
        /// </summary>
		public TCODKeyCode KeyCode {
			get { return keyCode; }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// True if the specified key is being pressed, false if released.
        /// </summary>
		public bool IsKeyPress {
			get { return isKeyPress; }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A ControlKeys bit array representing the current control keys that are being pressed
        /// </summary>
		public ControlKeys ControlKeys {
			get { return controlKeys; }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private Fields
        // /////////////////////////////////////////////////////////////////////////////////
        private ControlKeys controlKeys;
        private TCODKeyCode keyCode;
        private readonly bool isKeyPress;
        private readonly char character;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
	#endregion

	#region EventArgs

    /// <summary>
    /// Arguments for a mouse message or event, contains mouse state
    /// </summary>
	public class MouseEventArgs : EventArgs
	{
        /// <summary>
        /// Construct a MouseEventArgs given the MouseData
        /// </summary>
        /// <param name="mouseData"></param>
		public MouseEventArgs(MouseData mouseData)
		{
            if (mouseData == null)
            {
                throw new ArgumentNullException("mouseData");
            }

			this.mouseData = mouseData;
		}

        /// <summary>
        /// Get the mouse state as a MouseData object
        /// </summary>
		public MouseData MouseData {
			get { return mouseData; }
		}
        readonly MouseData mouseData;
	}

    /// <summary>
    /// Argument for a keyboard message or event, contains keyboard state
    /// </summary>
	public class KeyboardEventArgs : EventArgs
	{
        /// <summary>
        /// Construct a KeyboardEventArgs given the KeyboardData
        /// </summary>
        /// <param name="keyboardData"></param>
		public KeyboardEventArgs(KeyboardData keyboardData)
		{
            if (keyboardData == null)
            {
                throw new ArgumentNullException("keyboardData");
            }

			this.keyboardData = keyboardData;
		}

        /// <summary>
        /// Get the keyboard state as a KeyboardData object
        /// </summary>
		public KeyboardData KeyboardData {
			get { return keyboardData; }
		}
        readonly KeyboardData keyboardData;
	}

    /// <summary>
    /// Argument for a MouseDrag event.
    /// </summary>
    public class MouseDragEventArgs : EventArgs
    {
        /// <summary>
        /// Construct a MouseDragEventArgs instance given the screen position related to the
        /// drag action.
        /// </summary>
        /// <param name="sPos"></param>
        public MouseDragEventArgs(Point sPos)
        {
            this.SPos = sPos;
        }

        /// <summary>
        /// The position in screen space coordinates related to the drag action.  For DragBegin,
        /// this position is the origin of the drag (not the current mouse position).  For DragEnd,
        /// this position is the mouse position when the left mouse button was released.
        /// </summary>
        public Point SPos { get; private set; }
    }
	#endregion
	
	
	
	
	
	
	
}
