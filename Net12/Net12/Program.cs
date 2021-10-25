using Net12.Maze;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net12
{
    class Program
    {
        static void Main(string[] args)
        {        
            var mazeBuilder = new MazeBuilder();
            var maze = mazeBuilder.Build(6, 4);
            var drawer = new MazeDrawer();

            while (true)
            {
                drawer.Draw(maze);

                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        maze.HeroStep(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        maze.HeroStep(Direction.Right);
                        break;
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        maze.HeroStep(Direction.Up);
                        break;
                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        maze.HeroStep(Direction.Down);
                        break;
                    default:
                        break;
                }
            }
        }    
    }
}
