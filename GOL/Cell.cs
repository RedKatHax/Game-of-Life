using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{

    public class Cell
    {
        public bool isAlive;
        //public int GensLived;
        public int xCoord;
        public int yCoord;

        public static int GetXCoords(Cell cell)
        {
            Cell temp = cell;
            int x = temp.xCoord;
            return x;
        }
        public static Cell setCell(Cell cell, int x, int y, bool Alive)
        {
            Cell temp = cell;
            temp.isAlive = Alive;
            temp.xCoord = x;
            temp.yCoord = y;
          
            return temp;
        }
        public static int GetYCoords(Cell cell)
        {
            Cell temp = cell;
            int y = temp.yCoord;
            return y;
        }

        public static bool GetALife(Cell cell)
        {
            Cell temp = cell;
            bool aLife = temp.isAlive;
            return aLife;
        }

        public static int CountNeighborsFinite(Cell cell, Cell[,] universe)
        {
            Cell temp = cell;
            int x = temp.xCoord;
            int y = temp.yCoord;

            int count = 0;

            int xLen = universe.GetLength(0);

            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {

                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if ((xOffset == 0) && (yOffset == 0))
                    {
                        continue;
                    }
                    if (xCheck < 0)
                    {
                        continue;
                    }

                    if (yCheck < 0)
                    {
                        continue;
                    }
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    if (yCheck >= yLen)
                    {
                        continue;
                    }

                    Cell tempCell = universe[xCheck, yCheck];
                    bool tempBool = tempCell.isAlive;
                    if (tempBool == true)
                    {
                        count++;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return count;
        }

        public static int CountNeighborsToroidal(Cell cell, Cell[,] universe)
        {
            Cell temp = cell;
            int x = temp.xCoord;
            int y = temp.yCoord;
            int count = 0;

            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {

                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {

                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if ((xOffset == 0) && (yOffset == 0))
                    {
                        continue;
                    }

                    if (xCheck < 0)
                    {
                        xCheck = ((xLen)-1);
                    }

                    if (yCheck < 0)
                    {
                        yCheck = ((yLen)-1);
                        
                    }

                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                        
                    }
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                        
                    }

                    Cell tempCell = universe[xCheck, yCheck];
                    bool tempBool = tempCell.isAlive;
                    if (tempBool == true)
                    {
                        count++;
                    }

                    else
                    {
                        continue;
                    }
                }
            }
            return count;

        }

        public static bool checkNextGen(Cell cell, int count)
        {
            bool willLive = true;
            Cell temp = cell;
            int tempCount = count;
            if (temp.isAlive == true)
            {
                if ((tempCount < 2) || (tempCount > 3))
                {
                    willLive = false;
                    return willLive;
                }
                if ((tempCount == 2) || (tempCount == 3))
                {
                    willLive = true;
                    return willLive;

                }
            }

            if (temp.isAlive == false)
            {
                if (tempCount == 3)
                {
                    willLive = true;
                    return willLive;
                }
                else
                {
                    willLive = false;
                    return willLive;
                }
            }
            return willLive;
        }
    }

}    

    



