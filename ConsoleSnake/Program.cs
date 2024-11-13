
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;


//public Snake snakeGame = new Snake();


public class Snake()
{
    public bool gameRunning;

    public int score;
    public string movementDirection;
    public string lastMovedDirection;
    public int[,] mapGrid;

    public int mapHeight = 30;
    public int mapWidth = 60;

    public char borderChar = '#';
    public char snakeChar = '#';
    public char pointChar = 'O';

    public int Run()
    {
        mapGrid = new int[mapWidth, mapHeight];
        score = 0;
        movementDirection = "Right";
        lastMovedDirection = "Right";
        gameRunning = true;

        while (gameRunning)
        {
            Render();
            Move(Console.ReadKey().Key);
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
                }
            }
        }
        #endregion

        #region Check collision & Move Player
        if (gameRunning)
        {
            if (movementDirection == "Up")
            {

            }
            else if (movementDirection == "Down")
            {

            }
            else if (movementDirection == "Left")
            {

            }
            else if (movementDirection == "Right")
            {

            }
        }
        #endregion
    }
    public void Render()
    {
        Console.Clear();
        for (int i = 0; i < mapGrid.GetLength(0); i++)
        {
            for (int y = 0; y < mapGrid.GetLength(1); y++)
            {
                if (mapGrid[i, y] == 1)
                {
                    Console.Write(borderChar);
                }
                else if (mapGrid[i, y] == 2)
                {
                    Console.Write(snakeChar);
                }
                else if (mapGrid[i, y] == 3)
                {
                    Console.Write(pointChar);
                }
            }
        }
    }
}