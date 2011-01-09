using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UITesting;

namespace Miku.Client.Test
{
    class TestInit
    {
        public static void Init()
        {
            Playback.Initialize();            
        }
    }
}
