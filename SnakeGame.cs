using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    // class: program entry point
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    // class: controls the game loop and game logic
    class Game
    {
        private int windowWidth;
        private int windowHeight;
        private int score;
        private int gameOver;
        private Random randomNumber;

        private Snake snake;
        private Food food;

        public void Start()
        {

            Console.WindowHeight = 16;
            Console.WindowWidth = 32;

            // Renamed screenwidth
            windowWidth = Console.WindowWidth;

            // Renamed screenheight
            windowHeight = Console.WindowHeight;

            // Renamed randomnummer
            randomNumber = new Random();

            score = 5;

            // Renamed gameover
            gameOver = 0;

            snake = new Snake(windowWidth, windowHeight);
            food = new Food(randomNumber, windowWidth, windowHeight);

            while (true)
            {
                Console.Clear();

                gameOver = snake.CheckWallCollision(windowWidth, windowHeight);

                DrawBorders();

                Console.ForegroundColor = ConsoleColor.Green;

                if (food.foodX == snake.snakeHead.xPosition &&
                    food.foodY == snake.snakeHead.yPosition)
                {
                    score++;
                    food.GenerateNewPosition();
                }

                gameOver = snake.DrawSnakeBody(gameOver);

                if (gameOver == 1)
                    break;

                snake.DrawSnakeHead();
                food.DrawFood();

                snake.HandleMovement();
                snake.MoveSnake();
                snake.TrimSnakeTail(score);
            }

            Console.SetCursorPosition(windowWidth / 5, windowHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
        }

        // method: draw screen borders
        private void DrawBorders()
        {
            for (int i = 0; i < windowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, windowHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < windowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(windowWidth - 1, i);
                Console.Write("■");
            }
        }
    }

    // class: handles snake behavior
    class Snake
    {
        public Pixel snakeHead;

        private List<int> snakeBodyXPositions;
        private List<int> snakeBodyYPositions;
        private string currentDirection;
        private string buttonPressed;
        private DateTime startTime;

        public Snake(int windowWidth, int windowHeight)
        {
            // Renamed hoofd
            snakeHead = new Pixel();
            snakeHead.xPosition = windowWidth / 2;
            snakeHead.yPosition = windowHeight / 2;

            // Renamed schermkleur
            snakeHead.color = ConsoleColor.Red;

            // Renamed movement
            currentDirection = "RIGHT";

            // Renamed xposlijf
            snakeBodyXPositions = new List<int>();

            // Renamed yposlijf
            snakeBodyYPositions = new List<int>();

            // Renamed buttonpressed
            buttonPressed = "no";
        }

        // method: check for wall collision
        public int CheckWallCollision(int windowWidth, int windowHeight)
        {
            if (snakeHead.xPosition == windowWidth - 1 ||
                snakeHead.xPosition == 0 ||
                snakeHead.yPosition == windowHeight - 1 ||
                snakeHead.yPosition == 0)
            {
                return 1;
            }
            return 0;
        }

        // method: draw snake body and check self collision
        public int DrawSnakeBody(int gameOver)
        {
            for (int i = 0; i < snakeBodyXPositions.Count; i++)
            {
                Console.SetCursorPosition(snakeBodyXPositions[i], snakeBodyYPositions[i]);
                Console.Write("■");

                if (snakeBodyXPositions[i] == snakeHead.xPosition &&
                    snakeBodyYPositions[i] == snakeHead.yPosition)
                {
                    gameOver = 1;
                }
            }
            return gameOver;
        }

        // method: draw snake head
        public void DrawSnakeHead()
        {
            Console.SetCursorPosition(snakeHead.xPosition, snakeHead.yPosition);
            Console.ForegroundColor = snakeHead.color;
            Console.Write("■");
        }

        // method: handle movement input
        public void HandleMovement()
        {
            startTime = DateTime.Now;
            buttonPressed = "no";

            while (true)
            {
                if (DateTime.Now.Subtract(startTime).TotalMilliseconds > 500)
                    break;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.UpArrow && currentDirection != "DOWN" && buttonPressed == "no")
                    {
                        currentDirection = "UP";
                        buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.DownArrow && currentDirection != "UP" && buttonPressed == "no")
                    {
                        currentDirection = "DOWN";
                        buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.LeftArrow && currentDirection != "RIGHT" && buttonPressed == "no")
                    {
                        currentDirection = "LEFT";
                        buttonPressed = "yes";
                    }
                    if (keyInfo.Key == ConsoleKey.RightArrow && currentDirection != "LEFT" && buttonPressed == "no")
                    {
                        currentDirection = "RIGHT";
                        buttonPressed = "yes";
                    }
                }
            }
        }

        // method: move snake
        public void MoveSnake()
        {
            snakeBodyXPositions.Add(snakeHead.xPosition);
            snakeBodyYPositions.Add(snakeHead.yPosition);

            switch (currentDirection)
            {
                case "UP": snakeHead.yPosition--; break;
                case "DOWN": snakeHead.yPosition++; break;
                case "LEFT": snakeHead.xPosition--; break;
                case "RIGHT": snakeHead.xPosition++; break;
            }
        }

        // method: trim tail
        public void TrimSnakeTail(int score)
        {
            if (snakeBodyXPositions.Count > score)
            {
                snakeBodyXPositions.RemoveAt(0);
                snakeBodyYPositions.RemoveAt(0);
            }
        }
    }

    // class: handles food logic
    class Food
    {
        public int foodX;
        public int foodY;

        private Random randomNumber;
        private int windowWidth;
        private int windowHeight;

        public Food(Random randomNumber, int windowWidth, int windowHeight)
        {
            this.randomNumber = randomNumber;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;

            GenerateNewPosition();
        }

        // method: generate new food position
        public void GenerateNewPosition()
        {
            foodX = randomNumber.Next(1, windowWidth - 2);
            foodY = randomNumber.Next(1, windowHeight - 2);
        }

        // method: draw food
        public void DrawFood()
        {
            Console.SetCursorPosition(foodX, foodY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }
    }

    // class: represents a pixel (position and color)
    class Pixel
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public ConsoleColor color { get; set; }
    }
}