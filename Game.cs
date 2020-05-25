using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    public abstract class Game
    {
        public int inputAmount;
        public int outputAmount;
        public bool isEnd;

        public abstract void Tick(float[] input);
        public abstract int GetScore();
        public abstract float[] SetOutput();
        public abstract void Reset();
        public abstract Game GetNewGame();
    }
}
