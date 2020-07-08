using AI_Assignment_1514433;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;

namespace AI_Assignment
{
    class Program
    {
        #region Constants

        public const int StartingLadybirdCount = 5;
        public const int StartingGreenflyCount = 100;
        public const int MaxGridSize = 20;
        public const int EmptyCell = 1, LadybirdCell = 2, GreenflyCell = 3;

        #endregion

        public static Cell[,] Grid = new Cell[20, 20];
        public static List<Insect> Insects = new List<Insect>();
        
        public static Timer Timer = new Timer();
        public static int TimeStepCount { get; set; }

        static void Main(string[] args)
        {
            InitializeConsoleLayout();
            InitializeGrid();
            InitializeLadybirds();
            InitializeGreenflies();
            InitializeTimer();
            TimeStepCount = -1;
            string userInput;

            PrintGrid();

            do
            {
                Console.WriteLine("Press enter for the next time step:");
                userInput = Console.ReadLine();

                

                // Start the timer
                if (userInput == "g" || userInput == "G" && Timer.Enabled == false)
                    Timer.Enabled = true;

                if (Timer.Enabled == true)
                {
                    if (userInput == "s" || userInput == "S")
                        Timer.Enabled = false;
                    if (userInput == "1")
                        Timer.Interval = DecreaseTimerIntervalSpeed(-100, Timer.Interval);
                    if (userInput == "0")
                        Timer.Interval = IncreaseTimerIntervalSpeed(100, Timer.Interval);
                }


                MoveAllLadybirds();

                MoveAllGreenflies();

                Console.Clear();

                PrintGrid();
               
                // Reset the simuation
                if (userInput == "R" || userInput == "r")
                    ResetSimulation();

            } while (userInput != "q" || userInput != "Q");
        }
        
        /// <summary>
        /// Increase timer interval speed
        /// </summary>
        /// <param name="updatedSpeed"></param>
        /// <param name="currentSpeed"></param>
        /// <returns></returns>
        static double IncreaseTimerIntervalSpeed(double updatedSpeed, double currentSpeed) 
        {
            // Check if the speed is valid
            if (currentSpeed < 2000) 
            {
                currentSpeed += updatedSpeed;
            }
           
            return currentSpeed;
        }
     
        /// <summary>
        /// Decreases timer interval speed
        /// </summary>
        /// <param name="updatedSpeed"></param>
        /// <param name="currentSpeed"></param>
        /// <returns></returns>
        static double DecreaseTimerIntervalSpeed(double updatedSpeed, double currentSpeed)
        {
            // Check if the speed is valid
            if (currentSpeed > 700)
            {
                currentSpeed += updatedSpeed;
            }

            return currentSpeed;
        }

        /// <summary>
        /// Resets the simulation
        /// </summary>
        static void ResetSimulation() 
        {
            Timer.Stop();
            Insects.Clear(); 
            InitializeConsoleLayout();
            InitializeGrid();
            InitializeLadybirds();
            InitializeGreenflies();
            InitializeTimer();
            TimeStepCount = -1;
            PrintGrid();
        }
        
        /// <summary>
        /// Perform the next time step for all ladbirds
        /// </summary>
        static void MoveAllLadybirds()
        {
            // Loop through all insect 
            for (int i = 0; i < Insects.Count; i++)
            {
                // Find a ladybird
                if (Insects[i] is Ladybird)
                {
                    if (Insects[i].RecentlyBred == true)
                    {
                        Insects[i].RecentlyBred = false;
                        continue;
                    }

                    // Check if the insect should starve
                    if (Insects[i].StarveCount > 3)
                    {
                        // Update the grid 
                        Grid[Insects[i].PositionX, Insects[i].PositionY].CellStatus = EmptyCell;

                        // Update the surrounding insects after the move
                        UpdateSurroundingInsects(Insects[i]);

                        // Remove the insect from the list
                        Insects.Remove(Insects[i]);

                        continue;
                    }

                    Insects[i].UpdateMovePositions(Grid, Insects);

                    // Move the insect
                    Insects[i].NextTimeStep();

                    // Update the surrounding insects after the move
                    UpdateSurroundingInsects(Insects[i]);

                    UpdateGrid(Insects[i]);

                    Insects[i].UpdateMovePositions(Grid, Insects);

                    // Check if the insects age is appropriate to breed
                    if (Insects[i].TimeStepAge >= 8)
                    {
                        // Update the insects positions so the insect find updated breed positions
                        Insects[i].UpdateMovePositions(Grid, Insects);

                        // Check if a breeding position is available 
                        if (Insects[i].CanBreedUp || Insects[i].CanBreedRight || Insects[i].CanBreedDown ||
                            Insects[i].CanBreedLeft)
                        {
                            // Create an adjacent insect, add it to the Insects list & update the map
                            var adjacentInsect = Insects[i].Breed();
                            adjacentInsect.RecentlyBred = true;

                            Insects.Add(adjacentInsect);

                            UpdateGrid(adjacentInsect);
                        }
                    }

                }
            }
        }
        
