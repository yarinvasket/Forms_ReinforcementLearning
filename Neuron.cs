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

        public static void Mutate(ref float value, float range)
        {
            if (Manager.random.Next(0, 10) == 0)
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
        public static InputNeuron WithMutations(InputNeuron other)
        {
            InputNeuron ret = new InputNeuron(other.connections.Length);
            for (int i = 0; i < ret.connections.Length; i++)
            {
                ret.connections[i] = other.connections[i];
                Mutate(ref ret.connections[i], 0.4f);
            }
            return ret;
        }

        public static InputNeuron NoMutations(InputNeuron other)
        {
            InputNeuron ret = new InputNeuron(other.connections.Length);
            for (int i = 0; i < ret.connections.Length; i++)
            {
                ret.connections[i] = other.connections[i];
            }
            return ret;
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
        public static HiddenNeuron WithMutations(HiddenNeuron other)
        {
            HiddenNeuron ret = new HiddenNeuron(other.connections.Length, other.bias);
            for (int i = 0; i < ret.connections.Length; i++)
            {
                ret.connections[i] = other.connections[i];
                Mutate(ref ret.connections[i], 0.4f);
            }
            ret.bias = other.bias;
            Mutate(ref ret.bias, 2);
            return ret;
        }

        /// <summary>
        /// Copy without mutations
        /// </summary>
        /// <param name="other"></param>
        public static HiddenNeuron NoMutations(HiddenNeuron other)
        {
            HiddenNeuron ret = new HiddenNeuron(other.connections.Length, other.bias);
            for (int i = 0; i < ret.connections.Length; i++)
            {
                ret.connections[i] = other.connections[i];
            }
            ret.bias = other.bias;
            return ret;
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
        public static OutputNeuron WithMutations(OutputNeuron other)
        {
            OutputNeuron ret = new OutputNeuron(other.bias);
            Mutate(ref ret.bias, 2);
            return ret;
        }

        public static OutputNeuron NoMutations(OutputNeuron other)
        {
            return new OutputNeuron(other.bias);
        }

        public void Sigmoid(float value)
        {
            this.value = 1 / (1 + (float)Math.Exp(bias - value));
        }
    }
}
