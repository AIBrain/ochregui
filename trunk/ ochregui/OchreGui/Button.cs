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

namespace OchreGui
{
    #region Button Template class

    /// <summary>
    /// Contains the setttings needed to construct a Button object.  
    /// </summary>
    public class ButtonTemplate : ControlTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        public ButtonTemplate()
        {
            this.LabelAlignment = HorizontalAlignment.Left;
            this.Label = "";
            this.MinimumWidth = 0;
            HilightWhenMouseOver = true;
            CanHaveKeyboardFocus = false;
            HasFrameBorder = true;
            VAlignment = VerticalAlignment.Center;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The text displayed by the button.  Defaults to empty string ""
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The minimum width of the button.  This property is ignored if either AutoSizeOverride
        /// is set to a non-zero size, or if MinimumWidth is less than the automatically calculated
        /// size.  Defaults to 0.
        /// </summary>
        public int MinimumWidth { get; set; }

        /// <summary>
        /// The horizontal alignment of the label.  Defaults to HorizontalAlignment.Left.
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// True if the button will draw itself with hilight colors when under the mouse
        /// pointer.  Defaults to true.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }

        /// <summary>
        /// Set to true if this button can take the keyboard focus by being left-clicked on.
        /// Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// If true, the button is sized to accomodate a border, and a frame is drawn
        /// in the border by default.  Defaults to true.
        /// </summary>
        public bool HasFrameBorder { get; set; }

        /// <summary>
        /// Overrides the automatically calculated size.  Set this to create a button
        /// whose height is larger than 3.
        /// </summary>
        public Size AutoSizeOverride { get; set; }

        /// <summary>
        /// The vertical alignment of the label.  Only used if the AutoSizeOverride property
        /// has been set to a height of greater than 1.  Defaults to VerticalAlignment.Center.
        /// </summary>
        public VerticalAlignment VAlignment { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Auto generates the size of the button based on the other options.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            if (AutoSizeOverride.IsEmpty)
            {
                int len = Label.Length;
                int width = len;
                int height = 1;

                if (HasFrameBorder)
                {
                    width += 2;
                    height += 2;
                }

                return new Size(Math.Max(width, MinimumWidth), height);
            }
            else
            {
                return AutoSizeOverride;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region Button Class
    /// <summary>
    /// A button can be clicked, which happens when the left mouse button is pressed then released while
    /// over the button.
    /// </summary>
    public class Button : Control
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when a button has been clicked on (mouse down then up).
        /// </summary>
        public event EventHandler ButtonClicked;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        public Button(ButtonTemplate template)
            :base(template)
        {
            this.Label = template.Label;
            this.LabelAlignment = template.LabelAlignment;
            HilightWhenMouseOver = template.HilightWhenMouseOver;
            HasFrame = template.HasFrameBorder;
            CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;

            LabelRect = new Rect(Point.Origin, this.Size);
            VAlignment = template.VAlignment;

            if (template.HasFrameBorder)
            {
                //LabelRect = new Rect(1, 1, this.Size.Width - 2, 1);
                LabelRect = Rect.Inflate(LabelRect, -1, -1);
                
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the button Label.
        /// </summary>
        protected string Label { get; private set; }

        /// <summary>
        /// Get or set the label's horizontal alignment.
        /// </summary>
        protected HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// Get or set the label's vertical alignment.
        /// </summary>
        protected VerticalAlignment VAlignment { get; set; }
        // ////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region  Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This base method draws the label if OwnerDraw is false.  Override to add custom
        /// drawing code here.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();
            if (!OwnerDraw)
            {
                Canvas.PrintStringAligned(LabelRect.UpperLeft,Label, 
                    LabelAlignment,VAlignment,
                    LabelRect.Size);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the color of the main control area based on its current state.
        /// </summary>
        /// <returns></returns>
        protected override ColorStyle GetMainStyle()
        {
            if (IsActive && IsBeingPushed)
            {
                return DefaultStyles.Depressed;
            }
            return base.GetMainStyle();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when a mouse button is pressed while over this button.  Triggers proper
        /// events.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonUp(MouseData mouseData)
        {
            bool wasBeingPushed = IsBeingPushed; // store, since base call will reset this to false

            base.OnMouseButtonUp(mouseData);

            if (mouseData.MouseButton == MouseButton.LeftButton && wasBeingPushed)
            {
                OnButtonClicked();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Triggers the OnButtonClicked() event.  Called by the framework when a buton click
        /// action is performed.
        /// </summary>
        protected virtual void OnButtonClicked()
        {
            if (ButtonClicked != null)
            {
                ButtonClicked(this, EventArgs.Empty);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        private Rect LabelRect { get; set; }
        #endregion
    }
    #endregion
}
