using System;
using System.Collections.Generic;
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

        public Vector2D pointPosition;
        public const int pointWallDistanceMin = 2;

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
        public char pointChar = 'P';

        public string gameoverStringStart = "################################\n" + "# Game over! Your score was: ";
        public string gameoverStringEnd = " #\n################################";
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
                    Move(Console.ReadKey().Key);
                }

                Gameover();
            }
            Environment.Exit(0);
        }

        void Move(ConsoleKey keyPressed)
        {
            #region Translate Keypress
            if (gameRunning)
            {
                if (keyPressed == ConsoleKey.Escape)
                {
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
            #endregion

            #region Check collision & Move Player
            if (gameRunning)
            {
                if (movementDirection == "Up")
                {
                    if ((snakePositions[0].Y - 1) == pointPosition.Y && pointPosition.X == (snakePositions[0].X))
                    {
                        AddPoint();
                    }
                    else
                    {
                        if (snakePositions.GetLength(0)! > 1)
                        {
                            for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
                            {
                                if (i == 0)
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y--;
                                }
                                else
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y = snakePositions[i - 1].Y;
                                    snakePositions[i].X = snakePositions[i - 1].X;
                                }
                            }
                        }
                        else
                        {
                            mapGrid[snakePositions[0].Y, snakePositions[0].X] = 0;
                            snakePositions[0].Y--;
                        }
                    }
                }
                else if (movementDirection == "Down")
                {
                    if ((snakePositions[0].Y + 1) == pointPosition.Y && pointPosition.X == (snakePositions[0].X))
                    {
                        AddPoint();
                    }
                    else
                    {
                        if (snakePositions.GetLength(0)! > 1)
                        {
                            for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
                            {
                                if (i == 0)
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y++;
                                }
                                else
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y = snakePositions[i - 1].Y;
                                    snakePositions[i].X = snakePositions[i - 1].X;
                                }
                            }
                        }
                        else
                        {
                            mapGrid[snakePositions[0].Y, snakePositions[0].X] = 0;
                            snakePositions[0].Y++;
                        }
                    }
                }
                else if (movementDirection == "Left")
                {
                    if ((snakePositions[0].Y) == pointPosition.Y && pointPosition.X == (snakePositions[0].X - 1))
                    {
                        AddPoint();
                    }
                    else
                    {
                        if (snakePositions.GetLength(0)! > 1)
                        {
                            for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
                            {
                                if (i == 0)
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].X--;
                                }
                                else
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y = snakePositions[i - 1].Y;
                                    snakePositions[i].X = snakePositions[i - 1].X;
                                }
                            }
                        }
                        else
                        {
                            mapGrid[snakePositions[0].Y, snakePositions[0].X] = 0;
                            snakePositions[0].X--;
                        }
                    }
                }
                else if (movementDirection == "Right")
                {
                    if ((snakePositions[0].Y) == pointPosition.Y && pointPosition.X == (snakePositions[0].X + 1))
                    {
                        AddPoint();
                    }
                    else
                    {
                        if (snakePositions.GetLength(0)! > 1)
                        {
                            for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
                            {
                                if (i == 0)
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].X++;
                                }
                                else
                                {
                                    mapGrid[snakePositions[i].Y, snakePositions[i].X] = 0;
                                    snakePositions[i].Y = snakePositions[i - 1].Y;
                                    snakePositions[i].X = snakePositions[i - 1].X;
                                }
                            }
                        }
                        else
                        {
                            mapGrid[snakePositions[0].Y, snakePositions[0].X] = 0;
                            snakePositions[0].X++;
                        }
                    }
                }
                //Check if collided with wall or body;
                for (int i = 0; i < snakePositions.GetLength(0); i++)
                {
                    //Check if collided with wall
                    if (snakePositions[0].Y == 0 || snakePositions[0].Y == mapHeight - 1 || snakePositions[0].X == 0 || snakePositions[0].X == mapWidth - 1)
                    {
                        gameRunning = false;
                    }

                    //Check if collided with body
                    if (snakePositions[0].Y == snakePositions[i].Y && snakePositions[0].X == snakePositions[i].X)
                    {
                        if (i != 0)
                        {
                            gameRunning = false;
                        }
                    }
                }
            }
            #endregion
        }

        /* To do list:
        Bugfix:
        [ ] Fix teleportation bug
        [ ] Fix Point disappearing
        [ ] Fix border top and bottom being invisible
        [ ] Fix snakehead becoming P

        Features:
        [ ] Make game run by ticks instead of only when moving
         */

        void AddPoint()
        {
            score++;

            Vector2D[] oldPositions;
            oldPositions = new Vector2D[snakePositions.GetLength(0)];

            //Save current positions in oldPositions
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                oldPositions[i] = snakePositions[i];
            }

            //Overwrite snakePositions vectorarray with new vectorarray 1 element bigger.
            snakePositions = new Vector2D[oldPositions.GetLength(0) + 1];

            // Give new vectorarray old positions
            for (int i = 0; i < oldPositions.GetLength(0); i++)
            {
                snakePositions[i + 1] = oldPositions[i];
            }
            snakePositions[0] = pointPosition;

            mapGrid[pointPosition.Y, pointPosition.Y] = 2;

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
                    mapGrid[yCord, xCord] = 4;
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
                Console.Write(gameoverStringStart + score + gameoverStringEnd + "\n");
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
                    if (x == 0 || x == mapWidth - 1)
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
            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                snakePositions[i] = new Vector2D();
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
