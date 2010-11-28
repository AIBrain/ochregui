using System;
using OchreGui.Utility;
using OchreGui;

/// This example builds on Sample 1 to provide a more rogue-like "hello world" - a little '@' walking around
/// the screen.  The symbol can be moved with either the keyboard arrow keys, or by clicking the directional
/// buttons.  As an added feature, hovering over the @ symbol helpfully confirms what the @ symbol represents...

namespace OchreGui.Sample2
{

    /// Create the application class as we did in Sample1.
    class MyApplication : Application
    {
        protected override void Setup(ApplicationInfo info)
        {
            base.Setup(info);

            MyWindow myWindow = new MyWindow(new WindowTemplate());

            SetWindow(myWindow);
        }
    }



    /// Again, our Window is going to contain most of the logic.
    /// 
    class MyWindow : Window
    {
        public MyWindow(WindowTemplate template)
            : base(template)
        {
        }


        /// Here we create all of the Gui elements and add them to this window.
        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            /// Set the label strings for each of the directional buttons
            string upLabel = ((char)libtcod.TCODSpecialCharacter.ArrowNorth).ToString();
            string downLabel = ((char)libtcod.TCODSpecialCharacter.ArrowSouth).ToString();
            string rightLabel = ((char)libtcod.TCODSpecialCharacter.ArrowEast).ToString();
            string leftLabel = ((char)libtcod.TCODSpecialCharacter.ArrowWest).ToString();



            /// We are going to create a series of 4 buttons, each representing a direction.
            /// To do this, I'll illustrate some of the helper methods for positioning controls.
            /// 
            /// First we position the upButton normally...
            ButtonTemplate upButtonTemplate = new ButtonTemplate()
            {
                Label = upLabel,
                UpperLeftPos = new Point(3, 0)
            };



            /// Now the RIGHT direction button, which I want to sit just off the lower right side
            /// of the previously defined UP button.  Notice the string of methods used to set this
            /// UpperLeftPos.  We can break it down as follows:
            ///     1.  upButtonInfo.CalculateRect() returns the Rect structure representing the previously
            ///         defined UP button
            ///     2.  Rect.LowerRight gives us the Point structure of the lower right corner of that rect
            ///     3.  Shift(int dx, int dy) shifts that point 1 space to the right and 1 space down.
            /// Basically this is a shorthand way (especially in todays world of
            /// intellisense IDE's) of specifying a position relative to another button.  In the next example,
            /// we will see a different (and more intuitive) way of doing this sort of relative layout.

            ButtonTemplate rightButtonTemplate = new ButtonTemplate()
            {
                Label = rightLabel,
                UpperLeftPos = upButtonTemplate.CalculateRect().LowerRight.Shift(1,1)
            };


            

            /// For the DOWN button we will need to do something a little different.  I want to set the upper right
            /// corner of the DOWN button off the lower left corner of the RIGHT button.  However, ButtonInfo
            /// does not expose a property to set the upper right position (since this would allow the user
            /// to specify two different positions in the same constructor).  But there is still a way to do this:
            /// just create the ButtonInfo normally, leaving out the position...
            ButtonTemplate downButtonTemplate = new ButtonTemplate()
            {
                Label = downLabel,
            };

            /// Then we use the SetUpperRight method to set the position, like so:

            downButtonTemplate.SetUpperRight(rightButtonTemplate.CalculateRect().LowerLeft.Shift(-1, 1));



            /// Finally, our LEFT button's upper right corner is just off the lower left corner
            /// of the UP button:

            ButtonTemplate leftButtonTemplate = new ButtonTemplate()
            {
                Label = leftLabel
            };
            leftButtonTemplate.SetUpperRight(upButtonTemplate.CalculateRect().LowerLeft.Shift(-1, 1));




            /// We just need to make sure we actually create the buttons, like so:

            Button upButton = new Button(upButtonTemplate);
            Button rightButton = new Button(rightButtonTemplate);
            Button downButton = new Button(downButtonTemplate);
            Button leftButton = new Button(leftButtonTemplate);



            /// Our quit button, this time we will give it the upper left position explicitly.  Also, since
            /// we don't need to keep the ButtonTemplate around, we do the creation in one step.
            Button quitButton = new Button(new ButtonTemplate()
            {
                Label = "QUIT",
                UpperLeftPos = new Point(74, 0)
            });




            /// This time we use AddControls to add multiple controls to the window.  The controls are added
            /// in order, from lowest to highest.
            AddControls(quitButton, upButton, downButton, leftButton, rightButton);

            /// Hook into each of the required button event handlers.
            quitButton.ButtonPushed += new EventHandler(quitButton_ButtonClickedEventHandler);

