using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Miku.Client.Helpers
{
    class ListBoxItemsHelper
    {
        public static void AddListBixItem(int num,string strText, ListBox listBox)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Height = 25;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = ">>  " + strText; 
            stackPanel.Children.Add(textBlock);
            listBox.Items.Add(stackPanel);
        }
        public static void AddListBixItem(string[] strText, ListBox listBox)
        {
            if (strText.Length == 3)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Height = 25;
                stackPanel.Orientation = Orientation.Horizontal;

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Text = strText[0];
                textBlock1.Width = 180;
                textBlock1.Foreground = new SolidColorBrush(Colors.Gray);
                stackPanel.Children.Add(textBlock1);

                TextBlock block = new TextBlock();
                block.Width = 20;
                stackPanel.Children.Add(block);

                TextBlock textBlock2 = new TextBlock();
                textBlock2.Text = strText[1];
                textBlock2.Width = 120;
                textBlock2.Foreground = new SolidColorBrush(Colors.Gray);
                stackPanel.Children.Add(textBlock2);

                TextBlock block1 = new TextBlock();
                block1.Width = 20;
                stackPanel.Children.Add(block1);

                TextBlock textBlock3 = new TextBlock();
                textBlock3.Text = strText[2];
                textBlock3.Width = 100;
                textBlock3.Foreground = new SolidColorBrush(Colors.Gray);
                stackPanel.Children.Add(textBlock3);

                listBox.Items.Add(stackPanel);
            }
        }
    }
}
