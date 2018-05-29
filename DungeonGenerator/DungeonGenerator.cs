using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGenerator
{
    public class Dungeon
    {
        public bool[,] Tiles { get; set; }
        public DungeonTile Entrance { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DungeonTile Exit { get; set; }
    }

    public class DungeonTile
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class DungeonGeneratorOptions
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Seed { get; set; }
        public int MaxCoveragePercent { get; set; }

    }
    public static class DungeonUtilities
    {


        public static Dungeon GenerateDungeon(int width, int height, int seed = 0)
        {
            DungeonGeneratorOptions options = new DungeonGeneratorOptions { Width = width, Height = height, Seed = seed, MaxCoveragePercent = 50 };
            return GenerateDungeon(options);
        }

        public static Dungeon GenerateDungeon(DungeonGeneratorOptions options)
        {

            Dungeon dungeon = new Dungeon();
            Random random = new Random(options.Seed);

            if (options.Seed == 0)
            {
                random = new Random();
            }

            Stack<DungeonTile> openTiles = new Stack<DungeonTile>();
            float maxCoveragePercent = options.MaxCoveragePercent;
            int floorCounter = 0;

            dungeon.Width = options.Width;
            dungeon.Height = options.Height;
            dungeon.Tiles = new bool[dungeon.Width, dungeon.Height];

            //            dungeon.Loot = GenerateLoot(dungeon);


            for (int i = 0; i < dungeon.Width; i++)
            {
                for (int j = 0; j < dungeon.Height; j++)
                {
                    dungeon.Tiles[i, j] = true; // fill dungeon with walls.
                }
            }

            var startX = random.Next(dungeon.Width - 1);
            var starty = random.Next(dungeon.Height - 1);

            dungeon.Entrance = new DungeonTile { X = startX, Y = starty };

            bool done = false;
            var currentTile = new DungeonTile { X = startX, Y = starty };
            DungeonTile lastOpenTile = new DungeonTile { X = startX, Y = starty };

            int doneConuter = 0;
            while (!done)
            {

                int maxLength = random.Next(3);
                var foundValidDirectionForCorridor = false;

                Direction d = (Direction)random.Next(4);
                DungeonTile possibleNextTile;
                for (int i = 0; i < 4; i++)
                {
                    if (!foundValidDirectionForCorridor)
                    {
                        possibleNextTile = new DungeonTile { X = currentTile.X, Y = currentTile.Y };
                        d++;
                        if ((int)d > 4) { d = 0; }

                        if (d == Direction.North)
                        {
                            possibleNextTile.X++;
                        }
                        else if (d == Direction.South)
                        {
                            possibleNextTile.X--;
                        }
                        else if (d == Direction.East)
                        {
                            possibleNextTile.Y++;
                        }
                        else if (d == Direction.West)
                        {
                            possibleNextTile.Y--;
                        }
                        if (CanAddRoomInTile(possibleNextTile, dungeon))
                        {
                            foundValidDirectionForCorridor = true;
                        }
                    }

                }
                if (foundValidDirectionForCorridor)
                {
                    for (int i = 0; i < maxLength; i++)
                    {
                        possibleNextTile = new DungeonTile { X = currentTile.X, Y = currentTile.Y };
                        if (d == Direction.North)
                        {
                            possibleNextTile.X++;
                        }
                        else if (d == Direction.South)
                        {
                            possibleNextTile.X--;
                        }
                        else if (d == Direction.East)
                        {
                            possibleNextTile.Y++;
                        }
                        else if (d == Direction.West)
                        {
                            possibleNextTile.Y--;
                        }
                        if (CanAddRoomInTile(possibleNextTile, dungeon))
                        {
                            foundValidDirectionForCorridor = true;
                            currentTile = possibleNextTile;
                            dungeon.Tiles[currentTile.X, currentTile.Y] = false;
                            floorCounter++;
                            if (TileHasMoreThanOnePossibleDirection(currentTile, dungeon))
                            {
                                openTiles.Push(currentTile);
                            };
                        }
                    }
                }
                else
                {
                    doneConuter++;

                    if (openTiles.Count > 0)
                    {
                        currentTile = openTiles.Pop();
                        if (openTiles.Count > 0 && random.Next(100) > 50)
                        {
                            currentTile = openTiles.Pop();
                        }
                    }

                }


                if (doneConuter > 500)
                {
                    done = true;
                }

                if (floorCounter > ((dungeon.Width * dungeon.Height) / 100) * maxCoveragePercent)
                {
                    done = true;
                }

                if (done)
                {
                    dungeon.Exit = new DungeonTile { X = currentTile.X, Y = currentTile.Y };
                }

            }


            return dungeon;
        }

        private static bool TileHasMoreThanOnePossibleDirection(DungeonTile currentTile, Dungeon dungeon)
        {
            int possibleDirections = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (currentTile.X + i > 0 &&
                        currentTile.X + i < dungeon.Width - 1 &&
                        currentTile.Y + j > 0 &&
                        currentTile.Y + j < dungeon.Height - 1 &&
                        dungeon.Tiles[currentTile.X + i, currentTile.Y + j])
                    {
                        possibleDirections++;
                    }
                }
            }
            return possibleDirections > 1;
        }

        private static bool CanAddRoomInTile(DungeonTile currentTile, Dungeon dungeon)
        {
            return (
                currentTile.X < dungeon.Width - 1 &&
                currentTile.X > 0 &&
                currentTile.Y < dungeon.Height - 1 &&
                currentTile.Y > 0 &&
                dungeon.Tiles[currentTile.X, currentTile.Y]
                );
        }

        public enum Direction
        {
            North,
            East,
            South,
            West
        }

    }
}
