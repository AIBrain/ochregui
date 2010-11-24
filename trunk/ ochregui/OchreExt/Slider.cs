//Copyright (c) 2010 Shane Baker
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Extended
{
    public class SliderTemplate : ControlTemplate
    {
        public SliderTemplate()
        {

        }
        /// <summary>
        /// The minimum value that this spin control can have.  Defaults to 0.
        /// </summary>
        public int MinimumValue { get; set; }

        /// <summary>
        /// The maximum value that this spin control can have.  Defaults to 1.
        /// </summary>
        public int MaximumValue { get; set; }

        /// <summary>
        /// An optional label to display to the left of the numerical entry.  Defaults
        /// to empty string (no label).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The value that the slider will initially have.  Defaults to 0.
        /// </summary>
        public int StartingValue { get; set; }

        public int MinimumWidth { get; set; }

        public Pigment BarPigment { get; set; }

        public override Size CalculateSize()
        {
            int width = 2; // for frame

            if (!string.IsNullOrEmpty(Label))
            {
                width += Label.Length + 1;
            }

            width += NumberEntryTemplate.CalculateFieldWidth(MaximumValue, MinimumValue);
            width += 3;

            width = Math.Max(width, MinimumWidth);

            return new Size(width, 4);
        }
    }

    public class Slider : Control
    {
        public event EventHandler ValueChanged;

        public Slider(SliderTemplate template)
            : base(template)
        {
            MinimumValue = template.MinimumValue;
            MaximumValue = template.MaximumValue;

            Label = template.Label;
            if (Label == null)
            {
                Label = "";
            }

            CurrentValue = template.StartingValue;
            if (CurrentValue < MinimumValue || CurrentValue > MaximumValue)
            {
                CurrentValue = MinimumValue;
            }

            HasFrame = true;
            CanHaveKeyboardFocus = false;
            HilightWhenMouseOver = false;

            BarPigment = template.BarPigment;

        }


        /// <summary>
        /// Get the minimum value that this spin control can have.
        /// </summary>
        public int MinimumValue { get; private set; }

        /// <summary>
        /// Get the maximum value that this spin control can have.
        /// </summary>
        public int MaximumValue { get; private set; }

        /// <summary>
        /// Get the optional label to display to the left of the numerical entry.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Get or set the slider bar color pigment.
        /// </summary>
        public Pigment BarPigment { get; set; }

        /// <summary>
        /// Get the current value of the slider.
        /// </summary>
        public int CurrentValue 
        {
            get { return _currentValue; }

            protected set
            {
                int newVal = value;

                if (newVal < MinimumValue)
                    newVal = MinimumValue;

                if (newVal > MaximumValue)
                    newVal = MaximumValue;

                if (newVal != _currentValue)
                {
                    _currentValue = newVal;
                    OnValueChanged();
                }
            }
        }
        private int _currentValue;


        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }


        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            Point fieldPos;
            if (!string.IsNullOrEmpty(Label))
            {
                labelRect = new Rect(1, 1, Label.Length + 1, 1);
                fieldPos = new Point(Label.Length + 2, 1);
            }
            else
            {
                fieldPos = new Point(1, 1);
            }

            int fieldWidth = NumberEntryTemplate.CalculateFieldWidth(MaximumValue, MinimumValue);
            Size fieldSize = new Size(fieldWidth, 1);
            fieldRect = new Rect(fieldPos, fieldSize);

            if (BarPigment == null)
                BarPigment = DetermineMainPigment();

            numEntry = new NumberEntry(new NumberEntryTemplate()
            {
                HasFrameBorder = false,
                MinimumValue = this.MinimumValue,
                MaximumValue = this.MaximumValue,
                StartingValue = CurrentValue,
                DefaultPigments = this.DefaultPigments,
                CommitOnLostFocus = true,
                ReplaceOnFirstKey = true,
                UpperLeftPos = this.LocalToScreen(fieldRect.UpperLeft)
            });

            valueBar = new ValueBar(new ValueBarTemplate()
            {
                UpperLeftPos = this.LocalToScreen(new Point(1,2)),
                Width = this.Size.Width-4,
                MaximumValue = this.MaximumValue,
                MinimumValue = this.MinimumValue,
                StartingValue = this.CurrentValue,
                BarPigment = this.BarPigment
            });

            ParentWindow.AddControls(valueBar, numEntry);

            numEntry.EntryChanged += new EventHandler(numEntry_EntryChanged);

            valueBar.MouseMoved += new EventHandler<MouseEventArgs>(valueBar_MouseMoved);

            valueBar.MouseButtonDown += new EventHandler<MouseEventArgs>(valueBar_MouseButtonDown);
        }


        void valueBar_MouseMoved(object sender, MouseEventArgs e)
        {
            if (e.MouseData.MouseButton == MouseButton.LeftButton)
            {
                CurrentValue = CalculateValue(e.MouseData.PixelPosition.X);
                

                //numEntry.CurrentValue = CurrentValue;
                numEntry.TrySetValue(CurrentValue);
                valueBar.CurrentValue = CurrentValue;
            }
        }

        int CalculateValue(int pixelPosX)
        {
            int charWidth = Canvas.GetCharSize().Width;
            int currPx = pixelPosX;

            currPx = currPx - charWidth * valueBar.ScreenRect.Left - charWidth;

            int widthInPx = (valueBar.Size.Width - 2) * charWidth;

            float pixposPercent = (float)currPx / (float)widthInPx;

            return (int)((float)(MaximumValue - MinimumValue) * pixposPercent) + MinimumValue;
        }

        protected override void Redraw()
        {
            base.Redraw();

            Canvas.PrintString(labelRect.UpperLeft, Label);
        }


        void numEntry_EntryChanged(object sender, EventArgs e)
        {
            int value = numEntry.CurrentValue;

            valueBar.CurrentValue = value;
        }


        void valueBar_MouseButtonDown(object sender, MouseEventArgs e)
        {
            if (e.MouseData.MouseButton == MouseButton.LeftButton)
            {
                CurrentValue = CalculateValue(e.MouseData.PixelPosition.X);

                //numEntry.CurrentValue = CurrentValue;
                numEntry.TrySetValue(CurrentValue);
                valueBar.CurrentValue = CurrentValue;
            }            
        }

        NumberEntry numEntry;
        ValueBar valueBar;
        Rect labelRect;
        Rect fieldRect;
    }
}
