using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    interface Game
    {
        void Tick(float[] input);
        int GetInputAmount();
        int GetOutputAmount();
        int GetScore();
        bool IsEnd();
        float[] SetOutput();
        void GetInput();
    }
}
