﻿using System;
using System.Collections;
using System.Collections.Generic;
using OchreGui;
using OchreGui.Extended;
using OchreGui.Utility;

namespace OchreGui.Demo
{
    class Page1 : DemoWindow
    {
        public Page1(DemoWindowTemplate template)
            : base(template)
        {

        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            // Start creating and adding the various sample controls.
            ButtonTemplate bt0 = new ButtonTemplate()
            {
                Label = "Autosize",
                UpperLeftPos = ViewRect.UpperLeft.Shift(1,3),
            };
            AddControl(new Button(bt0));

            ButtonTemplate bt1 = new ButtonTemplate()
            {
                Label = "Min. Size",
                MinimumWidth = 15
            };
            bt1.UpperLeftPos = bt0.CalculateRect().LowerLeft.Shift(0, 2);
            AddControl(new Button(bt1));

            ButtonTemplate bt2 = new ButtonTemplate()
            {
                Label = "No Border",
                HasFrameBorder = false,

            };
            bt2.UpperLeftPos = bt1.CalculateRect().LowerLeft.Shift(0, 2);
            AddControl(new Button(bt2));

            ButtonTemplate bt3 = new ButtonTemplate()
            {
                Label = "Centered",
                MinimumWidth = 15,
                LabelAlignment = HorizontalAlignment.Center
            };
            bt3.UpperLeftPos = bt2.CalculateRect().LowerLeft.Shift(0, 2);
            AddControl(new Button(bt3));

            ButtonTemplate bt4 = new ButtonTemplate()
            {
                Label = "Right",
                MinimumWidth = 15,
                LabelAlignment = HorizontalAlignment.Right
            };
            bt4.UpperLeftPos = bt3.CalculateRect().LowerLeft.Shift(0, 2);
            AddControl(new Button(bt4));

            ButtonTemplate bt5 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15,5),
                VAlignment = VerticalAlignment.Top,
                LabelAlignment = HorizontalAlignment.Left
            };
            bt5.AlignTo(LayoutDirection.East, bt0, 8);
            AddControl(new Button(bt5));

