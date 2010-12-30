using System;

using OchreGui;
using OchreGui.Extended;
using OchreGui.Utility;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (MyApp myApp = new MyApp())
            {
                /// I got tired of looking at the tiny terminal.png font, so I'm going to start
                /// using something a little easier on the eyes.  Of course, we need to make sure that
                /// "courier12x12_aa_tc.png" is in our binary folder or the application will fail on start.
                myApp.Start(new ApplicationInfo()
                {
                    Title = "Test",
                    ScreenSize = new Size(40, 40),
                    Font = "courier12x12_aa_tc.png",
                    FontFlags = libtcod.TCODFontFlags.LayoutTCOD,

                });
            }
        }
    }


    class MyApp : Application
    {
        protected override void Setup(ApplicationInfo info)
        {
            base.Setup(info);

            MyWindow win = new MyWindow(new WindowTemplate()
            {
                HasFrame = true,
            });

            SetWindow(win);
        }

    }

    class MyWindow : Window
    {
        public MyWindow(WindowTemplate template)
            : base(template)
        {

        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            CheckBox cb1 = new CheckBox(new CheckBoxTemplate()
            {
                UpperLeftPos = new Point(1,1),
                Label = "check1",
                MinimumWidth = 10,
                HasFrameBorder = false,
                LabelAlignment = HorizontalAlignment.Center,
                Tooltip = "Testing Tootlip"
            });

            CheckBox cb2 = new CheckBox(new CheckBoxTemplate()
            {
                UpperLeftPos = new Point(1,3),
                Label = "check2",
                HasFrameBorder = false,
                AutoSizeOverride = new Size(11,1),
                LabelAlignment = HorizontalAlignment.Center,
            });

            CheckBox cb3 = new CheckBox(new CheckBoxTemplate()
            {
                UpperLeftPos = new Point(1, 5),
                Label = "check3",
                MinimumWidth = 10,
                HasFrameBorder = false,
                LabelAlignment = HorizontalAlignment.Center,
                CheckOnLeft = false,
            });

            CheckBox cb4 = new CheckBox(new CheckBoxTemplate()
            {
                UpperLeftPos = new Point(1, 7),
                Label = "check4",
                HasFrameBorder = false,
                AutoSizeOverride = new Size(11, 1),
                LabelAlignment = HorizontalAlignment.Center,
                CheckOnLeft = false,
            });

            ValueBar vb = new ValueBar(new ValueBarTemplate()
            {
                UpperLeftPos = new Point(1,12),
                MaximumValue = 100,
                StartingValue = 50,
                MinimumBGIntensity = .3f,
                MinimumFGIntensity = .5f,
                Width = 15,
                Tooltip = "Testing Tooltip"
            });

            Pigment p = new Pigment(0xffffff, 0x334455);

            AddControls(cb1, cb3,vb);

            AddSchedule(new Schedule(Callback, 2));
        }

        int test = 5;

        void Callback()
        {
            int x = 6;

            test += x;
        }

    }
}
