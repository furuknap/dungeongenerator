using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int height = 25;
            int width = 40;
            bool done = false;
            while (!done)
            {
                ShowDungeon(height, width, 0);
                Console.WriteLine("Click space to generate another dungeon or any other key to quit");
                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    ShowDungeon(height, width, 0);
                }
                else
                {
                    done = true;
                }
            }

        }

        private static void ShowDungeon(int height, int width, int seed)
        {
            Console.Clear();
            var dungeon = DungeonUtilities.GenerateDungeon(width, height, seed);

            for (int i = height - 1; i >= 0; i--)
            {
                for (int j = 0; j < width; j++)
                {
                    if (dungeon.Entrance.X == j && dungeon.Entrance.Y == i)
                    {

                        Console.Write("*");
                    }
                    else if (dungeon.Exit.X == j && dungeon.Exit.Y == i)
                    {

                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(dungeon.Tiles[j, i] ? "#" : " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
