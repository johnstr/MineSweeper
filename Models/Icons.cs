using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MineSweeper.Models
{

    public class Icons : List<Box>
    {
        private const string ICONS_SOURCE = @"C:\Users\tvstr\Documents\visual studio 2015\Projects\MineSweeper\MineSweeper\Resources\icons";

        public Box this[string iconName]
        {
            get
            {
                return (Box)this.Where(i => i.IconName == iconName).SingleOrDefault();
            }
        }
            
        public Icons()
        {
            LoadIcons();
        }

        private void LoadIcons()
        {

            string[] fileEntries = Directory.GetFiles(ICONS_SOURCE);
            foreach (string filePath in fileEntries)
            {
                string iconName = Path.GetFileNameWithoutExtension(filePath).ToUpper();
                this.Add(new Box(iconName, getBitmapImage(filePath)));
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


