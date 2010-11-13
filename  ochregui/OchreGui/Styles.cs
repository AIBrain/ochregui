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

namespace OchreGui
{
    /// <summary>
    /// Stores all of the default styles used by the framework.
    /// </summary>
    public class Styles : IDisposable
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Defaults to the predefined styles.
        /// </summary>
        public Styles()
        {
            Window = new ColorStyle(0xDDDDDD, 0x000000);
            Frame = new ColorStyle(0x6D3D00, 0x3E1D00);
            Active = new ColorStyle(0xDDDDDD, 0x622E00);
            Inactive = new ColorStyle(0x7C7C7C, 0x2E2E2E);
            Hilight = new ColorStyle(0xFFFFAA, 0x723E00);
            Depressed = new ColorStyle(0x6B6B6B, 0x431E00);
            Selected = new ColorStyle(0x0098F4, 0x622E00);
            Tooltip = new ColorStyle(0x2B2B8F, 0xCCEEFF);
            DragItem = new ColorStyle(0xD6AC8B, 0xF45B00);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        public Styles Copy()
        {
            return (Styles)this.MemberwiseClone();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The main widget drawing area style, mainly used for owner-drawn controls,
        /// Windows, and Panels.
        /// </summary>
        public ColorStyle Window { get; set; }

        /// <summary>
        /// The style of the frame.
        /// </summary>
        public ColorStyle Frame { get; set; }

        /// <summary>
        /// A control's style when active.
        /// </summary>
        public ColorStyle Active { get; set; }

        /// <summary>
        /// A control's style when inactive.
        /// </summary>
        public ColorStyle Inactive { get; set; }

        /// <summary>
        /// A control's style when hilighted, for example when the mouse
        /// pointer is over it.
        /// </summary>
        public ColorStyle Hilight { get; set; }

        /// <summary>
        /// A button's style when depressed.
        /// </summary>
        public ColorStyle Depressed { get; set; }

        /// <summary>
        /// A control's style when selected.
        /// </summary>
        public ColorStyle Selected { get; set; }

        /// <summary>
        /// The style of a tooltip.
        /// </summary>
        public ColorStyle Tooltip { get; set; }

        /// <summary>
        /// The style of the visual component of a drag and drop operation.
        /// <Note>Drag and drop is not currently implemented.</Note>
        /// </summary>
        public ColorStyle DragItem { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #region Dispose
        private bool _alreadyDisposed;

        ~Styles()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed)
                return;
            if (isDisposing)
            {
                Window.Dispose();
                Frame.Dispose();
                Active.Dispose();
                Inactive.Dispose();
                Hilight.Dispose();
                Depressed.Dispose();
                Selected.Dispose();
                Tooltip.Dispose();
                DragItem.Dispose();               
            }
            _alreadyDisposed = true;
        }
        #endregion
    }
}

