
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System;

public class Vector2D()
{
    public int Y { get; set; }
    public int X { get; set; }
    public int Value { get; set; }
}

public class Snake()
{
    public bool gameRunning;


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
    public char snakeChar = '0';
    public char pointChar = 'P';

    public int Run()
    {
        Console.CursorVisible = false;

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

        gameRunning = true;

        Translate();
        Render();

        while (gameRunning)
        {
            Render();
            Move(Console.ReadKey().Key);
            Translate();
        }

        return score;
    }

    public void Move(ConsoleKey keyPressed)
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
                    if (snakePositions.GetLength(0) !> 1)
                    {
                        for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
                        {
                            if (i == 0)
                            {
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
                        snakePositions[0].X++;
                    }
                }
            }
        }
        #endregion
    }

    public void AddPoint()
    {
        score++;

        Vector2D[] oldPositions;
        oldPositions = new Vector2D[snakePositions.GetLength(0)];

        //Save current positions in oldPositions
        for (int i = 0; i < snakePositions.GetLength(0); i++)
        {
            oldPositions[i] = snakePositions[i];
        }



        //Create new snakepositions with 1 more position
        for (int i = snakePositions.GetLength(0) - 1; i > -1; i--)
        {
            if (i == 0)
            {
                snakePositions[i].Y = pointPosition.Y;
                snakePositions[i].X = pointPosition.X;
            }
            else
            {
                snakePositions[i].Y = oldPositions[i - 1].Y;
                snakePositions[i].X = oldPositions[i - 1].X;
            }
        }

        NewPoint();
    }

    public void NewPoint()
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
            int posFound = 0;

            Random rng = new Random();

            yCord = rng.Next(boundTop, boundBottom);
            xCord = rng.Next(boundLeft, boundRight);

            for (int i = 0; i < snakePositions.GetLength(0); i++)
            {
                if (snakePositions[i].Y == yCord && snakePositions[i].X == xCord)
                {
                    posFound++;
                }
            }

            if (posFound == 0)
            {
                newPositionGood = true;
            }

        }

        pointPosition.Y = yCord;
        pointPosition.X = xCord;
    }

    public void Translate()
    {
        //Clear snake & point to 
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (mapGrid[y, x] == 2 || mapGrid[y, x] == 3)
                {
                    mapGrid[y, x] = 0;
                }
            }
        }

        //Translate snake & point position to mapgrid values
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int i = 0; i < snakePositions.GetLength(0); i++)
                {
                    if (snakePositions[i].Y == y && snakePositions[i].X == x)
                    {
                        mapGrid[y, x] = 2;
                    }

                    if (pointPosition.Y == y && pointPosition.X == x)
                    {
                        mapGrid[y, x] = 3;
                    }
                }
            }
        }
    }

    public void Render()
    {
        Console.Clear();

        for (int y = 0; y < mapGrid.GetLength(0); y++)
        {
            for (int x = 0; x < mapGrid.GetLength(1); x++)
            {
                //Draw Map
                if (mapGrid[y, x] == 1)
                {
                    Console.Write(borderChar);
                }
                //Draw Snake
                else if (mapGrid[y, x] == 2)
                {
                    Console.Write(snakeChar);
                }
                //Draw Point
                else if (mapGrid[y, x] == 3)
                {
                    Console.Write(pointChar);
                }
                //Draw Blankspaces
                else 
                {
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
}

class Program
{
    static void Main()
    {
        Snake snakeGame = new Snake();
        snakeGame.Run();
    }
}