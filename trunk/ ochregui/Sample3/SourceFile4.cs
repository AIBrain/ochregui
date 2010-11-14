using System;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Sample3
{
    /// A Manager object provides a way to have an element that receives framework and input messages as a
    /// control would, but does not have any visual element associated with it.  That is, unlike a Widget (and
    /// all of its derivatives), a Manager does not have any concept of position or size, and does not
    /// contain a Canvas or any drawing methods.
    /// 
    /// What a Manager does provide is one way to divide up user interface logic into discreet parts, one of the
    /// things we are trying to show in this example.  Unlike other fully-featured gui API's, OchreGUI does not
    /// have a concept of recursively defined control elements (controls cannot be added to other controls).  However,
    /// when controls need to be grouped logically (not necessarily visually), then using a Manager is often a 
    /// good way to go about it.
    /// 
    /// Here we will derive a custom MyManager class, and have it take care of setting up and responding to all of the
    /// buttons, along with checking for key presses.
    class MyManager : Manager
    {
        public MyManager(MapView mapView)
        {
            this.mapView = mapView;
        }

        MapView mapView;

        /// <summary>
        /// As mentioned, MyManager is responsible for initializing all of the buttons and 
        /// user interface logic, which we do in the OnSettingUp method.
        /// </summary>
        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            /// First define the teleport button template.  Here we are going to use the MinimumWidth
            /// properties for all of these buttons so that they are all the same size, and thus
            /// line up nicely.
            ButtonTemplate telButtonTemplate = new ButtonTemplate()
            {
                Label = "Teleport",
                UpperLeftPos = new Point(3, 1),
                MinimumWidth = 12
            };

            /// Here we see a more intuitive way of positioning a control (or, more technically, 
            /// a control template) relative to another one.  In this case, we are going to
            /// place our redButtonTemplate directly underneath the telButtonTemplate, with 1
            /// extra space in between.

            ButtonTemplate redButtonTemplate = new ButtonTemplate()
            {
                Label = "Turn Red",
                MinimumWidth = 12
            };
            redButtonTemplate.AlignTo(LayoutDirection.South, telButtonTemplate, 1);

            /// This one will go directly to the right of the red button template.

            ButtonTemplate blueButtonTemplate = new ButtonTemplate()
            {
                Label = "Turn Blue",
                MinimumWidth = 12
            };
            blueButtonTemplate.AlignTo(LayoutDirection.East, redButtonTemplate, 2);




            /// Create the buttons, add them to the window, and hook into their events.

            Button teleportButton = new Button(telButtonTemplate);
            Button turnRedButton = new Button(redButtonTemplate);
            Button turnBlueButton = new Button(blueButtonTemplate);

            Button quitButton = new Button(new ButtonTemplate()
            {
                Label = "QUIT",
                UpperLeftPos = new Point(74,0)
            });

            ParentWindow.AddControls(quitButton, turnBlueButton, turnRedButton, teleportButton);

            quitButton.ButtonPushed += new EventHandler(quitButton_ButtonClicked);

            teleportButton.ButtonPushed += new EventHandler(teleportButton_ButtonClicked);

            turnBlueButton.ButtonPushed += new EventHandler(turnBlueButton_ButtonClicked);

            turnRedButton.ButtonPushed += new EventHandler(turnRedButton_ButtonClicked);




            /// Here we hilight the scheduling feature of the framework.  All descendents of Component, which
            /// includes our MyManager class, have access to the AddSchedule method.  This method takes as
            /// a parameter a Schedule instance, which in turn is constructed with 2 arguments:
            /// 1.  A delegate indiciating which method to call at the appropriate time
            /// 2.  A delay value in milliseconds.
            /// 
            /// Here, we are telling the scheduler to call our Flicker method every 100 milliseconds.
            AddSchedule(new Schedule(Flicker, 100));
        }

        void turnRedButton_ButtonClicked(object sender, EventArgs e)
        {
            mapView.SetPlayerColor(new Color(255, 0, 0));
        }

        void turnBlueButton_ButtonClicked(object sender, EventArgs e)
        {
            mapView.SetPlayerColor(new Color(libtcod.TCODColor.blue));
        }

        void teleportButton_ButtonClicked(object sender, EventArgs e)
        {
            mapView.TeleportPlayer();
        }

        /// <summary>
        /// This method gets called automatically by the scheduler every 100 millisceonds.  We will
        /// use this feature to add some animation.
        /// </summary>
        void Flicker()
        {
            float rndScale = Rand.Float() / 2f + 0.5f;

            mapView.SetPlayerIntensity(rndScale);
        }

        /// <summary>
        /// Our quit button event handler again, exept we have to go back an extra level
        /// to get to the parent application object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void quitButton_ButtonClicked(object sender, EventArgs e)
        {
            ParentWindow.ParentApplication.IsQuitting = true;
        }

        /// <summary>
        /// It should be noted that Managers always receive input messages, unlike controls.  A control
        /// needs to have the current keyboard focus before it can receive keyboard messages, for example.  All
        /// Manager objects added to the current Window will receive all mouse and keyboard messages.
        /// </summary>
        /// <param name="keyData"></param>
        protected override void OnKeyPressed(KeyboardData keyData)
        {
            base.OnKeyPressed(keyData);

            switch (keyData.KeyCode)
            {
                case libtcod.TCODKeyCode.Up:
                    mapView.MovePlayer(0, -1);
                    break;

                case libtcod.TCODKeyCode.Down:
                    mapView.MovePlayer(0, 1);
                    break;

                case libtcod.TCODKeyCode.Right:
                    mapView.MovePlayer(1, 0);
                    break;

                case libtcod.TCODKeyCode.Left:
                    mapView.MovePlayer(-1, 0);
                    break;
            }
        }

    }
}
