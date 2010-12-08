using System;
using System.Collections.Generic;
using OchreGui;
using OchreGui.Utility;
using OchreGui.Extended;

namespace OchreGui.Demo
{
    class DemoWindowTemplate : WindowTemplate
    {
        /// <summary>
        /// Marks this page as having a previous page.  This controls the
        /// revious button's IsActive state.
        /// </summary>
        public bool HasPrevious { get; set; }

        /// <summary>
        /// Marks this page as having a next page.  This controls the next
        /// button's IsActive state.
        /// </summary>
        public bool HasNext { get; set; }
    }

    class DemoWindow : Window
    {
        public DemoWindow(DemoWindowTemplate template)
            : base(template)
        {
            hasNext = template.HasNext;
            hasPrev = template.HasPrevious;
        }

        protected override void OnSettingUp()
        {
            base.OnSettingUp();

            // Create and add interior panel that will "hold" all of the example controls
            Size panelSize = new Size(this.Size.Width-2,this.Size.Height-24);

            PanelTemplate panelTemplate = new PanelTemplate()
            {
                HasFrame = true,
                UpperLeftPos = this.LocalRect.UpperLeft.Shift(1,3),
                Size = panelSize
            };
            //panelTemplate.SetLowerRight(this.ScreenRect.LowerRight.Shift(-1, -17));
            AddControl(new Panel(panelTemplate));

            ViewRect = Rect.Inflate(panelTemplate.CalculateRect(),-1,-1);


            // Create the page info TextBox control.  Each page will have one.
            Rect textBoxRect = new Rect(panelTemplate.CalculateRect().LowerLeft.Shift(0, 1),
                this.LocalRect.LowerRight.Shift(-1, -1));

            TextBoxTemplate tbt = new TextBoxTemplate()
            {
                Size = textBoxRect.Size,
                UpperLeftPos = textBoxRect.UpperLeft,
                TextSpeed = 5,
                Pigments = new PigmentAlternatives()
                {
                    {PigmentType.Window,new Pigment(0xaaaaaa,0x111511)}
                }
            };

            PageInfo = new TextBox(tbt);
            AddControl(PageInfo);

            // Create the next, previous and quit buttons.  Each page will have these
            // buttons
            ButtonTemplate nextButtonTemplate = new ButtonTemplate()
            {
                Label = "Next->",
                Tooltip = "Click to go to the next page",
                HilightWhenMouseOver = true,
                LabelAlignment = HorizontalAlignment.Center,
                MinimumWidth = 12
            };
            nextButtonTemplate.SetUpperRight(Application.ScreenRect.UpperRight);

            ButtonTemplate previousButtonTemplate = new ButtonTemplate()
            {
                Label = "<-Previous",
                Tooltip = "Click to go to the previous page",
                HilightWhenMouseOver = true,
                UpperLeftPos = new Point(0, 0),
                LabelAlignment = HorizontalAlignment.Center,
                MinimumWidth = 12
            };


            ButtonTemplate quitButtonTemplate = new ButtonTemplate()
            {
                Label = "QUIT",
                Tooltip = "Quit the demo",
                HilightWhenMouseOver = true,
                LabelAlignment = HorizontalAlignment.Center,
                MinimumWidth = 38
            };
            quitButtonTemplate.SetTopCenter(Application.ScreenRect.TopCenter);

            Button quitButton = new Button(quitButtonTemplate);
            nextButton = new Button(nextButtonTemplate);
            prevButton = new Button(previousButtonTemplate);

            AddControls(nextButton, prevButton, quitButton);

            quitButton.ButtonPushed += new EventHandler(quitButton_ButtonClicked);
            prevButton.ButtonPushed += new EventHandler(prevButton_ButtonClicked);
            nextButton.ButtonPushed += new EventHandler(nextButton_ButtonClicked);

            prevButton.IsActive = hasPrev;
            nextButton.IsActive = hasNext;

        }

        /// <summary>
        /// The ViewRect holds the rect for the panel view, minus the frame border.
        /// We'll provide this so our child windows (pages) can position the controls easier
        /// </summary>
        protected Rect ViewRect { get; set; }

        protected TextBox PageInfo { get; set; }

        void nextButton_ButtonClicked(object sender, EventArgs e)
        {
            (ParentApplication as DemoApplication).NextPage();
        }

        void prevButton_ButtonClicked(object sender, EventArgs e)
        {
            (ParentApplication as DemoApplication).PreviousPage();
        }

        void quitButton_ButtonClicked(object sender, EventArgs e)
        {
            ParentApplication.IsQuitting = true;
        }

        bool hasPrev;
        bool hasNext;
        Button nextButton;
        Button prevButton;
    }
}
