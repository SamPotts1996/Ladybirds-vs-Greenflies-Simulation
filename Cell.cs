using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Assignment_1514433
{
    public class Cell
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string Text { get; set; }

        public int EmptyCell = 1, LadybirdCell = 2, GreenflyCell = 3;

        // Upon the update to the Cell Status, update the text of the cell
        private int _cellStatus;
        public int CellStatus
        {
            get => _cellStatus;
            set
            {   
                _cellStatus = value;

                if (CellStatus == EmptyCell)
                    Text = "#";
                else if (CellStatus == LadybirdCell)
                    Text = "X";
                else if (CellStatus == GreenflyCell)
                    Text = "O";
            }
        }

        public Cell(int xPosition, int yPosition, int cellStatus)
        {
            this.XPosition = xPosition;
            this.YPosition = yPosition;

            this.CellStatus = cellStatus;
        }
    }
}
