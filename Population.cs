using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinforcement
{
    public class Population<T> where T : Game
    {
        private NeuralNetwork[] m_networks;
        private T m_game;
        private CompareNetworks comparer;

        public Population(int popSize, int[] layers, T game)
        {
            m_networks = new NeuralNetwork[popSize];
            for (int i = 0; i < popSize; i++)
            {
                m_networks[i] = new NeuralNetwork(layers);
            }
            m_game = game;
            comparer = new CompareNetworks();
        }

        public void IncrementGeneration()
        {
            //Rate every neural network
            for (int i = 0; i < m_networks.Length; i++)
            {
                TestNetwork(i);
            }
            Array.Sort(m_networks, comparer);
        }

        public void TestNetwork(int idx)
        {
            m_game.Reset();

            while (!m_game.isEnd)
            {
                m_game.Tick(m_networks[idx].FeedForward(m_game.SetOutput()));
            }
            m_networks[idx].AddScore(m_game.GetScore());
        }
    }
}
