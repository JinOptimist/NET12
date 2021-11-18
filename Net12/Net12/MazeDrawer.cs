using Net12.Maze;
using Net12.Maze.Cells.Enemies;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                { typeof(Walker), "E"},
                { typeof(BullEnemy), "!"},
                { typeof(Wallworm), "W"},
                { typeof(Geyser), "G"},
                { typeof(Slime), "☺"},
                { typeof(Goblin), "g"},
            };

        private Dictionary<Type, ConsoleColor> ColorSymbolDictionary =
            new Dictionary<Type, ConsoleColor>()
            {
                { typeof(WeakWall), ConsoleColor.DarkGray},
                { typeof(Coin), ConsoleColor.DarkYellow},
                { typeof(GoldMine), ConsoleColor.Yellow},
                { typeof(HealPotion), ConsoleColor.Green},
                { typeof(Trap), ConsoleColor.Red},
                { typeof(Puddle), ConsoleColor.Cyan},

            };

        private Dictionary<Type, ConsoleColor> ColorBackgroundDictionary =
            new Dictionary<Type, ConsoleColor>()
            {
                { typeof(WeakWall), ConsoleColor.Black},
                { typeof(Coin), ConsoleColor.DarkBlue},
                { typeof(GoldMine), ConsoleColor.DarkMagenta},
                { typeof(HealPotion), ConsoleColor.White},
                { typeof(Trap), ConsoleColor.Yellow},
                { typeof(Puddle), ConsoleColor.Magenta},
            };

        private Random rand = new Random();

        public void Draw(MazeLevel maze)
        {

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(maze.Message);
            Console.CursorVisible = false;

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    Console.SetCursorPosition(x, y + 1);
                    var cell = maze.GetCellOrUnit(x, y);

                    var symbol = GetSymbolByCellType(cell);


                    var origenalColor = Console.ForegroundColor;
                    var currentBackground = Console.BackgroundColor;

                    var color = GetSymbolColorByCellType(cell);
                    var background = GetBackgroundColorByCellType(cell);
                    Console.ForegroundColor = color;
                    Console.BackgroundColor = background;

                    Console.Write(symbol);

                    Console.ForegroundColor = origenalColor;
                    Console.BackgroundColor = currentBackground;
                    Console.ResetColor();

                }


                Console.WriteLine();
            }

            Console.SetCursorPosition(0, maze.Height);
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(0, maze.Height);
            Console.WriteLine($"\nMoney :{ maze.Hero.Money}");

            Console.SetCursorPosition(0, maze.Height + 2);
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(0, maze.Height + 2);
            Console.WriteLine($"Fatigue: {maze.Hero.CurrentFatigue}/{maze.Hero.MaxFatigue}");

            Console.SetCursorPosition(0, maze.Height + 3);
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(0, maze.Height + 3);
            Console.WriteLine($"HP: {maze.Hero.Hp}");

        }

        private string GetSymbolByCellType(IBaseCell cell)
        {
            var type = cell.GetType();

            return TypeSymbolDictionary[type];
        }

        private ConsoleColor GetSymbolColorByCellType(IBaseCell cell)
        {
            var type = cell.GetType();

            if (!ColorSymbolDictionary.ContainsKey(type))
            {
                ConsoleColor color;
                do
                {
                    color = (ConsoleColor)rand.Next(0, 16);
                } while (ColorSymbolDictionary.ContainsValue(color) && ColorSymbolDictionary.Count < 16);

                ColorSymbolDictionary.Add(type, color);
            }

            return ColorSymbolDictionary[type];
        }

        private ConsoleColor GetBackgroundColorByCellType(IBaseCell cell)
        {
            var type = cell.GetType();

            if (!ColorBackgroundDictionary.ContainsKey(type))
            {
                ConsoleColor color;
                do
                {
                    color = (ConsoleColor)rand.Next(0, 16);
                } while ((ColorBackgroundDictionary.ContainsValue(color) && ColorBackgroundDictionary.Count < 16) || ColorSymbolDictionary[type] == color);

                ColorBackgroundDictionary.Add(type, color);
            }

            return ColorBackgroundDictionary[type];
        }
    }
}

