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

    public class NumberEntryTemplate : EntryTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor
        /// </summary>
        public NumberEntryTemplate()
        {
            MaximumValue = 1;
            MinimumValue = 0;
            StartingValue = 0;
        }
        // /////////////////////////////////////////////////////////////////////////////////


        public int MinimumValue { get; set; }

        public int MaximumValue { get; set; }

        public int StartingValue { get; set; }

        // /////////////////////////////////////////////////////////////////////////////////
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

        public int CalculateMaxCharacters()
        {
            return Math.Max(MaximumValue.ToString().Length,
                MinimumValue.ToString().Length);
        }

        static public int CalculateFieldWidth(int maxValue,int minValue)
        {
            return Math.Max(maxValue.ToString().Length,
                minValue.ToString().Length) + 1;
        }
    }
    #endregion

    #region TextEntry Class
    public class NumberEntry : Entry
    {
        #region Constructors

        public NumberEntry(NumberEntryTemplate template)
            : base(template)
        {

            MaximumValue = template.MaximumValue;
            MinimumValue = template.MinimumValue;

            TrySetField(template.StartingValue.ToString());

            _maximumCharacters = template.CalculateMaxCharacters();

            CommittedField = CurrentValue.ToString();
            CurrentText = CommittedField;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties

        protected int MaximumValue { get; private set; }

        protected int MinimumValue { get; private set; }

        // /////////////////////////////////////////////////////////////////////////////////
        private int _currentValue;
        public int CurrentValue
        {
            get { return _currentValue; }
            protected set
            {
                if(ValidateValue(value))
                {
                    _currentValue = value;
                    CommittedField = _currentValue.ToString();
                    CurrentText = CommittedField;
                }
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        public bool TrySetValue(int changeTo)
        {
            CurrentText = changeTo.ToString();
            return TryCommit();

        }
        #region Protected Methods
        protected bool ValidateValue(int value)
        {
            if (value < MinimumValue || value > MaximumValue)
            {
                return false;
            }

            return true;
        }
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
        #endregion

        protected override int MaximumCharacters
        {
            get
            {
                return _maximumCharacters;
            }
        }

        protected override bool ValidateField(string entry)
        {
            int value;

            if (!int.TryParse(entry, out value))
            {
                return false;
            }

            return ValidateValue(value);
        }

        protected override string DefaultField
        {
            get
            {
                return MinimumValue.ToString();
            }
        }

        protected override void OnFieldChanged()
        {
            CurrentValue = int.Parse(CommittedField);

            base.OnFieldChanged();
        }
        
    #endregion

        private int _maximumCharacters;

    }
}
