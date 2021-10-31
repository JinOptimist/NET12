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
        private Dictionary<Type, string> TypeSymbolDictionary =
            new Dictionary<Type, string>()
            {
                { typeof(Hero), "@"},
                { typeof(Wall), "#"},
                { typeof(WeakWall), "#"},
                { typeof(Ground), "."},
                { typeof(GoldMine), "M"},
                { typeof(Coin), "c"},
                { typeof(Bed), "п"},
                { typeof(Puddle), "+"},
                { typeof(VitalityPotion), "V"},
                { typeof(Bless), "$"},
                { typeof(TeleportIn), ":"},
                { typeof(TeleportOut), ";"},
                { typeof(Fountain), "F"},
                { typeof(Trap), "~"},
                { typeof(HealPotion), "h"},
                { typeof(WolfPit), "*"},
                { typeof(Tavern), "T"},
                { typeof(Healer), "H"},
            };

        public void Draw(MazeLevel maze)
        {

            var NumOfString = 0;

            string Spaces()
            {
                return ("   "); // 8 spaces
            }

            //When Height >= 8

            int lengthStringWhenHeightValueBig = (maze.Width + 3 + 20); // (8 * spaces) + (20 * "-") + (maze_width) + (2 * "|")

            string consoleLengthRegulatorWhenHeightValueBig = new string('-', lengthStringWhenHeightValueBig);
            string consoleАlignmentWhenHeightValueBig = ($"|{consoleLengthRegulatorWhenHeightValueBig}|");

            if (maze.Height >= 8)
            {
                Console.Clear();

                Console.WriteLine(consoleАlignmentWhenHeightValueBig);
                Console.WriteLine($"Subtitles: {maze.Message}");
                Console.WriteLine(consoleАlignmentWhenHeightValueBig);
            }

            //When Height < 8

            int lengthStringWhenHeightValueSmall = (maze.Width - 1); // (2 * "|") + (maze_width - 1)

            string consoleLengthRegulatorWhenHeightValueSmall = new string('-', lengthStringWhenHeightValueSmall);
            string consoleАlignmentWhenHeightValueSmall = ($"|{consoleLengthRegulatorWhenHeightValueSmall}|");

            if (maze.Height < 8)
            {
                Console.Clear();

                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);
                Console.WriteLine($"Subtitles: {maze.Message}");
                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);
            }

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    NumOfString++;

                    var cell = maze.GetCellOrUnit(x, y);

                    var symbol = GetSymbolByCellType(cell);

                    var origenalColor = Console.ForegroundColor;
                    if (cell is WeakWall)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write(symbol);

                    Console.ForegroundColor = origenalColor;
                }

                if (maze.Height >= 8)
                {
                    if (NumOfString == maze.Width)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Spaces() + $"     Hero Status:");
                        Console.ResetColor();
                    }
                    if (NumOfString == 2 * maze.Width)
                    {
                        Console.Write(Spaces() + "|--------------------|"); // 20 - " - "; 2 - " | " 
                    }
                    if (NumOfString == 3 * maze.Width)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(Spaces() + $" HP: {maze.Hero.Hp}");
                        Console.ResetColor();
                    }
                    if (NumOfString == 4 * maze.Width)
                    {
                        Console.Write(Spaces() + "|--------------------|");
                    }
                    if (NumOfString == 5 * maze.Width)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(Spaces() + $" Money : { maze.Hero.Money}");
                        Console.ResetColor();
                    }
                    if (NumOfString == 6 * maze.Width)
                    {
                        Console.Write(Spaces() + "|--------------------|");
                    }
                    if (NumOfString == 7 * maze.Width)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(Spaces() + $" Fatigue: {maze.Hero.CurrentFatigue}/{maze.Hero.MaxFatigue}");
                        Console.ResetColor();
                    }
                    if (NumOfString == 8 * maze.Width)
                    {
                        Console.Write(Spaces() + "|--------------------|");
                    }
                    if (NumOfString > 8 * maze.Width)
                    {
                        Console.Write(Spaces() + "                     |");
                    }
                    if ((NumOfString >= 8 * maze.Width) && (NumOfString == (maze.Height * maze.Width)))
                    {
                        Console.Write("\n");
                    }
                    if ((NumOfString >= 8 * maze.Width) && (NumOfString == (maze.Height * maze.Width)))
                    {
                        Console.WriteLine(consoleАlignmentWhenHeightValueBig);
                    }
                    else Console.WriteLine();
                }

                if (maze.Height < 8)
                {
                    Console.Write("|");
                    Console.WriteLine();

                }

            }

            if (maze.Height < 8)
            {
                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"Hero Status:\n");
                Console.ResetColor();

                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($" HP: {maze.Hero.Hp}\n");
                Console.ResetColor();

                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($" Money : { maze.Hero.Money}\n");
                Console.ResetColor();

                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.Write($" Fatigue: {maze.Hero.CurrentFatigue}/{maze.Hero.MaxFatigue}\n");
                Console.ResetColor();

                Console.WriteLine(consoleАlignmentWhenHeightValueSmall);
            }

        }

        private string GetSymbolByCellType(IBaseCell cell)
        {
            var type = cell.GetType();

            return TypeSymbolDictionary[type];
        }
    }
}
