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
    #region ApplicationInfo
    /// <summary>
    /// This class holds the options passed to Application.Start().
    /// </summary>
    public class ApplicationInfo
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor, sets data to defaults
        /// </summary>
        public ApplicationInfo()
        {
            Fullscreen = false;
            ScreenSize = new Size(80, 60);
            Title = "";
            Font = null;
            FontFlags = TCODFontFlags.LayoutAsciiInColumn;
            DefaultStyles = new Styles();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// True if fulscreen, defaults to false
        /// </summary>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// The size of the screen.  This sets the screen resolution if Fullscreen is true,
        /// otherwise affacts the system window size.  Defaults to 80x60 characters.
        /// </summary>
        public Size ScreenSize { get; set; }

        /// <summary>
        /// The title of the system window - only applicable if Fullscreen is false
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the font file to use
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Information about the specified font as per TCODFontFlags
        /// </summary>
        public TCODFontFlags FontFlags { get; set; }

        /// <summary>
        /// The ColorStyles that are passed down (by default) to child Windows.  Defaults to a 
        /// pre-generated set of ColorStyles.
        /// </summary>
        public Styles DefaultStyles { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region Application Class
    /// <summary>
    /// Main application class.  Override Setup() and Update() to add application-specific
    /// setup code.
    /// </summary>
    public class Application : IDisposable
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the application is setting up.  This is raised after TCODInitRoot() has
        /// been called (setting the screen size and font).
        /// </summary>
        public event EventHandler SetupEventHandler;

        /// <summary>
        /// Raised each iteration of the main application loop
        /// </summary>
        public event EventHandler UpdateEventHandler;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        public Application()
        {
            IsQuitting = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// If the application wants to quit.  Set to true to quit.
        /// </summary>
        public bool IsQuitting { get; set; }

        /// <summary>
        /// The DefaultStyles that are passed to child Windows.
        /// </summary>
        public Styles DefaultStyles { get; protected set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes libtcod and starts the application's main loop.  This will loop 
        /// until IsQuitting is set to true or the main system window is closed.
        /// </summary>
        public void Start(ApplicationInfo setupInfo)
        {
            Setup(setupInfo);

            Run();

            CurrentWindow.OnQuitting();
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Sets the current Window, which will immediately begin to receive
        /// framework messages.  Windows can be changed at any time.
        /// </summary>
        /// <param name="win"></param>
        public void SetWindow(Window win)
        {
            if (win == null)
            {
                throw new ArgumentNullException("win");
            }

            Input = new InputManager(win);
            CurrentWindow = win;
            win.ParentApplication = this;

            if (!win.isSetup)
            {
                win.OnSettingUp();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of TCODConsole.root
        /// </summary>
        public static Size ScreenSize
        {
            get
            {
                return new Size(TCODConsole.root.getWidth(), TCODConsole.root.getHeight());
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets a Rect representing the screen (or the system window).  The UpperLeft position
        /// will always be 0,0.
        /// </summary>
        public static Rect ScreenRect
        {
            get
            {
                return new Rect(Point.Origin, ScreenSize);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the current window
        /// </summary>
        protected Window CurrentWindow { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the current InputManager for the current Window
        /// </summary>
        protected InputManager Input { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Override and add implementation specific setup code after calling base method
        /// </summary>
        /// <param name="info"></param>
        protected virtual void Setup(ApplicationInfo info)
        {
            if (!string.IsNullOrEmpty(info.Font))
            {
                TCODConsole.setCustomFont(info.Font,
                    (int)(TCODFontFlags.LayoutTCOD | TCODFontFlags.Grayscale));
            }

            TCODConsole.initRoot(info.ScreenSize.Width, info.ScreenSize.Height, info.Title,
                info.Fullscreen, TCODRendererType.SDL);

            TCODMouse.showCursor(true);

            if (SetupEventHandler != null)
            {
                SetupEventHandler(this, EventArgs.Empty);
            }

            DefaultStyles = info.DefaultStyles;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called each iteration of the main loop (each frame).  
        /// Override and add implentation specific update code after calling base method.
        /// </summary>
        protected virtual void Update()
        {
            if (UpdateEventHandler != null)
            {
                UpdateEventHandler(this, EventArgs.Empty);
            }

            uint ellapsed = TCODSystem.getElapsedMilli();

            CurrentWindow.OnTick();
            Input.Update(ellapsed);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private int Run()
        {
            if (CurrentWindow == null)
            {
                Window win = new Window(new WindowTemplate());
                SetWindow(win);
            }

            while (!TCODConsole.isWindowClosed() && !IsQuitting)
            {
                Update();
                Draw();
            }

            return 0;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        private void Draw()
        {
            CurrentWindow.OnDraw();

            TCODConsole.flush();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Dispose
        private bool _alreadyDisposed;

        ~Application()
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
                if(CurrentWindow != null)
                    CurrentWindow.Dispose();
            }
            _alreadyDisposed = true;
        }
        #endregion
    }
    #endregion
}

