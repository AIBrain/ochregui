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
    public class SpinBoxTemplate : ControlTemplate
    {
        public SpinBoxTemplate()
        {
            MaximumValue = 1;
            MaximumValue = 0;
            Label = "";
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
        /// The delay in milliseconds after first clicking on a spin button before
        /// the spin cycle starts.  Defaults to 0.
        /// </summary>
        public uint SpinDelay { get; set; }

        /// <summary>
        /// The speed of the spin cycle.  This is measure in the millisecond delay before
        /// each step; thus, a smaller number here means a faster spin.  Defaults to 0.
        /// </summary>
        public uint SpinSpeed { get; set; }

        /// <summary>
        /// And optional label to display to the left of the numerical entry and spin buttons.  Defaults
        /// to empty string (no label).
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The value that the spin box will initially have.  Defaults to 0.
        /// </summary>
        public int StartingValue { get; set; }

        public override Size CalculateSize()
        {
            int width = 2; // for frame

            if (!string.IsNullOrEmpty(Label))
            {
                width += Label.Length + 1;
            }

            width += NumberEntryTemplate.CalculateFieldWidth(MaximumValue, MinimumValue);
            width += 3; // for buttons

            return new Size(width, 3);
        }
    }

    public class SpinBox : Control
    {
        public SpinBox(SpinBoxTemplate template)
            : base(template)
        {
            MinimumValue = template.MinimumValue;
            MaximumValue = template.MaximumValue;
            SpinDelay = template.SpinDelay;
            SpinSpeed = template.SpinSpeed;
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

        }

        /// <summary>
        /// The current value of the spin box.
        /// </summary>
        public int CurrentValue { get; protected set; }

        /// <summary>
        /// The minimum value that this spin control can have.  Defaults to 0.
        /// </summary>
        protected int MinimumValue { get; private set; }

        /// <summary>
        /// The delay in milliseconds after first clicking on a spin button before
        /// the spin cycle starts.  Defaults to 0.
        /// </summary>
        protected int MaximumValue { get; private set; }

        /// <summary>
        /// The delay in milliseconds after first clicking on a spin button before
        /// the spin cycle starts.  Defaults to 0.
        /// </summary>
        protected uint SpinDelay { get; set; }

        /// <summary>
        /// And optional label to display to the left of the numerical entry and spin buttons.  Defaults
        /// to empty string (no label).
        /// </summary>
        protected uint SpinSpeed { get; set; }

        /// <summary>
        /// And optional label to display to the left of the numerical entry and spin buttons.  Defaults
        /// to empty string (no label).
        /// </summary>
        protected string Label { get; private set; }



        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            if (!string.IsNullOrEmpty(Label))
            {
                labelRect = new Rect(1, 1, Label.Length + 1, 1);
                upButtonPos = new Point(Label.Length + 2, 1);
            }
            else
            {
                upButtonPos = new Point(1, 1);
            }

            int fieldWidth = NumberEntryTemplate.CalculateFieldWidth(MaximumValue, MinimumValue);
            Size fieldSize = new Size(fieldWidth, 1);
            fieldRect = new Rect(upButtonPos.Shift(2, 0), fieldSize);

            downButtonPos = fieldRect.UpperRight.Shift(1, 0);
            
            numEntry = new NumberEntry(new NumberEntryTemplate()
            {
                HasFrameBorder = false,
                MinimumValue = this.MinimumValue,
                MaximumValue = this.MaximumValue,
                StartingValue = CurrentValue,
                DefaultStyles = this.DefaultStyles,
                CommitOnLostFocus = true,
                ReplaceOnFirstKey = true,
                UpperLeftPos = fieldRect.UpperLeft
            });

            upButton = new EmitterButton(new EmitterButtonTemplate()
            {
                HasFrameBorder = false,
                Label = ((char)libtcod.TCODSpecialCharacter.ArrowNorthNoTail).ToString(),
                UpperLeftPos = upButtonPos,
                StartEmittingDelay = SpinDelay,
                Speed = SpinSpeed
            });

            downButton = new EmitterButton(new EmitterButtonTemplate()
            {
                HasFrameBorder = false,
                Label = ((char)libtcod.TCODSpecialCharacter.ArrowSouthNoTail).ToString(),
                UpperLeftPos = downButtonPos,
                StartEmittingDelay = SpinDelay,
                Speed = SpinSpeed
            });

            ParentWindow.AddControls(downButton, upButton, numEntry);

            upButton.Emit += new EventHandler(upButton_Emit);
            downButton.Emit += new EventHandler(downButton_Emit);
            numEntry.EntryChanged += new EventHandler(numEntry_EntryChanged);
        }

        void numEntry_EntryChanged(object sender, EventArgs e)
        {
            NumberEntry entry = sender as NumberEntry;

            this.CurrentValue = entry.CurrentValue;
        }

        void downButton_Emit(object sender, EventArgs e)
        {
            if (CurrentValue > MinimumValue)
            {
                numEntry.TryCommit();
                CurrentValue--;
                numEntry.CurrentValue = CurrentValue;
            }
        }

        void upButton_Emit(object sender, EventArgs e)
        {
            if (CurrentValue < MaximumValue)
            {
                numEntry.TryCommit();
                CurrentValue++;
                numEntry.CurrentValue = CurrentValue;
            }
        }


        protected override void Redraw()
        {
            base.Redraw();
            if (!string.IsNullOrEmpty(Label))
            {
                Canvas.PrintString(labelRect.UpperLeft, Label);

                Canvas.PrintChar(labelRect.UpperRight.Shift(0, -1),
                    (int)libtcod.TCODSpecialCharacter.TeeSouth,
                    GetFrameStyle());

                Canvas.PrintChar(labelRect.UpperRight.Shift(0, 0),
                    (int)libtcod.TCODSpecialCharacter.VertLine,
                    GetFrameStyle());

                Canvas.PrintChar(labelRect.UpperRight.Shift(0, 1),
                    (int)libtcod.TCODSpecialCharacter.TeeNorth,
                    GetFrameStyle());

            }
        }

        NumberEntry numEntry;
        EmitterButton downButton;
        EmitterButton upButton;
        Rect fieldRect;
        Rect labelRect;
        Point upButtonPos;
        Point downButtonPos;

    }
}
