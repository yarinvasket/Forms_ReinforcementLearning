using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    [Serializable]
    public class NeuralNetwork
    {
        private InputNeuron[] m_inputNeurons;
        private HiddenNeuron[][] m_hiddenNeurons;
        private OutputNeuron[] outputNeurons;
        private int[] m_layers;

        public NeuralNetwork(int[] layers, float[] connections, float[] biases)
        {

        }
    }
}
