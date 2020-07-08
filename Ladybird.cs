using AI_Assignment;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;

namespace AI_Assignment_1514433
{
    class Ladybird : Insect
    {
        public Ladybird(int positionX, int positionY)
        {
            base.PositionX = positionX;
            base.PositionY = positionY;
            base.TimeStepAge = 0;
            base.FoodCount = 0;
            base.StarveCount = 0;
            base.Text = "X";
            base.Eaten = false;
        }
      
        /// <summary>
        /// Update the move positions of the insect
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="insects"></param>
        public override void UpdateMovePositions(Cell[,] grid, List<Insect> insects)
        {
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
                // Check for surrounding greenflies to eat
                if (grid[TopPosX, TopPosY].CellStatus == GreenflyCell)
                {
                    CanMoveUp = true;
                    CanEatUp = true;
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
                // Check for surrounding greenflies to eat
                if (grid[RightPosX, RightPosY].CellStatus == GreenflyCell)
                {
                    CanMoveRight = true;
                    CanEatRight = true;
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
                // Check for surrounding greenflies to eat
                if (grid[DownPosX, DownPosY].CellStatus == GreenflyCell)
                {
                    CanMoveDown = true;
                    CanEatDown = true;
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
                // Check for surrounding greenflies to eat
                if (grid[LeftPosX, LeftPosY].CellStatus == GreenflyCell)
                {
                    CanMoveLeft = true;
                    CanEatLeft = true;
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
        /// <summary>
        /// Perform prior checks before the move 
        /// </summary>
        public override void NextTimeStep()
        {
            // Store the previous insect position
            PreviousPositionX = PositionX;
            PreviousPositionY = PositionY;

            // Increment the age
            TimeStepAge++;

            // Check if the insect cannot move
            if (CanMoveUp == false && CanMoveRight == false && CanMoveDown == false && CanMoveLeft == false &&
                CanEatUp == false && CanEatRight == false && CanEatDown == false && CanEatLeft == false)
            {
                // Return and do not move the insect
                return;
            }

            // Check if the insect should eat
            if (CanEatUp == true || CanEatRight == true || CanEatDown == true || CanEatLeft == true)
            {
                Eat();
                StarveCount = 0;
            }
            // Check if the insect should move
            else if (CanMoveUp == true || CanMoveRight == true || CanMoveDown == true || CanMoveLeft == true)
            {
                Move();
                StarveCount++;
            }
        }
        /// <summary>
       /// Move the insect
       /// </summary>
        public override void Move()
        {
            var random = new Random();
            var hasMoved = false;

            do
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        if (CanMoveUp)
                        {
                            PositionX = TopPosX;
                            PositionY = TopPosY;
                            hasMoved = true;
                        }

                        break;
                    case 1:
                        if (CanMoveRight)
                        {
                            PositionX = RightPosX;
                            PositionY = RightPosY;
                            hasMoved = true;
                        }

                        break;
                    case 2:
                        if (CanMoveDown)
                        {
                            PositionX = DownPosX;
                            PositionY = DownPosY;
                            hasMoved = true;
                        }

                        break;
                    case 3:
                        if (CanMoveLeft)
                        {
                            PositionX = LeftPosX;
                            PositionY = LeftPosY;
                            hasMoved = true;
                        }

                        break;
                }

            } while (hasMoved == false);
        }
        /// <summary>
        /// Breed the insect
        /// </summary>
        /// <returns></returns>
        public override Insect Breed()
        {
            TimeStepAge = 0;

            var random = new Random();
            var hasBred = false;

            do
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        if (CanBreedUp)
                        {
                            hasBred = true;
                            return new Ladybird(TopPosX, TopPosY);
                        }

                        break;
                    case 1:
                        if (CanBreedRight)
                        {
                            hasBred = true;
                            return new Ladybird(RightPosX, RightPosY);
                        }

                        break;
                    case 2:
                        if (CanBreedDown)
                        {
                            hasBred = true;
                            return new Ladybird(DownPosX, DownPosY);
                        }

                        break;
                    case 3:
                        if (CanBreedLeft)
                        {
                            hasBred = true;
                            return new Ladybird(LeftPosX, LeftPosY);
                        }

                        break;
                }

            } while (hasBred == false);

            Debug.WriteLine("Error in Ladybird Breed Method");
            return null;
        }
       /// <summary>
       /// Move the insect to a position where another insect exists 
       /// </summary>
        public void Eat()
        {
            var random = new Random();
            var hasMoved = false;

            do
            {
                switch (random.Next(0, 4))
                {
                    case 0:
                        if (CanEatUp)
                        {
                            if (TopInsect == null)
                            {

                                return;
                            }

                            PositionX = TopPosX;
                            PositionY = TopPosY;
                            base.TopInsect.Eaten = true;
                            hasMoved = true;
                        }

                        break;
                    case 1:
                        if (CanEatRight)
                        {
                            if (RightInsect == null)
                            {
                                return;
                            }

                            PositionX = RightPosX;
                            PositionY = RightPosY;
                            base.RightInsect.Eaten = true;
                            hasMoved = true;
                        }

                        break;
                    case 2:
                        if (CanEatDown)
                        {
                            if (DownInsect == null)
                            {
                                return;
                            }

                            PositionX = DownPosX;
                            PositionY = DownPosY;
                            base.DownInsect.Eaten = true;
                            hasMoved = true;
                        }

                        break;
                    case 3:
                        if (CanEatLeft)
                        {
                            if (LeftInsect == null)
                            {
                                return;
                            }

                            PositionX = LeftPosX;
                            PositionY = LeftPosY;
                            base.LeftInsect.Eaten = true;
                            hasMoved = true;
                        }

                        break;
                }

            } while (hasMoved == false);
        }
    }
}
