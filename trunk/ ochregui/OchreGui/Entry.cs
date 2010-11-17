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
using libtcod;

namespace OchreGui
{
    /// <summary>
    /// Base class used for Entry template classes.
    /// </summary>
    public abstract class EntryTemplate : ControlTemplate
    {

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor initializes properties to their defaults.
        /// </summary>
        public EntryTemplate()
        {
            Label = "";
            CanHaveKeyboardFocus = true;
            HilightWhenMouseOver = false;
            CommitOnLostFocus = false;
            ReplaceOnFirstKey = false;
            HasFrameBorder = true;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Displayed label, defautls to empty string
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// True if this control initially accepts the keyboard focus.  Defaults to true.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// True if this control is drawn hilighted when under the mouse pointer.  Defaults
        /// to false.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }

        /// <summary>
        /// If true, the entered text will be committed if the control looses the keyboard
        /// focus before the ENTER key is pressed.  Defaults to false.
        /// </summary>
        public bool CommitOnLostFocus { get; set; }

        /// <summary>
        /// If true, simulates the "select-all and replace on first keypress" behaviour
        /// seen in other GUI systems.  Defaults to false.
        /// </summary>
        public bool ReplaceOnFirstKey { get; set; }

        /// <summary>
        /// True if this control has a border for a frame.  Defaults to true.
        /// </summary>
        public bool HasFrameBorder { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////

    }

    /// <summary>
    /// This is the base class for an Entry control.  Entry controls allow keyboard input
    /// from the user, and the input can be validated on a character by character basis and/or
    /// validate the entire entry field when it is being committed.
    /// </summary>
    public abstract class Entry : Control
    {
        #region Events

        /// <summary>
        /// Triggered when the entered text has been committed, usually by pressing the ENTER key.
        /// </summary>
        public event EventHandler EntryChanged;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct an Entry instance from the given template.
        /// </summary>
        /// <param name="template"></param>
        public Entry(EntryTemplate template)
            :base(template)
        {
            this.Label = template.Label;
            this.HasFrame = template.HasFrameBorder;

            CommitOnLostFocus = template.CommitOnLostFocus;
            ReplaceOnFirstKey = template.ReplaceOnFirstKey;

            this.CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;
            this.HilightWhenMouseOver = template.HilightWhenMouseOver;

            this.CommmittedField = "";
            this.waitingToCommitText = false;
            this.CurrentText = CommmittedField;

            if (template.HasFrameBorder)
            {
                labelPosX = 1;
                labelPosY = 1;
            }
            else
            {
                labelPosX = 0;
                labelPosY = 0;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the last valid, committed text that has been entered.  This may or may not be
        /// the same as what is being currently displayed by the entry (as a user types input,
        /// for example).
        /// </summary>
        public string CommmittedField { get; protected set; }

        /// <summary>
        /// The current text state of the control as it is typed.  This text has not yet been
        /// validated.
        /// </summary>
        public string CurrentText { get; protected set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        /// <summary>
        /// Tries to set the entry's Field to the specified text.  This method calls TryCommit()
        /// to see if the commit was successful, and returns true if the commit was successful.
        /// </summary>
        /// <param name="changeTo"></param>
        /// <returns></returns>
        public bool TrySetField(string changeTo)
        {
            CurrentText = changeTo;

            return TryCommit();
        }

        /// <summary>
        /// Trys to commmit the current text, by calling ValidateField.  If successful,
        /// the CommittedField will be set to the current text, and OnFieldChanged will
        /// be called.
        /// </summary>
        /// <returns></returns>
        public bool TryCommit()
        {
            if (ValidateField(CurrentText) &&
                CurrentText.Length <= MaximumCharacters)
            {
                CommmittedField = CurrentText;

                OnFieldChanged();
                return true;
            }
            CurrentText = CommmittedField;
            return false;
        }
        #endregion
        #region Protected Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the maximum number of characters that can be typed
        /// </summary>
        protected abstract int MaximumCharacters { get; }

        /// <summary>
        /// Gets what the field defaults to if there is not current or previous valid entries.
        /// </summary>
        protected abstract string DefaultField { get; }

        /// <summary>
        /// Get the label string
        /// </summary>
        protected string Label { get; private set; }
 
        /// <summary>
        /// Get the current position of the entry cursor, representing the position
        /// of the next typed character.
        /// </summary>
        protected int CursorPos { get; private set; }

        /// <summary>
        /// If true, the entered text will be committed if the control looses the keyboard
        /// focus before the ENTER key is pressed.  Defaults to false.
        /// </summary>
        protected bool CommitOnLostFocus { get; set; }

        /// <summary>
        /// If true, simulates the "select-all and replace on first keypress" behaviour
        /// seen in other GUI systems.  Defaults to false.
        /// </summary>
        protected bool ReplaceOnFirstKey { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Return true if character is a valid entry.  An invalid character will be ignored
        /// by the entry and not added to the entry field.  Override to implement custom character
        /// validation.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected abstract bool ValidateCharacter(char character);

        /// <summary>
        /// Returns true if the provided entry is valid.  This is checked when the field is about
        /// to be committed; if invalid, the field will revert to the last valid field.  Override to implement
        /// custom field validation.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected abstract bool ValidateField(string entry);

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the color pigment based on whether the entry has the keyboard focus or not.
        /// </summary>
        protected override Pigment DetermineMainPigment()
        {
            if (this.HasKeyboardFocus)
            {
                return(DefaultPigments.Hilight);
            }
            return base.DetermineMainPigment();
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method sets up a scheduler for blinking the cursor.
        /// </summary>
        protected internal override void OnSettingUp()
        {
            base.OnSettingUp();

            AddSchedule(new Schedule(ToggleCursor, blinkDelay));
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the entry, including the cursor and any state-dependant colors.  Override
        /// to add custom drawing code.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            Canvas.PrintString(labelPosX, labelPosY, Label);

            if (waitingToOverwrite)
            {
                Canvas.PrintString(Label.Length + labelPosX, labelPosY, 
                    CurrentText, DefaultPigments.Selected);
            }
            else
            {
                Canvas.PrintString(Label.Length + labelPosX, labelPosY, CurrentText);
            }

            if (cursorOn && HasKeyboardFocus)
            {
                Canvas.PrintChar(Label.Length + labelPosX + CursorPos, labelPosY, 
                    (int)TCODSpecialCharacter.Block1, 
                    DefaultPigments.Selected);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the a different field has been committed to the entry.
        /// </summary>
        protected virtual void OnFieldChanged()
        {
            if (EntryChanged != null)
            {
                EntryChanged(this, EventArgs.Empty);
            }
        }

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method handles keyboard input for the entry.  Override to add custom
        /// behavior.
        /// </summary>
        internal protected override void OnKeyPressed(KeyboardData keyData)
        {
            base.OnKeyPressed(keyData);

            if (keyData.Character != 0 && 
                ValidateCharacter(keyData.Character))
            {
                if (waitingToOverwrite)
                {
                    CurrentText = keyData.Character.ToString();
                    CursorPos = 1;
                    waitingToOverwrite = false;
                }
                else if(CurrentText.Length < MaximumCharacters)
                {
                    CurrentText += keyData.Character;
                    CursorPos++;
                }
            }
            else if (keyData.KeyCode == TCODKeyCode.Backspace &&
                CurrentText.Length > 0)
            {
                CurrentText = CurrentText.Substring(0, CurrentText.Length - 1);
                CursorPos--;
            }
            else if (keyData.KeyCode == TCODKeyCode.Enter)
            {
                waitingToCommitText = true;
                ParentWindow.ReleaseKeyboard(this);

            }
            else if (keyData.KeyCode == TCODKeyCode.Escape)
            {
                CurrentText = CommmittedField;
                waitingToCommitText = true;
                ParentWindow.ReleaseKeyboard(this);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method gets the entry ready for user keyboard input.  Override to add
        /// custom handling after calling this base method.
        /// </summary>
        internal protected override void OnTakeKeyboardFocus()
        {
            base.OnTakeKeyboardFocus();

            waitingToCommitText = false;
            CurrentText = CommmittedField;

            if (ReplaceOnFirstKey)
            {
                waitingToOverwrite = true;
            }

            this.CursorPos = CommmittedField.Length;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method performs any necessary cleanup when the keyboard focus is lost.  Override
        /// to add custom handling after calling this base method.
        /// </summary>
        internal protected override void OnReleaseKeyboardFocus()
        {
            base.OnReleaseKeyboardFocus();

            if (waitingToCommitText || CommitOnLostFocus)
            {
                TryCommit();
            }
            else
            {
                CurrentText = CommmittedField;
            }
            waitingToOverwrite = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private bool cursorOn = true;
        private bool waitingToCommitText { get; set; }
        private bool waitingToOverwrite;

        // /////////////////////////////////////////////////////////////////////////////////
        // TODO: consider making definable, not a constant
        const uint blinkDelay = 500;
        // /////////////////////////////////////////////////////////////////////////////////
        private int labelPosX;
        private int labelPosY;

        private void ToggleCursor()
        {
            if (cursorOn)
            {
                cursorOn = false;
            }
            else
            {
                cursorOn = true;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
}
