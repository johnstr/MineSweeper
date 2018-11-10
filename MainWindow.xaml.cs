using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MineSweeper.Models;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MineSweeper
{  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IconCollection GameIcons;
        public Game MsGame;  
        public static int MX_Width = 10;
        public static int MX_Height = 10;
        public static int NUM_BOMBS = 10;
        private int[] Cords = new int[2];

        //Config window size
        private void InitWindow()
        {
            gameCanvas.Height = GameIcon.SIZE * MX_Height;
            gameCanvas.Width = GameIcon.SIZE * MX_Width;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
        }

        //Fill Canvas with Images
        private void FillWithBoxes()
        { 
            for(int i = 0; i < MsGame.UpperMatrix.Count; i++)
            {  
                for (int j = 0; j < MsGame.UpperMatrix[i].Count; j++)
                {
                    string imageName = MsGame.UpperMatrix[i][j].IconName;
                    GameIcon image = new GameIcon(GameIcons[imageName]);
                    Canvas.SetLeft(image, j*GameIcon.SIZE);
                    Canvas.SetTop(image, i*GameIcon.SIZE);
                    gameCanvas.Children.Add(image);
                }
            }
        }

        public void RefreshCanvas()
        {
            for (int i = 0; i < MsGame.UpperMatrix.Count; i++)
            {
                for (int j = 0; j < MsGame.UpperMatrix[i].Count; j++)
                {
                    ((GameIcon)gameCanvas.Children[i * MsGame.UpperMatrix[i].Count + j]).Source = GameIcons[MsGame.UpperMatrix[i][j].IconName];
                }
            }
        }

        public void Resume()
        {
            if ((MsGame = GameToFile.Load()) != null)
            {
                GameToFile.RemoveFile();
                MX_Height = MsGame.UpperMatrix.Count;
                MX_Width = MsGame.UpperMatrix[0].Count;
                NUM_BOMBS = MsGame.LowerMatrix.getNumBombs();
                FillWithBoxes();
                InitWindow();
            }
        }

        public void Restart()
        {
            gameCanvas.Children.Clear();
            StartNewGame();
        }

        public void StartNewGame()
        {
            MsGame = new Game(MX_Height, MX_Width, NUM_BOMBS);
            FillWithBoxes();
            InitWindow();
        }

        public MainWindow()
        {
            InitializeComponent();
            GameIcons = new IconCollection();
            if (GameToFile.isFileExist())
                Resume();
            else
               StartNewGame();
        }


        private int[] getBoxCords(object sender, MouseButtonEventArgs e)
        {
            //Modify to get Box
            Point mouseClickPoint = e.GetPosition((IInputElement)sender);
            Cords[0] = (int)mouseClickPoint.Y / GameIcon.SIZE;
            Cords[1] = (int)mouseClickPoint.X / GameIcon.SIZE;
            return Cords;
        }

        public void MessageBoxDispaly(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Game Satus", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Restart();
                    break;
                case MessageBoxResult.No:
                    this.Close();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        public void CheckGameStatus()
        {
            switch (MsGame.GameStatus)
            {
                case Status.Won:
                    {
                        this.MessageBoxDispaly("Congradulations you won the game!!! Do you want to play again?");
                        break;
                    }
                case Status.Lost:
                    {
                        EnableUndoButton();
                        this.MessageBoxDispaly("You LOST the Game!!! Do you want to play again?");
                        break;
                    }
                default:
                    break;
            }
        }

        private void EnableUndoButton()
        {
            if (MsGame.GameStatus == Status.Lost)
            {
                undoButton.IsEnabled = true;
            }
        }

        private void gameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int[] cords = getBoxCords(sender, e);
            if (MsGame.isPlaying())
            {
                MsGame.OpenBox(cords[0], cords[1]);
                RefreshCanvas();
                CheckGameStatus();
            }
        }

        private void gameCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int[] cords = getBoxCords(sender, e);
            if (MsGame.isPlaying())
            {
                MsGame.FlagToggle(cords[0], cords[1]);
                RefreshCanvas();
                CheckGameStatus();
            }
        }
      
        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
                MsGame.CloseBox(Cords[0], Cords[1]);
                RefreshCanvas();
                MsGame.GameStatus = Status.Play;
        }

        private void bombMapButton_Click(object sender, RoutedEventArgs e)
        {
            MsGame.DisplayAllBombs();
            RefreshCanvas();

        }
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        private void saveGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameToFile.Save(this.MsGame);
        }

        private void gameSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Window gameSettings = new GameSettings(this, MX_Height, MX_Width, NUM_BOMBS);
            gameSettings.ShowDialog();
            
        }

        private void exitGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
