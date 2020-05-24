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
    }

    public class InputNeuron : Neuron
    {
        public float[] connections;

        public InputNeuron(int length)
        {
            connections = new float[length];
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

        public void Sigmoid(float value)
        {
            this.value = 1 / (1 + (float)Math.Exp(bias - value));
        }
    }
}
