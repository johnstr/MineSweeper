using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MineSweeper.Models
{

    [Serializable]
    public class Box
    {
        public string IconName { get; set; }
        public int X { get; }
        public int Y { get; }

        public Box(string iconName, int y, int x)
        {
            IconName = iconName;
            Y = y;
            X = x;
        }
    }

}
