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

            SpinBox spin = new SpinBox(new SpinBoxTemplate()
            {
                Label = "TEST",
                MinimumValue = 0,
                MaximumValue = 1000,
                SpinDelay = 200,
                SpinSpeed = 10,
                StartingValue = -5
            });

            AddControl(spin);

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
                MinimumValue = -30,
                MaximumValue = 30,
                UpperLeftPos = new Point(10,5),
                MinimumWidth = 12,
                Label = "Slide",
                BarPigment = new Pigment(0x0044ef,0x221144)
            }));

            tBox = new TextBox(new TextBoxTemplate()
            {
                Size = new Size(25,10),
                UpperLeftPos = new Point(1,10),
                HasFrame = false,
                TextSpeed = 20
            });

            Button addTextBtn = new Button(new ButtonTemplate()
            {
                Label = "Add Text",
                UpperLeftPos = new Point(15, 1)
            });

            AddControl(addTextBtn);

            addTextBtn.ButtonPushed += new EventHandler(addTextBtn_ButtonPushed);

            AddControl(tBox);

            spin.ValueChanged += new EventHandler(spin_ValueChanged);

            AddSchedule(new Schedule(TestAnimBar,100));

            tBox.AddText("This is a bunch of text that will get printed to the console.");
        }

        void addTextBtn_ButtonPushed(object sender, EventArgs e)
        {
            tBox.AddText("Some more text to get printed, with some commas and stuff.");
            tBox.AddText("a b c d e f g h i j k l m n o p q r s t u v w x y z aa bb cc dd ee ff gg hhh iii jjj kkk");
        }

        void spin_ValueChanged(object sender, EventArgs e)
        {
            tBox.TextSpeed = (uint)(sender as SpinBox).CurrentValue;
        }

        protected override void Redraw()
        {
            base.Redraw();

            if (HasFrame)
            {
                Canvas.PrintFrame(null, DetermineFramePigment());
            }

            Canvas.PrintString(0, 8, currBarVal.ToString(),DefaultPigments.Active);
        }

        int currBarVal;
        ValueBar bar;

        TextBox tBox;

        void TestAnimBar()
        {
            currBarVal++;
            if (currBarVal > 100)
                currBarVal = 0;

            bar.CurrentValue = currBarVal;
        }
    }
}
