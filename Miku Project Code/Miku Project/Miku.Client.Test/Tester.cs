using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Miku.Client.Views.ActionViews;

namespace Miku.Client.Test
{
    class Tester
    {        
        [STAThread]
        static void Main()
        {            
            TestInit.Init();
            ActionViewUITest c = new ActionViewUITest();
            c.CodedUITestMethod1();
        }
    }
}
