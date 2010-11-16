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
    #region Layout Direction Enum
    /// <summary>
    /// Specifies a cardinal direction (assuming Up is North) for use in the layout helper methods.
    /// </summary>
    public enum LayoutDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    };
    #endregion
    #region ControlInfo

    /// <summary>
    /// This class builds on the Widget Template, and offers some layout helper methods for
    /// positioning controls relative to each other.
    /// </summary>
    public abstract class ControlTemplate : WidgetTemplate
    {
        protected ControlTemplate()
        {
            this.Tooltip = null;
            IsActiveInitially = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The upper left position of this control.  Defaults to the origin (0,0)
        /// </summary>
        public Point UpperLeftPos { get; set; }

        /// <summary>
        /// If not null (the default), the text that is displayed as a tooltip.
        /// </summary>
        public string Tooltip { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// If true (the default), this control will be Active when created.
        /// </summary>
        public bool IsActiveInitially { get; set; }

        /// <summary>
        /// Calculates the Rect (in screen coordinates) of this control.
        /// </summary>
        /// <returns></returns>
        public Rect CalculateRect()
        {
            return new Rect(UpperLeftPos, CalculateSize());
        }

        /// <summary>
        /// Layout helper - positions the control by setting the upper right coordinates.
        /// </summary>
        /// <param name="upperRight"></param>
        public void SetUpperRight(Point upperRight)
        {
            UpperLeftPos = upperRight.Shift(1 - CalculateSize().Width, 0);
        }

        /// <summary>
        /// Layout helper - positions the control by setting the lower right coordinates.
        /// </summary>
        /// <param name="lowerRight"></param>
        public void SetLowerRight(Point lowerRight)
        {
            UpperLeftPos = lowerRight.Shift(1 - CalculateSize().Width,
                1 - CalculateSize().Height);
        }

        /// <summary>
        /// Layout helper - positions the control by setting the lower left coordinates.
        /// </summary>
        /// <param name="lowerLeft"></param>
        public void SetLowerLeft(Point lowerLeft)
        {
            UpperLeftPos = lowerLeft.Shift(0, 1 - CalculateSize().Height);
        }

        /// <summary>
        /// Layout helper - positions the control by setting the top center coordinates.
        /// </summary>
        /// <param name="topCenter"></param>
        public void SetTopCenter(Point topCenter)
        {
            Point ctr = CalculateRect().Center;

            UpperLeftPos = new Point(topCenter.X - ctr.X, topCenter.Y);
        }

        /// <summary>
        /// Layout helper - positions the control by setting the center right coordinates.
        /// </summary>
        /// <param name="rightCenter"></param>
        public void SetRightCenter(Point rightCenter)
        {
            Point ctr = CalculateRect().Center;

            SetUpperRight(new Point(rightCenter.X, rightCenter.Y - ctr.Y));
        }

        /// <summary>
        /// Layout helper - positions the control by setting the bottom center coordinates.
        /// </summary>
        /// <param name="bottomCenter"></param>
        public void SetBottomCenter(Point bottomCenter)
        {
            Point ctr = CalculateRect().Center;

            SetLowerLeft(new Point(bottomCenter.X - ctr.X, bottomCenter.Y));
        }

        /// <summary>
        /// Layout helper - positions the control by setting the center left coordinates.
        /// </summary>
        /// <param name="leftCenter"></param>
        public void SetLeftCenter(Point leftCenter)
        {
            Point ctr = CalculateRect().Center;

            UpperLeftPos = new Point(leftCenter.X, leftCenter.Y - ctr.Y);
        }

        /// <summary>
        /// Layout helper - Aligns this control to the specified direction of the spcecified
        /// control template.  This provides a means to specify control positions relative to
        /// previously created templates.
        /// </summary>
        /// <param name="toDirection"></param>
        /// <param name="ofControl"></param>
        /// <param name="padding"></param>
        public void AlignTo(LayoutDirection toDirection, ControlTemplate ofControl,int padding = 0)
        {

            switch (toDirection)
            {
                case LayoutDirection.North:
                    AlignNorth(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.NorthEast:
                    AlignNorthEast(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.East:
                    AlignEast(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.SouthEast:
                    AlignSouthEast(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.South:
                    AlignSouth(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.SouthWest:
                    AlignSouthWest(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.West:
                    AlignWest(ofControl.CalculateRect(), padding);
                    break;

                case LayoutDirection.NorthWest:
                    AlignNorthWest(ofControl.CalculateRect(), padding);
                    break;
            }
        }

        // UNDONE: Implement
        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="template1"></param>
        /// <param name="template2"></param>
        public void AlignBetween(ControlTemplate template1, ControlTemplate template2)
        {

        }

        #region Private
        private void AlignSouth(Rect ofRect, int padding)
        {
            Point ourCtr = CalculateRect().Center;
            Point ofCtr = ofRect.Center;

            UpperLeftPos = new Point(ofCtr.X - ourCtr.X, ofRect.Bottom + 1 + padding);
        }

        private void AlignEast(Rect ofRect, int padding)
        {
            Point ourCtr = CalculateRect().Center;
            Point ofCtr = ofRect.Center;

            UpperLeftPos = new Point(ofRect.Right + 1 + padding, ofCtr.Y - ourCtr.Y);
        }

        private void AlignNorth(Rect ofRect, int padding)
        {
            Point ourCtr = CalculateRect().Center;
            Point ofCtr = ofRect.Center;

            SetLowerLeft(new Point(ofCtr.X - ourCtr.X, ofRect.Top -(1 + padding)));
        }

        private void AlignWest(Rect ofRect, int padding)
        {
            Point ourCtr = CalculateRect().Center;
            Point ofCtr = ofRect.Center;

            SetUpperRight(new Point(ofRect.Left - (1 + padding), ofCtr.Y - ourCtr.Y));
        }

        private void AlignNorthEast(Rect ofRect, int padding)
        {
            SetLowerLeft(ofRect.UpperRight.Shift(1 + padding, -(1 + padding)));
        }

        private void AlignSouthEast(Rect ofRect, int padding)
        {
            UpperLeftPos = ofRect.LowerRight.Shift(1 + padding, 1 + padding);
        }

        private void AlignSouthWest(Rect ofRect, int padding)
        {
            SetUpperRight(ofRect.LowerLeft.Shift(-(1 + padding), 1 + padding));
        }

        private void AlignNorthWest(Rect ofRect, int padding)
        {
            SetLowerRight(ofRect.UpperLeft.Shift(-(1 + padding), -(1 + padding)));
        }
        #endregion
    }
    #endregion


    #region Control
    /// <summary>
    /// Controls are added to a window, through which they receive action and system
    /// messages.
    /// </summary>
	public abstract class Control : Widget
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when control has taken the keyboard focus.  This typically happens after
        /// the control recieves a left mouse button down message.
        /// </summary>
		public event EventHandler TakeKeyboardFocus;

        /// <summary>
        /// Raised when the control has released the keyboard focus.  This typically
        /// happens when a left mouse button down action happens away from this control.
        /// </summary>
		public event EventHandler ReleaseKeyboardFocus;

        /// <summary>
        /// Raised when the mouse cursor has entered the control region and the control
        /// is topmost at that position.
        /// </summary>
        public event EventHandler MouseEnter;

        /// <summary>
        /// Raised when the mouse cursor has left the control region and the control
        /// is topmost at that position.
        /// </summary>
        public event EventHandler MouseLeave;

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        protected Control(ControlTemplate template)
            :base(template)
		{
            ScreenPosition = template.UpperLeftPos;

			HasKeyboardFocus = false;
			CanHaveKeyboardFocus = true;
			
			IsActive = true;

            this.HasFrame = true;
            this.TooltipText = template.Tooltip;

            this.HilightWhenMouseOver = false;

            this.IsActive = template.IsActiveInitially;
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// True if currently has keyboard focus.  This is set automatically by
        /// the framework in response to user input, or by calling Window.TakeKeyboard.
        /// </summary>
        public bool HasKeyboardFocus { get; private set; }

        /// <summary>
        /// True tells parent window that this control is able to
        /// capture keyboard focus.
        /// </summary>
		public bool CanHaveKeyboardFocus { get; set; }

		/// <summary>
		/// If false, notifies framework that it does not want to receive user input messages.  This
        /// control will stil receive system messages.
		/// </summary>
		public bool IsActive { get; set; }

        /// <summary>
        /// True if this control will draw itself hilighted when the mouse is over it.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }

        /// <summary>
        /// True if the mouse pointer is currently over this control, and the control
        /// is topmost at that position.
        /// </summary>
        public bool IsMouseOver { get; private set; }

        /// <summary>
        /// True if this control is currently being pushed (left mouse button down while over)
        /// </summary>
        public bool IsBeingPushed { get; private set; }

        /// <summary>
        /// Set to true if a frame should be drawn around the boder.
        /// </summary>
        public bool HasFrame { get; protected set; }

        /// <summary>
        /// Set to a non-empty string to display a tooltip over this control on a hover action.
        /// </summary>
        public string TooltipText { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////	    
        /// <summary>
        /// Translates the given screen space position to local space.  This is often necessary
        /// when handling mouse messages, since the position contained in MouseData is in screen
        /// space.
        /// </summary>
        /// <param name="screenPos"></param>
        /// <returns></returns>
		public Point ScreenToLocal(Point screenPos)
		{
			return new Point(screenPos.X - ScreenRect.UpperLeft.X, screenPos.Y - ScreenRect.UpperLeft.Y);
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Translates the given local space position to screen space position.
        /// </summary>
        /// <param name="localPos"></param>
        /// <returns></returns>
		public Point LocalToScreen(Point localPos)
		{
			return new Point(localPos.X + ScreenRect.UpperLeft.X, localPos.Y + ScreenRect.UpperLeft.Y);
		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the current parent window of control
        /// </summary>
        protected internal Window ParentWindow { get; internal set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draw a frame around the control border.  If the <paramref name="pigment"/> is null,
        /// the frame will drawn with the Canvas' current default pigment.
        /// </summary>
        protected void DrawFrame(Pigment pigment = null)
        {
            Canvas.PrintFrame(null,pigment);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base class clears the Canvas and draws the frame if HasFrame is true.  If OwnerDraw
        /// is true, this method does nothing. Override to add custom drawing
        /// code after calling base class.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            if (HasFrame && OwnerDraw == false)
            {
                DrawFrame(GetFramePigment());
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the colors for the control according to its state.
        /// Override to return a custom color for the main drawing area of the button, or to add
        /// additional colors for the button based on custom states.
        /// </summary>
        /// <remarks>
        /// The possible Pigments returned by this base method (based on current state) are as follows:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>DefaultPigments.Active</description>
        ///         </item>
        ///         <item>
        ///             <description>DefaultPigments.Inactive</description>
        ///         </item>
        ///         <item>
        ///             <description>DefaultPigments.Hilight</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        protected override Pigment GetMainPigment()
        {
            if (IsActive)
            {
                if (IsMouseOver && HilightWhenMouseOver)
                {
                    return (DefaultPigments.Hilight);
                }
                else
                {
                    return (DefaultPigments.Active);
                }
            }
            else
            {
                return (DefaultPigments.Inactive);
            }
        }

        /// <summary>
        /// Returns a string representing the displayed tooltip, or null if none.  Base method
        /// simply returns the TooltipText property.  Override to add custom tooltip code, e.g.
        /// when the tooltip depends on where the mouse is positioned.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTooltipText()
        {
            return TooltipText;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method sets HasKeyboardFocus to true, and raises the TakeKBFocus event.  Override
        /// to add custom handling code after calling this base method.
        /// </summary>
        internal protected virtual void OnTakeKeyboardFocus()
		{
			HasKeyboardFocus = true;
			
			if (TakeKeyboardFocus != null)
			{
				TakeKeyboardFocus(this, EventArgs.Empty);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method sets HasKBFocus to false, and raises the ReleaseKeyboardFocus event.
        /// Override to add custom handling code after calling this base method.
        /// </summary>
        internal protected virtual void OnReleaseKeyboardFocus()
		{
			HasKeyboardFocus = false;
			
			if (ReleaseKeyboardFocus != null)
			{
				ReleaseKeyboardFocus(this, EventArgs.Empty);
			}
		}
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called by the framework once when this control is first added to a Window.  Later
        /// adds will not cause this method to be called again.  Override and place custom
        /// startup code here after calling this base method.
        /// </summary>
        protected internal override void OnSettingUp()
        {
            base.OnSettingUp();

            if (DefaultPigments == null)
                DefaultPigments = ParentWindow.DefaultPigments.Copy();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This raises the Enter event and sets IsMouseOver to true.  Override to add
        /// custom handling code after calling this base method.
        /// </summary>
        internal protected virtual void OnMouseEnter()
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, EventArgs.Empty);
            }

            IsMouseOver = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method raises the Leave event and sets IsMouseOver to false.  Override to add
        /// custom handling code after calling this base method.
        /// </summary>
        internal protected virtual void OnMouseLeave()
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, EventArgs.Empty);
            }

            IsMouseOver = false;
            IsBeingPushed = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method sets the IsBeingPushed state if applicable.  Override to add
        /// custom handling code after calling this base method.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                IsBeingPushed = true;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method sets the IsBeingPushed state if applicable.  Override to add
        /// custom handling code after calling this base method.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonUp(MouseData mouseData)
        {
            base.OnMouseButtonUp(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                IsBeingPushed = false;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method requests that a tooltip be displayed, calling this.GetTooltipText()
        /// to get the displayed text.  Override to add custom handling code after calling 
        /// this base method.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseHoverBegin(MouseData mouseData)
        {
            base.OnMouseHoverBegin(mouseData);
            ParentWindow.RequestTooltip(GetTooltipText(), mouseData.Position);
        }
        // /////////////////////////////////////////////////////////////////////////////////
		#endregion
	}
    #endregion

}

