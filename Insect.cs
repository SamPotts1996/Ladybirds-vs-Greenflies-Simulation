using AI_Assignment_1514433;
using System;
using System.Collections.Generic;
using System.Text;

namespace AI_Assignment
{
    public abstract class Insect
    {
        public const int EmptyCell = 1, LadybirdCell = 2, GreenflyCell = 3;

        public int TimeStepAge = 0;
        public int PreviousPositionX { get; set; }
        public int PreviousPositionY { get; set; }

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int FoodCount { get; set; }
        public int StarveCount { get; set; }


        public bool Eaten { get; set; }
        public bool RecentlyBred { get; set; }
        public string Text { get; set; }

        // Top Position
        public bool CanMoveUp { get; set; }
        public bool CanEatUp { get; set; }
        public bool CanBreedUp { get; set; }
        public bool IsTopBorder { get; set; }

        public bool IsTopInsect { get; set; }
        public Insect TopInsect { get; set; }
        public int TopPosX { get; set; }
        public int TopPosY { get; set; }

        // Right Position
        public bool CanMoveRight { get; set; }
        public bool CanEatRight { get; set; }
        public bool CanBreedRight { get; set; }
        public bool IsRightBorder { get; set; }
        public bool IsRightInsect { get; set; }
        public Insect RightInsect { get; set; }
        public int RightPosX { get; set; }
        public int RightPosY { get; set; }

        // Down Position 
        public bool CanMoveDown { get; set; }
        public bool CanEatDown { get; set; }
        public bool CanBreedDown { get; set; }
        public bool IsDownBorder { get; set; }
        public bool IsDownInsect { get; set; }
        public Insect DownInsect { get; set; }

        public int DownPosX { get; set; }
        public int DownPosY { get; set; }

        // Left Position 
        public bool CanMoveLeft { get; set; }
        public bool CanEatLeft { get; set; }
        public bool CanBreedLeft { get; set; }
        public bool IsLeftBorder { get; set; }
        public bool IsLeftInsect { get; set; }
        public Insect LeftInsect { get; set; }
        public int LeftPosX { get; set; }
        public int LeftPosY { get; set; }

        public abstract void UpdateMovePositions(Cell[,] grid, List<Insect> insects);

        public abstract void Move();
        public abstract Insect Breed();
        public abstract void NextTimeStep();

    }
}
