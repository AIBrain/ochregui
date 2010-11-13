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

    public class CheckBoxTemplate : ControlTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor, sets all properties to the default.
        /// </summary>
        public CheckBoxTemplate()
        {
            this.Label = "CheckBox";
            this.MinimumWidth = 0;
            LabelAlignment = HorizontalAlignment.Left;
            CheckOnLeft = true;
            HilightWhenMouseOver = false;
            CanHaveKeyboardFocus = false;
            HasFrameBorder = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The text displayed by the checkbox.  Defaults to "Checkbox"
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The minimum width of the button.  This property is ignored if MinimumWidth is less 
        /// than the automatically calculated size.  Defaults to 0.
        /// </summary>
        public int MinimumWidth { get; set; }

        /// <summary>
        /// If there is extra spacing in the label area, the label gets aligned
        /// according to this.  Defautls to TCODAlignment.Left
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

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
        /// Set to true if this control can take the keyboard focus by being left-clicked on.
        /// Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// If true, the checkbox is sized to accomodate a border, and a frame is drawn
        /// in the border by default.  Defaults to true.
        /// </summary>
        public bool HasFrameBorder { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Auto generates the size of the control based on the other options.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            int width = Label.Length + 2;
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
    /// A checkbox control has a label and a checkable element that displays current state.  The state
    /// is toggled by mouse selection, or by setting the IsChecked property.
    /// </summary>
    public class CheckBox : Control
    {
        #region Events
        /// <summary>
        /// Raised when the state of a checkbox has been toggled.  Get IsChecked to get
        /// current state.
        /// </summary>
        public event EventHandler CheckBoxToggled;
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        public CheckBox(CheckBoxTemplate template)
            : base(template)
        {
            HasFrame = template.HasFrameBorder
                ;
            HilightWhenMouseOver = template.HilightWhenMouseOver;
            CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;

            this.Label = template.Label;
            if (Label == null)
                Label = "";

            this.CheckOnLeft = template.CheckOnLeft;
            this.LabelAlignment = template.LabelAlignment;

            if (template.HasFrameBorder)
            {
                if (CheckOnLeft)
                {
                    labelPosX = 3;
                    checkPosX = 1;
                }
                else
                {
                    labelPosX = 1;
                    checkPosX = Size.Width - 2;
                }
                labelPosY = 1;
                labelFieldLength = Size.Width - 4;
            }
            else
            {
                if (CheckOnLeft)
                {
                    labelPosX = 2;
                    checkPosX = 0;
                }
                else
                {
                    labelPosX = 0;
                    checkPosX = Size.Width - 1;
                }
                labelPosY = 0;
                labelFieldLength = Size.Width - 2;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get or set the current state of the checkbox
        /// </summary>
        public bool IsChecked { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the label string
        /// </summary>
        protected string Label { get; private set; }

        /// <summary>
        /// True if the check element appears left of the label, otherwise on right side
        /// </summary>
        protected bool CheckOnLeft { get; set; }

        /// <summary>
        /// Text alignment of the label
        /// </summary>
        protected HorizontalAlignment LabelAlignment { get; set; }
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
                Canvas.PrintStringAligned(labelPosX, labelPosY, Label,
                    LabelAlignment, labelFieldLength);
            }

            if (IsActive)
            {
                if (IsChecked)
                {
                    Canvas.PrintChar(checkPosX, labelPosY, 225, DefaultStyles.Hilight);
                }
                else
                {
                    Canvas.PrintChar(checkPosX, labelPosY, 224, DefaultStyles.Active);
                }
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private 
        // /////////////////////////////////////////////////////////////////////////////////
        private int labelPosX;
        private int labelFieldLength;
        private int labelPosY;
        private int checkPosX;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
