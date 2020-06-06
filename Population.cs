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
        private NeuralNetwork bestNetwork;
        public int generation;
        private T[] m_games;
        private CompareNetworks comparer;
        private Thread[] threads;
        private bool started = false;
        public const int CPUs = 8;

        public Population(int popSize, int[] layers, T game)
        {
            m_networks = new NeuralNetwork[popSize];
            for (int i = 0; i < popSize; i++)
            {
                m_networks[i] = new NeuralNetwork(layers);
            }
            generation = 0;
            m_games = new T[CPUs];
            for (int i = 0; i < CPUs; i++)
            {
                    m_games[i] = (T)game.GetNewGame();
            }
            comparer = new CompareNetworks();

            threads = new Thread[CPUs];
        }

        public void IncrementGeneration()
        {
            generation++;
            //Recreate the threads
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
            bestNetwork = m_networks[0];
            started = true;

            int creaturesCreated = 0;
            int half = m_networks.Length / 2;
            float ratio = 2f / half;
            for (int i = 0; i < half; i++)
            {
                if (creaturesCreated >= half) break;

                int children = (int)Math.Round(ratio * (half - i));
                for (int j = 0; j < children; j++)
                {
                    m_networks[half + creaturesCreated++] = new NeuralNetwork(m_networks[i]);
                }
            }
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

        public NeuralNetwork GetBestNeuralNetwork()
        {
            while (!started) {}
            return NeuralNetwork.NoMutations(bestNetwork);
        }
    }
}
