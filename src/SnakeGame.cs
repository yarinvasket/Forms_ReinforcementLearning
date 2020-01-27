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
        private HashSet<SPoint> spaces;
        private int foodDuration;
        private bool isEnd;

        public SnakeGame(int height, int width)
        {
            this.height = height;
            this.width = width;
            board = new Block[height, width];
            spaces = new HashSet<SPoint>();

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

            SPoint snakeLoc = spaces.ToArray()[Manager.random.Next(0, spaces.Count)];
            board[snakeLoc.y, snakeLoc.x] = Block.Snake;
            spaces.Remove(snakeLoc);
            snake = new LinkedList<SPoint>();
            snake.AddLast(snakeLoc);

            foodDuration = 0;
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
        }
    }

    public enum Block
    {
        Snake, Blank, Food
    }
}
