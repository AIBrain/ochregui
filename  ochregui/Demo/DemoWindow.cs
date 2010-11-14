using System;
using System.Collections.Generic;
using OchreGui;
using OchreGui.Utility;

namespace OchreGui.Demo
{
    class DemoWindowTemplate : WindowTemplate
    {
        public bool HasPrevious { get; set; }
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

            Size panelSize = new Size(this.Size.Width-2,this.Size.Height-3);

            PanelTemplate panelTemplate = new PanelTemplate()
            {
                HasFrame = true,
                UpperLeftPos = new Point(0,0),
                Size = panelSize
            };
            panelTemplate.SetLowerRight(this.ScreenRect.LowerRight.Shift(-1, -1));

            AddControl(new Panel(panelTemplate));

            ViewRect = Rect.Inflate(panelTemplate.CalculateRect(),-1,-1);

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

        protected Rect ViewRect { get; set; }


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
