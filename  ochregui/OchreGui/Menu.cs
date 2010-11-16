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

namespace OchreGui
{
    #region Menu Helper Classes
    public class MenuItemSelectedEventArgs : EventArgs
    {
        public MenuItemSelectedEventArgs(int index)
        {
            Index = index;
        }

        public int Index { get; private set; }
    }

    /// <summary>
    /// Represents a single menu choice.
    /// </summary>
    public class MenuItemData
    {
        public MenuItemData(string label, string toolTip = null)
        {
            this.Label = label;
            this.TooltipText = toolTip;
        }

        public string Label { get; set; }
        public string TooltipText { get; set; }
    }

    #endregion


    #region MenuInfo Class

    public class MenuTemplate : ControlTemplate
    {
        // /////////////////////////////////////////////////////////////////////////////////
        public MenuTemplate()
        {
            Items = new List<MenuItemData>();
            LabelAlignment = HorizontalAlignment.Left;
            HilightWhenMouseOver = false;
            CanHaveKeyboardFocus = false;
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// A list of menu options.  Defaults to empty list.
        /// </summary>
        public List<MenuItemData> Items { get; set; }

        /// <summary>
        /// The alignment of the menu option labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }

        /// <summary>
        /// True if the menu can receive the keyboard focus.  Defaults to false.
        /// </summary>
        public bool CanHaveKeyboardFocus { get; set; }

        /// <summary>
        /// True if the menu is hilighted by mouse over.  Defaults to false.  Note that this
        /// property pertains to the entire menu, not the items themselves which are hilighted
        /// by mosue over automatically.
        /// </summary>
        public bool HilightWhenMouseOver { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        public override Size CalculateSize()
        {
            int width = 0;
            foreach (MenuItemData data in Items)
            {
                if (data.Label == null)
                    data.Label = "";

                if (data.Label.Length > width)
                    width = data.Label.Length;
            }

            width += 4;

            int height = Items.Count + 2;

            return new Size(width, height);
        }
        // /////////////////////////////////////////////////////////////////////////////////
    }
    #endregion


    #region Menu Class
    /// <summary>
    /// A menu is similar to a list box, except it does not have a title and item selection
    /// immediately closes the menu.  A menu is also automatically closed (removed from
    /// parent window) when the mouse leaves the menu region.
    /// </summary>
    public class Menu : Control
    {
        #region Events
        // /////////////////////////////////////////////////////////////////////////////////
        public event EventHandler<MenuItemSelectedEventArgs> ItemSelected;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Constructors
        // /////////////////////////////////////////////////////////////////////////////////
        public Menu(MenuTemplate template)
            : base(template)
        {
            HasFrame = true;
            HilightWhenMouseOver = template.HilightWhenMouseOver;
            CanHaveKeyboardFocus = template.CanHaveKeyboardFocus;

            LabelAlignment = template.LabelAlignment;
            Items = template.Items;
            mouseOverIndex = -1;

        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Properties
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// The alignment of the menu option labels.
        /// </summary>
        public HorizontalAlignment LabelAlignment { get; set; }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Public Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the label of the menu option item with the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetItemLabel(int index)
        {
            if (index < 0 || index >= Items.Count)
                throw new ArgumentOutOfRangeException("index");

            return Items[index].Label;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Protected Methods
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws all of the menu items.
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
        /// Draws the specified menu item.
        /// </summary>
        /// <param name="index"></param>
        protected void DrawItem(int index)
        {
            MenuItemData item = Items[index];

            if (index == mouseOverIndex)
            {
                Canvas.PrintStringAligned(1, index + 1, item.Label, LabelAlignment,
                    Size.Width - 2, DefaultPigments.Hilight);
            }
            else
            {
                Canvas.PrintStringAligned(1, index + 1, item.Label, LabelAlignment,
                    Size.Width - 2, DefaultPigments.Active);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Returns the index of the item at the given position, or -1 if there is not item
        /// at that position.  The position is in local space coordinates.
        /// </summary>
        /// <param name="lPos"></param>
        /// <returns></returns>
        protected int GetItemAt(Point lPos)
        {
            int index = -1;

            if (lPos.X > 0 && lPos.X < Size.Width - 1)
            {
                int i = lPos.Y - 1;
                if (i >= 0 && i < Size.Height - 2)
                {
                    index = i;
                }
            }
            return index;
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Message Hanglers
        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Draws the menu items.  Override to add custom drawing code.
        /// </summary>
        protected override void Redraw()
        {
            base.Redraw();

            DrawItems();
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method detects if the mouse pointer is currently over a menu items and
        /// sets the state accordingly.  Override to add custom handling.
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
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method detects if a menu item was selected, and calls OnItemSelected if this
        /// is the case.  Override to add custom handling.
        /// </summary>
        /// <param name="mouseData"></param>
        protected internal override void OnMouseButtonDown(MouseData mouseData)
        {
            base.OnMouseButtonDown(mouseData);

            if (mouseOverIndex != -1)
            {
                OnItemSelected(mouseOverIndex);
            }
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Base method removes this menu from the parent window.  Override to add custom
        /// handling (but any code placed after calling this base method will come after
        /// this menu has been removed from the window).
        /// </summary>
        protected internal override void OnMouseLeave()
        {
            base.OnMouseLeave();

            ParentWindow.RemoveControl(this);
        }
        // /////////////////////////////////////////////////////////////////////////////////

        // /////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Triggers the appropriate event, and removes this menu from the parent window.  Override
        /// to add custom handling.
        /// </summary>
        /// <param name="index"></param>
        protected virtual void OnItemSelected(int index)
        {
            if (ItemSelected != null)
            {
                ItemSelected(this, new MenuItemSelectedEventArgs(index));
            }

            ParentWindow.RemoveControl(this);
        }
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
        #region Private
        // /////////////////////////////////////////////////////////////////////////////////
        private List<MenuItemData> Items;
        private int mouseOverIndex;
        // /////////////////////////////////////////////////////////////////////////////////
        #endregion
    }
    #endregion
}
