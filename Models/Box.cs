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
    public class Box : Image
    {
        public const int SIZE = 50;
        public string IconName;
        public int X;
        public int Y;

        public Box(string iconName, BitmapImage source)
        {
            this.Source = source;
            this.IconName = iconName;
           // this.Height = this.Width = SIZE;
        }

        public Box(string iconName, BitmapImage source, int x, int y)
        {
            this.Source = source;
            this.X = x;
            this.Y = y;
            this.IconName = iconName;
           // this.Height = this.Width = SIZE;
        }

        public Box(Box box, int x, int y):this(box.IconName, (BitmapImage)box.Source, x, y) {}

        public Box(Box box) : this(box.IconName, (BitmapImage)box.Source) { }

        public void SetBox(Box box)
        {
            this.Source = box.Source;
            this.IconName = box.IconName;
        }

    }
}
