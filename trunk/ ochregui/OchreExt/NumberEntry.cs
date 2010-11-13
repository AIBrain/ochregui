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

            int len = Label.Length;

            return new Size(len + CalculateMaxCharacters() + 3, 3);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        public int CalculateMaxCharacters()
        {
            return Math.Max(MaximumValue.ToString().Length,
                MinimumValue.ToString().Length);
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

            if (ValidateValue(template.StartingValue))
            {
                CurrentValue = template.StartingValue;
            }
            else
            {
                CurrentValue = MinimumValue;
            }

            _maximumCharacters = template.CalculateMaxCharacters();

            CurrentValue = MinimumValue;
            Field = CurrentValue.ToString();
            CurrentText = Field;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties

        protected int MaximumValue { get; private set; }

        protected int MinimumValue { get; private set; }

        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Properties
        public int CurrentValue
        {
            get;
            set;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
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
            base.OnFieldChanged();

            CurrentValue = int.Parse(Field);
        }
        
    #endregion

        private int _maximumCharacters;

    }
}
