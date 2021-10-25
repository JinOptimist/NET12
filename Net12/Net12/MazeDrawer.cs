using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net12
{
    // test
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
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("@");
                        Console.ResetColor();
                    }
                    else if (cell is GoldMine)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("M");
                        Console.ResetColor();
                    }
                    else if (cell is Wall)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("#");
                        Console.ResetColor();

                    }
                    else if (cell is Coin)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("c");
                        Console.ResetColor();
                    }
                    else if (cell is Ground)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(".");
                        Console.ResetColor();
                    }
                    else if (cell is Puddle)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("+");
                        Console.ResetColor();
                    }
                    else if (cell is VitalityPotion)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write("V");
                        Console.ResetColor();

                    } 
                    else if (cell is Bless)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("$");
                        Console.ResetColor();
                    }
                    else if (cell is TeleportIn)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(":");
                        Console.ResetColor();
                    }
                    else if (cell is TeleportOut)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.Write(";");
                        Console.ResetColor();
                    }
                               
                    else if (cell is Trap)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("~");
                        Console.ResetColor();
                    }
                    else if (cell is HealPotion)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("h");
                        Console.ResetColor();
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
