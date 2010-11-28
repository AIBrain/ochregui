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
    /// This class holds the application options passed to Application.Start().
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
            DefaultPigments = null;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// True if fulscreen.  Defaults to false
        /// </summary>
        public bool Fullscreen { get; set; }

        /// <summary>
        /// The size of the screen.  This sets the screen resolution if Fullscreen is true,
        /// otherwise affacts the system window size.  Defaults to 80x60 characters.
        /// </summary>
        public Size ScreenSize { get; set; }

        /// <summary>
        /// The title of the system window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The name of the font file to use, which must be in the same path as the executable.
        /// </summary>
        public string Font { get; set; }

        /// <summary>
        /// Information about the specified font as per TCODFontFlags.
        /// </summary>
        public TCODFontFlags FontFlags { get; set; }

        public DefaultPigments DefaultPigments { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region Application Class
    /// <summary>
    /// Represents the entire application, and controls top-level logic and state.  The Application
    /// contains a Window, which is a container for all of the controls.<para>This object, of which there
    /// is only one being executed, handles libtcod initialization, encapsulates the main application loop,
    /// and is the ultimate origin for all top level messages</para>
    /// <remarks>A custom class should be derived from Application to, at minimal, implement setup code by
    /// overriding OnSetup.  Call Application.Start to initialize and start the application loop, which will
    /// continue until IsQuitting is set to true.</remarks>
    /// </summary>
    public class Application : IDisposable
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when the application is setting up.  This is raised after TCODInitRoot() has
        /// been called, so place any intitialization code dependant on libtcod being initialized here.
        /// This event is provided in case the
        /// framework is being used in a non-standard way - typically, the derived class will place top level
        /// setup code in an overriden Setup method.
        /// </summary>
        public event EventHandler SetupEventHandler;

        /// <summary>
        /// Raised each iteration of the main application loop.  This event is provided in case the
        /// framework is being used in a non-standard way - typically, the derived class will place top level
        /// logic updating in an overriden Update method, or within a custom Window class.
        /// </summary>
        public event EventHandler UpdateEventHandler;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Application()
        {
            IsQuitting = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// True if the application wants to quit.  Set to true to quit.
        /// </summary>
        public bool IsQuitting { get; set; }


        public DefaultPigments DefaultPigments { get; protected set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Initializes libtcod and starts the application's main loop.  This will loop 
        /// until IsQuitting is set to true or the main system window is closed.
        /// </summary>
        /// <param name="setupInfo">An ApplicationInfo object containing the options specific
        /// to this application</param>
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
        /// framework messages.  Windows can be changed at any time.  If the specified window
        /// has not yet received a SettingUp message (i.e. it has not already been set as
        /// an application window previously), then OnSettingUp will be called.
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
            win.Pigments = new PigmentAlternatives(this.DefaultPigments, win.PigmentOverrides);

            if (!win.isSetup)
            {
                win.OnSettingUp();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the size of TCODConsole.root, which is the size of the screen (or system window)
        /// in cells.
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
        /// will always be the origin (0,0).
        /// </summary>
        public static Rect ScreenRect
        {
            get
            {
                return new Rect(Point.Origin, ScreenSize);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the Application's current window.
        /// </summary>
        public Window CurrentWindow { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the current InputManager for the current Window.
        /// </summary>
        protected InputManager Input { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called after Application.Start has been called.  Override and place application specific
        /// setup code here after calling base method.
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

            DefaultPigments = info.DefaultPigments;

            if (DefaultPigments == null)
            {
                DefaultPigments = DefaultPigments.FrameworkDefaults;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called each iteration of the main loop (each frame).  
        /// Override and add specific logic update code after calling base method.
        /// </summary>
        protected virtual void Update()
        {
            if (UpdateEventHandler != null)
            {
                UpdateEventHandler(this, EventArgs.Empty);
            }

            uint elapsed = TCODSystem.getElapsedMilli();

            CurrentWindow.OnTick();
            Input.Update(elapsed);
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

        /// <summary>
        /// Default finalizer calls Dispose.
        /// </summary>
        ~Application()
        {
            Dispose(false);
        }

        /// <summary>
        /// Safely dispose this object and all of its contents.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Override to add custom disposing code.
        /// </summary>
        /// <param name="isDisposing"></param>
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

