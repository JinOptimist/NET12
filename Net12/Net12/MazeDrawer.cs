using Net12.Maze;
using Net12.Maze.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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

        private Dictionary<Type, ConsoleColor> ColorSymbolDictionary =
            new Dictionary<Type, ConsoleColor>()
            {               

            };

        private static Random rand = new Random();

        public void Draw(MazeLevel maze)
        {
            Console.Clear();           
            Console.WriteLine(maze.Message);

            for (int y = 0; y < maze.Height; y++)
            {
                for (int x = 0; x < maze.Width; x++)
                {
                    var cell = maze.GetCellOrUnit(x, y);
                   
                    var symbol = GetSymbolByCellType(cell);
                  
                    var color = GetColorByCellType(cell);
                    Console.ForegroundColor = color;

                    Console.Write(symbol);

                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.WriteLine($"\nMoney :{ maze.Hero.Money}");
            Console.WriteLine($"Fatigue: {maze.Hero.CurrentFatigue}/{maze.Hero.MaxFatigue}");
            Console.WriteLine($"HP: {maze.Hero.Hp}");
        }

        private string GetSymbolByCellType(IBaseCell cell)
        {
            var type = cell.GetType();

            return TypeSymbolDictionary[type];
        }

        private ConsoleColor GetColorByCellType(BaseCell cell)
        {
            var type = cell.GetType();

            if (!ColorSymbolDictionary.ContainsKey(type))
            {               
                
                var color = (ConsoleColor)rand.Next(0,16);

                while (ColorSymbolDictionary.ContainsValue(color))
                {                                      
                    if (ColorSymbolDictionary.Count >= 16)
                    {
                        break;
                    }
                    color = (ConsoleColor)rand.Next(0, 16);
                }
                ColorSymbolDictionary.Add(type, color);
            }

            return ColorSymbolDictionary[type];
        }
    }
}
