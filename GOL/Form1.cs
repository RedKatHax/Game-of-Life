using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Rectangle = System.Drawing.Rectangle;

namespace GOL
{
    public partial class Form1 : Form
    {
        // The universe array
        static int sizeX = 50;
        static int sizeY = 50;
        Cell[,] universe = new Cell[50, 50];
        Cell[,] scratchPad = new Cell[50, 50]; 
        //if uniChoice == 0, universe == finite; 1 == universe == torodial;
        int uniChoice;
        //hudSelect == 0, hud off; hudSelect == 1, hud on


        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Purple;
        Color BackColor = Properties.Settings.Default.deadCell;

        // The Timer class
        Timer timer = new Timer();

        // HUD count and Settings Selection
        int generations = 0;
        int cellCount = 0;
        int gridSelect = 1; //default grid is displayed, 0 == off 
        int hudSelect = 1; //default hud is displayed, 0 == off
        int displayNeighbor = 1; //default neighbor count displayed in cell, 0 == off

        public Form1()
        {
            
            InitializeComponent();
            sizeX = Properties.Settings.Default.universeX;
            sizeY = Properties.Settings.Default.universeY;
            hudSelect = Properties.Settings.Default.hudSelect;
            gridSelect = Properties.Settings.Default.gridSelect;
            uniChoice = Properties.Settings.Default.uniChoice;
            graphicsPanel1.BackColor = Properties.Settings.Default.deadCell;
            cellColor = Properties.Settings.Default.liveCell;
            displayNeighbor = Properties.Settings.Default.displayNeighbor;
            //graphicsPanel1.BackColor = Properties.Settings.Default.deadCell;
            // Setup the timer
            timer.Interval = Properties.Settings.Default.interval; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
            newUniverse();

        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {
            int universeType = uniChoice;
            cellCount = 0;

            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                   
                    if (universeType == 0)
                    {
                        Cell temp = universe[(int)x, (int)y];
                        Cell newTemp = new Cell();
                        int count = Cell.CountNeighborsFinite(temp, universe);
                        bool Alive = Cell.checkNextGen(temp, count);
                        if (Alive == true)
                        {
                            cellCount++;
                        }
                        Cell.setCell(newTemp, (int)x, (int)y, Alive);
                        scratchPad[(int)x, (int)y] = newTemp;
                        toolStripStatusUniverse.Text = "Finite";
                    }
                    if (uniChoice == 1)
                    {
                        Cell temp = universe[(int)x, (int)y];
                        Cell newTemp = new Cell();
                        int count = Cell.CountNeighborsToroidal(temp, universe);
                        bool Alive = Cell.checkNextGen(temp, count);
                        if (Alive == true)
                        {
                            cellCount++;
                        }
                        Cell.setCell(newTemp, (int)x, (int)y, Alive);
                        scratchPad[(int)x, (int)y] = newTemp;
                        toolStripStatusUniverse.Text = "Torodial";
                    }
                }
            }
            generations++;
            toolStripStatusInterval.Text = "Interval = " + timer.Interval.ToString() + " ms";
            toolStripStatusCell.Text = "Cell Count = " + cellCount.ToString();
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            ArraySwap();
            graphicsPanel1.Invalidate();
        }
        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void ArraySwap()
        {
            Cell[,] tempUniverse = universe;
            universe = scratchPad;
            scratchPad = tempUniverse;
        }
        public void newUniverse()
        {
            //initilizes blank universe
            int sizeX = Properties.Settings.Default.universeX;
            int sizeY = Properties.Settings.Default.universeY;
            universe = new Cell[sizeX, sizeY];
            scratchPad = new Cell[sizeX, sizeY];

            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                    Cell temp = new Cell();
                    Cell.setCell(temp, (int)x, (int)y, false);

                    universe[(int)x, (int)y] = temp;
                }
            }
            timer.Enabled = false;
            int generations = 0;
            int cellCount = 0;
            if (uniChoice == 0)
            {
                toolStripStatusUniverse.Text = "Finite";
            }
            if (uniChoice == 1)
            {
                toolStripStatusUniverse.Text = "Torodial";
            }
            toolStripStatusInterval.Text = "Interval = " + timer.Interval.ToString() + " ms";
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        public void Randomize()
        {
            Random rand = new Random();

            cellCount = 0;

            bool randomizeLife(int min, int max)
            {
                int randNum = rand.Next(min, max);

                if (randNum == 0)
                {
                    bool randAlive = true;
                    return randAlive;
                }
                else
                {
                    bool randAlive = false;
                    return randAlive;
                }

            }

            for (float y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (float x = 0; x < universe.GetLength(0); x++)
                {
                    //set temp Cell values = to current cell at (x,y)
                    Cell temp = new Cell();

                    //If randomizeLife == 0, isAlive == true
                    bool isAlive = randomizeLife(0, 3);
                    Cell.setCell(temp, (int)x, (int)y, isAlive);
                    if (isAlive == true)
                    {
                        //increase count by 1 to updated HUD
                        cellCount++;
                    }
                    universe[(int)x, (int)y] = temp;
                }
            }

            timer.Enabled = false;
            int generations = 0;
            toolStripStatusCell.Text = "Cell Count = " + cellCount.ToString();
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {

            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)(graphicsPanel1.ClientSize.Width) / (float)(universe.GetLength(0));
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)(graphicsPanel1.ClientSize.Height) / (float)(universe.GetLength(1));

            int xMax = universe.GetLength(0);
            int yMax = universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);
            Pen thickPen = new Pen(gridColor, 2);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            //check whether to display grid 
            int gridOpt = gridSelect;
            int hudOn = hudSelect;
            int NumsOn = displayNeighbor;
            if (hudOn == 0)
            {
                statusStrip1.Visible = false;
            }

            if (hudOn == 1)
            {
                statusStrip1.Visible = true;
            }

            if (NumsOn == 1)
            {

                for (float y = 0; y <= universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (float x = 0; x <= universe.GetLength(0); x++)
                    {
                        if ((x == universe.GetLength(0)) || (y == universe.GetLength(1)))
                        {
                            continue;
                        }

                        else
                        {
                            Cell temp = universe[(int)x, (int)y];
                            RectangleF cellRect = Rectangle.Empty;

                            cellRect.X = (x * cellWidth);
                            cellRect.Y = (y * cellHeight);
                            cellRect.Width = cellWidth;
                            cellRect.Height = cellHeight;


                            if ((temp.isAlive == true))
                            {
                                Font font = new Font("Calibri", 10f, FontStyle.Bold);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;
                                RectangleF rect = new RectangleF(cellRect.X, cellRect.Y, cellWidth, cellHeight);

                                if (uniChoice == 1)
                                {
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                    int neighbors = Cell.CountNeighborsToroidal(temp, universe);
                                    e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);

                                }
                                if (uniChoice == 0)
                                {
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                    int neighbors = Cell.CountNeighborsFinite(temp, universe);
                                    e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);
                                  
                                }
                            }

                        }
                    }
                }
            }

            if (NumsOn == 0)
            {
                for (float y = 0; y <= universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (float x = 0; x <= universe.GetLength(0); x++)
                    {
                        if ((x == universe.GetLength(0)) || (y == universe.GetLength(1)))
                        {
                            continue;
                        }

                        else
                        {
                            Cell temp = universe[(int)x, (int)y];
                            RectangleF cellRect = Rectangle.Empty;

                            cellRect.X = (x * cellWidth);
                            cellRect.Y = (y * cellHeight);
                            cellRect.Width = cellWidth;
                            cellRect.Height = cellHeight;


                            if ((temp.isAlive == true))
                            {


                                if (uniChoice == 1)
                                {
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                    int neighbors = Cell.CountNeighborsToroidal(temp, universe);


                                }
                                if (uniChoice == 0)
                                {
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                    int neighbors = Cell.CountNeighborsFinite(temp, universe);


                                }
                            }

                        }
                    }
                }
            }
                if (gridOpt == 1)
            {
                // Iterate through the universe in the y, top to bottom
                for (float y = 0; y <= universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (float x = 0; x <= universe.GetLength(0); x++)
                    {
                        float lineX = (x * cellWidth);
                        float lineY = (y * cellHeight);

                        if ((x % 10 == 0))
                        {
                            e.Graphics.DrawLine(thickPen, lineX, lineY, lineX, yMax);
                            e.Graphics.DrawLine(gridPen, lineX, lineY, xMax, lineY);

                        }

                        if ((y % 10 == 0))
                        {
                            e.Graphics.DrawLine(thickPen, lineX, lineY, xMax, lineY);
                            e.Graphics.DrawLine(gridPen, lineX, lineY, lineX, yMax);

                        }

                        else
                        {
                            e.Graphics.DrawLine(gridPen, lineX, lineY, lineX, yMax);
                            e.Graphics.DrawLine(gridPen, lineX, lineY, xMax, lineY);
                        }
                    }
                }
            }
        
            if (gridOpt == 0)
            {
                for (float y = 0; y <= universe.GetLength(1); y++)
                {
                    // Iterate through the universe in the x, left to right
                    for (float x = 0; x <= universe.GetLength(0); x++)
                    {
                        if ((x == universe.GetLength(0)) || (y == universe.GetLength(1)))
                        {
                            continue;
                        }
                        else
                        {

                            RectangleF cellRect = Rectangle.Empty;

                            cellRect.X = (x * cellWidth);
                            cellRect.Y = (y * cellHeight);
                            cellRect.Width = cellWidth;
                            cellRect.Height = cellHeight;
                            Cell temp = universe[(int)x, (int)y];
                            if (temp.isAlive == true) 
                            {
                                Font font = new Font("Calibri", 10f, FontStyle.Bold);

                                StringFormat stringFormat = new StringFormat();
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Center;
                                RectangleF rect = new RectangleF(cellRect.X, cellRect.Y, cellWidth, cellHeight);

                                if (uniChoice == 1)
                                {
                                    int neighbors = Cell.CountNeighborsToroidal(temp, universe);
                                    e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                }
                                if (uniChoice == 0)
                                {
                                    int neighbors = Cell.CountNeighborsFinite(temp, universe);
                                    e.Graphics.DrawString(neighbors.ToString(), font, Brushes.Black, rect, stringFormat);
                                    e.Graphics.FillRectangle(cellBrush, cellRect);
                                }


                            }
                        }

                    }
                }
                
                // Cleaning up pens and brushes
                gridPen.Dispose();
                thickPen.Dispose();
                cellBrush.Dispose();
            }
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)(graphicsPanel1.ClientSize.Width) / (float)(universe.GetLength(0));
                float cellHeight = (float)(graphicsPanel1.ClientSize.Height) / (float)(universe.GetLength(1));

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = (float)(e.X) / cellWidth;

                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = (float)(e.Y) / cellHeight;

                // Toggle the cell's state
                Cell temp = universe[(int)(x), (int)(y)];
                temp.isAlive = (!(temp.isAlive));

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void playToolStripButton1_Click(object sender, EventArgs e)
        {
            //start universe tool strip button 
            timer.Enabled = true;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Next toolstrip button 
            timer.Enabled = false;
            NextGeneration();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            //New universe tool strip button 
            generations = 0;
            cellCount = 0;
            newUniverse();
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //Erase universe tool strip button 
            timer.Enabled = false;
            generations = 0;
            cellCount = 0;
            newUniverse();
        }

        private void pauseToolStripButton1_Click(object sender, EventArgs e)
        {
            //Pause timer tool strip button 
            if (timer.Enabled == true)
            {
                timer.Enabled = false;
            }
            if (timer.Enabled == false)
            {
                timer.Enabled = false;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            //Randomize universe tool strip button 
            timer.Enabled = false;
            generations = 0;
            Randomize();
        }


        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dialog box to set cellColor
            ColorDialog dlg = new ColorDialog();

            dlg.Color = cellColor;
            Properties.Settings.Default.liveCell = cellColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
                Properties.Settings.Default.liveCell = cellColor;
                graphicsPanel1.Invalidate();
            }
           
        }

        private void backgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dialog box to set gridColor
            ColorDialog dlg = new ColorDialog();

            dlg.Color = gridColor;
            Properties.Settings.Default.gridColor = dlg.Color;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
                Properties.Settings.Default.gridColor = gridColor;
                graphicsPanel1.Invalidate();
            }
        }

        private void widthsetXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Modal Dialog box to set universe size 
            ModalDialog dlg = new ModalDialog();

            //sets width (x) of universe
            dlg.setX = sizeX;
            //sets height (y) of universe
            dlg.setY = sizeY;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                
                sizeX = dlg.setX;
                sizeY = dlg.setY;
                //reset universe and scratchpad size to user selection 
                universe = new Cell[sizeX, sizeY];
                scratchPad = new Cell[sizeX, sizeY];
                //initialize new universe with user settings 
                newUniverse();
                //update HUD 
                toolStripStatusUniSize.Text = "Universe Size = " + sizeX.ToString() + "," + sizeY.ToString();
                graphicsPanel1.Invalidate();
            }
        }

        private void deadCellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dialog box to set background/dead cell color
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;
            Properties.Settings.Default.deadCell = dlg.Color;
                
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
                Color BackColor = dlg.Color;
                Properties.Settings.Default.deadCell = BackColor;
                graphicsPanel1.Invalidate();
            }
        }

        private void torodialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //selects Torodial universe 
            uniChoice = 1;
            Properties.Settings.Default.uniChoice = uniChoice;
            toolStripStatusUniverse.Text = "Torodial";
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //selects Finite universe 
            uniChoice = 0;
            Properties.Settings.Default.uniChoice = uniChoice;
            toolStripStatusUniverse.Text = "Finite";
            graphicsPanel1.Invalidate();
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridSelect = 0;
            Properties.Settings.Default.gridSelect = gridSelect;
            graphicsPanel1.Invalidate();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridSelect = 1;
            Properties.Settings.Default.gridSelect = gridSelect;
            graphicsPanel1.Invalidate();
        }

        private void hudOffToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hudSelect = 0;
            Properties.Settings.Default.hudSelect = hudSelect;
            graphicsPanel1.Invalidate();
        }

        private void hudOnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hudSelect = 1;
            Properties.Settings.Default.hudSelect = hudSelect;
            graphicsPanel1.Invalidate();
        }

        private void reloadOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();

            timer.Interval = Properties.Settings.Default.interval;
            sizeX = Properties.Settings.Default.universeX;
            sizeY = Properties.Settings.Default.universeY;
            hudSelect = Properties.Settings.Default.hudSelect;
            gridSelect = Properties.Settings.Default.gridSelect;
            uniChoice = Properties.Settings.Default.uniChoice;
            graphicsPanel1.BackColor = Properties.Settings.Default.deadCell;
            gridColor = Properties.Settings.Default.gridColor;
            cellColor = Properties.Settings.Default.liveCell;
            displayNeighbor = Properties.Settings.Default.displayNeighbor;
            toolStripStatusInterval.Text = "Interval = " + timer.Interval.ToString() + " ms";

            if (uniChoice == 0)
            {
                toolStripStatusUniverse.Text = "Finite";
            }
            if (uniChoice == 1)
            {
                toolStripStatusUniverse.Text = "Torodial";
            }
            newUniverse();
            graphicsPanel1.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.universeX = sizeX;
            Properties.Settings.Default.universeY = sizeY;
            Properties.Settings.Default.hudSelect = hudSelect;
            Properties.Settings.Default.gridSelect = gridSelect;
            Properties.Settings.Default.uniChoice = uniChoice;
            Properties.Settings.Default.deadCell = graphicsPanel1.BackColor;
            Properties.Settings.Default.liveCell = cellColor;
            Properties.Settings.Default.interval = timer.Interval;
            Properties.Settings.Default.gridColor = gridColor;
            Properties.Settings.Default.displayNeighbor = displayNeighbor;
            Properties.Settings.Default.Save();
        }

        private void resetOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            timer.Interval = Properties.Settings.Default.interval;
            sizeX = Properties.Settings.Default.universeX;
            sizeY = Properties.Settings.Default.universeY;
            hudSelect = Properties.Settings.Default.hudSelect;
            gridSelect = Properties.Settings.Default.gridSelect;
            uniChoice = Properties.Settings.Default.uniChoice;
            graphicsPanel1.BackColor = Properties.Settings.Default.deadCell;
            cellColor = Properties.Settings.Default.liveCell;
            gridColor = Properties.Settings.Default.gridColor;
            displayNeighbor = Properties.Settings.Default.displayNeighbor;

            toolStripStatusInterval.Text = "Interval = " + timer.Interval.ToString() + " ms";

            if (uniChoice == 0)
            {
                toolStripStatusUniverse.Text = "Finite";
            }
            if (uniChoice == 1)
            {
                toolStripStatusUniverse.Text = "Torodial";
            }
            newUniverse();
            graphicsPanel1.Invalidate();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first. Prefix with !
                writer.WriteLine("!Start");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    StringBuilder currentRow = new StringBuilder();

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        //set temp Cell values = to current cell at (x,y)
                        Cell temp = universe[(int)x, (int)y];
                        
                        //Check if cell located at x,y is alive
                        if (temp.isAlive == true)
                        {
                            // If the universe[x,y] is alive then append 'O' (capital O) to row
                            currentRow.Append('O');
                        }

                        // Else if the universe[x,y] is dead then append '.' (period) to row
                        if (temp.isAlive == false)
                        {
                            currentRow.Append('.');
                        }
                    }
                    //Write current row to file 
                    writer.WriteLine(currentRow);

                }

                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    char checkChar = (char)row[0];
                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (checkChar.Equals('!'))
                    {
                        continue;
                    }
                    if ((checkChar.Equals('O')) || (checkChar.Equals('.')))
                    {
                        // If the row is not a comment then it is a row of cells.
                        // Increment the maxHeight variable for each row read.

                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;


                   
                }
                sizeX = maxWidth;
                sizeY = maxHeight;
                universe = new Cell[sizeX, sizeY];
                scratchPad = new Cell[sizeX, sizeY];

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                int y = 0;
                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();
                    char checkChar = (char)row[0];
                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    if (checkChar.Equals('!'))
                    {
                        continue;
                    }
                    else
                    {                                      
                        //while(y<maxHeight)
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {

                            char cellCheck = row[xPos];
                            if (cellCheck.Equals('O'))
                            {
                                //CELL IS ALIVE
                                Cell temp = new Cell();
                                Cell.setCell(temp, xPos, y, true);

                                universe[(int)xPos, (int)y] = temp;
                                      
                            }
                            if (cellCheck.Equals('.'))
                            {
                                //cell is dead
                                Cell temp = new Cell();
                                Cell.setCell(temp, xPos, y, false);

                                universe[(int)xPos, (int)y] = temp;
                            }
                        }

                        y++;
                    }                       
                       
                }

                // Close the file.
                reader.Close();
                graphicsPanel1.Invalidate();
            }
        }

        private void setIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dlg = new Form2();

            //dlg.SetNumber(number);
            dlg.setInterval = timer.Interval;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                //number = dlg.GetNumber()
                timer.Interval = dlg.setInterval;
                Properties.Settings.Default.interval = timer.Interval;
                toolStripStatusInterval.Text = "Interval = " + timer.Interval.ToString() + " ms";
            }
        }

        private void onToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            displayNeighbor = 1;
            Properties.Settings.Default.displayNeighbor = displayNeighbor;
            graphicsPanel1.Invalidate();
        }

        private void offToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            displayNeighbor = 0;
            Properties.Settings.Default.displayNeighbor = displayNeighbor;
            graphicsPanel1.Invalidate();
        }
    }
}
