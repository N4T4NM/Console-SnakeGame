using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public class Tilemap
    {
        TileValue[][] tiles;
        TileValue[][] doubleBuffer;

        public readonly char[] TileChar = new char[4] { ' ', '@', 'X', '#' };

        public readonly int MaxWidth;
        public readonly int MaxHeight;

        public Tilemap(int width, int height)
        {
            MaxWidth = width;
            MaxHeight = height;

            Init();
        }
        void Init()
        {
            tiles = new TileValue[MaxWidth][];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i] = new TileValue[MaxHeight];

            doubleBuffer = new TileValue[MaxWidth][];
            for (int i = 0; i < doubleBuffer.Length; i++)
                doubleBuffer[i] = new TileValue[MaxHeight];
        }

        public void DrawChanges()
        {
            for (int x = 0; x < MaxWidth; x++)
                for (int y = 0; y < MaxHeight; y++)
                {
                    if (tiles[x][y] == doubleBuffer[x][y])
                        continue;

                    Console.SetCursorPosition(x, y);
                    Console.Write(TileChar[(int)tiles[x][y]]);

                    doubleBuffer[x][y] = tiles[x][y];
                }
        }
        public Tuple<int, int> FindTileWithValue(TileValue value)
        {
            for (int x = 0; x < MaxWidth; x++)
                for (int y = 0; y < MaxHeight; y++)
                    if (this[x, y] == value)
                        return new(x, y);

            return null;
        }

        public List<Tuple<int, int>> FindTilesWithValue(TileValue value)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();
            for (int x = 0; x < MaxWidth; x++)
                for (int y = 0; y < MaxHeight; y++)
                    if (this[x, y] == value)
                        result.Add(new(x, y));

            return result;
        }

        public TileValue this[int x, int y]
        {
            get => tiles[x][y];
            set => tiles[x][y] = value;
        }
    }

    public enum TileValue
    {
        Empty = 0,
        Snake = 1,
        Score = 2,
        Wall = 3
    }
}
