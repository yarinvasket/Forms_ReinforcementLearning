using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    interface Game
    {
        void Tick();
        int GetInputAmount();
        int GetOutputAmount();
        float GetScore();
        bool IsEnd();
        float[] SetOutput();
        void GetInput();
    }
}