            ButtonTemplate bt6 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Center,
                LabelAlignment = HorizontalAlignment.Left
            };
            bt6.AlignTo(LayoutDirection.South, bt5, 2);
            AddControl(new Button(bt6));

            ButtonTemplate bt7 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Bottom,
                LabelAlignment = HorizontalAlignment.Left
            };
            bt7.AlignTo(LayoutDirection.South, bt6, 2);
            AddControl(new Button(bt7));

            ButtonTemplate bt8 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Top,
                LabelAlignment = HorizontalAlignment.Center,
                HasFrameBorder = false,
            };
            bt8.AlignTo(LayoutDirection.East, bt5, 2);
            AddControl(new Button(bt8));

            ButtonTemplate bt9 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Center,
                LabelAlignment = HorizontalAlignment.Center,
                HasFrameBorder = false,
            };
            bt9.AlignTo(LayoutDirection.South, bt8, 2);
            AddControl(new Button(bt9));

            ButtonTemplate bt10 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Bottom,
                LabelAlignment = HorizontalAlignment.Center,
                HasFrameBorder = false,
            };
            bt10.AlignTo(LayoutDirection.South, bt9, 2);
            AddControl(new Button(bt10));

            ButtonTemplate bt11 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Top,
                LabelAlignment = HorizontalAlignment.Right
            };
            bt11.AlignTo(LayoutDirection.East, bt8, 2);
            AddControl(new Button(bt11));

            ButtonTemplate bt12 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Center,
                LabelAlignment = HorizontalAlignment.Right
            };
            bt12.AlignTo(LayoutDirection.South, bt11, 2);
            AddControl(new Button(bt12));

            ButtonTemplate bt13 = new ButtonTemplate()
            {
                Label = "Sized",
                AutoSizeOverride = new Size(15, 5),
                VAlignment = VerticalAlignment.Bottom,
                LabelAlignment = HorizontalAlignment.Right,
            };
            bt13.AlignTo(LayoutDirection.South, bt12, 2);
            AddControl(new Button(bt13));

            ButtonTemplate bt14 = new ButtonTemplate()
            {
                Label = "Animated",
                AutoSizeOverride = new Size(12,5),
                VAlignment = VerticalAlignment.Center,
                LabelAlignment = HorizontalAlignment.Center
            };
            bt14.AlignTo(LayoutDirection.South, bt4, 4);
            AddControl(new WaveButton(bt14));

            ButtonTemplate bt15 = new ButtonTemplate()
            {
                Label = "Normal Draw",
                LabelAlignment = HorizontalAlignment.Center
            };
            bt15.AlignTo(LayoutDirection.South, bt14, 4);
            AddControl(new OwnerButton(bt15));

            ButtonTemplate bt16 = new ButtonTemplate()
            {
                Label = "901 Animated Buttons!",
            };
            bt16.AlignTo(LayoutDirection.South, bt10, 2);
            AddControl(new Button(bt16));

            // Create the matrix of animated buttons.
            // Note there is actually a large amount of overhead here - this is 
            // NOT the efficient way to do this.  However, this is meant to 
            // "stress test" the library, so this is how we are doing it.
            for (int y = 30; y < 47; y++)
            {
                for (int x = 20; x < 73; x++)
                {
                    ButtonTemplate bt17 = new ButtonTemplate()
                    {
                        Label = " ",
                        AutoSizeOverride = new Size(1, 1),
                        HasFrameBorder = false,
                        UpperLeftPos = new Point(x, y)
                    };

                    AddControl(new SparkleButton(bt17));
                }
            }

            PageInfo.AddText("Here is a sample selection of buttons, in various styles." +
                "\n\nTry clicking on the Animated and Owner Draw buttons to see custom behavior." +
                "\n\nNote that the 901 animated button matrix is a stress test - each button is being " +
                "created individually, each with its own scheduler, message hooks, and memory console. This is an " +
                "enormous amount of overhead, and here we can see how much it slows down the framework by "+
                "comparing the text output of this TextBox to the other pages.");

            PageInfo.AddText("\n\nHere are a couple of general things to note about the demo project:");
            PageInfo.AddText("\n* Each page is a seperate Window with its own controls" +
                "\n* The popup tooltips are intelligently constrained to the window"+
                "\n* This text is being drawn by a TextBox control from extended library",
                PageInfo.Pigments[PigmentType.Window].ReplaceForeground(new Color(0x885599))
                );
        }
        
    }



    class WaveButton : Button
    {
        public WaveButton(ButtonTemplate template)
            : base(template)
        {
        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            AddSchedule(new Schedule(Anim, 50));
        }

        void Anim()
        {
            animStep++;
        }

        protected override void Redraw()
        {
            base.Redraw();

            for (int y = 0; y < this.Size.Height; y++)
            {
                for (int x = 0; x < this.Size.Width; x++)
                {
                    Canvas.SetCharPigment(x, y, CalcColor(x,y));
                }
            }
        }

        Pigment CalcColor(int x,int y)
        {
            Pigment btnPigment = Pigments[PigmentType.ViewNormal];

            float fx = (float)x;
            float fy = (float)y;

            float val = (float)animStep;
            float scale = (float)Math.Sin((fx - fy + val)/5f);
            scale = (scale/1.5f + 1.5f) / 3f;

            Color bg = btnPigment.Background.ScaleValue(scale);
            Color fg = btnPigment.Foreground.ScaleValue(scale);

            if (this.IsBeingPushed)
            {
                //bg = bg.ScaleSaturation((float)Math.Cos(val));
                bg = bg.ReplaceHue(val);
            }

            return new Pigment(fg,bg);
        }

        uint animStep;
    }

    class OwnerButton : Button
    {
        public OwnerButton(ButtonTemplate template)
            : base(template)
        {
            OwnerDraw = true;

        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            ownerChars = new Queue<char>(ownerLabel.ToCharArray());

            AddSchedule(new Schedule(Animate, 100));
        }

        void Animate()
        {
            animStep++;
            if (animStep >= ownerLabel.Length)
            {
                animStep = 0;
            }

            char c = (char)ownerChars.Dequeue();
            ownerChars.Enqueue(c);
            

            
        }

        protected override void OnButtonPushed()
        {
            base.OnButtonPushed();

            if (OwnerDraw)
                OwnerDraw = false;
            else
                OwnerDraw = true;
        }

        protected override void Redraw()
        {
            base.Redraw();

            if (OwnerDraw)
            {
                string lbl = new string(ownerChars.ToArray());

                Canvas.SetDefaultPigment(new Pigment(0x11ee33, 0x003344));
                Canvas.Clear();

                Canvas.PrintString(0, 0, lbl);

                Canvas.ToScreen(this.ScreenPosition);
            }
        }

        string ownerLabel = " Owner Draw ";
        Queue<char> ownerChars;
        
        int animStep;
    }

    class SparkleButton : Button
    {
        public SparkleButton(ButtonTemplate template)
            : base(template)
        {
            bg = new Color(Rand.Int32(0,0xffffff));
            AddSchedule(new Schedule(Animate, 50));

            mod = Rand.Float()*50f;
        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();
        }

        void Animate()
        {
            animStep++;

            float val = (float)(animStep * mod % 360);

            bg = bg.ReplaceHue(val);
        }

        protected override Pigment DetermineMainPigment()
        {
            if (!IsMouseOver)
            {
                return new Pigment(new Color(0x000000), bg);
            }

            return Pigments[PigmentType.ViewNormal];
        }

        Color bg;
        int animStep;
        float mod;
    }
}
