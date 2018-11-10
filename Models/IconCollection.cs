using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MineSweeper.Models
{

    public class GameIcon : Image
    {
        public static int SIZE = 45;
        public string IconName;

        public GameIcon(BitmapImage source)
        {
            this.Source = source;
            this.Height = this.Width = SIZE;
        }
    }

    public class IconCollection : Dictionary<string, BitmapImage>
    {
        private const string ICONS_SOURCE = @"C:\Users\tvstr\Documents\visual studio 2015\Projects\MineSweeper\MineSweeper\Resources\icons";

        public IconCollection()
        {
            LoadIcons();
        }

        private void LoadIcons()
        {

            string[] fileEntries = Directory.GetFiles(ICONS_SOURCE);
            foreach (string filePath in fileEntries)
            {
                string iconName = Path.GetFileNameWithoutExtension(filePath).ToUpper();
                this.Add(iconName, getBitmapImage(filePath));
            }
        }

        private BitmapImage getBitmapImage(string filepath)
        {
            BitmapImage bitmapIcon = new BitmapImage();
            bitmapIcon.BeginInit();
            bitmapIcon.UriSource = new Uri(filepath);
            bitmapIcon.EndInit();
            return bitmapIcon;
        }
    }
}
