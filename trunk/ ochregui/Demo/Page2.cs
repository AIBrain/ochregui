using System;
using OchreGui.Utility;
using OchreGui;

namespace OchreGui.Demo
{
    class Page2 : DemoWindow
    {
        public Page2(DemoWindowTemplate template)
            : base(template)
        {
        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            CheckBoxTemplate cb1 = new CheckBoxTemplate()
            {
                Label = " A Checkbox",
                UpperLeftPos = ViewRect.UpperLeft.Shift(1, 3),
            };
            AddControl(new CheckBox(cb1));

            CheckBoxTemplate cb2 = new CheckBoxTemplate()
            {
                Label = " Checkbox",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = true
            };
            cb2.AlignTo(LayoutDirection.South, cb1, 1);
            AddControl(new CheckBox(cb2));

            CheckBoxTemplate cb3 = new CheckBoxTemplate()
            {
                Label = "Checkbox",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = true,
                LabelAlignment = HorizontalAlignment.Right
            };
            cb3.AlignTo(LayoutDirection.South, cb2, 1);
            AddControl(new CheckBox(cb3));

            CheckBoxTemplate cb4 = new CheckBoxTemplate()
            {
                Label = "Checkbox",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = true,
                LabelAlignment = HorizontalAlignment.Center,
            };
            cb4.AlignTo(LayoutDirection.South, cb3, 1);
            AddControl(new CheckBox(cb4));

            CheckBoxTemplate cb5 = new CheckBoxTemplate()
            {
                Label = "Checkbox",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = false,
                LabelAlignment = HorizontalAlignment.Left,
                VerticalAlign = VerticalAlignment.Center,
                AutoSizeOverride = new Size(14,3),
                HasFrameBorder = false,
            };
            cb5.AlignTo(LayoutDirection.East, cb2, 1);
            AddControl(new CheckBox(cb5));

            CheckBoxTemplate cb6 = new CheckBoxTemplate()
            {
                Label = "Checkbox ",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = false,
                LabelAlignment = HorizontalAlignment.Right,
                VerticalAlign = VerticalAlignment.Center,
                AutoSizeOverride = new Size(14, 3),
                HasFrameBorder = false,
            };
            cb6.AlignTo(LayoutDirection.South, cb5, 1);
            AddControl(new CheckBox(cb6));

            CheckBoxTemplate cb7 = new CheckBoxTemplate()
            {
                Label = "Checkbox ",
                CheckOnLeft = false,
                LabelAlignment = HorizontalAlignment.Center,
                HasFrameBorder = false,
            };
            cb7.AlignTo(LayoutDirection.South, cb6, 1);
            AddControl(new CheckBox(cb7));

            CheckBoxTemplate cb8 = new CheckBoxTemplate()
            {
                Label = "Custom",
                MinimumWidth = cb1.CalculateSize().Width,
                CheckOnLeft = false,
                LabelAlignment = HorizontalAlignment.Center,
            };
            cb8.AlignTo(LayoutDirection.North, cb5, 1);
            AddControl(new FancyCheck(cb8));


            ListBoxTemplate lb1 = new ListBoxTemplate()
            {
                Title = "A List Box",
                Items = new System.Collections.Generic.List<ListItemData>()
                {
                    new ListItemData("Item 1","Item 1 Tooltip"),
                    new ListItemData("Item 2","Item 2 Tooltip"),
                    new ListItemData("Item 3","Item 3 Tooltip"),
                    new ListItemData("Item 4","Item 4 Tooltip")
                },
            };
            lb1.AlignTo(LayoutDirection.South, cb4, 2);
            AddControl(new ListBox(lb1));

            ListBoxTemplate lb2 = new ListBoxTemplate()
            {
                Title = "A List Box",
                TitleAlignment = HorizontalAlignment.Right,
                LabelAlignment = HorizontalAlignment.Center,
                MinimumListBoxWidth = 20,
                Items = new System.Collections.Generic.List<ListItemData>()
                {
                    new ListItemData("Item 1","Item 1 Tooltip"),
                    new ListItemData("Item 2","Item 2 Tooltip"),
                    new ListItemData("Item 3","Item 3 Tooltip"),
                    new ListItemData("Item 4","Item 4 Tooltip")
                }
            };
            lb2.AlignTo(LayoutDirection.East, lb1, 2);
            AddControl(new ListBox(lb2));
            

            TextEntryTemplate t1 = new TextEntryTemplate()
            {
                Label = "Text Entry (Hit Enter to keep): ",
                MaximumCharacters = 6
            };
            t1.AlignTo(LayoutDirection.East, cb8, 2);
            AddControl(new TextEntry(t1));

            TextEntryTemplate t2 = new TextEntryTemplate()
            {
                Label = "Symbols Only: ",
                Validation = TextEntryValidations.Symbols,
                MaximumCharacters = 25
            };
            t2.AlignTo(LayoutDirection.South, t1, 2);
            AddControl(new TextEntry(t2));

            TextEntryTemplate t3 = new TextEntryTemplate()
            {
                Label = "Numbers & Symbols: ",
                Validation = TextEntryValidations.Numbers | TextEntryValidations.Symbols,
                MaximumCharacters = 20
            };
            t3.AlignTo(LayoutDirection.South, t2, 2);
            AddControl(new TextEntry(t3));

            TextEntryTemplate t4 = new TextEntryTemplate()
            {
                Label = "Don't need to hit Enter: ",
                MaximumCharacters = 4,
                CommitOnLostFocus = true,
            };
            t4.AlignTo(LayoutDirection.South, t3, 2);
            t4.UpperLeftPos = t4.UpperLeftPos.Shift(4, 0);
            AddControl(new TextEntry(t4));

            TextEntryTemplate t5 = new TextEntryTemplate()
            {
                Label = "Replace Text: ",
                MaximumCharacters = 11,
                CommitOnLostFocus = true,
                ReplaceOnFirstKey = true,
                StartingField = "Replace me",
                HasFrameBorder = false
            };
            t5.AlignTo(LayoutDirection.South, t4, 2);
            AddControl(new TextEntry(t5));


            ButtonTemplate mb1 = new ButtonTemplate()
            {
                Label = "Right Click for Menu"
            };
            mb1.AlignTo(LayoutDirection.South, lb2,3);
            AddControl(new MenuButton(mb1));


            RadioGroupTemplate rg1 = new RadioGroupTemplate()
            {
                Items = new System.Collections.Generic.List<RadioItemData>()
                {
                    new RadioItemData("Radio One","Tooltip for radio 1"),
                    new RadioItemData("Radio Two","Tooltip for radio 2"),
                    new RadioItemData("Radio Three","Tooltip for radio 3")
                },
            };
            rg1.AlignTo(LayoutDirection.South, t5, 2);
            AddControl(new RadioGroup(rg1));

            RadioGroupTemplate rg2 = new RadioGroupTemplate()
            {
                Items = new System.Collections.Generic.List<RadioItemData>()
                {
                    new RadioItemData("Radio One","Tooltip for radio 1"),
                    new RadioItemData("Radio Two","Tooltip for radio 2"),
                    new RadioItemData("Radio Three","Tooltip for radio 3")
                },
                HasFrameBorder = false,
                RadioOnLeft = false
            };
            rg2.AlignTo(LayoutDirection.South, rg1, 1);
            AddControl(new RadioGroup(rg2));


            PageInfo.AddText("This page shows a selection of Checkbox, TextEntry, ListBox, and RadioGroup controls." +
                "\n\nCheckbox controls are similar to buttons, except that there state (IsChecked) is persistant, and" +
                " this state is, by default, shown by a check box graphic." +
                "\n\nTextEntry controls allow user input.  Various behaviors of validation and committing the input are available." +
                "\n\nListBox controls are basically a list of clickable buttons with persistant state - one item is always the currently selected." +
                "\n\nMenu controls are similar to ListBox, except they automatically close if an item is selected or the mouse pointer leaves the area." +
                "\n\nRadioGroups are the same as ListBoxes, except the currently selected item is displayed differently.");
        }


