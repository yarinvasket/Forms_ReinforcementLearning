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
            HashSet<SPoint> spaces = new HashSet<SPoint>();

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

            SPoint snakeLoc = spaces.ToArray()[Manager.random.Next(0, spaces.Count)];
            SPoint leftSnake = new SPoint(snakeLoc.x - 1, snakeLoc.y);
            if (!spaces.Contains(leftSnake))
                leftSnake = new SPoint(snakeLoc.x + 1, snakeLoc.y);
            board[snakeLoc.y, snakeLoc.x] = Block.Snake;
            board[leftSnake.y, leftSnake.x] = Block.Snake;
            spaces.Remove(snakeLoc);
            spaces.Remove(leftSnake);
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
            Block block = board[newY, newX];
            if (newX >= width || newY >= height)
            {
                isEnd = true;
                return;
            }
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
            }
            else
            {
                if (foodDuration <= 0)
                {
                    SPoint tail = snake.Last.Value;
                    board[tail.y, tail.x] = Block.Blank;
                    snake.RemoveLast();
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
