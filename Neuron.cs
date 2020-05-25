using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    public class Neuron
    {
        public float value;

        public static void PinchValue(ref float value, float range)
        {
            value += NeuralNetwork.GetRandomNeuronValue(range);
            float bound = range * 5;
            if (value > bound)
            {
                value = bound;
            }
            else if (value < -bound)
            {
                value = -bound;
            }
        }
    }

    public class InputNeuron : Neuron
    {
        public float[] connections;

        public InputNeuron(int length)
        {
            connections = new float[length];
        }

        /// <summary>
        /// Copy with mutations
        /// </summary>
        /// <param name="other"></param>
        public InputNeuron(InputNeuron other)
        {
            connections = new float[other.connections.Length];
            for (int i = 0; i < connections.Length; i++)
            {
                connections[i] = other.connections[i];
                if (Manager.random.Next(0, 10) == 0)
                {
                    PinchValue(ref connections[i], 0.2f);
                }
            }
        }

        public float GetWeight(int idx)
        {
            return value * connections[idx];
        }
    }

    public class HiddenNeuron : Neuron
    {
        public float[] connections;
        public float bias;

        public HiddenNeuron(int length, float bias)
        {
            connections = new float[length];
            this.bias = bias;
        }

        /// <summary>
        /// Copy with mutations
        /// </summary>
        /// <param name="other"></param>
        public HiddenNeuron(HiddenNeuron other)
        {
            connections = new float[other.connections.Length];
            for (int i = 0; i < connections.Length; i++)
            {
                connections[i] = other.connections[i];
                if (Manager.random.Next(0, 10) == 0)
                {
                    PinchValue(ref connections[i], 0.2f);
                }
            }
            bias = other.bias;
            if (Manager.random.Next(0, 10) == 0)
            {
                PinchValue(ref bias, 1);
            }
        }

        public void Sigmoid(float value)
        {
            this.value = 1 / (1 + (float)Math.Exp(bias - value));
        }

        public float GetWeight(int idx)
        {
            return value * connections[idx];
        }
    }

    public class OutputNeuron : Neuron
    {
        public float bias;

        public OutputNeuron(float bias)
        {
            this.bias = bias;
        }

        /// <summary>
        /// Copy with mutations
        /// </summary>
        /// <param name="other"></param>
        public OutputNeuron(OutputNeuron other)
        {
            bias = other.bias;
            if (Manager.random.Next(0, 10) == 0)
            {
                PinchValue(ref bias, 1);
            }
        }

        public void Sigmoid(float value)
        {
            this.value = 1 / (1 + (float)Math.Exp(bias - value));
        }
    }
}
