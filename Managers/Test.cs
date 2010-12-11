using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Managers
{
    public class Test
    {
        [STAThread]
        static void Main()
        {
            IconManager manager = new IconManager();
            manager.DemoSort();
            Console.ReadKey();
        }
    }
}
