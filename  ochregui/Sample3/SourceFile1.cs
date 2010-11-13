using System;
using OchreGui.Utility;
using OchreGui;

/// For this example, we are going to build on Sample2 and try out some new features of OchreGui.
/// 
/// 
/// This example grew quit large, so I divided into four seperate source files.

namespace OchreGui.Sample3
{
    /// Here we will simulate some game state by defining a Player class.  This class will hold
    /// the current position, color, and intensity (which we will use later).
    class Player
    {
        public Player()
        {
            BaseColor = new Color(0x00ff00);
        }

        public Point CurrentPosition { get; set; }
        public Color BaseColor { get; set; }
        public float Intensity { get; set; }
    }


    /// All of this should look similar.  The only difference is that we are creating the Player
    /// object, and passing that object to our MyWindow instance.
    class MyApplication : Application
    {
        protected override void Setup(ApplicationInfo info)
        {
            base.Setup(info);

            player = new Player();

            MyWindow myWindow = new MyWindow(new WindowTemplate(),player);

            SetWindow(myWindow);
        }

        private Player player;
    }

    /// Again, mostly the same boilerplate code here, with the only change being the use of a different font.
    class MainClass
    {
        static void Main(string[] args)
        {
            using (MyApplication myApp = new MyApplication())
            {
                /// I got tired of looking at the tiny terminal.png font, so I'm going to start
                /// using something a little easier on the eyes.  Of course, we need to make sure that
                /// "courier12x12_aa_tc.png" is in our binary folder or the application will fail on start.
                myApp.Start(new ApplicationInfo()
                {
                    Title = "Sample3 - Walker, Part Two",
                    ScreenSize = new Size(40, 25),
                    Font = "courier12x12_aa_tc.png",
                    FontFlags = libtcod.TCODFontFlags.LayoutTCOD
                });
            }
        }
    }


}
