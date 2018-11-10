using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper.Models
{
   [Serializable]
   public class GameMatrix : List<List<Box>>
   {
        private Random Rand;
        private int NumberOfBombs;

        private void SetUpBombs()
        {
            int randI, randJ;
            for (int i = 0; i < NumberOfBombs; i++)
            {
                do
                {
                    randI = getRandomNumber(this.Count);
                    randJ = getRandomNumber(this[0].Count);
                } while (this[randI][randJ].IconName == "BOMB");
                this[randI][randJ].IconName = "BOMB";
            }
        }

        private int getRandomNumber(int limit)
        {
            return Rand.Next(limit);
        }

        private void InitializeMatrix(int mx_hg, int mx_wid)
        {
            for (int i = 0; i < mx_hg; i++)
            {
                this.Add(new List<Box>());
                for (int j = 0; j < mx_wid; j++)
                {
                    this[i].Add(new Box("CLOSED", i, j));
                }
            }
        }

        private int CountBombsAround(Box box)
        {
            int count = 0;
            for (int i = box.Y - 1; i <= box.Y + 1; i++)
                for (int j = box.X - 1; j <= box.X + 1; j++)
                    if (i > -1 && i < this.Count && j > -1 && j < this[0].Count)
                        if (this[i][j].IconName == "BOMB") count++;
            return count;
        }

        private void SetUpNumbers()
        {
            for (int i = 0; i < this.Count; i++)
                for (int j = 0; j < this[i].Count; j++)
                {
                    if (this[i][j].IconName != "BOMB")
                    {
                        int count = CountBombsAround(this[i][j]);
                        if (count == 0)
                            this[i][j].IconName = "ZERO";
                        else
                            this[i][j].IconName = "NUM" + count.ToString();
                    }
                }
        }

        public int getNumBombs()
        {
            return this.NumberOfBombs;
        }


        public GameMatrix() { }

        //constructor for upperlevel matrix
        public GameMatrix(int mx_hg, int mx_wid) : this()
        {
            InitializeMatrix(mx_hg, mx_wid);
        }

        //Constructor for lower levelmatrix
        public GameMatrix(int mx_hg, int mx_wid, int numOfBombs) : this(mx_hg, mx_wid)
        {
            Rand = new Random();
            NumberOfBombs = numOfBombs;
            SetUpBombs();
            SetUpNumbers();
        }

    }

}

