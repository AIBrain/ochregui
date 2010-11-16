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
    /// Stores all of the default pigments used by the framework.
    /// </summary>
    public class DefaultPigments : IDisposable
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Defaults to the predefined pigments.
        /// </summary>
        public DefaultPigments()
        {
            Window = new Pigment(0xDDDDDD, 0x000000);
            Frame = new Pigment(0x6D3D00, 0x3E1D00);
            Active = new Pigment(0xDDDDDD, 0x622E00);
            Inactive = new Pigment(0x7C7C7C, 0x2E2E2E);
            Hilight = new Pigment(0xFFFFAA, 0x723E00);
            Depressed = new Pigment(0x6B6B6B, 0x431E00);
            Selected = new Pigment(0x0098F4, 0x622E00);
            Tooltip = new Pigment(0x2B2B8F, 0xCCEEFF);
            DragItem = new Pigment(0xD6AC8B, 0xF45B00);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        public DefaultPigments Copy()
        {
            return (DefaultPigments)this.MemberwiseClone();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The main widget drawing area pigment, mainly used for owner-drawn controls,
        /// Windows, and Panels.
        /// </summary>
        public Pigment Window { get; set; }

        /// <summary>
        /// The pigment of the frame.
        /// </summary>
        public Pigment Frame { get; set; }

        /// <summary>
        /// A control's pigment when active.
        /// </summary>
        public Pigment Active { get; set; }

        /// <summary>
        /// A control's pigment when inactive.
        /// </summary>
        public Pigment Inactive { get; set; }

        /// <summary>
        /// A control's pigment when hilighted, for example when the mouse
        /// pointer is over it.
        /// </summary>
        public Pigment Hilight { get; set; }

        /// <summary>
        /// A button's pigment when depressed.
        /// </summary>
        public Pigment Depressed { get; set; }

        /// <summary>
        /// A control's pigment when selected.
        /// </summary>
        public Pigment Selected { get; set; }

        /// <summary>
        /// The pigment of a tooltip.
        /// </summary>
        public Pigment Tooltip { get; set; }

        /// <summary>
        /// The pigment of the visual component of a drag and drop operation.
        /// <Note>Drag and drop is not currently implemented.</Note>
        /// </summary>
        public Pigment DragItem { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #region Dispose
        private bool _alreadyDisposed;

        ~DefaultPigments()
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

