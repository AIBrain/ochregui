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

namespace OchreGui
{
    #region PanelInfo Class
    /// <summary>
    /// This class builds on the Control Template, and adds options specific to a Panel.
    /// </summary>
    public class PanelTemplate : ControlTemplate
    {
        /// <summary>
        /// Default constructor initializes properties to their defaults.
        /// </summary>
        public PanelTemplate()
        {
            HasFrame = true;
            CanHaveKeyboardFocus = false;
            HilightedWhenMouseOver = false;
            Size = new Size(1, 1);
        }

        /// <summary>
        /// The size of the panel, defaults to 1 x 1.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// True if a frame will initially be drawn around the panel.  Defaults to true.
        /// </summary>
        public bool HasFrame { get; set; }

        /// <summary>
        /// True if the panel can receive the keyboard focus.  Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// True if the panel will be drawn with hilighted colors when under the mouse pointer.
        /// Defaults to false.
        /// </summary>
        public bool HilightedWhenMouseOver { get; set; }

        /// <summary>
        /// Calculates the size of the panel.  For a panel, the size is specified by the 
        /// Size property; this method simply returns that property.
        /// </summary>
        /// <returns></returns>
        public override OchreGui.Utility.Size CalculateSize()
        {
            return Size;
        }
    }
    #endregion


    #region Panel Class
    /// <summary>
    /// A panel is a simple control whose size is manually set.  Other than drawin a frame,
    /// a panel provides little default drawing or message handling code.
    /// </summary>
    public class Panel : Control
    {
        /// <summary>
        /// Construct a Panel instance from the given template.
        /// </summary>
        /// <param name="template"></param>
        public Panel(PanelTemplate template)
            : base(template)
        {
            this.HasFrame = template.HasFrame;

            this.CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;
            this.HilightWhenMouseOver = template.HilightedWhenMouseOver;
        }


        /// <summary>
        /// Returns the pigment for the main area of the panel.  Base method returns
        /// Pigments[PigmentType.Window]
        /// </summary>
        /// <returns></returns>
        protected override Pigment DetermineMainPigment()
        {
            return Pigments[PigmentType.Window];
        }
    }
    #endregion
}
