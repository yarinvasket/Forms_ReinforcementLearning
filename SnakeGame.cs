﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    [Serializable]
    public class SnakeGame : Game
    {
        private int height, width;
        private Block[,] board;
        private LinkedList<SPoint> snake;
        private int foodX, foodY;
        private int foodDuration;
        private int starve;
        private Random random;

        public SnakeGame(int height, int width)
        {
            inputAmount = 26;
            outputAmount = 3;
            this.height = height;
            this.width = width;
            board = new Block[height, width];
            random = new Random();

            SPoint snakeLoc = new SPoint(random.Next(1, width - 1), random.Next(1, height - 1));
            SPoint leftSnake = new SPoint(snakeLoc.x - 1, snakeLoc.y);
            board[snakeLoc.y, snakeLoc.x] = Block.Snake;
            board[leftSnake.y, leftSnake.x] = Block.Snake;
            snake = new LinkedList<SPoint>();
            snake.AddLast(leftSnake);
            snake.AddLast(snakeLoc);

            GenFood();

            isEnd = false;

            foodDuration = 0;
            starve = 150;
        }

        public Block[,] GetBoard()
        {
            return board;
        }

        public SPoint[] GetDiff()
        {
            SPoint tail = snake.First.Value;
            return new SPoint[] { snake.Last.Value,
            new SPoint(tail.x - 1, tail.y),
            new SPoint(tail.x + 1, tail.y),
            new SPoint(tail.x, tail.y - 1),
            new SPoint(tail.x, tail.y + 1),
            new SPoint(foodX, foodY)};
        }

        public override int GetScore()
        {
            return snake.Count;
        }

        public override float[] SetOutput()
        {
            float[] output = new float[inputAmount];
            LinkedListNode<SPoint> tmp = snake.Last;
            int x = foodX - tmp.Value.x;
            int y = tmp.Value.y - foodY;
            int dx = tmp.Value.x - tmp.Previous.Value.x;
            int dy = tmp.Value.y - tmp.Previous.Value.y;
            const int bk = 2;
            const int area = 5;

            //If the snake is heading up
            if (dy < 0)
            {
                int startY = tmp.Value.y - bk;
                int startX = tmp.Value.x - bk;
                int endY = startY + area;
                int endX = startX + area;
                for (int i = startY; i < endY; i++)
                {
                    int row = (i - startY) * area;
                    if (i < 0 || i >= height)
                    {
                        for (int j = 0; j < area; j++)
                        {
                            output[row + j] = 1;
                        }
                        continue;
                    }
                    for (int j = startX; j < endX; j++)
                    {
                        int column = j - startX;
                        if (j < 0 || j >= width)
                        {
                            output[row + column] = 1;
                            continue;
                        }
                        output[row + column] = board[i, j] == Block.Snake ? 1 : 0;
                    }
                }
            }
            //If the snake is heading down
            else if (dy > 0)
            {
                int startY = tmp.Value.y + bk;
                int startX = tmp.Value.x + bk;
                int endY = startY - area;
                int endX = startX - area;
                for (int i = startY; i > endY; i--)
                {
                    int row = (startY - i) * area;
                    if (i < 0 || i >= height)
                    {
                        for (int j = 0; j < area; j++)
                        {
                            output[row + j] = 1;
                        }
                        continue;
                    }
                    for (int j = startX; j > endX; j--)
                    {
                        int column = startX - j;
                        if (j < 0 || j >= width)
                        {
                            output[row + column] = 1;
                            continue;
                        }
                        output[row + column] = board[i, j] == Block.Snake ? 1 : 0;
                    }
                }

                x = -x;
                y = -y;
            }
            //If the snake is heading right
            else if (dx > 0)
            {
                int startY = tmp.Value.x + bk;
                int startX = tmp.Value.y - bk;
                int endY = startY - area;
                int endX = startX + area;
                for (int i = startY; i > endY; i--)
                {
                    int row = (startY - i) * area;
                    if (i < 0 || i >= width)
                    {
                        for (int j = 0; j < area; j++)
                        {
                            output[row + j] = 1;
                        }
                        continue;
                    }
                    for (int j = startX; j < endX; j++)
                    {
                        int column = j - startX;
                        if (j < 0 || j >= height)
                        {
                            output[row + column] = 1;
                            continue;
                        }
                        output[row + column] = board[j, i] == Block.Snake ? 1 : 0;
                    }
                }

                int temp = x;
                x = -y;
                y = temp;
            }
            //If the snake is heading left
            else
            {
                int startY = tmp.Value.x - bk;
                int startX = tmp.Value.y + bk;
                int endY = startY + area;
                int endX = startX - area;
                for (int i = startY; i < endY; i++)
                {
                    int row = (i - startY) * area;
                    if (i < 0 || i >= width)
                    {
                        for (int j = 0; j < area; j++)
                        {
                            output[row + j] = 1;
                        }
                        continue;
                    }
                    for (int j = startX; j > endX; j--)
                    {
                        int column = startX - j;
                        if (j < 0 || j >= width)
                        {
                            output[row + column] = 1;
                            continue;
                        }
                        output[row + column] = board[j, i] == Block.Snake ? 1 : 0;
                    }
                }

                int temp = x;
                x = y;
                y = -temp;
            }

            output[25] = (float)Math.Atan2(y, x);
            return output;
        }

        public override void Tick(float[] input)
        {
            LinkedListNode<SPoint> last = snake.Last;

            int dx = last.Value.x - last.Previous.Value.x;
            int dy = last.Value.y - last.Previous.Value.y;

            int neuronNo = 0;
            float highest = input[0];
            for (int i = 1; i < input.Length; i++)
                if (input[i] > highest)
                {
                    highest = input[i];
                    neuronNo = i;
                }

            //If the snake didn't turn, don't change dx and dy
            if (neuronNo == 1)
            {
                //If the snake turns left
                int temp = -dx;
                dx = dy;
                dy = temp;
            }
            else if (neuronNo == 2)
            {
                //If the snake turns right
                int temp = -dy;
                dy = dx;
                dx = temp;
            }

            int newX = last.Value.x + dx;
            int newY = last.Value.y + dy;
            if (newX >= width || newY >= height || newX < 0 || newY < 0)
            {
                isEnd = true;
                return;
            }
            Block block = board[newY, newX];
            if (block == Block.Snake)
            {
                isEnd = true;
                return;
            }

            board[newY, newX] = Block.Snake;
            snake.AddLast(new SPoint(newX, newY));
            if (block == Block.Food)
            {
                foodDuration += 2;
                starve += 100;
                GenFood();
            }
            else
            {
                if (foodDuration <= 0)
                {
                    SPoint tail = snake.First.Value;
                    board[tail.y, tail.x] = Block.Blank;
                    snake.RemoveFirst();
                }
                else
                {
                    foodDuration--;
                }
                starve--;
                if (starve <= 0)
                {
                    isEnd = true;
                    return;
                }
            }
        }

        public void GenFood()
        {
            foodX = random.Next(0, width);
            foodY = random.Next(0, height);

            if (board[foodY, foodX] != Block.Blank)
            {
                GenFood();
                return;
            }

            board[foodY, foodX] = Block.Food;
        }

        public override void Reset()
        {
            board = new Block[height, width];

            SPoint snakeLoc = new SPoint(random.Next(1, width - 1), random.Next(1, height - 1));
            SPoint leftSnake = new SPoint(snakeLoc.x - 1, snakeLoc.y);
            board[snakeLoc.y, snakeLoc.x] = Block.Snake;
            board[leftSnake.y, leftSnake.x] = Block.Snake;
            snake = new LinkedList<SPoint>();
            snake.AddLast(leftSnake);
            snake.AddLast(snakeLoc);

            GenFood();

            isEnd = false;

            foodDuration = 0;
            starve = 150;
        }

        public override Game GetNewGame()
        {
            return new SnakeGame(height, width);
        }
    }

    public enum Block
    {
        Blank, Snake, Food
    }
}
