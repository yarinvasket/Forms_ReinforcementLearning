using System;
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

            //If the snake is heading up
            if (dy > 0)
            {
                EvaluateBoard(output, tmp.Value.y - 2, tmp.Value.x - 2, 1, 1);
            }
            //If the snake is heading down
            if (dy < 0)
            {
                EvaluateBoard(output, tmp.Value.y + 2, tmp.Value.x + 2, -1, -1);
                x = -x;
                y = -y;
            }
            //If the snake is heading left
            else if (dx > 0)
            {
                EvaluateBoard(output, tmp.Value.y + 2, tmp.Value.x - 2, 1, -1);
                int temp = x;
                x = y;
                y = -temp;
            }
            //If the snake is heading right
            else
            {
                EvaluateBoard(output, tmp.Value.y - 2, tmp.Value.x + 2, -1, 1);
                int temp = x;
                x = -y;
                y = temp;
            }

            output[0] = (float)Math.Atan2(y, x);
            return output;
        }

        public void EvaluateBoard(float[] arr, int startY, int startX, int dy, int dx)
        {
            int endY = startY + dy * 5;
            for (int i = startY; i != endY; i += dy)
            {
                int row = dy > 0 ? (i - startY) * 5 : (startY - i) * 5;
                if (i < 0 || i >= height)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        arr[row + j] = 1;
                    }
                    continue;
                }
                int endX = startX + dx * 5;
                for (int j = startX; j != endX; j += dx)
                {
                    int column = dx > 0 ? j - startX : startX - j;
                    if (j < 0 || j >= width)
                    {
                        arr[row + column] = 1;
                        continue;
                    }
                    arr[row + column] = board[i, j] == Block.Snake ? 1 : 0;
                }
            }
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
            SPoint foodPoint = spaces[random.Next(0, spaces.Count)];
            board[foodPoint.y, foodPoint.x] = Block.Food;
            foodX = foodPoint.x;
            foodY = foodPoint.y;
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
