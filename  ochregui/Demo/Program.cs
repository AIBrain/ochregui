using System;
using System.Collections.Generic;
using OchreGui;
using OchreGui.Utility;

namespace OchreGui.Demo
{
    class DemoApplication : Application
    {
        protected override void Setup(ApplicationInfo info)
        {
            base.Setup(info);

            DemoWindowTemplate demoTmpl = new DemoWindowTemplate()
            {
                HasFrame = true,
                HasNext = true,
                HasPrevious = false
            };

            pages.Add(new Page1(demoTmpl));

            demoTmpl.HasNext = false;
            demoTmpl.HasPrevious = true;
            pages.Add(new Page2(demoTmpl));

            SetWindow(pages[0]);
        }

        
        internal void NextPage()
        {
            if (currPage < pages.Count)
            {
                currPage++;
                SetWindow(pages[currPage]);
            }
        }

        internal void PreviousPage()
        {
            if (currPage >= 0)
            {
                currPage--;
                SetWindow(pages[currPage]);
            }
        }

        List<DemoWindow> pages = new List<DemoWindow>();
        int currPage;
    }

    class Program
    {
        static void Main(string[] args)
        {
            ApplicationInfo appInfo = new ApplicationInfo()
            {
                Title = "OchreGui Demo",
                ScreenSize = new Size(80, 70),
                Font = "courier12x12_aa_tc.png",
                FontFlags = libtcod.TCODFontFlags.LayoutTCOD,
            };

            using (DemoApplication app = new DemoApplication())
            {
                app.Start(appInfo);
            }
        }
    }
}
