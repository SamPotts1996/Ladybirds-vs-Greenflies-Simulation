using AI_Assignment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AI_Assignment_1514433
{
    class Greenfly : Insect
    {
        public Greenfly(int positionX, int positionY)
        {
            base.PositionX = positionX;
            base.PositionY = positionY;
            base.TimeStepAge = 0;
            base.Text = "O";
            base.Eaten = false;
        }
        
        /// <summary>
        /// Breed the insect
        /// </summary>
        /// <returns></returns>
        public override Insect Breed()
        {
            // Used to keep attempting the breed command, return is used to break the loop
            var hasBred = false;
            var random = new Random();
   
            do
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        if (CanBreedUp)
                        {
                            // Breed the insect upwards
                           var newInsect = new Greenfly(TopPosX, TopPosY);
                            return newInsect;
                      
                        }
                        break;
                    case 1:
                        if (CanBreedRight)
                        {
                            // Breed the insect right
                            var newInsect = new Greenfly(RightPosX, RightPosY);
                            return newInsect;
                        }
                        break;
                    case 2:
                        if (CanBreedDown)
                        {
                            // Breed the insect downwards
                            var newInsect = new Greenfly(DownPosX, DownPosY);
                            return newInsect;
                        }
                        break;
                    case 3:
                        if (CanBreedLeft)
                        {
                            // Breed the insect left
                           var newInsect = new Greenfly(LeftPosX, LeftPosY);
                            return newInsect;
                           
                        }
                        break;
                }

            } while (hasBred == false);


            return null;


        }
        /// <summary>
        /// Moves the Insect
        /// </summary>
        public override void Move()
        {
            var hasMoved = false;
            var random = new Random();
           
            do {

                switch (random.Next(0,4)) 
                {
                    case 0:
                        if (CanMoveUp) 
                        {
                            // Move to the top position
                            PositionX = TopPosX;
                            PositionY = TopPosY;
                            hasMoved = true;
                        }
                        break;
                    case 1:
                        if (CanMoveRight) 
                        {
                            // Move to the right position
                            PositionX = RightPosX;
                            PositionY = RightPosY;
                            hasMoved = true;
                        }
                        break;
                    case 2:
                        if (CanMoveDown) 
                        {
                            // Move to the down position
                            PositionX = DownPosX;
                            PositionY = DownPosY;
                            hasMoved = true;
                        }
                        break;
                    case 3:
                        if (CanMoveLeft) 
                        {
                            // Move to the left position
                            PositionX = LeftPosX;
                            PositionY = LeftPosY;
                            hasMoved = true;
                        }
                        break;
                }

            } while (hasMoved == false);
        }
        /// <summary>
        /// Perform prior checks before moving
        /// </summary>
        public override void NextTimeStep()
        {
            // Store the previous position of the insect
            PreviousPositionX = PositionX;
            PreviousPositionY = PositionY;

            // Increment the age of the insect
            TimeStepAge++;

            // Check if the insect can move
            if (CanMoveUp == false && CanMoveRight == false && CanMoveDown == false && CanMoveLeft == false) 
            {
                // Don't move insect
                return;
            }

            // Move the insect
            Move();
        }
        /// <summary>
        /// Update the move mositions of the object
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="insects"></param>
        public override void UpdateMovePositions(Cell[,] grid, List<Insect> insects)
        {
            if (PositionX == 0 && PositionY == 0)
            {

            }


            #region Set the moveable positions

            // Top Coordinate
            base.TopPosX = base.PositionX;
            base.TopPosY = base.PositionY - 1;

            // Right Coordinate
            base.RightPosX = base.PositionX + 1;
            base.RightPosY = base.PositionY;

            // Down Coordinate
            base.DownPosX = base.PositionX;
            base.DownPosY = base.PositionY + 1;

            // Left Coordinate
            base.LeftPosX = base.PositionX - 1;
            base.LeftPosY = base.PositionY;

            #endregion

            #region Reset the flags

            // Reset the border flags
            IsTopBorder = false;
            IsRightBorder = false;
            IsDownBorder = false;
            IsLeftBorder = false;

            // Reset the Insect flags
            IsTopInsect = false;
            IsRightInsect = false;
            IsDownInsect = false;
            IsLeftInsect = false;

            TopInsect = null;
            RightInsect = null;
            DownInsect = null;
            LeftInsect = null;

            // Reset eat flags
            CanEatUp = false;
            CanEatRight = false;
            CanEatDown = false;
            CanEatLeft = false;

            // Reset the Breed flags
            CanBreedUp = false;
            CanBreedRight = false;
            CanBreedDown = false;
            CanBreedLeft = false;

            // Reset the Move flags
            CanMoveUp = false;
            CanMoveRight = false;
            CanMoveDown = false;
            CanMoveLeft = false;

            #endregion

            #region Check if the insect Could move off the grid

            if (TopPosX > 19 || TopPosX < 0 || TopPosY > 19 || TopPosY < 0)
            {
                CanMoveUp = false;
                IsTopBorder = true;
            }

            if (RightPosX > 19 || RightPosX < 0 || RightPosY > 19 || RightPosY < 0)
            {
                CanMoveRight = false;
                IsRightBorder = true;
            }

            if (DownPosX > 19 || DownPosX < 0 || DownPosY > 19 || DownPosY < 0)
            {
                CanMoveDown = false;
                IsDownBorder = true;
            }

            if (LeftPosX > 19 || LeftPosX < 0 || LeftPosY > 19 || LeftPosY < 0)
            {
                CanMoveLeft = false;
                IsLeftBorder = true;
            }

            #endregion

            #region Check surrounding cells

            if (IsTopBorder == false)
            {
                // Check for surrounding Ladybirds
                if (grid[TopPosX, TopPosY].CellStatus == LadybirdCell)
                {
                    CanMoveUp = false;
                    IsTopInsect = true;
                }
                // Check for surrounding Greenflies
                if (grid[TopPosX, TopPosY].CellStatus == GreenflyCell)
                {
                    CanMoveUp = false;
                    IsTopInsect = true;
                }
                // Check if any surrounding cells are empty
                if (grid[TopPosX, TopPosY].CellStatus == EmptyCell)
                {
                    CanMoveUp = true;
                    CanBreedUp = true;
                    IsTopInsect = false;
                }
            }

            if (IsRightBorder == false)
            {
                // Check for surrounding Ladybirds
                if (grid[RightPosX, RightPosY].CellStatus == LadybirdCell)
                {
                    CanMoveRight = false;
                    IsRightInsect = true;
                }
                // Check for surrounding Greenflies
                if (grid[RightPosX, RightPosY].CellStatus == GreenflyCell)
                {
                    CanMoveRight = false;
                    IsRightInsect = true;
                }
                // Check if any surrounding cells are empty
                if (grid[RightPosX, RightPosY].CellStatus == EmptyCell)
                {
                    CanMoveRight = true;
                    CanBreedRight = true;
                    IsRightInsect = false;
                }
            }

            if (IsDownBorder == false)
            {
                // Check for surrounding Ladybirds
                if (grid[DownPosX, DownPosY].CellStatus == LadybirdCell)
                {
                    CanMoveDown = false;
                    IsDownInsect = true;
                }
                // Check for surrounding Greenflies
                if (grid[DownPosX, DownPosY].CellStatus == GreenflyCell)
                {
                    CanMoveDown = false;
                    IsDownInsect = true;
                }
                // Check if any surrounding cells are empty
                if (grid[DownPosX, DownPosY].CellStatus == EmptyCell)
                {
                    CanMoveDown = true;
                    CanBreedDown = true;
                    IsDownInsect = false;
                }
            }

            if (IsLeftBorder == false)
            {
                // Check for surrounding Ladybirds
                if (grid[LeftPosX, LeftPosY].CellStatus == LadybirdCell)
                {
                    CanMoveLeft = false;
                    IsLeftInsect = true;
                }
                // Check for surrounding Greenflies
                if (grid[LeftPosX, LeftPosY].CellStatus == GreenflyCell)
                {
                    CanMoveLeft = false;
                    IsLeftInsect = true;
                }
                // Check if any surrounding cells are empty
                if (grid[LeftPosX, LeftPosY].CellStatus == EmptyCell)
                {
                    CanMoveLeft = true;
                    CanBreedLeft = true;
                    IsLeftInsect = false;
                }
            }

            #endregion

            #region Get the surrounding insects

            foreach (var insect in insects)
            {
                if (IsTopInsect)
                {
                    if (insect.PositionX == TopPosX && insect.PositionY == TopPosY)
                    {
                        TopInsect = insect;
                    }
                }
                if (IsRightInsect)
                {
                    if (insect.PositionX == RightPosX && insect.PositionY == RightPosY)
                    {
                        RightInsect = insect;
                    }
                }
                if (IsDownInsect)
                {
                    if (insect.PositionX == DownPosX && insect.PositionY == DownPosY)
                    {
                        DownInsect = insect;
                    }
                }
                if (IsLeftInsect)
                {
                    if (insect.PositionX == LeftPosX && insect.PositionY == LeftPosY)
                    {
                        LeftInsect = insect;
                    }
                }
            }

            #endregion
        }
    }
}
