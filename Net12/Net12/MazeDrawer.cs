﻿using Net12.Maze;
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
                    else if (cell is Wall)
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
                    else if (cell is Fountain)
                    {
                        Console.Write("F");
                    }
                    else if (cell is Bed)
                    {
                        Console.Write("B");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine($"Fatigue: {maze.Hero.CurrentFatigue}");
        }
    }
}
