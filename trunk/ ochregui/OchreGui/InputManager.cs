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
    /// <summary>
    /// Handles all input polling and message dispatch to attached
    /// Window
    /// </summary>
	public class InputManager
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create an InputManager instance by providing the attached Window.
        /// </summary>
        public InputManager(Component component)
		{
            if (component == null)
            {
                throw new ArgumentNullException("window");
            }

            attachedComponent = component;
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Call on each update tick to perform input polling.  The InputManager instance
        /// will send the appropriate user input messages to the attached window provided
        /// during construction.
        /// </summary>
        /// <param name="ellapsed"></param>
        public void Update(uint ellapsed)
		{
			PollMouse(ellapsed);
			PollKeyboard();
		}

        public static bool IsKeyDown(TCODKeyCode key)
        {
            return TCODConsole.isKeyPressed(key);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private Fields
        // /////////////////////////////////////////////////////////////////////////////////
        private readonly Component attachedComponent;
        private Point lastMousePosition;
        private Point lastMousePixelPosition;
        private MouseButton lastMouseButton;
        private float lastMouseMoveTime;
        private bool isHovering;
        private Point startLBDown;
        private bool isDragging;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private Constants
        // /////////////////////////////////////////////////////////////////////////////////
        // NOTE: consider making these configurable instead of constants
        private const int dragPixelTol = 24;
        private const float hoverMSTol = 600f;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Keyboard Input
        // /////////////////////////////////////////////////////////////////////////////////
        private void PollKeyboard()
		{
			TCODKey key = TCODConsole.checkForKeypress((int)TCODKeyStatus.KeyPressed
                | (int)TCODKeyStatus.KeyReleased);
			
			if (key.KeyCode != TCODKeyCode.NoKey)
			{
                if (key.Pressed)
                {
                    attachedComponent.OnKeyPressed(new KeyboardData(key));
                }
                else
                {
                    attachedComponent.OnKeyReleased(new KeyboardData(key));
                }
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Mouse Input
        // /////////////////////////////////////////////////////////////////////////////////
        private void PollMouse(uint totalEllapsed)
		{
			MouseData mouse = new MouseData(TCODMouse.getStatus());

		    CheckMouseButtons(mouse);
				
			// check for mouse move
            //if (mouse.Position != lastMousePosition)
            //{
            //    DoMouseMove(mouse);
					
            //    lastMousePosition = mouse.Position;
            //    lastMouseMoveTime = totalEllapsed;
            //}
            if (mouse.PixelPosition != lastMousePixelPosition)
            {
                DoMouseMove(mouse);

                lastMousePosition = mouse.Position;
                lastMousePixelPosition = mouse.PixelPosition;
                lastMouseMoveTime = totalEllapsed;
            }
				
			// check for hover
			if ( (totalEllapsed - lastMouseMoveTime) > hoverMSTol &&
				isHovering == false)
			{
				StartHover(mouse);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void CheckMouseButtons(MouseData mouse)
		{
			if (mouse.MouseButton != lastMouseButton)
			{
				if (lastMouseButton == MouseButton.None)
				{
					DoMouseButtonDown(mouse);
				} else
				{
					DoMouseButtonUp(new MouseData(lastMouseButton, mouse.Position, mouse.PixelPosition));
				}
				
				lastMouseButton = mouse.MouseButton;
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void StartHover(MouseData mouse)
		{
            attachedComponent.OnMouseHoverBegin(mouse);
			
			isHovering = true;
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void StopHover(MouseData mouse)
		{
            attachedComponent.OnMouseHoverEnd(mouse);
			
			isHovering = false;
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void StartDrag(MouseData mouse)
		{
			isDragging = true;
			
            // TODO fix this, it does not pass origin of drag as intended
            attachedComponent.OnMouseDragBegin(mouse.Position);
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void StopDrag(MouseData mouse)
		{
			isDragging = false;
			
            attachedComponent.OnMouseDragEnd(mouse.Position);
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoMouseMove(MouseData mouse)
		{
			StopHover(mouse);
			
            attachedComponent.OnMouseMoved(mouse);
			
			// check for BeginDrag
			if (mouse.MouseButton == MouseButton.LeftButton)
			{
				int delta = Math.Abs(mouse.PixelPosition.X - startLBDown.X) +
					Math.Abs(mouse.PixelPosition.Y - startLBDown.Y);
				
				if (delta > dragPixelTol && isDragging == false)
				{
					StartDrag(mouse);
				}
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoMouseButtonDown(MouseData mouse)
		{
			if (isDragging)
				StopDrag(mouse);
			
			if (mouse.MouseButton == MouseButton.LeftButton)
			{
				startLBDown = mouse.PixelPosition;
			}
			
            attachedComponent.OnMouseButtonDown(mouse);
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void DoMouseButtonUp(MouseData mouse)
		{
			if (isDragging)
				StopDrag(mouse);
			
            attachedComponent.OnMouseButtonUp(mouse);
		}
        // /////////////////////////////////////////////////////////////////////////////////
		#endregion
	}
}

