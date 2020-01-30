using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    class SnakeGame : Game
    {
        private int height, width;
        private Block[,] board;
        private LinkedList<SPoint> snake;
        private int foodDuration;
        private int starve;
        private bool isEnd;

        public SnakeGame(int height, int width)
        {
            this.height = height;
            this.width = width;
            board = new Block[height, width];
            List<SPoint> spaces = new List<SPoint>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    board[i, j] = Block.Blank;
                    spaces.Add(new SPoint(j, i));
                }
            }

            SPoint foodLoc = new SPoint(Manager.random.Next(0, height), Manager.random.Next(0, width));
            board[foodLoc.y, foodLoc.x] = Block.Food;
            spaces.Remove(foodLoc);

            for (int i = 0; i < height; i++)
            {
                if (i == 0 || i == height - 1)
                {
                    for (int j = 0; j < width; j++)
                    {
                        spaces.Remove(new SPoint(j, i));
                    }
                }
                else
                {
                    spaces.Remove(new SPoint(0, i));
                    spaces.Remove(new SPoint(width - 1, i));
                }
            }

            SPoint snakeLoc = spaces[Manager.random.Next(0, spaces.Count)];
            SPoint leftSnake = new SPoint(snakeLoc.x - 1, snakeLoc.y);
            if (!spaces.Contains(leftSnake))
                leftSnake = new SPoint(snakeLoc.x + 1, snakeLoc.y);
            board[snakeLoc.y, snakeLoc.x] = Block.Snake;
            board[leftSnake.y, leftSnake.x] = Block.Snake;
            snake = new LinkedList<SPoint>();
            snake.AddLast(leftSnake);
            snake.AddLast(snakeLoc);
            isEnd = false;

            foodDuration = 0;
            starve = 150;
        }

        public Block[,] GetBoard()
        {
            return board;
        }

        public void GetInput()
        {
            throw new NotImplementedException();
        }

        public int GetInputAmount()
        {
            throw new NotImplementedException();
        }

        public int GetOutputAmount()
        {
            return 3;
        }

        public int GetScore()
        {
            return snake.Count;
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public float[] SetOutput()
        {
            throw new NotImplementedException();
        }

        public void Tick(float[] input)
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
                List<SPoint> spaces = new List<SPoint>();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (board[i, j] == Block.Blank) spaces.Add(new SPoint(j, i));
                    }
                }
                if (spaces.Count == 0)
                {
                    isEnd = true;
                    return;
                }
                SPoint foodPoint = spaces[Manager.random.Next(0, spaces.Count)];
                board[foodPoint.y, foodPoint.x] = Block.Food;
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
    }

    public enum Block
    {
        Snake, Blank, Food
    }
}
