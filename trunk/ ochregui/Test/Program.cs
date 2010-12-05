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
                StartingValue = 15,
            });

            AddControl(spin);

            Slider sld = new Slider(new SliderTemplate()
            {
                MinimumValue = -30,
                MaximumValue = 30,
                UpperLeftPos = new Point(10,5),
                //MinimumWidth = 12,
                Label = "Slide",
                BarPigment = new Pigment(0x0044ef,0x221144),
                StartingValue = 5
            });

            AddControl(sld);

            tBox = new TextBox(new TextBoxTemplate()
            {
                Size = new Size(25,10),
                UpperLeftPos = new Point(1,10),
                HasFrame = false,
                TextSpeed = 20,
                Pigments = new PigmentAlternatives()
                {
                    {PigmentType.Window,new Pigment(0X55CCAA,0X222222)}
                }
            });

            Button addTextBtn = new Button(new ButtonTemplate()
            {
                Label = "Add Text",
                UpperLeftPos = new Point(15, 1)
            });

            RadioGroup rg = new RadioGroup(new RadioGroupTemplate()
            {
                Items = new System.Collections.Generic.List<RadioItemData>()
                {
                    new RadioItemData("Radio 1"),
                    new RadioItemData("Radio 2"),
                    new RadioItemData("Radio 3")
                },
                UpperLeftPos = new Point(5,30),
                RadioOnLeft = true,
                InitialSelectedIndex = 1,
                //AutoSizeOverride = new Size(1,3),
                //HasFrameBorder = false,
                HilightRadioMouseOver = true
            });

            AddControl(rg);

            AddControl(addTextBtn);

            addTextBtn.ButtonPushed += new EventHandler(addTextBtn_ButtonPushed);

            AddControl(tBox);

            spin.ValueChanged += new EventHandler(spin_ValueChanged);

            sld.ValueChanged += new EventHandler(sld_ValueChanged);

            tBox.AddText("This is a bunch of text that will get printed to the console.");
        }

        void sld_ValueChanged(object sender, EventArgs e)
        {
            int spd = (sender as Slider).CurrentValue;

            if (spd >= 0)
            {
                tBox.TextSpeed = (uint)spd;
                tBox.AddText("\nChanged: " + tBox.TextSpeed.ToString() + "\n",
                    new Pigment(0xcc3355,0x000000));
            }
        }

        void addTextBtn_ButtonPushed(object sender, EventArgs e)
        {
            tBox.AddText("\n\nSome more text to get printed, with some commas and stuff.");
            tBox.AddText("\na b c d e f g h i j k l m n o p q r s t u v w x y z aa bb cc dd ee ff gg hhh iii jjj kkk");
        }

        void spin_ValueChanged(object sender, EventArgs e)
        {
            tBox.TextSpeed = (uint)(sender as SpinBox).CurrentValue;
            tBox.AddText("\nchanged: " + tBox.TextSpeed.ToString() + "\n",
                new Color(libtcod.TCODColor.amber));
        }

        protected override void Redraw()
        {
            base.Redraw();

            if (HasFrame)
            {
                Canvas.PrintFrame(null, DetermineFramePigment());
            }

        }

        TextBox tBox;

    }
}
