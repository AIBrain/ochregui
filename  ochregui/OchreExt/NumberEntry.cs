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
using OchreGui.Utility;
using libtcod;

namespace OchreGui.Extended
{
    #region NumberEntryTemplate

    /// <summary>
    /// This class builds on the EntryTemplate class, and adds properties to specify the
    /// minimum, maximum, and starting values.  Also adds methods to size this control
    /// based on the possible range of values.
    /// </summary>
    public class NumberEntryTemplate : EntryTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Contructs object with defaults.
        /// </summary>
        public NumberEntryTemplate()
        {
            MaximumValue = 1;
            MinimumValue = 0;
            StartingValue = 0;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The minimum value that the entry can have.  Defaults to 0.
        /// </summary>
        public int MinimumValue { get; set; }

        /// <summary>
        /// The maximum value that the entry can have.  Defaults to 1.
        /// </summary>
        public int MaximumValue { get; set; }

        /// <summary>
        /// The value that the entry will start with.  Defaults to 0.
        /// </summary>
        public int StartingValue { get; set; }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Handles autosizing according to the largest field possible with the minimum
        /// and maximum values.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            if (Label == null)
                Label = "";

            int width = Label.Length;
            int height = 1;

            width += CalculateFieldWidth(MaximumValue,MinimumValue);

            if (HasFrameBorder)
            {
                width += 2;
                height += 2;
            }

            return new Size(width, height);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the maximum number of characters for the maximum value or the minimum value.
        /// Used to autosize a NumberEntry.
        /// </summary>
        /// <returns></returns>
        public override int CalculateMaxCharacters()
        {
            return Math.Max(MaximumValue.ToString().Length,
                MinimumValue.ToString().Length);
        }

        /// <summary>
        /// Returns the largest width of a NumberEntry field that has the specified minimum
        /// and maximum values.  Used to autosize a NumberEntry.
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns></returns>
        static public int CalculateFieldWidth(int maxValue,int minValue)
        {
            return Math.Max(maxValue.ToString().Length,
                minValue.ToString().Length) + 1;
        }
    }
    #endregion

    #region TextEntry Class
    /// <summary>
    /// Represents an Entry that handles and validates numerical (integer) input.  A
    /// NumberEntry only allows the entry of digits and a sign indicator.  The field
    /// is validated by the specified minimum and maximum values.
    /// </summary>
    public class NumberEntry : Entry
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a NumberEntry with the specified template.
        /// </summary>
        /// <param name="template"></param>
        public NumberEntry(NumberEntryTemplate template)
            : base(template)
        {
            MaximumValue = template.MaximumValue;
            MinimumValue = template.MinimumValue;

            TrySetField(template.StartingValue.ToString());
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The maximum value that this entry can have.
        /// </summary>
        protected int MaximumValue { get; private set; }

        /// <summary>
        /// The minimum value that his entry can have.
        /// </summary>
        protected int MinimumValue { get; private set; }

        /// <summary>
        /// The current, committed value of the NumberEntry.  This may or may
        /// not be the same as what is currently being shown in the entry as it is
        /// being input.
        /// </summary>
        public int CurrentValue
        {
            get { return _currentValue; }
            protected set
            {
                if(ValidateValue(value))
                {
                    _currentValue = value;
                    CurrentText = _currentValue.ToString();
                    TextInput = CurrentText;
                }
            }
        }
        private int _currentValue;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// If the specified value is valid, this method changes this entry to that value and
        /// the EntryChanged event is raised.
        /// </summary>
        /// <param name="changeTo"></param>
        /// <returns></returns>
        public bool TrySetValue(int changeTo)
        {
            TextInput = changeTo.ToString();
            return TryCommit();

        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if the specified value is within the range for this entry.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool ValidateValue(int value)
        {
            if (value < MinimumValue || value > MaximumValue)
            {
                return false;
            }

            return true;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns true if character is a valid entry.  Override to implement custom
        /// validation.  Base method uses the property Validation to make a determination
        /// if the specified character is valid.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected override bool ValidateCharacter(char character)
        {
            if (char.IsNumber(character))
            {
                return true;
            }

            if (character == '+' || character == '-')
            {
                return true;
            }

            return false;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        protected override bool ValidateField(string entry)
        {
            int value;

            if (!int.TryParse(entry, out value))
            {
                return false;
            }

            return ValidateValue(value);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        protected override string DefaultField
        {
            get
            {
                return MinimumValue.ToString();
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        protected override void OnFieldChanged()
        {
            CurrentValue = int.Parse(CurrentText);

            base.OnFieldChanged();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
    #endregion
}
