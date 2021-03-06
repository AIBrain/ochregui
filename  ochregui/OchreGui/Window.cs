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
using System.Collections.ObjectModel;


namespace OchreGui
{
    #region Window Template Class
    /// <summary>
    /// When subclassing a type of Window, consider
    /// also subclassing WindowTemplate to provide an interface for the client to specify
    /// options.
    /// </summary>
    public class WindowTemplate : WidgetTemplate
    {
        /// <summary>
        /// Default constructor initializes properties to their defaults.
        /// </summary>
        public WindowTemplate()
        {
            HasFrame = false;

            TooltipFGAlpha = 1.0f;
            TooltipBGAlpha = 0.6f;
        }

        /// <summary>
        /// True if a frame is drawn around the window initially.
        /// </summary>
        public bool HasFrame { get; set; }

        /// <summary>
        /// The foreground alpha for any tooltips shown on this window.  Default to 1.0.
        /// </summary>
        public float TooltipFGAlpha { get; set; }

        /// <summary>
        /// The background alpha for any tooltips shown on this window.  Defaults to 0.6.
        /// </summary>
        public float TooltipBGAlpha { get; set; }

        /// <summary>
        /// Returns the screen size.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            return Application.ScreenSize;
        }
    }
    #endregion


    #region Window Class
    /// <summary>
    /// A Window acts as both a drawing region and a container for controls.  A Window is always
    /// the size of the screen, and the application has exactly one Window active at a time.  Since
    /// Window derives from Widget, providing custom drawing code can be accomplished by overriding
    /// Redraw().  The Window handles all message dispatch to children automatically.
    /// </summary>
	public class Window : Widget
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a Window instance from the given template.
        /// </summary>
        /// <param name="template"></param>
        public Window(WindowTemplate template)
            :base(template)
		{
			this.controlList = new List<Control>();
            this.managerList = new List<Manager>();

            HasFrame = template.HasFrame;
            TooltipBGAlpha = template.TooltipBGAlpha;
            TooltipFGAlpha = template.TooltipFGAlpha;
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The parent Application instance.
        /// </summary>
        public Application ParentApplication { get; internal set; }

        /// <summary>
        /// If true, a frame will be drawn around the border of the window.
        /// </summary>
        public bool HasFrame { get; set; }

        /// <summary>
        /// The foreground alpha for any tooltips shown on this window.
        /// </summary>
        public float TooltipFGAlpha { get; protected set; }

        /// <summary>
        /// The background alpha for any tooltips shown on this window.
        /// </summary>
        public float TooltipBGAlpha { get; protected set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        /// <summary>
        /// Add a previously constructed Manager object to this window.  All added instances
        /// must be reference-unique, or this method will throw an ArgumentException.
        /// </summary>
        /// <param name="manager"></param>
        /// <exception cref="System.ArgumentException">Thrown when the specified
        /// <paramref name="manager"/> instance is already contained by this window.</exception>
        public void AddManager(Manager manager)
        {
            if (managerList.Contains(manager) || managerAddList.Contains(manager))
            {
                throw new ArgumentException("Added manager instances must be unique.");
            }

            managerAddList.Add(manager);
            manager.ParentWindow = this;

            if (!manager.isSetup)
            {
                manager.OnSettingUp();
            }
        }

        /// <summary>
        /// Adds several specified Managers to this window.  All added instances must be
        /// reference-unique, or this method will throw an ArgumentException.
        /// </summary>
        /// <param name="managers"></param>
        public void AddManagers(params Manager[] managers)
        {
            foreach (Manager m in managers)
            {
                AddManager(m);
            }
        }

        /// <summary>
        /// Removes the specified manager from the Window.  The Window will wait until next tick
        /// to actually remove the manager.
        /// </summary>
        /// <param name="manager"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="manager"/> is
        /// null.</exception>
        public void RemoveManager(Manager manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            if(managerList.Contains(manager))
            {
                managerRemoveList.Add(manager);
            }

            // make sure to remove any managers waiting to be added
            if(managerAddList.Contains(manager))
            {
                managerAddList.Remove(manager);
            }
        }

        /// <summary>
        /// Adds a control instance to this window.  All controls must be reference-unique, or this
        /// method will throw an ArgumentException.  This method will also throw an ArgumentExeption
        /// if the control is too large to fit on the screen.  A newly added control may receive
        /// a MouseEnter message if the mouse is within it's region, and will always receive a 
        /// SettingUp message if it hasn't received one previously.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified <paramref name="control"/>
        /// is already contained by this window.</exception>
        public bool AddControl(Control control)
        {
            if (ContainsControl(control) || controlAddList.Contains(control))
            {
                throw new ArgumentException("CurrentWindow already contians an instance of this control");
            }

            this.controlAddList.Add(control);

            bool atRequestedPos = CheckNewlyAddedControlPosition(control);

            if (!atRequestedPos)
            {
                if (!ScreenRect.Contains(control.ScreenRect.UpperLeft) ||
                    !ScreenRect.Contains(control.ScreenRect.LowerRight))
                {
                    throw new ArgumentException("The specified control is too large to fit on the screen.");
                }
            }

            CheckNewlyAddedControlMessages(control);

            control.ParentWindow = this;
            control.Pigments = new PigmentMap(Pigments, 
                control.PigmentOverrides);
            
            if (!control.isSetup)
            {
                control.OnSettingUp();
            }

            return atRequestedPos;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds several controls to the window.  See AddControl() method.
        /// </summary>
        /// <param name="controls"></param>
        public void AddControls(params Control[] controls)
        {
            foreach (Control c in controls)
            {
                AddControl(c);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Remove the provided control from the window.
        /// </summary>
        /// <param name="control"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="control"/>
        /// is null.</exception>
		public void RemoveControl(Control control)
		{
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (controlList.Contains(control))
            {
                controlRemoveList.Add(control);
            }

            if (controlAddList.Contains(control))
            {
                controlAddList.Remove(control);
            }
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if Window currently contains the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public bool ContainsControl(Control control)
        {
            return ControlList.Contains(control);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves the specified control to the top of the draw order.  Controls on top
        /// are drawn over lower controls.
        /// </summary>
        /// <param name="control"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="control"/>
        /// is null.</exception>
        public void MoveToTop(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if(ContainsControl(control))
            {
                controlList.Remove(control);
                controlList.Add(control);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Moves specified control to the bottom of the draw order.  Controls on bottom
        /// are drawn first (covered up by higher controls).
        /// </summary>
        /// <param name="control"></param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="control"/>
        /// is null.</exception>
        public void MoveToBottom(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if(ContainsControl(control))
            {
                controlList.Remove(control);
                controlList.Insert(0, control);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Release the keyboard focus from the provided control.  The control will receive
        /// a ReleaseKB message (and raise the related RelaseKeyboardEvent)
        /// </summary>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="control"/>
        /// is null.</exception>
        public void ReleaseKeyboard(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (control == CurrentKeyboardFocus)
            {
                control.OnReleaseKeyboardFocus();
                CurrentKeyboardFocus = null;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Forces the keyboard focus to the given control, sending a TakeKeyboardFocus
        /// message to the specified control.  If a control currently has the
        /// keyboard focus, that control will receive a ReleaseKeyboardFocus message.
        /// </summary>
        public void TakeKeyboard(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (control != CurrentKeyboardFocus)
            {
                control.OnTakeKeyboardFocus();
                if (CurrentKeyboardFocus != null)
                {
                    CurrentKeyboardFocus.OnReleaseKeyboardFocus();
                }

                CurrentKeyboardFocus = control;
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The list of controls currently added to the Window.
        /// </summary>
        protected ReadOnlyCollection<Control> ControlList
        {
            get
            {
                return new ReadOnlyCollection<Control>(controlList);
            }
        }


        /// <summary>
        /// Control that has currently has keyboard focus, null if none.
        /// </summary>
        protected Control CurrentKeyboardFocus { get; private set; }

        /// <summary>
        /// The topmost Control that is currently located under the mouse, null if none
        /// </summary>
        protected Control CurrentUnderMouse { get; private set; }

        /// <summary>
        /// Control that is the origin of a left button down, and is now
        /// a candidate for a click (or a drag) message. null for none
        /// </summary>
        protected Control LastLBDown { get; private set; }

        /// <summary>
        /// Control that is is the current origin of a drag action, null for none
        /// </summary>
        protected Control CurrentDragging { get; private set; }

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the topmost control at the given position, or null
        /// for none
        /// </summary>
        /// <param name="screenPos"></param>
        /// <returns></returns>
        protected Control GetTopControlAt(Point screenPos)
        {
            Control retControl = null;

            for(int i = ControlList.Count-1;i>=0;i--)
            {
                Control c = ControlList[i];
                if (c.ScreenRect.Contains(screenPos) && c.IsActive)
                {
                    retControl = c;
                    break;
                    // stop searching when topmost control found
                }
            }

            return retControl;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Request that this Window display a tooltip with the specified text near the
        /// specified position in screen space.  If the specified <paramref name="text"/>
        /// is null or empty, then this method does nothing.
        /// 
        /// The Control base class calls this method automatically when it receives a
        /// MouseHoverBegin message.
        /// See <see cref="OchreGui.Control.DetermineTooltipText"/> and 
        /// <see cref="OchreGui.Control.TooltipText"/>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sPos"></param>
        protected internal void ShowTooltip(string text,Point sPos)
        {
            if (!string.IsNullOrEmpty(text))
            {
                CurrentTooltip = new Tooltip(text, sPos, this);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method prints the frame if applicable.  Override to add custom drawing code.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            if (HasFrame)
            {
                Canvas.PrintFrame("", DetermineFramePigment());
            }
        }

        /// <summary>
        /// Returns the Pigment for the main window area.
        /// </summary>
        /// <returns></returns>
        protected override Pigment DetermineMainPigment()
        {
            return Pigments[PigmentType.Window];
        }

        /// <summary>
        /// Returns the Pigment for the window frame.
        /// </summary>
        /// <returns></returns>
        protected override Pigment DetermineFramePigment()
        {
            return Pigments[PigmentType.FrameNormal];
        }
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method draws each of the controls, and the tooltip if applicable.
        /// </summary>
        protected internal override void OnDraw()
		{
			base.OnDraw();
			
            // propagate Draw message to all children
			foreach (Control c in ControlList)
			{
                c.OnDraw();
			}

            if (CurrentTooltip != null)
            {
                CurrentTooltip.DrawToScreen();
            }
			
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method sends tick message to controls and managers.  Override to add
        /// custom handling.
        /// </summary>
        internal protected override void OnTick()
		{
			base.OnTick();

            AddManagersFromList();
            RemoveManagersFromList();

            AddControlsFromList();
            RemoveControlsFromList();

            foreach (Manager m in managerList)
            {
                m.OnTick();
            }

			foreach (Control c in ControlList)
			{
                c.OnTick();
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnQuitting()
		{
			base.OnQuitting();

            foreach (Manager m in managerList)
            {
                m.OnQuitting();
            }

			foreach (Control c in ControlList)
			{
                c.OnQuitting();
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnKeyPressed(KeyboardData keyData)
		{
            base.OnKeyPressed(keyData);
            
            foreach (Manager m in managerList)
            {
                m.OnKeyPressed(keyData);
            }

			if (CurrentKeyboardFocus != null)
			{
                CurrentKeyboardFocus.OnKeyPressed(keyData);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        protected internal override void OnKeyReleased(KeyboardData keyData)
        {
            base.OnKeyReleased(keyData);

            foreach (Manager m in managerList)
            {
                m.OnKeyReleased(keyData);
            }

            if (CurrentKeyboardFocus != null)
            {
                CurrentKeyboardFocus.OnKeyReleased(keyData);
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseButtonDown(MouseData mouseData)
		{
            base.OnMouseButtonDown(mouseData);

            foreach (Manager m in managerList)
            {
                m.OnMouseButtonDown(mouseData);
            }

			// If applicable, forward MouseDown and Select to child control
			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
                CurrentUnderMouse.OnMouseButtonDown(mouseData);

                LastLBDown = CurrentUnderMouse;
			}
			
			// Check to see if a control looses KBFocus if user hit any mouse button outside current focused control
			if (CurrentKeyboardFocus != null)
			{
				if (CurrentKeyboardFocus != CurrentUnderMouse)
				{
                    CurrentKeyboardFocus.OnReleaseKeyboardFocus();
					CurrentKeyboardFocus = null;
				}
			}
			
			// Give KBFocus to child on left button down, if applicable
			if (CurrentUnderMouse != null && 
				CurrentUnderMouse.CanHaveKeyboardFocus && 
				CurrentUnderMouse.HasKeyboardFocus == false &&
                mouseData.MouseButton == MouseButton.LeftButton &&
                CurrentUnderMouse.IsActive)
			{
				CurrentKeyboardFocus = CurrentUnderMouse;
				
                CurrentKeyboardFocus.OnTakeKeyboardFocus();
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseButtonUp(MouseData mouseData)
		{
            base.OnMouseButtonUp(mouseData);

            foreach (Manager m in managerList)
            {
                m.OnMouseButtonUp(mouseData);
            }

			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
                CurrentUnderMouse.OnMouseButtonUp(mouseData);
			}

			LastLBDown = null;
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseMoved(MouseData mouseData)
		{
            base.OnMouseMoved(mouseData);

            foreach (Manager m in managerList)
            {
                m.OnMouseMoved(mouseData);
            }

            Control checkUnderMouse = GetTopControlAt(mouseData.Position);
			
			if (checkUnderMouse != CurrentUnderMouse)
			{
                // check for Leave and Enter actions

				if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
				{
                    CurrentUnderMouse.OnMouseLeave();
				}
				
				if (checkUnderMouse != null && checkUnderMouse.IsActive)
				{
                    checkUnderMouse.OnMouseEnter();
				}
				
				CurrentUnderMouse = checkUnderMouse;
			}
			
			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
                CurrentUnderMouse.OnMouseMoved(mouseData);
			}
		
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseHoverBegin(MouseData mouseData)
		{
            base.OnMouseHoverBegin(mouseData);

            foreach (Manager m in managerList)
            {
                m.OnMouseHoverBegin(mouseData);
            }

			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
                CurrentUnderMouse.OnMouseHoverBegin(mouseData);

    		}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseHoverEnd(MouseData mouseData)
		{
            if (CurrentTooltip != null)
            {
                CurrentTooltip.Dispose();
                CurrentTooltip = null;
            }

            base.OnMouseHoverEnd(mouseData);

            foreach (Manager m in managerList)
            {
                m.OnMouseHoverEnd(mouseData);
            }

			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
                CurrentUnderMouse.OnMouseHoverEnd(mouseData);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseDragBegin(Point sPosOrigin)
		{
            base.OnMouseDragBegin(sPosOrigin);

            foreach (Manager m in managerList)
            {
                m.OnMouseDragBegin(sPosOrigin);
            }

			if (LastLBDown != null && LastLBDown.IsActive)
			{
				CurrentDragging = LastLBDown;
                LastLBDown.OnMouseDragBegin(sPosOrigin);
			}
			
			// TODO handle drag/drop operation
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method propagates messages to children controls and managers.  Override to
        /// add custom handling.
        /// </summary>
        internal protected override void OnMouseDragEnd(Point sPos)
		{
            base.OnMouseDragEnd(sPos);

            foreach (Manager m in managerList)
            {
                m.OnMouseDragEnd(sPos);
            }

			if (CurrentUnderMouse != null && CurrentUnderMouse.IsActive)
			{
				CurrentDragging = null;
                CurrentUnderMouse.OnMouseDragEnd(sPos);
			}
		}

        /// <summary>
        /// Called during a Window's setup, and is called only once after the Window is
        /// set to the Application's Window with the Application.SetWindow method.
        /// This base method checks to see if WindowPigments if null, and if so inherits
        /// it's pigments from the parent application.
        /// Override to add specific setup code.
        /// </summary>
        protected internal override void OnSettingUp()
        {
            base.OnSettingUp();
            
            
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Internal
        // /////////////////////////////////////////////////////////////////////////////////
        internal Tooltip CurrentTooltip { get; set; }

        private List<Manager> managerAddList = new List<Manager>();
        private List<Manager> managerRemoveList = new List<Manager>();

        private List<Control> controlAddList = new List<Control>();
        private List<Control> controlRemoveList = new List<Control>();

        private List<Control> controlList;
        private List<Manager> managerList;
        // /////////////////////////////////////////////////////////////////////////////////

        internal Point AutoPosition(Point nearPos, Size sizeOfControl)
        {
            Rect conRect = new Rect(nearPos, sizeOfControl);
            int dx = 0;
            int dy = 0;

            int screenRight = Application.ScreenSize.Width - 1;
            int screenBottom = Application.ScreenSize.Height - 1;

            if (conRect.Left < 0)
                dx = -conRect.Left;
            else if (conRect.Right > screenRight)
                dx = screenRight - conRect.Right;

            if (conRect.Top < 0)
                dy = -conRect.Top;
            else if (conRect.Bottom > screenBottom)
                dy = screenBottom - conRect.Bottom;

            int finalX = nearPos.X + dx;
            int finalY = nearPos.Y + dy;


            return new Point(finalX, finalY);
        }

        private void CheckNewlyAddedControlMessages(Control control)
        {
            if (control.ScreenRect.Contains(CurrentMousePos))
            {
                control.OnMouseEnter();
                CurrentUnderMouse = control;
            }
        }

        private bool CheckNewlyAddedControlPosition(Control control)
        {
            Point newPos = AutoPosition(control.ScreenPosition,
                control.Size);

            if (newPos == control.ScreenPosition)
            {
                return true;
            }
            control.ScreenPosition = newPos;
            return false;
        }

        private void AddControlsFromList()
        {
            if (controlAddList.Count == 0)
                return;

            foreach (Control control in controlAddList)
            {
                controlList.Add(control);
            }

            controlAddList.Clear();
        }

        private void RemoveControlsFromList()
        {
            if (controlRemoveList.Count == 0)
                return;

            foreach (Control control in controlRemoveList)
            {
                controlList.Remove(control);
            }
            controlRemoveList.Clear();
        }

        private void AddManagersFromList()
        {
            if (managerAddList.Count == 0)
                return;

            foreach (Manager manager in managerAddList)
            {
                managerList.Add(manager);
            }

            managerAddList.Clear();
        }

        private void RemoveManagersFromList()
        {
            if (managerRemoveList.Count == 0)
                return;

            foreach (Manager manager in managerRemoveList)
            {
                managerList.Remove(manager);
            }

            managerRemoveList.Clear();
        }
        #endregion
        #region Dispose
        /// <summary>
        /// Override to add custom disposing code.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                if (CurrentTooltip != null)
                    CurrentTooltip.Dispose();
            }
        }
        #endregion
    }
    #endregion
}

