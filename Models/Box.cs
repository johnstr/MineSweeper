using System;
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

   

    //enum Ico
    //{
    //    ZERO,
    //    NUM1,
    //    NUM2,
    //    NUM3,
    //    NUM4,
    //    NUM5,
    //    NUM6,
    //    NUM7,
    //    NUM8,
    //    OPENED,
    //    CLOZED,
    //    FLAGED,
    //    NOBOMB,
    //    BOMB,
    //    BOMBED
    //};

    struct Icon
    {
        public string IconName;
        public BitmapImage BitmapIcon;

   
        public Icon(string iconName, string bitmapIconPath)
        {
            //set IconName   
            IconName = iconName;
            //set BitmapIcon
            BitmapImage bitmapIcon = new BitmapImage();
            bitmapIcon.BeginInit();
            bitmapIcon.UriSource = new Uri(bitmapIconPath);
            bitmapIcon.EndInit();
            BitmapIcon = bitmapIcon;
        }
    }


    class Icons
    {
        const string ICONS_SOURCE = @"C:\Users\tvstr\Documents\visual studio 2015\Projects\MineSweeper\MineSweeper\Resources\icons";

        List<Icon> listOfIcons;

        public void LoadIcons()
        {

            string[] fileEntries = Directory.GetFiles(ICONS_SOURCE);
            foreach (string filePath in fileEntries)
            {
                string iconName = Path.GetFileNameWithoutExtension(filePath).ToUpper();
                listOfIcons.Add(new Icon(iconName, filePath));
            }
        }
    }


    //class Box
    //{
    //    public int X;
    //    public int Y;
    //  //  public Image Icon;


    //}
}
