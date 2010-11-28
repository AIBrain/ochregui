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
using libtcod;


namespace OchreGui
{
    #region ListBox Helper Classes
    /// <summary>
    /// This is the argument sent as part of a ListBox.ItemSelected event.
    /// </summary>
    public class ListItemSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Construct a ListItemSelectedEventArgs object with the specified item index number.
        /// </summary>
        /// <param name="index"></param>
        public ListItemSelectedEventArgs(int index)
        {
            Index = index;
        }

        /// <summary>
        /// The index of the selected item.
        /// </summary>
        public int Index { get; private set; }

    }

    /// <summary>
    /// Contains the label and tooltip text for each Listitem that will be added
    /// to a Listbox.
    /// </summary>
    public class ListItemData
    {
        /// <summary>
        /// Construct a ListItemData instance given the label and an optional tooltip.
        /// </summary>
        /// <param name="label"></param>
        /// <param name="toolTip"></param>
        public ListItemData(string label, string toolTip = null)
        {
            this.Label = label;
            this.TooltipText = toolTip;
        }

        /// <summary>
        /// The label of this list item.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The optional tooltip text for this list item.
        /// </summary>
        public string TooltipText { get; set; }
    }
    #endregion


    #region ListBoxInfo
    /// <summary>
    /// This class builds on the Control Template, and adds options specific to a ListBox.
    /// </summary>
    public class ListBoxTemplate : ControlTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor initializes properties to their defaults.
        /// </summary>
        public ListBoxTemplate()
        {
            Items = new List<ListItemData>();
            Title = "";
            LabelAlignment = HorizontalAlignment.Left;
            TitleAlignment = HorizontalAlignment.Center;
            InitialSelectedIndex = 0;
            CanHaveKeyboardFocus = false;
            HilightWhenMouseOver = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The list of ListItemData elements that will be included in the list box.  Defaults
        /// to an empty list.
        /// </summary>
        public List<ListItemData> Items { get; set; }

        /// <summary>
        /// The horizontal alignment of the item labels.  Defaults to left.
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// The horiontal alignment of the title. Defaults to left.
        /// </summary>
        public HorizontalAlignment TitleAlignment { get; set; }

        /// <summary>
        /// The title string, defaults to ""
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The list box width if larger than the calculated width.  Defaults to 0.
        /// </summary>
        public int MinimumListBoxWidth { get; set; }

        /// <summary>
        /// Which item index will be selected initially.  Defaults to 0.
        /// </summary>
        public int InitialSelectedIndex { get; set; }

        /// <summary>
        /// Specifies if this control can receive the keyboard focus.  Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// Specifies if this control is drawn in hilighted colors when under the mouse pointer.
        /// Defaults to false.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculates the ListBox size based on the properties of this template.
        /// </summary>
        /// <returns></returns>
        public override Size CalculateSize()
        {
            int width = Title.Length;
            foreach (ListItemData i in Items)
            {
                if (i.Label == null)
                    i.Label = "";

                if (i.Label.Length > width)
                    width = i.Label.Length;
            }

            width += 4;

            if (this.MinimumListBoxWidth > width)
                width = MinimumListBoxWidth;

            int height = Items.Count + 4;

            return new Size(width, height);
        }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region ListBox
    /// <summary>
    /// A ListBox control allows the selection of a single option among a list of
    /// options presented in rows.  The selection state of an item is persistant, and
    /// is marked as currently selected.
    /// </summary>
    public class ListBox : Control
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Raised when an item has been selected by the left mouse button.
        /// </summary>
        public event EventHandler<ListItemSelectedEventArgs> ItemSelected;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct a ListBox instance from the given template.
        /// </summary>
        /// <param name="template"></param>
        public ListBox(ListBoxTemplate template)
            : base(template)
        {
            Items = template.Items;
            Title = template.Title;
            CurrentSelected = -1;
            OwnerDraw = template.OwnerDraw;

            HasFrame = true;
            HilightWhenMouseOver = template.HilightWhenMouseOver;
            CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;

            LabelAlignment = template.LabelAlignment;
            TitleAlignment = template.TitleAlignment;
            CurrentSelected = template.InitialSelectedIndex;

            mouseOverIndex = -1;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The horizontal alignment of the item labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// The horiontal alignment of the title.
        /// </summary>
        public HorizontalAlignment TitleAlignment { get; set; }

        /// <summary>
        /// The title string.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Get the index of the item currently selected.
        /// </summary>
        public int CurrentSelected { get; private set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the label of the item with the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetItemLabel(int index)
        {
            if(index < 0 || index >= Items.Count)
                throw(new ArgumentOutOfRangeException("index"));

            return Items[index].Label;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the title and title frame.
        /// </summary>
        protected void DrawTitle()
        {

            if (!string.IsNullOrEmpty(Title))
            {
                Canvas.PrintStringAligned(1, 1, Title, TitleAlignment,
                    Size.Width - 2);
            }

            Canvas.SetDefaultPigment(DetermineFramePigment());
            Canvas.DrawHLine(1, 2, Size.Width - 2);
            Canvas.PrintChar(0, 2, (int)TCODSpecialCharacter.TeeEast);
            Canvas.PrintChar(Size.Width - 1, 2, (int)TCODSpecialCharacter.TeeWest);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws each of the items in the list.
        /// </summary>
        protected void DrawItems()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                DrawItem(i);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws a single item with the given index.
        /// </summary>
        /// <param name="index"></param>
        protected void DrawItem(int index)
        {
            ListItemData item = Items[index];

            if (index == CurrentSelected)
            {
                Canvas.PrintStringAligned(1, index + 3, item.Label, LabelAlignment,
                    Size.Width - 2, Pigments[PigmentType.ViewSelected]);

                Canvas.PrintChar(Size.Width-2, index + 3,
                    (int)TCODSpecialCharacter.ArrowWest, Pigments[PigmentType.ViewSelected]);
            }
            else if (index == mouseOverIndex)
            {
                Canvas.PrintStringAligned(1, index + 3, item.Label, LabelAlignment,
                    Size.Width - 2, Pigments[PigmentType.ViewHilight]);
            }
            else
            {
                Canvas.PrintStringAligned(1, index + 3, item.Label, LabelAlignment,
                    Size.Width - 2, Pigments[PigmentType.ViewNormal]);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the index of the item that contains the provided point, specified in local
        /// space coordinates.  Returns -1 if no items are at that position.
        /// </summary>
        /// <param name="lPos"></param>
        /// <returns></returns>
        protected int GetItemAt(Point lPos)
        {
            int index = -1;

            if (lPos.X > 0 && lPos.X < Size.Width - 1)
            {
                int i = lPos.Y - 3;
                if (i >= 0 && i < Size.Height - 4)
                {
                    index = i;
                }
            }
            return index;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Handlers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the title and items.  Override to add custom drawing code.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            DrawTitle();
            DrawItems();
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method detects if the mouse is over one of the items, and changes state
        /// accordingly.  Override to add custom handling.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseMoved(MouseData mouseData)
        {
            base.OnMouseMoved(mouseData);

            Point lPos = ScreenToLocal(mouseData.Position);

            mouseOverIndex = GetItemAt(lPos);

            if (mouseOverIndex != -1)
            {
                TooltipText = Items[mouseOverIndex].TooltipText;
            }
            else
            {
                TooltipText = null;
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Detects which, if any, item has been selected by a left mouse button.  Override
        /// to add custom handling.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseOverIndex != -1)
            {
                if (CurrentSelected != mouseOverIndex)
                {
                    CurrentSelected = mouseOverIndex;
                    OnItemSelected(CurrentSelected);
                }

            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when one of the items in the list has been selected with the left mouse
        /// button.  Base method triggers appropriate event.  Override to add custom handling.
        /// </summary>
        /// <param name="index"></param>
        protected virtual void OnItemSelected(int index)
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, new ListItemSelectedEventArgs(index));
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private List<ListItemData> Items;
        private int mouseOverIndex;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
    #endregion
}
