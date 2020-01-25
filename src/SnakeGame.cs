using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    class SnakeGame : Game
    {
        private Block[,] board;
        private int foodDuration;
        private LinkedList<int> snake;
        private bool isEnd;

        public SnakeGame(int height, int width)
        {
            board = new Block[height, width];
            foodDuration = 0;
            snake = new LinkedList<int>();
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

        public float GetScore()
        {
            throw new NotImplementedException();
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public float[] SetOutput()
        {
            throw new NotImplementedException();
        }

        public void Tick()
        {
            throw new NotImplementedException();
        }
    }

    public enum Block
    {
        Snake, Blank, Food
    }
}
