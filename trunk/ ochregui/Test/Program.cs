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
                    ScreenSize = new Size(40, 25),
                    Font = "courier12x12_aa_tc.png",
                    FontFlags = libtcod.TCODFontFlags.LayoutTCOD
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

            AddControl(new SpinBox(new SpinBoxTemplate()
            {
                Label = "TEST",
                MinimumValue = 0,
                MaximumValue = 1000,
                SpinDelay = 200,
                SpinSpeed = 10,
                StartingValue = -5
            }));

            bar = new ValueBar(new ValueBarTemplate()
            {
                UpperLeftPos = new Point(0,5),
                MinimumValue = -50,
                MaximumValue = 50,
                StartingValue = 40,
                Width = 10
            });

            AddControl(bar);



            AddControl(new Slider(new SliderTemplate()
            {
                MinimumValue = 0,
                MaximumValue = 255,
                UpperLeftPos = new Point(10,5),
                MinimumWidth = 12,
                //Label = "Slide me"
            }));


            AddSchedule(new Schedule(TestAnimBar,100));
        }

        protected override void Redraw()
        {
            base.Redraw();

            Canvas.PrintString(0, 8, currBarVal.ToString());
        }

        int currBarVal;
        ValueBar bar;
        void TestAnimBar()
        {
            currBarVal++;
            if (currBarVal > 100)
                currBarVal = 0;

            bar.CurrentValue = currBarVal;
        }
    }
}
