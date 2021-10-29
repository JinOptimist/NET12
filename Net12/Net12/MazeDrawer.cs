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

            Console.SetCursorPosition(0, 0);
            Console.WriteLine(maze.Message);
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(new string(' ', 100));
            Console.CursorVisible = false;
           
            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(maze.Message);
                    Console.SetCursorPosition(0, 0);
                    Console.SetCursorPosition(x, y + 1);
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

                Console.WriteLine();
            }

            Console.SetCursorPosition(0, maze.Height );
            Console.WriteLine(new string(' ', 100));
            Console.SetCursorPosition(0, maze.Height );
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
    }
}

