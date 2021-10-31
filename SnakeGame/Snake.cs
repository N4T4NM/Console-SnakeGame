using System;
using System.Collections.Generic;

namespace SnakeGame
{
    public class Snake
    {
        public MoveDirection MoveDirection = MoveDirection.Right;
        Tilemap tilemap;
        List<Tuple<int, int>> bodyPositions = new();

        public Snake(Tilemap tilemap)
        {
            this.tilemap = tilemap;

            Init();
        }

        public void Init()
        {
            int startY = tilemap.MaxHeight / 2;
            bodyPositions.Add(new(4, startY));
            bodyPositions.Add(new(3, startY));
            bodyPositions.Add(new(2, startY));

            SendSnakePosition();
        }

        void SendSnakePosition()
        {
            foreach (Tuple<int, int> bodyPart in tilemap.FindTilesWithValue(TileValue.Snake))
                tilemap[bodyPart.Item1, bodyPart.Item2] = TileValue.Empty;

            foreach (Tuple<int, int> bodyPart in bodyPositions)
                tilemap[bodyPart.Item1, bodyPart.Item2] = TileValue.Snake;
        }

        void UpdateBodyChain(Tuple<int, int> nextPosition, int current)
        {
            if (current == -1)
                return;

            Tuple<int, int> nextBodyPartPos = bodyPositions[current];
            bodyPositions[current] = nextPosition;

            UpdateBodyChain(nextBodyPartPos, current - 1);
        }

        public void Step()
        {
            Tuple<int, int> head = bodyPositions[bodyPositions.Count - 1];

            int x = head.Item1;
            int y = head.Item2;

            switch (MoveDirection)
            {
                case MoveDirection.Right:
                    x++;
                    break;
                case MoveDirection.Left:
                    x--;
                    break;
                case MoveDirection.Down:
                    y++;
                    break;
                case MoveDirection.Up:
                    y--;
                    break;
            }

            if (tilemap[x, y] == TileValue.Score)
            {
                Game.Instance.SetScorePosition();
                Game.Instance.speed++;
                bodyPositions.Insert(0, new(x, y));
            }

            if(tilemap[x, y] == TileValue.Wall)
            {
                Game.Instance.GameOver();
                return;
            }

            UpdateBodyChain(new(x, y), bodyPositions.Count - 1);
            SendSnakePosition();
        }
    }

    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
}
