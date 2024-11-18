
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System;

namespace ConsoleSnake
{
    class Program
    {
        static void Main()
        {
            Snake snakeGame = new Snake();
            snakeGame.Run();
        }
    }
}