using System;
using OchreGui.Utility;
using OchreGui;


namespace OchreGui.Sample3
{
    /// Our custom window is going to create the two main components of the application: the map area and
    /// the controls area.  We are going to offload all of the logic having to do with the buttons and moving
    /// the player around to seperate objects - the MyManager class and the MapView class, which we will
    /// define later.  The only thing we are going to manage here is basically the setup code.
    class MyWindow : Window
    {
        /// Store the MapView and Player classes as fields - we will use them later.
        MapView map;
        Player player;


        /// The constructor has been modified so that it must be passed a Player object, which
        /// we will store in a field for later use.
        public MyWindow(WindowTemplate template,Player player)
            : base(template)
        {
            this.player = player;
        }

        /// Again, most of our setup code goes in the overriden OnSettingUp() method.
        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            /// Our MapView element, which we will look at later, will be responsible for drawing
            /// the player.  This is the area in which the player can move freely.  Note that in
            /// addition to the standard Template parameter, our MapView class also needs a Player
            /// parameter.
            map = new MapView(new PanelTemplate()
            {
                Size = new Size(38, 13),
                UpperLeftPos = new Point(1, 11),
                HasFrame = false
            }, 
            player);

            /// We are going to use another Panel object to provde a frame around our MapView area.  We could
            /// have, of course, just added a frame to the MapView, but that would have complicated
            /// dealing with the player position.  This way, if the player position is at 0;0, then
            /// we draw the player symbol at position 0;0 in the MapView.
            Panel mapFrame = new Panel(new PanelTemplate()
            {
                Size = new Size(40, 15),
                UpperLeftPos = new Point(0, 10)
            });

            /// We have to be sure to add the MapView after the mapFrame, or else the map will
            /// be covered up the mapFrame and will not be visible.
            AddControls(mapFrame, map);

            /// Add the Manager, which we will look at later.
            AddManager(new MyManager(map));
        }





    }
}
