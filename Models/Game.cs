using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
    public enum Status : int { Play, Lost, Won, NotPalaying };

    [Serializable]
    public class Game
    {
        public GameMatrix LowerMatrix;
        public GameMatrix UpperMatrix;
        public Status GameStatus;
        private int NumOfOpenedBoxes;

        private void OpenBoxesAround(int y, int x)
        {
            UpperMatrix[y][x].IconName = LowerMatrix[y][x].IconName;
            for (int i = y - 1; i <= y + 1; i++)
                for (int j = x - 1; j <= x + 1; j++)
                    if (i > -1 && i < UpperMatrix.Count && j > -1 && j < UpperMatrix[i].Count)
                    {
                        if (!LowerMatrix[i][j].IconName.Equals("BOMB")) OpenBox(i, j);

                    }
        }

        public void OpenBox(int y, int x)
        {
            string lowerIconName = "";
            if (UpperMatrix[y][x].IconName.Equals("CLOSED"))
            {
                lowerIconName = LowerMatrix[y][x].IconName;
                NumOfOpenedBoxes--;
                CheckIfWin();
                if (lowerIconName.Equals("ZERO"))
                    OpenBoxesAround(y, x);
                else if (LowerMatrix[y][x].IconName.Contains("NUM"))
                    UpperMatrix[y][x].IconName = LowerMatrix[y][x].IconName;
                else
                {
                    UpperMatrix[y][x].IconName = "BOMBED";
                    GameStatus = Status.Lost;
                }
            }
        }

        public void CloseBox(int y, int x)
        {
            UpperMatrix[y][x].IconName = "CLOSED";
            NumOfOpenedBoxes++;
        }

        public bool isAnyBoxOpen()
        {
            return NumOfOpenedBoxes < UpperMatrix.Count * UpperMatrix.Count;
        }

        private void CheckIfWin()
        {
            if (NumOfOpenedBoxes == 0)
                GameStatus = Status.Won;
        }

        public bool isPlaying()
        {
            return GameStatus == Status.Play;
        }

        public void FlagToggle(int y, int x)
        {
            if (UpperMatrix[y][x].IconName.Equals("FLAGED"))
            {
                UpperMatrix[y][x].IconName = "CLOSED";
                NumOfOpenedBoxes++;
            }
            else if (UpperMatrix[y][x].IconName.Equals("CLOSED"))
            {
                UpperMatrix[y][x].IconName = "FLAGED";
                NumOfOpenedBoxes--;
                CheckIfWin();
            }
        }

        public void DisplayAllBombs()
        {
            for (int i = 0; i < UpperMatrix.Count; i++)
                for (int j = 0; j < UpperMatrix[0].Count; j++)
                {
                    if (LowerMatrix[i][j].IconName.Equals("BOMB") && !UpperMatrix[i][j].IconName.Equals("BOMBED"))
                        UpperMatrix[i][j].IconName = "BOMB";
                }
            GameStatus = Status.NotPalaying;
        }

        public Game(int mx_w, int mx_h, int numOfBombs)
        {
            this.UpperMatrix = new GameMatrix(mx_w, mx_h);
            this.LowerMatrix = new GameMatrix(mx_w, mx_h, numOfBombs);
            this.GameStatus = Status.Play;
            this.NumOfOpenedBoxes = mx_w * mx_h;
        }
    }

}