        /// <summary>
      /// Perform the next time step for all greenflies
      /// </summary>
        static void MoveAllGreenflies()
        {
            // Loop through all insect 
            for (int i = 0; i < Insects.Count; i++)
            {
                if (Insects[i].Eaten)
                {
                    Insects.RemoveAt(i);
                    continue;
                }
                // Find a Greenfly
                if (Insects[i] is Greenfly)
                {
                    if (Insects[i].RecentlyBred == true)
                    {
                        Insects[i].RecentlyBred = false;
                        continue;
                    }

                    Insects[i].UpdateMovePositions(Grid, Insects);

                    Insects[i].NextTimeStep();

                    // Update the surrounding insects after the move
                    UpdateSurroundingInsects(Insects[i]);
                    

                    UpdateGrid(Insects[i]);

                    Insects[i].UpdateMovePositions(Grid, Insects);

                    // Check if the insects age is appropriate to breed
                    if (Insects[i].TimeStepAge > 3)
                    {
                        // Update the insects positions so the insect will breed after the move 
                        Insects[i].UpdateMovePositions(Grid, Insects);

                        // Check if a breeding position is available 
                        if (Insects[i].CanBreedUp || Insects[i].CanBreedRight || Insects[i].CanBreedDown ||
                            Insects[i].CanBreedLeft)
                        {
                            // Create an adjacent insect, add it to the Insects list & update the map
                            var adjacentInsect = Insects[i].Breed();
                            adjacentInsect.RecentlyBred = true;

                            Insects.Add(adjacentInsect);

                            UpdateGrid(adjacentInsect);

                            adjacentInsect.UpdateMovePositions(Grid, Insects);

                            // Update the surrounding insects after the move
                            UpdateSurroundingInsects(Insects[i]);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Spawn in the ladybirds based on the constant count
        /// </summary>
        static void InitializeLadybirds()
        {
            var random = new Random();

            // Spawn the starting count of insects
            for (int i = 0; i < StartingLadybirdCount; i++)
            {
                var validPosition = false;
                //qwertyuiopasdfghjklzxcvbnm
                // Retrieve a random insect coordinate that is not occupied
                do
                {
                    var randomXPosition = random.Next(0, MaxGridSize);
                    var randomYPosition = random.Next(0, MaxGridSize);

                    if (Grid[randomXPosition, randomYPosition].CellStatus == EmptyCell)
                    {
                        // Assign the cell to a ladybird
                        Grid[randomXPosition, randomYPosition].CellStatus = LadybirdCell;

                        // Create the insect
                        var newInsect = new Ladybird(randomXPosition, randomYPosition);

                        // Add the insect to the list
                        Insects.Add(newInsect);

                        // Flag that the position generated was valid
                        validPosition = true;
                    }

                    // Retry as random coordinate has already been occupied
                } while (validPosition == false);
            }
        }
        
        /// <summary>
        /// Spawn in the greenflies based on the constant count
        /// </summary>
        static void InitializeGreenflies()
        {
            var random = new Random();

            // Spawn the starting count of insects
            for (int i = 0; i < StartingGreenflyCount; i++)
            {
                var validPosition = false;

                // Retrieve a random insect coordinate that is not occupied
                do
                {
                    var randomXPosition = random.Next(0, MaxGridSize);
                    var randomYPosition = random.Next(0, MaxGridSize);

                    if (Grid[randomXPosition, randomYPosition].CellStatus == EmptyCell)
                    {
                        // Assign the cell to a ladybird
                        Grid[randomXPosition, randomYPosition].CellStatus = GreenflyCell;

                        // Create the insect
                        var newInsect = new Greenfly(randomXPosition, randomYPosition);

                        // Add the insect to the list
                        Insects.Add(newInsect);

                        // Flag that the position generated was valid
                        validPosition = true;
                    }

                    // Retry as random coordinate has already been occupied
                } while (validPosition == false);
            }

        }
        
        /// <summary>
        /// Setup the grid
        /// </summary>
        static void InitializeGrid()
        {
            // Represents x axis 
            for (int x = 0; x < MaxGridSize; x++)
            {
                // Represents y axis
                for (int y = 0; y < MaxGridSize; y++)
                {
                    // Add the cell to the Grid array
                    var newGridCoordinate = new Cell(x, y, EmptyCell);
                    Grid[x, y] = newGridCoordinate;
                }
            }
        }
        
        /// <summary>
        /// Setup the colours of the console
        /// </summary>
        static void InitializeConsoleLayout()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "Insect Simulation";
            Console.WindowTop = 0;
            Console.Clear();

        }
        
        /// <summary>
        /// Set a default time fo ther timer object
        /// </summary>
        static void InitializeTimer()
        {
            // Set the timer to a standard one second 
            Timer = new Timer(1000);

            // Assign an action listener to the timer object
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
        }
        
        /// <summary>
        /// Next time step upon elapsed timer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            MoveAllLadybirds();

            MoveAllGreenflies();

            Console.Clear();

            PrintGrid();
        }
        
        /// <summary>
       /// Prints the grid of ladybirds and greenflies
       /// </summary>
        static void PrintGrid()
        {
            var ladybirdQuery = Insects.OfType<Ladybird>();
            var greenflyQuery = Insects.OfType<Greenfly>();
            var y = 0;
            var x = 0;

            for (int i = 0; i < Grid.Length; i++)
            {
                // Create a new line when the loop reaches the end of the grid
                if (x == MaxGridSize)
                {
                    Console.Write("\n");
                    y++;
                    x = 0;
                }
                if (Grid[x, y].CellStatus == LadybirdCell)
                {
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (Grid[x, y].CellStatus == GreenflyCell)
                {
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ResetColor();
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }

                // Add a space so the grid looks symmetrical 
                Console.Write($"{Grid[x, y].Text} ");
                x++;
            }

            TimeStepCount++;
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.SetCursorPosition(41, 0);
            Console.WriteLine($"Time steps: {TimeStepCount}");
            Console.SetCursorPosition(41, 2);
            Console.Write($"Ladybirds: {ladybirdQuery.Count()}");
            Console.SetCursorPosition(41, 4);
            Console.Write($"Greenflys: {greenflyQuery.Count()}\n");
            Console.SetCursorPosition(41, 6);

            if (Timer.Enabled == false)
            {
                Console.WriteLine("Enter 'G' to start the step timer.");
            }
            else if (Timer.Enabled == true)
            {
                Console.WriteLine("Enter 'S' to stop the step timer.");
                Console.SetCursorPosition(41, 7);
                Console.WriteLine("Enter '1' to increase the speed.");
                Console.SetCursorPosition(41, 8);
                Console.WriteLine("Enter '0' to decrease the speed.");
                Console.SetCursorPosition(41, 10);
                Console.WriteLine($"The timer interval is set to {Timer.Interval.ToString()}");

            
            }

            Console.SetCursorPosition(41, 14);
            Console.WriteLine("Press 'R' to reset the simulation.");
            Console.SetCursorPosition(0, 21);

        }
        
        /// <summary>
        /// Updates the cell status on the passed insects coodinates
        /// </summary>
        /// <param name="insect"></param>
        static void UpdateGrid(Insect insect)
        {
            // Remove the previous insect position from the grid
            Grid[insect.PreviousPositionX, insect.PreviousPositionY].CellStatus = EmptyCell;

            // Update the insect position on the grid
            if (insect is Ladybird)
                Grid[insect.PositionX, insect.PositionY].CellStatus = LadybirdCell;

            if (insect is Greenfly)
                Grid[insect.PositionX, insect.PositionY].CellStatus = GreenflyCell;

        }
        
        /// <summary>
        /// Refreshes the surrounding insects move positions
        /// </summary>
        /// <param name="insect"></param>
        static void UpdateSurroundingInsects(Insect insect)
        {
            // Update the surrounding insects after the move
            insect.TopInsect?.UpdateMovePositions(Grid, Insects);
            insect.RightInsect?.UpdateMovePositions(Grid, Insects);
            insect.DownInsect?.UpdateMovePositions(Grid, Insects);
            insect.LeftInsect?.UpdateMovePositions(Grid, Insects);
        }
    }
}
