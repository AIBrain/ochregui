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
    /// Implemented by widget templates and their derivatives. Defines the CalculateSize
    /// method, which should return the exact and final size of the widget.
    /// </summary>
    public interface ITemplate
    {
        Size CalculateSize();
    }

    /// <summary>
    /// The abstract base class for widget templates.  When subclassing a type of Widget, consider
    /// also subclassing WidgetTemplate to provide an interface for the client to specify
    /// options, and override CalculateSize to ensure that the widget is created with the correct
    /// size.
    /// </summary>
    public abstract class WidgetTemplate : ITemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        protected WidgetTemplate()
        {
            DefaultStyles = null;
            OwnerDraw = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The default styles that this widget will have.  Defaults to null, which tells
        /// the child classes to inherit or create new styles.
        /// </summary>
        public Styles DefaultStyles { get; set; }

        /// <summary>
        /// If true, then base classes will not do any drawing to the canvas, including clearing
        /// or blitting to the screen.  This property is present so that subclasses can implement
        /// specialized drawing code for optimization or that would otherwise be difficult to 
        /// implement using overrides/events.  Defaults to false.
        /// </summary>
        public bool OwnerDraw { get; set; }

        public abstract Size CalculateSize();
        // /////////////////////////////////////////////////////////////////////////////////
    }

    /// <summary>
    /// Base class for any component that gets drawn on the screen.  A widget provides
    /// an offscreen TCODConsole (Widget.Console) to draw to.  The framework automatically
    /// blits the Console to the screen each frame.
    /// </summary>
	public abstract class Widget : Component, IDisposable
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        public event EventHandler Draw;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        protected Widget(WidgetTemplate template)
		{
            this.DefaultStyles = template.DefaultStyles;

            this.ScreenPosition = new Point(0, 0);
            this.Size = template.CalculateSize();
            this.Canvas = new Canvas(Size);

            this.OwnerDraw = template.OwnerDraw;

		}
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns widget's rect in screen space coordinates
        /// </summary>
        public Rect ScreenRect {
			get { return new Rect(ScreenPosition, Size); }
		}

        /// <summary>
        /// Returns the widget's rect in local space coordinates.  The UpperLeft coordinate
        /// will always be the Origin (0,0).
        /// </summary>
        public Rect LocalRect
        {
            get { return new Rect(Point.Origin, Size); }
        }

        /// <summary>
        /// Get the Canvas object associated with this widget.
        /// </summary>
        public Canvas Canvas { get; protected set; }

        /// <summary>
        /// Get the default Styles associated with this widget.
        /// </summary>
        public Styles DefaultStyles { get; protected set; }

        /// <summary>
        /// Get the the Size of the widget.
        /// </summary>
        public Size Size { get; private set; }

        /// <summary>
        /// If true, then base classes will not do any drawing to the canvas, including clearing
        /// or blitting to the screen.  This property is present so that subclasses can implement
        /// specialized drawing code for optimization or that would otherwise be difficult to 
        /// implement using overrides/events.
        /// </summary>
        protected bool OwnerDraw { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The upper left position of this widget in screen space coordinates.
        /// </summary>
		protected internal Point ScreenPosition { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// If OwnerDraw is false, this base method sets the Canvas colors according to the
        /// GetMainStyle() method, and clears the Canvas.
        /// </summary>
        protected virtual void Redraw()
        {
            if (!OwnerDraw)
            {
                Canvas.SetDefaultColors(GetMainStyle());
                Canvas.Clear();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the ColorStyle of the main drawing area for the widget.  Override to change
        /// which style is used.  Base method returns this.DefaultStyles.Window.
        /// </summary>
        /// <returns></returns>
        protected virtual ColorStyle GetMainStyle()
        {
            return DefaultStyles.Window;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the ColorStyle of the frame area for this widget.  Override to change which
        /// style is used.  Base method returns this.DefaultStyles.Frame
        /// </summary>
        /// <returns></returns>
        protected virtual ColorStyle GetFrameStyle()
        {
            return DefaultStyles.Frame;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called during the drawing phase of the application loop.  Base method calls Redraw(), 
        /// triggers appropriate event, and blits the canvas to the screen if OwnerDraw is false.
        /// This method should rarely need to be overriden - instead, to provide custom drawing code
        /// (whether OwnerDraw is true or false), override Redraw(), GetMainStyle(), and GetFrameStyle().
        /// </summary>
        internal protected virtual void OnDraw()
        {
            Redraw();

            if (Draw != null)
            {
                Draw(this, EventArgs.Empty);
            }

            if (!OwnerDraw)
            {
                Canvas.ToScreen(this.ScreenPosition);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
		#endregion
        #region Dispose
        private bool _alreadyDisposed;

        ~Widget()
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
                if(Canvas != null)
                    Canvas.Dispose();

                if(DefaultStyles != null)
                    DefaultStyles.Dispose();
            }
            _alreadyDisposed = true;
        }
        #endregion
	}

}

