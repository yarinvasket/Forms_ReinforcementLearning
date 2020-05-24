using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reinforcement
{
    [Serializable]
    public class NeuralNetwork
    {
        private InputNeuron[] m_inputNeurons;
        private HiddenNeuron[][] m_hiddenNeurons;
        private OutputNeuron[] m_outputNeurons;
        private int[] m_layers;

        /// <summary>
        /// The constructor for initializing a new neural network
        /// </summary>
        /// <param name="layers"></param>
        /// <param name="connections"></param>
        /// <param name="biases"></param>
        public NeuralNetwork(int[] layers, float[] connections, float[] biases)
        {
            m_layers = layers;
            m_inputNeurons = new InputNeuron[layers[0]];
            m_hiddenNeurons = new HiddenNeuron[m_layers.Length - 2][];
            m_outputNeurons = new OutputNeuron[m_layers[m_layers.Length - 1]];
            int biasesIdx = 0;
            int connectionsIdx = 0;

            //Input neurons loop
            for (int i = 0; i < m_layers[0]; i++)
            {
                int nextLayer = m_layers[1];
                m_inputNeurons[i] = new InputNeuron(nextLayer);

                for (int j = 0; j < nextLayer; j++)
                {
                    m_inputNeurons[i].connections[j] = connections[connectionsIdx++];
                }
            }

            //Hidden neurons loop
            for (int i = 0; i < m_hiddenNeurons.Length; i++)
            {
                int layer = m_layers[i + 1];
                m_hiddenNeurons[i] = new HiddenNeuron[layer];
                for (int j = 0; j < layer; j++)
                {
                    int nextLayer = m_layers[i + 2];
                    m_hiddenNeurons[i][j] = new HiddenNeuron(nextLayer, biases[biasesIdx++]);
                    for (int k = 0; k < nextLayer; k++)
                    {
                        m_hiddenNeurons[i][j].connections[k] = connections[connectionsIdx++];
                    }
                }
            }

            //Output neurons loop
            for (int i = 0; i < m_layers[m_layers.Length - 1]; i++)
            {
                m_outputNeurons[i] = new OutputNeuron(biases[biasesIdx++]);
            }
        }

        /// <summary>
        /// Takes another neural network and changes random values in it
        /// </summary>
        /// <param name="nn"></param>
        public NeuralNetwork(NeuralNetwork nn)
        {

        }

        /// <summary>
        /// Feeds input to the network
        /// </summary>
        /// <param name="input"></param>
        /// <returns>The output layer</returns>
        public float[] FeedForward(float[] input)
        {
            //Feed the input layer
            int inputLayer = m_layers[0];
            for (int i = 0; i < inputLayer; i++)
            {
                m_inputNeurons[i].value = input[i];
            }

            //Feed the first layer of the hidden neurons
            int layer = m_layers[1];
            for (int i = 0; i < layer; i++)
            {
                float sum = 0;
                for (int j = 0; j < inputLayer; j++)
                {
                    sum += m_inputNeurons[j].GetWeight(i);
                }
                m_hiddenNeurons[0][i].Sigmoid(sum);
            }

            //Feed the rest of the hidden neurons
            for (int i = 1; i < m_hiddenNeurons.Length; i++)
            {
                int layer2 = m_layers[i + 1];
                for (int j = 0; j < layer2; j++)
                {
                    int prevLayer = m_layers[i];
                    float sum = 0;
                    for (int k = 0; k < prevLayer; k++)
                    {
                        sum += m_hiddenNeurons[i][k].GetWeight(j);
                    }
                    m_hiddenNeurons[i][j].Sigmoid(sum);
                }
            }

            //Feed the output layer
            int outputLayer = m_layers[m_layers.Length - 1];
            int beforeOutputIdx = m_layers.Length - 2;
            int beforeOutputLayer = m_layers[beforeOutputIdx];
            float[] output = new float[outputLayer];
            for (int i = 0; i < outputLayer; i++)
            {
                float sum = 0;
                for (int j = 0; j < beforeOutputLayer; j++)
                {
                    sum += m_hiddenNeurons[beforeOutputIdx][j].GetWeight(i);
                }
                m_outputNeurons[i].Sigmoid(sum);
                output[i] = m_outputNeurons[i].value;
            }

            return output;
        }
    }
}
