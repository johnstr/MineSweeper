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


    public enum Status : int { Play, Lost, Won };

    public class Game
    {
        public GameMatrix LowerMatrix;
        public GameMatrix UpperMatrix;
        public Status GameStatus;
        private int NumOfOpenedBoxes;

        private void OpenBoxesAround(Box box)
        {
           box.SetBox(LowerMatrix[box.Y][box.X]);
            for (int i = box.Y - 1; i <= box.Y + 1; i++)
                for (int j = box.X - 1; j <= box.X + 1; j++)
                    if (i > -1 && i < UpperMatrix.Count && j > -1 && j < UpperMatrix.Count)
                    {
                        if (!LowerMatrix[i][j].IconName.Equals("BOMB")) OpenBox(i, j);
                   
                    }
        }

        public void OpenBox(int y, int x)
        {
            string lowerIconName = "";
            if(UpperMatrix[y][x].IconName.Equals("CLOSED")){
                lowerIconName = LowerMatrix[y][x].IconName;
                NumOfOpenedBoxes--;
                CheckIfWin();
                if (lowerIconName.Equals("ZERO"))
                    OpenBoxesAround(UpperMatrix[y][x]);
                else if (LowerMatrix[y][x].IconName.Contains("NUM"))
                    UpperMatrix[y][x].SetBox(LowerMatrix[y][x]);
                else
                {
                    UpperMatrix[y][x].SetBox(GameMatrix.GameIcons["BOMBED"]);
                    GameStatus = Status.Lost;
                }
            }
        }

        private void CheckIfWin()
        {
            if (NumOfOpenedBoxes == 0)
                GameStatus = Status.Won;
        }


        public void PutTheFlag(int y, int x)
        {
            UpperMatrix[y][x].SetBox(GameMatrix.GameIcons["FLAGED"]);
            NumOfOpenedBoxes--;
            CheckIfWin();
        }

        public Game(int mx_size, int numOfBombs)
        {
            this.UpperMatrix = new GameMatrix(mx_size);
            this.LowerMatrix = new GameMatrix(mx_size, numOfBombs);
            this.GameStatus = Status.Play;
            this.NumOfOpenedBoxes = UpperMatrix.Count * UpperMatrix.Count;
        }
    }
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game MsGame;  
        public const int MX_SIZE = 3;
        public const int NUM_BOMBS = 1;

        private void InitWindow()
        {
            gameCanvas.Height = gameCanvas.Width = Box.SIZE * MX_SIZE;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
        }


        private void FillWithBoxes()
        { 
            for(int i = 0; i < MsGame.UpperMatrix.Count; i++)
            {  
                for (int j = 0; j < MsGame.UpperMatrix[i].Count; j++)
                {
                    Box box = MsGame.UpperMatrix[i][j];
                    gameCanvas.Children.Add(box);
                    Canvas.SetLeft(box, j*Box.SIZE);
                    Canvas.SetTop(box, i*Box.SIZE);
                    Canvas a = new Canvas();
                }
            }
        }
    
        public MainWindow()
        {
            InitializeComponent();
            MsGame = new Game(MX_SIZE, NUM_BOMBS);
            FillWithBoxes();
            InitWindow();
        }


        private int[] getBoxCords(object sender, MouseButtonEventArgs e)
        {
            int[] cords = new int[2];
            Point mouseClickPoint = e.GetPosition((IInputElement)sender);
            cords[0] = (int)mouseClickPoint.Y / Box.SIZE;
            cords[1] = (int)mouseClickPoint.X / Box.SIZE;
            return cords;
        }

        private void gameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int[] cords = getBoxCords(sender, e);

            if(MsGame.GameStatus == Status.Play)
            {
                MsGame.OpenBox(cords[0], cords[1]);
                switch (MsGame.GameStatus)
                {
                    case Status.Won:/*write win message*/
                        break;
                    case Status.Lost:/*write win message*/
                        break;
                    default:
                        break;
                }
            }   
        }

        private void gameCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            int[] cords = getBoxCords(sender, e);
            if (MsGame.GameStatus == Status.Play)
            {
                MsGame.PutTheFlag(cords[0], cords[1]); 
                switch (MsGame.GameStatus)
                {
                    case Status.Won:/*write win message*/
                        break;
                    case Status.Lost:/*write win message*/
                        break;
                    default:
                        break;
                }
            }
           
        }
    }

}
