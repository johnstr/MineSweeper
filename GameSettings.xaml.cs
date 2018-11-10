using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MineSweeper.Models;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for GameSettings.xaml
    /// </summary>
    /// 
    public class GameProperties
    {
        public int FieldWidth { get; set; }
        public int FieldHeight { get; set; }
        public int NumOfBombs { get; set; }
    }



    public partial class GameSettings : Window
    {
        private MainWindow CallWindow;
        GameProperties Prop;

        public GameSettings(MainWindow callWindow, int mx_hg, int mx_wid, int numOfBombs)
        {
            Prop = new GameProperties { FieldHeight = mx_hg, FieldWidth = mx_wid, NumOfBombs = numOfBombs };
            CallWindow = callWindow;
            this.DataContext = Prop;
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void ValidateProperties()
        {
            if (Prop.FieldHeight > 15)
                         Prop.FieldHeight = 15;
             else if(Prop.FieldHeight < 5)
                         Prop.FieldHeight = 5;
            if (Prop.FieldWidth > 30)
                         Prop.FieldWidth = 30;
            else if(Prop.FieldWidth < 5)
                         Prop.FieldWidth = 5;
            if (Prop.NumOfBombs > Prop.FieldHeight * Prop.FieldWidth / 2)
                         Prop.NumOfBombs = (Prop.FieldHeight * Prop.FieldWidth) / 2;
            else if (Prop.NumOfBombs < (Prop.FieldHeight * Prop.FieldWidth) / 10)
                         Prop.NumOfBombs = (Prop.FieldHeight * Prop.FieldWidth) / 10;

        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateProperties();  
            MainWindow.MX_Height = Prop.FieldHeight;
            MainWindow.MX_Width = Prop.FieldWidth;
            MainWindow.NUM_BOMBS = Prop.NumOfBombs;
            this.Close();
            CallWindow.Restart();
            
        }
    }
}
