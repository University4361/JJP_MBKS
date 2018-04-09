using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab_4
{
    public static class DirectoryHelper
    {
        public static Dictionary<string, DirectoryObject> CurrentDirectories = new Dictionary<string, DirectoryObject>(); 

        public static void SetupDock(string path, out DockPanel panel, out TextBox textBox)
        {
            panel = new DockPanel
            {
                LastChildFill = true,
                Margin = new System.Windows.Thickness { Bottom = 5, Top = 5 }
            };

            Button changeRulesButton = new Button
            {
                Content = "Change rules",
                Margin = new System.Windows.Thickness { Left = 8, Right = 4 }
            };

            Button copyFilesButton = new Button
            {
                Content = "Copy files to...",
                Margin = new System.Windows.Thickness { Left = 4, Right = 8 }
            };

            panel.Children.Add(changeRulesButton);
            panel.Children.Add(copyFilesButton);


            textBox = new TextBox()
            {
                Text = path,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Margin = new System.Windows.Thickness { Left = 10, Right = 10 }
            };

            panel.Children.Add(textBox);
        }
    }
}
