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
    /// Contains the data needed to construct a CheckBox object.  A CheckBox will, by default, automatically
    /// generate its width based on the Label and MinimumWidth properties of the template, leaving space for
    /// the check element, and will always
    /// have a height of 3 (1 space for the label and 2 spaces for the borders).
    /// </summary>
    public class CheckBoxTemplate : ControlTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor, sets all properties to the default.
        /// </summary>
        public CheckBoxTemplate()
        {
            this.Label = "";
            this.MinimumWidth = 0;
            LabelAlignment = HorizontalAlignment.Left;
            CheckOnLeft = true;
            HilightWhenMouseOver = false;
            CanHaveKeyboardFocus = false;
            HasFrameBorder = true;
            VerticalAlign = VerticalAlignment.Center;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Set this to manually size the check box.  If empty (the default), then the 
        /// check box will be autosized.
        /// </summary>
        public Size AutoSizeOverride { get; set; }

        /// <summary>
        /// The text displayed by the checkbox.  Defaults to empty string ""
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The minimum width of the button when autosizing.  This property is ignored if MinimumWidth is less 
        /// than the automatically calculated size.  Defaults to 0.
        /// </summary>
        public int MinimumWidth { get; set; }

        /// <summary>
        /// If there is extra spacing in the label area, the label gets aligned
        /// according to this.  Defautls to HorizontalAlignment.Left
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// The vertical alignment of the label and check element.  Defaults to
        /// VerticalAlignment.Center
        /// </summary>
        public VerticalAlignment VerticalAlign { get; set; }

        /// <summary>
        /// If true, the check field is placed left of the label, otherwise on the 
        /// right.  Defaults to true.
        /// </summary>
        public bool CheckOnLeft { get; set; }

        /// <summary>
        /// True if the checkbox will draw itself with hilight colors when under the mouse
        /// pointer.  Defaults to false.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }

        /// <summary>
        /// Set to true if this control can take the keyboard focus.
        /// Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// If true, a border will be drawn around the check box.  If autosizing, space for the
        /// frame will be added.  Defaults to true.
        /// </summary>
        public bool HasFrameBorder { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Auto generates the size of the control based on the other options.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            if (!AutoSizeOverride.IsEmpty)
            {
                return AutoSizeOverride;
            }

            int width = Canvas.TextLength(Label) + 1;
            int height = 1;

            if (HasFrameBorder)
            {
                width += 2;
                height += 2;
            }
            width = Math.Max(width, MinimumWidth);

            return new Size(width, height);
        }
        // /////////////////////////////////////////////////////////////////////////////////
    }

    /// <summary>
    /// Represents a check box control.  A CheckBox has a label and a checkable element that displays the 
    /// current state of the IsChecked property.  This state
    /// is toggled by left mouse button clicks, or by setting the IsChecked property manually.
    /// </summary>
    public class CheckBox : Control
    {
        #region Events
        /// <summary>
        /// Raised when the state of a checkbox has been toggled by user input.  Get IsChecked to get
        /// current state.  Manually setting IsChecked property will not cause this event to be raised.
        /// </summary>
        public event EventHandler CheckBoxToggled;
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a CheckBox instance from the given template.
        /// </summary>
        /// <param name="template"></param>
        public CheckBox(CheckBoxTemplate template)
            : base(template)
        {
            HasFrame = template.HasFrameBorder;

            if (Size.Height < 3 || Size.Width < 3)
            {
                HasFrame = false;
            }

            HilightWhenMouseOver = template.HilightWhenMouseOver;
            CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;

            this.Label = template.Label;
            if (Label == null)
                Label = "";

            this.CheckOnLeft = template.CheckOnLeft;
            this.LabelAlignment = template.LabelAlignment;
            this.VerticalAlign = template.VerticalAlign;

            CalcMetrics(template);

        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get or set the current checked state of the checkbox.  Setting this property will
        /// not raise a CheckBoxToggled event.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Get the label string
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// True if the check element appears left of the label, otherwise on right side
        /// </summary>
        public bool CheckOnLeft { get; private set; }

        /// <summary>
        /// Text alignment of the label
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; private set; }

        /// <summary>
        /// The vertical alignment of the label and check element.
        /// </summary>
        public VerticalAlignment VerticalAlign { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Triggers appropriate events based on a mouse button down action.  Override to
        /// add custom mouse button handling code.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton)
            {
                if (IsChecked)
                {
                    IsChecked = false;
                }
                else
                {
                    IsChecked = true;
                }

                if (CheckBoxToggled != null)
                {
                    CheckBoxToggled(this, EventArgs.Empty);
                }
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the label and chec element based on current state.  Override to add custom
        /// drawing code.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            if (!string.IsNullOrEmpty(Label))
            {
                Canvas.PrintStringAligned(labelRect,
                    Label,
                    LabelAlignment,
                    VerticalAlign);
            }

            if (IsActive)
            {
                if (IsChecked)
                {
                    Canvas.PrintChar(checkPos, 225, Pigments[PigmentType.ViewNormal]);
                }
                else
                {
                    Canvas.PrintChar(checkPos, 224, Pigments[PigmentType.ViewNormal]);
                }
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private 
        // /////////////////////////////////////////////////////////////////////////////////
        //private int labelPosX;
        //private int labelFieldLength;
        //private int labelPosY;
        private Rect labelRect;
        private Point checkPos;

        private void CalcMetrics(CheckBoxTemplate template)
        {
            Rect inner = this.LocalRect;

            if (template.HasFrameBorder && template.CalculateSize().Height >= 3)
            {
                inner = Rect.Inflate(inner, -1, -1);
            }

            int checkX;

            if (CheckOnLeft)
            {
                checkX = inner.Left;
                labelRect = new Rect(inner.UpperLeft.Shift(1, 0),
                    inner.LowerRight);
            }
            else
            {
                checkX = inner.Right;
                labelRect = new Rect(inner.UpperLeft,
                    inner.LowerRight.Shift(-1, 0));
            }

            if (labelRect.Size.Width < 1)
            {
                Label = "";
            }

            switch (VerticalAlign)
            {
                case VerticalAlignment.Bottom:
                    checkPos = new Point(checkX, labelRect.Bottom);
                    break;

                case VerticalAlignment.Center:
                    checkPos = new Point(checkX, labelRect.Center.Y);
                    break;

                case VerticalAlignment.Top:
                    checkPos = new Point(checkX, labelRect.Top);
                    break;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
