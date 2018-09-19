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

namespace MineSweeper
{

    public class GameMatrix : List<List<Box>>
    {
        public static Icons GameIcons;
        private Random Rand;
        private int NumberOfBombs;

        private void SetUpBombs()
        {
           int randI, randJ; 
           for(int i = 0; i < NumberOfBombs; i++)
           {
                do
                {
                    randI = getRandomNumber(this.Count);
                    randJ = getRandomNumber(this.Count);
                } while (this[randI][randJ].IconName == "BOMB");
                this[randI][randJ] = new Box(GameIcons["BOMB"]);
           }
        }

        private int getRandomNumber(int limit)
        {
            return Rand.Next(limit);
        }

        private void InitializeMatrix(int size)
        {
            for (int i = 0; i < size; i++)
            {
                this.Add(new List<Box>());
                for (int j = 0; j < size; j++)
                {   
                  this[i].Add(new Box(GameIcons["CLOSED"], j, i));
                }
            } 
        }

        private int CountBombsAround(Box box)
        {
            int count = 0;
            for (int i = box.Y - 1; i <= box.Y + 1; i++)
                for (int j = box.X - 1; j <= box.X + 1; j++)
                    if (i > -1 && i < this.Count && j > -1 && j < this.Count)
                        if (this[i][j].IconName == "BOMB") count++;
            return count;
        }

        private void SetUpNumbers()
        {
            for (int i = 0; i < this.Count; i++)
                for (int j = 0; j < this[i].Count; j++)
                {
                    if(this[i][j].IconName != "BOMB")
                    {
                       int count = CountBombsAround(this[i][j]);
                       if(count == 0)
                            this[i][j] = new Box(GameIcons["ZERO"]);
                       else
                            this[i][j] = new Box(GameIcons["NUM" + count.ToString()]);
                    }   
                }
        }

        public GameMatrix()
        {
            if (GameIcons == null)
                GameIcons = new Icons();
        }

        //constructor for upperlevel matrix
        public GameMatrix(int size):this()
        {
            InitializeMatrix(size);
        }

        //Constructor for lower levelmatrix
        public GameMatrix(int size, int numOfBombs):this(size)
        {
            Rand = new Random();
            NumberOfBombs = numOfBombs;
            SetUpBombs();
            SetUpNumbers();
        }

    }

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameMatrix UnderMatrix;   
        public const int MX_SIZE = 9;

        private void InitWindow()
        {
            gameCanvas.Height = gameCanvas.Width = Box.SIZE * MX_SIZE;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void FillWithBoxes()
        { 
            for(int i = 0; i < UnderMatrix.Count; i++)
            {
              
                for (int j = 0; j < UnderMatrix[i].Count; j++)
                {
                    Box box = UnderMatrix[i][j];
                    gameCanvas.Children.Add(box);
                    Canvas.SetLeft(box, j*Box.SIZE);
                    Canvas.SetTop(box, i*Box.SIZE);
                }
            }
        }
    
        public MainWindow()
        {
            InitializeComponent();
            UnderMatrix = new GameMatrix(MX_SIZE,1);
            FillWithBoxes();
            InitWindow();
        }
    }

}