        class FancyCheck : CheckBox
        {
            public FancyCheck(CheckBoxTemplate template)
                : base(template)
            {
            }

            protected override Pigment DetermineFramePigment()
            {
                if (IsChecked)
                {
                    return Pigments[PigmentType.ViewHilight];
                }

                return base.DetermineFramePigment();
            }
        }

        class MenuButton : Button
        {
            public MenuButton(ButtonTemplate template)
                : base(template)
            {

            }

            protected override void OnSettingUp()
            {
                base.OnSettingUp();

            }

            protected override void OnMouseButtonDown(MouseData mouseData)
            {
                base.OnMouseButtonDown(mouseData);

                if (mouseData.MouseButton == MouseButton.RightButton)
                {
                    Menu menu = new Menu(new MenuTemplate()
                    {
                        Items = new System.Collections.Generic.List<MenuItemData>()
                        {
                            new MenuItemData("Menu Item 1","Selected Menu Item 1"),
                            new MenuItemData("Menu Item 2","Selected Menu Item 2"),
                            new MenuItemData("Menu Item 3","Selected Menu Item 3"),
                            new MenuItemData("Menu Item 4","Selected Menu Item 4")
                        },
                        UpperLeftPos = mouseData.Position,
                    });

                    ParentWindow.AddControl(menu);
                }
            }
        }
    }
}
