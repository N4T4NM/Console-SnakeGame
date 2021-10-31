using System;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Game
    {
        public int speed = 4;
        bool running;
        Tilemap tilemap;
        Snake snake;

        public static Game Instance;
        public Game()
        {
            tilemap = new Tilemap(Console.WindowHeight*2, Console.WindowHeight);
            snake = new Snake(tilemap);
            Instance = this;
        }

        void SetupWalls()
        {
            //Top
            for (int x = 0; x < tilemap.MaxWidth; x++)
                tilemap[x, 0] = TileValue.Wall;

            //Left
            for (int y = 0; y < tilemap.MaxHeight; y++)
                tilemap[0, y] = TileValue.Wall;

            //Bottom
            for (int x = 0; x < tilemap.MaxWidth; x++)
                tilemap[x, tilemap.MaxHeight - 1] = TileValue.Wall;

            //Right
            for (int y = 0; y < tilemap.MaxHeight; y++)
                tilemap[tilemap.MaxWidth - 1, y] = TileValue.Wall;
        }

        Random _rand = new Random();
        
        public void SetScorePosition()
        {
            Tuple<int, int> scorePos = tilemap.FindTileWithValue(TileValue.Score);
            if (scorePos != null)
                tilemap[scorePos.Item1, scorePos.Item2] = TileValue.Empty;

            int x = _rand.Next(tilemap.MaxWidth);
            int y = _rand.Next(tilemap.MaxHeight);

            if (tilemap[x, y] != TileValue.Empty)
            {
                SetScorePosition();
                return;
            }

            tilemap[x, y] = TileValue.Score;
        }
        public void GameOver()
        {
            running = false;
            Console.Clear();
            Thread.Sleep(1000 / speed);

            Console.WriteLine("=====  GAME OVER  =====\nRestarting in 5 seconds...");
            Thread.Sleep(5000);
            Console.Clear();
            Thread.Sleep(500);

            tilemap = new Tilemap(Console.WindowHeight * 2, Console.WindowHeight);
            snake = new Snake(tilemap);

            speed = 4;
            Run();
        }

        void Update()
        {
            snake.Step();
            tilemap.DrawChanges();
        }

        public void Run()
        {
            running = true;

            SetupWalls();
            SetScorePosition();

            new Task(() => //Update
            {
                while (running)
                {
                    Console.CursorVisible = false;
                    Update();
                    Thread.Sleep(1000 / speed);
                }
            }).Start();
            while (true) //Input
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        snake.MoveDirection = MoveDirection.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.MoveDirection = MoveDirection.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.MoveDirection = MoveDirection.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        snake.MoveDirection = MoveDirection.Right;
                        break;
                }
            }
        }
    }
}
