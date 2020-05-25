using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reinforcement
{
    public class Population<T> where T : Game
    {
        private NeuralNetwork[] m_networks;
        private T[] m_games;
        private CompareNetworks comparer;
        private Thread[] threads;
        public const int CPUs = 4;

        public Population(int popSize, int[] layers, T game)
        {
            m_networks = new NeuralNetwork[popSize];
            for (int i = 0; i < popSize; i++)
            {
                m_networks[i] = new NeuralNetwork(layers);
            }
            m_games = new T[CPUs];
            for (int i = 0; i < CPUs; i++)
            {
                m_games[i] = (T)game.GetNewGame();
            }
            comparer = new CompareNetworks();

            threads = new Thread[CPUs];
            int segmentSize = m_networks.Length / threads.Length;
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread((object o) =>
                {
                    int idx = (int)o;
                    int start = idx * segmentSize;
                    int dest = start + segmentSize;
                    for (int j = start; j < dest; j++)
                    {
                        TestNetwork(j, idx);
                    }
                });
            }
        }

        public void IncrementGeneration()
        {
            //Rate every neural network
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Start(i);
            }
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            //Sort the neural networks according to their average score
            Array.Sort(m_networks, comparer);
        }

        public void TestNetwork(int idx, int cpu)
        {
            T game = m_games[cpu];
            game.Reset();

            while (!game.isEnd)
            {
                game.Tick(m_networks[idx].FeedForward(game.SetOutput()));
            }
            m_networks[idx].AddScore(game.GetScore());
        }
    }
}
