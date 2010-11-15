/// General Note:
/// 
/// If these samples appear contrived and messy, this is because:
/// 
/// 1.  They are meant to quickly show some of the features of OchreGui, and also how to set up a typical
///     skeleton for an application.  
/// 2.  These samples are NOT meant to show proper OOP programming, nor are they meant
///     to reproduce any sort of realistic use scenario.
///     
/// These samples are also not to fully document the library - hopefully the documentation will be fleshed out
/// in the near future.


using System;

using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Sample1
{
    /// The first thing we need to do is derive our custom application class from OchreGui.Application.  The Application
    /// class provides boilerplate libtcod setup, looping, and drawing code, and ways to hook into them via overrides.
    /// We can opt to put applicaion-wide logic in here, as well.  In this example, we are going to put all of our logic
    /// in the Window class, since we are going to only use one window during the entire lifetime of the application.
    class MyApplication : Application
    {
        /// Setup() gets called immediately after Application.Start(), and we can override this to provide custom
        /// initialization code.
        /// 
        /// The first thing we need to do in this method, however, is call the base method - this
        /// initializes libtcod, opening the system window and setting the font, all according to the options provided in
        /// the ApplicationInfo parameter.
        /// 
        /// After calling the base method, we add our setup code, in this case creating the MyWindow (defined below) object 
        /// and setting it as the current Window.
        protected override void Setup(ApplicationInfo info)
        {
            /// One thing to note: in almost all cases, when overriding a framework method it is necessary
            /// to call the base method first.  The base methods handle most of the gritty details of triggering
            /// events, propagating messages, and calling the necessary stub methods.  
            base.Setup(info);

            /// Just use the default options in the WindowTemplate.
            WindowTemplate mainWindowTemplate = new WindowTemplate();

            /// Create the mainWindow with the options specified in mainWindowInfo.  We define MyWindow below.
            MyWindow mainWindow = new MyWindow(mainWindowTemplate);

            /// Set the mainWindow to be MyApplication's window.  Note that if we don't set the application window,
            /// a default one is created and set automatically.
            SetWindow(mainWindow);

            
        }
    }


    /// An application needs a window to act as a container for controls and to expose framework message handling
    /// for us to hook into.  The simplest way to do this is to subclass the Window class and add our implementation
    /// code, much as we did with MyApplication above.
    class MyWindow : Window
    {
        /// Any subclass of Window (actually, of Widget, of which Window is derived from) must call the base constructor
        /// with a single parameter of type WindowTemplate, or a class derived from WindowTemplate.  We are 
        /// keeping this simple, so we will use the default WindowTemplate class instead of deriving our own.
        public MyWindow(WindowTemplate template)
            : base(template)
        {
        }

        /// <summary>
        /// Every component gets a SettingUp message once when they are about to be added to the framework.  For Windows,
        /// this happens after it is set as the Application's window.
        /// </summary>
        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            /// We are going to add a handy quit button to our Window, so we need to define the options for the button
            /// in a ButtonTemplate class.  We give it a Label, and we will also give it a nice Tooltip that will popup
            /// over the button when the mouse hovers over it.
            /// 
            /// The UpperLeftPos property determines where the upper left corner of the control will be.  While most
            /// of the UI layout in OchreGui will have to be done by hand (there are no fancy layout grids in this
            /// library), there are some helper methods available, but we'll save those for our next example.
            /// 
            /// One thing you may have noticed is that there is no way to specify the size of the button.  This is because
            /// all buttons (and many other controls) automatically size themselves according to the other options.  For
            /// a button, the height will always be 3 (to allow 1 space for the label and 2 spaces for the border), and 
            /// the width will usually be Label.Length + 2 for width (again, allowing room for the border).  All 
            /// of the *Info classes implement (or derive from) ISetupInfo, which provides the CalculateSize() method.  
            /// This method returns the final size that the control will be, depending on its type and the specified options.
            /// 
            /// If the combination of the specified position and the calculated size puts some or all of the control
            /// out of the screen area, the framework will automatcially slide the control so that none of it is clipped.
            ButtonTemplate quitButtonTemplate = new ButtonTemplate()
            {
                Label = "QUIT",
                Tooltip = "Quit the application",
                UpperLeftPos = new Point(35,0)
            };

            /// Create the button with the specified options contained in quitButtonTemplate.
            Button quitButton = new Button(quitButtonTemplate);

            /// We are going to hook into the button clicked event, so we know when the user has clicked on the
            /// quit button.
            quitButton.ButtonPushed += new EventHandler(quitButton_ButtonClicked);

            /// And, finally, add it to this Window.  Note that all controls added to a window must be unique objects, or
            /// an exception will be thrown by this method.  Also note that some of the properties of a contol (such as
            /// ScreenRect) are meaningless until they are added to a Window, which is why any setup code that depends
            /// on a fully initialized instance belongs in an overriden OnSettingUp() method, not in the constructor.
            AddControl(quitButton);
        }

        /// This is our handler method for when the quit button is clicked.  It simply sets the parent application's
        /// IsQuitting property to true, indicating that we want to quit now.  The framework does not quit immediately
        /// after IsQuitting is set - some controls may still receive messages as part of the current application
        /// loop.
        /// 
        /// The framework takes care of all deinitialization tasks automatically after quitting.  If we need custom
        /// deinitialization for any window or control, we can override OnQuit() and place it there.
        void quitButton_ButtonClicked(object sender, EventArgs e)
        {
            ParentApplication.IsQuitting = true;
        }

        /// We override this method to add custom drawing code for this window.  In this case, our only drawing
        /// code is to print a string to the upper left corner of the window.  The Canvas class (which every Widget has)
        /// provides some wrappers around libtcod drawing functions.  However, if we need to reach into the lower levels
        /// of drawing, Canvas exposes the underlying TCODConsole with the Console property.
        protected override void Redraw()
        {
            base.Redraw();

            Canvas.PrintString(0, 0, "Hello, OchreGui!");
        }
    }

    #region MainMethod
    /// Here we provide our Main method.
    class MainEntry
    {
        static void Main(string[] args)
        {
            /// Creating an Application class is similar to creating windows and controls - first we need to define
            /// the options in an ApplicationInfo class.
            ///
            /// Here we give our application a title and a screen size.
            ApplicationInfo myApplicationInfo = new ApplicationInfo()
            {
                Title = "Sample1",
                ScreenSize = new Size(80,40)
            };

            /// And then create it.  Notice that the Application implements IDisposable, so placing this in a using...
            /// block ensures proper disposal of resources (including all of the IDispose objects carried over from
            /// libtcod).  Actually starting the application is as easy as calling the Start method and specifying
            /// a previously constructed ApplicationInfo object.
            using (MyApplication myApp = new MyApplication())
            {
                myApp.Start(myApplicationInfo);
            }
        }
    }
    #endregion
}

/// To build the application, you will need to add the references for Utility.dll and OchreGui.dll.  You will also need
/// all of the usual libtcod dependencies in the path or the same location as the binary.
/// 
/// When running, make sure to hold the mouse pointer still over the quit button to see the tooltip popup.
/// 