            upButton.ButtonPushed += new EventHandler(upButton_ButtonClicked);
            downButton.ButtonPushed += new EventHandler(downButton_ButtonClicked);
            rightButton.ButtonPushed += new EventHandler(rightButton_ButtonClicked);
            leftButton.ButtonPushed += new EventHandler(leftButton_ButtonClicked);
        }

        /// This will hold the current position of the @ symbol on the screen.
        Point currentPos;



        /// Handle the button clicks for any of the directional buttons.  Now is a good time to point out that
        /// the types in Utility, such as Point used here, are immutable data types (like System.String).  
        /// So operations such as Point.Shift do not modify the actual object, but returns a new Point instance.
        void leftButton_ButtonClicked(object sender, EventArgs e)
        {
            if (currentPos.X > 0)
                currentPos = currentPos.Shift(-1, 0);
            
        }

        /// <summary>
        /// The static method Application.ScreenSize returns the size of the screen (more technically, the 
        /// size of TCODConsole.root).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void rightButton_ButtonClicked(object sender, EventArgs e)
        {
            if (currentPos.X < Application.ScreenSize.Width-1)
                currentPos = currentPos.Shift(1, 0);
        }

        void downButton_ButtonClicked(object sender, EventArgs e)
        {
            if (currentPos.Y < Application.ScreenSize.Height-1)
                currentPos = currentPos.Shift(0, 1);
        }

        void upButton_ButtonClicked(object sender, EventArgs e)
        {
            if (currentPos.Y > 0)
                currentPos = currentPos.Shift(0, -1);
        }

        /// And we need to handle our quit button, as well.
        void quitButton_ButtonClickedEventHandler(object sender, EventArgs e)
        {
            ParentApplication.IsQuitting = true;
        }

        /// Draw the player symbol at the current position.  This time we specify
        /// the optional Pigment for the drawing operation.  WindowPigments holds some Pigments
        /// that are used automatically by the framework when drawing Gui elements.
        protected override void Redraw()
        {
            base.Redraw();

            Canvas.PrintChar(currentPos, '@', Pigments[PigmentType.ViewNormal]);
        }

        /// Here we handle the keyboard to check for the arrow keys.  KeyboardData contains basically the same
        /// information as TCODKey.
        /// 
        /// All of the On* methods of the base classes are message handlers - that is, they are called when the
        /// framework sends them a message.  The base classes typically perform all the grunge work of handling
        /// the messages (including triggering any events), so to add custom message handling we just override
        /// the relavent On* method.  An alternative to overriding is to hook into the events, but it is usually
        /// preferrable to using an override instead of events (for efficiency and code cleanliness reasons, among
        /// others).
        protected override void OnKeyPressed(KeyboardData keyData)
        {
            base.OnKeyPressed(keyData);

            switch (keyData.KeyCode)
            {
                case libtcod.TCODKeyCode.Left:
                    if (currentPos.X > 0)
                        currentPos = currentPos.Shift(-1, 0);
                    break;

                case libtcod.TCODKeyCode.Right:
                    if (currentPos.X < Application.ScreenSize.Width-1)
                        currentPos = currentPos.Shift(1, 0);
                    break;

                case libtcod.TCODKeyCode.Down:
                    if (currentPos.Y < Application.ScreenSize.Height-1)
                        currentPos = currentPos.Shift(0, 1);
                    break;

                case libtcod.TCODKeyCode.Up:
                    if (currentPos.Y > 0)
                        currentPos = currentPos.Shift(0, -1);
                    break;
            }
        }

        /// Usually, to add a tooltip, we would set a Control's TooltTipText property to the desired text.  However,
        /// here we are adding a tooltip to the entire window, so this won't work.  Window's do not provide automatic
        /// tooltip support (probably because it is usually a bad idea).  Fortunately, adding tooltips
        /// to a window is fairly easy.  We just override OnMouseHoverBegin, and call ShowTooltip().  The framework
        /// handles closing the tooltip automatically when the mouse moves.
        protected override void OnMouseHoverBegin(MouseData mouseData)
        {
            base.OnMouseHoverBegin(mouseData);

            /// We only show the tooltip when the mouse pointer is at the same position as the player position.
            if(mouseData.Position == currentPos)
                ShowTooltip("It's you!", mouseData.Position);
        }
    }

    /// Again, the boilerplate start code.
    class Sample2
    {
        static void Main(string[] args)
        {
            using (MyApplication myApp = new MyApplication())
            {
                myApp.Start(new ApplicationInfo()
                {
                    Title = "Sample2 - Walker",
                    ScreenSize = new Size(80,40)
                });
            }
        }
    }
}

