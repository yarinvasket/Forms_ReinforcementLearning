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
        private float[][] neurons;
        private float[] connections;
        private int[] layers;

        public NeuralNetwork(int[] layerrs)
        {
            int len = layerrs.Length;
            layers = new int[len];
            for (int i = 0; i < len - 1; i++)
            {
                layers[i] = layerrs[i] + 1;
            }
            layers[len - 1] = layerrs[len - 1];

            neurons = new float[len][];

            for (int i = 0; i < len; i++)
            {
                neurons[i] = new float[layers[i]];
            }
        }
    }
}
