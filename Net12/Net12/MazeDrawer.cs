using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12
{
    public class MazeDrawer
    {
        public void Draw(MazeLevel maze)
        {
            Console.Clear();

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    var cell = maze[x, y];
                    if (maze.Hero.X == x && maze.Hero.Y == y)
                    {
                        Console.Write("@");
                    }
                    else  if (cell is Wall)
                    {
                        Console.Write("#");
                    }
                    else if (cell is Coin)
                    {
                        Console.Write("c");
                    }
                    else if (cell is Ground)
                    {
                        Console.Write(".");
                    }
                    else if (cell is Healer)
                    {
                        var originColor = Console.ForegroundColor; // Предыдущий цвет
                        Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
                        Console.Write("H");
                        Console.ForegroundColor = originColor; // сбрасываем в стандартный
                        
                    }
                }

                Console.WriteLine();
            }
            Console.WriteLine($"Здоровье {maze.Hero.Healt}");
            Console.WriteLine($"Деньги   {maze.Hero.Money}");

        }
    }
}
