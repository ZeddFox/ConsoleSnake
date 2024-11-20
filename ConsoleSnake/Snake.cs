using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{           
    public class Snake()
    {
        #region Variables
        public bool playGame = true;
        public bool gameRunning = false;

        public int score;
        public string movementDirection;
        public string lastMovedDirection;

        public int[,] mapGrid;

        public Vector2D[] snakePositions;
        public Vector2D[] prevSnakePositions;

        public Vector2D pointPosition;
        public const int pointWallDistanceMin = 2;

        public const int frameDelay = 50;
        public int directionModifier;

        public const int snakeStartLength = 3;
        public const int startWallDistance = 1;

        public const int pointStartY = mapHeight / 2;
        public const int pointStartX = mapWidth - 5;

        public const int mapHeight = 28;
        public const int mapWidth = 60;

        public char borderChar = '#';
        public char blankChar = ' ';
        public char snakeheadChar = '0';
        public char snakebodyChar = 'o';
        public char pointChar = 'X';

        public string lossReason;
        #endregion
        public void Run()
        {
            Console.CursorVisible = false;

            while (playGame)
            {
                Initialize();

                while (gameRunning)
                {
                    Render();

                    Thread.Sleep(frameDelay + directionModifier);

                    Move();
                }

                Gameover();
            }
            Environment.Exit(0);
        }

        void Move()
        {
            #region Translate Keypress
            if (gameRunning)
            {
                DateTime time = DateTime.Now;
                while ((DateTime.Now - time).Milliseconds < frameDelay)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKey keyPressed = Console.ReadKey().Key;

                        if (keyPressed == ConsoleKey.Escape)
                        {
                            lossReason = "Quit";
                            gameRunning = false;
                        }
                        else if (keyPressed == ConsoleKey.UpArrow || keyPressed == ConsoleKey.W)
                        {
                            if (lastMovedDirection == "Down")
                            {
                                movementDirection = "Down";
                            }
                            else
                            {
                                movementDirection = "Up";
                                lastMovedDirection = "Up";
                            }
                        }
                        else if (keyPressed == ConsoleKey.DownArrow || keyPressed == ConsoleKey.S)
                        {
                            if (lastMovedDirection == "Up")
                            {
                                movementDirection = "Up";
                            }
                            else
                            {
                                movementDirection = "Down";
                                lastMovedDirection = "Down";
                            }
                        }
                        else if (keyPressed == ConsoleKey.LeftArrow || keyPressed == ConsoleKey.A)
                        {
                            if (lastMovedDirection == "Right")
                            {
                                movementDirection = "Right";
                            }
                            else
                            {
                                movementDirection = "Left";
                                lastMovedDirection = "Left";
                            }
                        }
                        else if (keyPressed == ConsoleKey.RightArrow || keyPressed == ConsoleKey.D)
                        {
                            if (lastMovedDirection == "Left")
                            {
                                movementDirection = "Left";
                            }
                            else
                            {
                                movementDirection = "Right";
                                lastMovedDirection = "Right";
                            }
                        }

                    }
                }
                switch (movementDirection)
                {
                    case "Up":
                    case "Down":
                        directionModifier = 25;
                        break;

                    case "Left":
                    case "Right":
                        directionModifier = 0;
                        break;
                }
            }
            #endregion

            #region Check collision & Move Player
            if (gameRunning)
            {
                // Set previous positions and remove old drawn positions
                for (int i = 0; i < snakePositions.GetLength(0); i++)
                {
                    prevSnakePositions[i].X = snakePositions[i].X;
                    prevSnakePositions[i].Y = snakePositions[i].Y;

                    mapGrid[prevSnakePositions[i].Y, prevSnakePositions[i].X] = 0;
                }

                // Move Player
                // Move Snake Head
                switch (movementDirection)
                {
                    case "Up":
                        snakePositions[0].Y -= 1;

                        mapGrid[prevSnakePositions[0].Y, prevSnakePositions[0].X] = 2;
                        break;

                    case "Down":
                        snakePositions[0].Y += 1;

                        mapGrid[prevSnakePositions[0].Y, prevSnakePositions[0].X] = 2;
                        break;

                    case "Left":
                        snakePositions[0].X -= 1;

                        mapGrid[prevSnakePositions[0].Y, prevSnakePositions[0].X] = 2;
                        break;

                    case "Right":
                        snakePositions[0].X += 1;

                        mapGrid[prevSnakePositions[0].Y, prevSnakePositions[0].X] = 2;
                        break;
                }

                // Move Snake Body
                for (int i = 1; i < snakePositions.GetLength(0); i++)
                {
                    snakePositions[i].X = prevSnakePositions[i - 1].X;
                    snakePositions[i].Y = prevSnakePositions[i - 1].Y;
                }

                //Check for Collision
                //Check if collided with point
                if (snakePositions[0].X == pointPosition.X && snakePositions[0].Y == pointPosition.Y)
                {
                    AddPoint();
                }

                //Check if collided with wall
                if (snakePositions[0].Y == 0 || snakePositions[0].Y == mapHeight - 1 || snakePositions[0].X == 0 || snakePositions[0].X == mapWidth - 1)
                {
                    lossReason = "Collided with wall";
                    gameRunning = false;
                }

                //Check if collided with body
                for (int i = 0; i < snakePositions.GetLength(0); i++)
                 {
                    if (snakePositions[0].Y == snakePositions[i].Y && snakePositions[0].X == snakePositions[i].X)
                    {
                        if (i != 0)
                        {
                            lossReason = "Collided with body";
                            gameRunning = false;
                        }
                    }
                }
            }
            #endregion
        }


        void ElongateSnake()
        {
            Vector2D[] oldPositions;
            Vector2D[] oldPrevPositions;
            oldPositions = new Vector2D[snakePositions.GetLength(0)];
            oldPrevPositions = new Vector2D[snakePositions.GetLength(0)];

            //Save current positions in oldPositions
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                oldPositions[i] = new Vector2D();
                oldPositions[i].X = snakePositions[i].X;
                oldPositions[i].Y = snakePositions[i].Y;

                oldPrevPositions[i] = new Vector2D();
                oldPrevPositions[i].X = prevSnakePositions[i].X;
                oldPrevPositions[i].Y = prevSnakePositions[i].Y;
            }

            //Overwrite snakePositions and prevSnakePositions vectorarrays with new vectorarrays 1 element bigger.
            snakePositions = new Vector2D[oldPositions.GetLength(0) + 1];
            prevSnakePositions = new Vector2D[oldPrevPositions.GetLength(0) + 1];

            // Define Array elements as Vector2D
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                snakePositions[i] = new Vector2D();
                prevSnakePositions[i] = new Vector2D();
            }

            // Give new vectorarray old positions
            for (int i = 0; i < oldPositions.GetLength(0); i++)
            {
                snakePositions[i].X = oldPositions[i].X;
                snakePositions[i].Y = oldPositions[i].Y;

                prevSnakePositions[i + 1].X = oldPrevPositions[i].X;
                prevSnakePositions[i + 1].Y = oldPrevPositions[i].Y;
            }
            snakePositions[snakePositions.GetLength(0) - 1].X = prevSnakePositions[snakePositions.GetLength(0) - 1].X;
            snakePositions[snakePositions.GetLength(0) - 1].Y = prevSnakePositions[snakePositions.GetLength(0) - 1].Y;
        }

        void AddPoint()
        {
            score++;

            mapGrid[pointPosition.Y, pointPosition.X] = 0;

            ElongateSnake();

            NewPoint();
        }

        void NewPoint()
        {
            int boundTop = pointWallDistanceMin;
            int boundLeft = pointWallDistanceMin;
            int boundBottom = mapHeight - pointWallDistanceMin;
            int boundRight = mapWidth - pointWallDistanceMin;

            int yCord = 0;
            int xCord = 0;

            bool newPositionGood = false;

            while (!newPositionGood)
            {
                int posCollision = 0;

                Random rng = new Random();

                yCord = rng.Next(boundTop, boundBottom);
                xCord = rng.Next(boundLeft, boundRight);

                for (int i = 0; i < snakePositions.GetLength(0); i++)
                {
                    if (snakePositions[i].Y == yCord && snakePositions[i].X == xCord)
                    {
                        posCollision++;
                    }
                }

                if (posCollision == 0)
                {
                    newPositionGood = true;
                }
            }

            pointPosition.Y = yCord;
            pointPosition.X = xCord;
        }

        void Render()
        {
            //Translate Snake Vector2 to Mapgrid
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                if (i == 0)
                {
                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 3;
                }
                else
                {
                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 2;
                }
            }

            //Translate Point Vector2 to Mapgrid
            mapGrid[pointPosition.Y, pointPosition.X] = 4;

            //Draw game
            for (int y = 0; y < mapGrid.GetLength(0); y++)
            {
                for (int x = 0; x < mapGrid.GetLength(1); x++)
                {
                    //Draw Map border
                    if (mapGrid[y, x] == 1)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(borderChar);
                    }
                    //Draw Snake head
                    else if (mapGrid[y, x] == 3)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(snakeheadChar);
                    }
                    //Draw Snake body
                    else if (mapGrid[y, x] == 2)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(snakebodyChar);
                    }
                    //Draw Point
                    else if (mapGrid[y, x] == 4)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(pointChar);
                    }
                    //Draw Blankspaces
                    else if (mapGrid[y, x] == 0)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(blankChar);
                    }
                    //Start new line
                    if (x == mapWidth - 1)
                    {
                        Console.Write("\n");
                    }
                }
            }
            Console.WriteLine("Your score is: " + score);
        }

        void Gameover()
        {
            bool retryAnswer = true;
            while (retryAnswer)
            {
                Console.Clear();
                if (lossReason == "Quit")
                {
                    Console.Write("G");
                }
                Console.WriteLine("Game over!");
                Console.WriteLine("Cause of losing: " + lossReason);
                Console.WriteLine("Your score was: " + score);
                Console.WriteLine("Press ( R ) to restart or ( ESC ) to exit.");

                ConsoleKey keyRead = Console.ReadKey().Key;

                if (keyRead == ConsoleKey.R)
                {
                    retryAnswer = false;
                }
                else if (keyRead == ConsoleKey.Escape)
                {
                    playGame = false;
                    retryAnswer = false;
                }
            }
        }

        void Initialize()
        {
            //Create Map border
            mapGrid = new int[mapHeight, mapWidth];
            for (int y = 0; y < mapGrid.GetLength(0); y++)
            {
                for (int x = 0; x < mapGrid.GetLength(1); x++)
                {
                    if (y == 0 || y == mapHeight - 1)
                    {
                        mapGrid[y, x] = 1;
                    }
                    else if (x == 0 || x == mapWidth - 1)
                    {
                        mapGrid[y, x] = 1;
                    }
                    else
                    {
                        mapGrid[y, x] = 0;
                    }
                }
            }

            //Create array of snake positions and set their start position
            snakePositions = new Vector2D[snakeStartLength];
            prevSnakePositions = new Vector2D[snakePositions.GetLength(0)];
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                snakePositions[i] = new Vector2D();
                prevSnakePositions[i] = new Vector2D();

                snakePositions[i].Y = mapHeight / 2;
                snakePositions[i].X = startWallDistance + snakeStartLength - i;
            }

            //Create new vector for position of the point and set start position
            pointPosition = new Vector2D();
            pointPosition.Y = pointStartY;
            pointPosition.X = pointStartX;

            score = 0;

            movementDirection = "Right";
            lastMovedDirection = "Right";

            Render();

            gameRunning = true;
        }

    }
}
