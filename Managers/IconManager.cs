using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using DesktopHelper;

namespace Managers
{
    public class IconManager
    {
        /// <summary>
        /// Align the desktop icons into v-shape
        /// </summary>
        public void DemoSort()
        {
            IconHelper helper = new IconHelper();
            int IconCounts = helper.ListView_GetItemCount();
            for (int i = 0; i < IconCounts / 2; i++)
            {
                int x = 25 * i;
                int y = 25 * i;
                helper.SetIconPosition(i, x, y);
            }

            for (int i = IconCounts / 2, j = IconCounts / 2; i < IconCounts; i++, j--)
            {
                int x = 25 * i;
                int y = 25 * j;
                helper.SetIconPosition(i, x, y);
            }
        }
    }
}
