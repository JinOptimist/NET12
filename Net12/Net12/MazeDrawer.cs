using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net12.Maze.Cells;

namespace Net12
{

    public class MazeDrawer
    {
        public void Draw(MazeLevel maze)
        {
            Console.Clear();

            Console.WriteLine(maze.Message);

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    var cell = maze.GetCellOrUnit(x, y);
                    if (maze.Hero.X == x && maze.Hero.Y == y)
                    {
                        Console.Write("@");
                    }
                    else if (cell is GoldMine)
                    {
                        Console.Write("M");
                    }
                    else if (cell is Wall)
                    {
                        var origenalColor = Console.ForegroundColor;
                        if (cell is WeakWall)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        Console.Write("#");
                        Console.ForegroundColor = origenalColor;

                    }
                    else if (cell is Coin)
                    {
                        Console.Write("c");
                    }
                    else if (cell is Ground)
                    {
                        Console.Write(".");
                    }
                    else if (cell is Puddle)
                    {
                        Console.Write("+");
                    }
                    else if (cell is VitalityPotion)
                    {
                        Console.Write("V");
                    }
                    else if (cell is Bless)
                    {
                        Console.Write("$");
                    }
                    else if (cell is TeleportIn)
                    {
                        Console.Write(":");
                    }
                    else if (cell is TeleportOut)
                    {
                        Console.Write(";");
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

                    else if (cell is Trap)
                    {
                        Console.Write("~");
                    }
                    else if (cell is HealPotion)
                    {
                        Console.Write("h");
                    }
                    else if (cell is WolfPit)
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"\nMoney :{ maze.Hero.Money}");
            Console.WriteLine($"Fatigue: {maze.Hero.CurrentFatigue}/{maze.Hero.MaxFatigue}");
            Console.WriteLine($"HP: {maze.Hero.Hp}");
        }
    }
}
