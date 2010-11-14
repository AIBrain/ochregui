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

namespace OchreGui
{
    #region Public Enums
    // /////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types of characters for use in entry validation.
    /// </summary>
    [Flags]
    public enum TextEntryValidations
    {
        /// <summary>
        /// Allow all printable characters (but no control codes)
        /// </summary>
        All = 15,
        /// <summary>
        /// Allow only letters, uppercase or lowercase, and spaces
        /// </summary>
        Letters = 1,
        /// <summary>
        /// Allow only the digits 0 through 9
        /// </summary>
        Numbers = 2,
        /// <summary>
        /// Allow plus, minus and decimal point signs along with numbers.
        /// </summary>
        Decimal = 4,
        /// <summary>
        /// Allow all printable symbols that are not numbers or letters.  Setting this
        /// flag will effectively override the Decimal flag validation of the plus, minus
        /// and decimal point symbols.
        /// </summary>
        Symbols = 8
    }
    // /////////////////////////////////////////////////////////////////////////////////
    #endregion


    #region TextEntryTemplate
    /// <summary>
    /// Simple data structure for passing to textentry's constructor.  This allows
    /// re-using the same setup parameters, for example, on multiple textentries.
    /// Also allows use of named initializers.
    /// </summary>
    public class TextEntryTemplate : EntryTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        public TextEntryTemplate()
        {
            MaximumCharacters = 1;
            Validation = TextEntryValidations.All;

            StartingField = "";
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Maximum number of accepted characters.  Defaults to 1.
        /// </summary>
        public int MaximumCharacters { get; set; }

        /// <summary>
        /// Characters allowed to be entered, defaults to All
        /// </summary>
        public TextEntryValidations Validation { get; set; }

        /// <summary>
        ///  The field of the text entry when first created.
        /// </summary>
        public string StartingField { get; set; }

        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        public override Size CalculateSize()
        {
            if (Label == null)
                Label = "";

            int len = Label.Length;

            int frameSize = 0;

            if (HasFrameBorder)
            {
                frameSize = 2;
            }

            return new Size(len + MaximumCharacters + 1 + frameSize, 1 + frameSize);
        }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region TextEntry Class
    /// <summary>
    /// A text entry accepts general text input from the user.  The input can be validated by 
    /// the maximum number of characters
    /// and which characters are accepted.  The control must have keyboard focus to receive
    /// input, and commits the changes to the text field when the enter key is pressed (by default).
    /// Hitting the escape key or otherwise loosing the keyboard focus (by default) before hitting 
    /// enter cancels the current text from being committed.
    /// </summary>
    public class TextEntry : Entry
    {
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        public TextEntry(TextEntryTemplate template)
            :base(template)
        {

            this._maximumCharacters = template.MaximumCharacters;
            this.Validation = template.Validation;
            //CommmittedField = template.StartingField;
            //CurrentText = CommmittedField;
            TrySetField(template.StartingField);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the validation type (allowed characters) for this control
        /// </summary>
        public TextEntryValidations Validation { get; protected set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        /// <summary>
        /// Get the maximum number of characters allowed.
        /// </summary>
        protected override int MaximumCharacters
        {
            get
            {
                return _maximumCharacters;
            }
        }

        /// <summary>
        /// Base method returns the empty string "".
        /// </summary>
        protected override string DefaultField
        {
            get
            {
                return "";
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        protected override bool ValidateField(string entry)
        {
            return true;
        }


        /// <summary>
        /// Returns true if character is a valid entry.  Override to implement custom
        /// validation.  Base method uses the property Validation to make a determination
        /// if the specified character is valid.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected override bool ValidateCharacter(char character)
        {
            bool valid = false;

            if(Validation.IsSet(TextEntryValidations.Numbers) ||
                Validation.IsSet(TextEntryValidations.Decimal))
            {
                if (char.IsNumber(character))
                {
                    valid = true;
                }
            }

            if (Validation.IsSet(TextEntryValidations.Letters))
            {
                if(char.IsLetter(character) ||
                    character == ' ')
                {
                    valid = true;
                }
            }

            if (Validation.IsSet(TextEntryValidations.Decimal))
            {
                if (character == '+' || character == '-' || character == '.')
                {
                    valid = true;
                }
            }

            if (Validation.IsSet(TextEntryValidations.Symbols))
            {
                if("!@#$%^&*()_+-={};:'\",<.>/?".Contains(character.ToString()))
                {
                    valid = true;
                }
            }

            return valid;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion

        #endregion

        private int _maximumCharacters;
    }



}
