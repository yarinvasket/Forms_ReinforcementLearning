using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Reinforcement
{
    public class ManagerAI : Manager
    {
        public static Population<SnakeGame> population;
        protected Label generationLabel;
        protected NeuralNetwork network;
        protected Timer timer;
        public const int populationSize = 1000;

        public ManagerAI() : base()
        {
            Load += (object sender, EventArgs e) =>
            {
                generationLabel.BringToFront();
            };

            Shown += ManagerAI_Shown;

            for (int i = 0; i < buttons.Length; i++)
            {
                Controls.Remove(buttons[i]);
            }
            buttons = null;
            Controls.Remove(toggleCheats);
            toggleCheats = null;

            int[] layers = { game.inputAmount, 30, game.outputAmount };
            population = new Population<SnakeGame>(populationSize, layers, game);
            Thread proceedGenerations = new Thread(() =>
            {
                while (true)
                {
                    population.IncrementGeneration();
                }
            });
            proceedGenerations.Start();

            network = population.GetBestNeuralNetwork();
            generationLabel = new Label
            {
                Size = new Size(blockSize, blockSize),
                Location = new Point(blockSize / 2, blockSize / 2),
                Text = population.generation.ToString(),
                ForeColor = Color.White,
                Font = new Font(FontFamily.GenericMonospace, blockSize / 4),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(generationLabel);
        }

        private void ManagerAI_Shown(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 100; //Delay between each turn
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!game.isEnd)
            {
                float[] input = game.SetOutput();
                float[] output = network.FeedForward(input);
                game.Tick(output);

                SPoint[] diffs = game.GetDiff();
                foreach (SPoint diff in diffs)
                {
                    if (diff.y >= height || diff.x >= width || diff.x < 0 || diff.y < 0)
                        continue;
                    board[diff.y][diff.x].BackColor = GetBlockColor(game.GetBoard()[diff.y, diff.x]);
                }
            }
            else
            {
                game = new SnakeGame(height, width);
                Block[,] blocks = game.GetBoard();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        board[i][j].BackColor = GetBlockColor(blocks[i, j]);
                    }
                }
                network = population.GetBestNeuralNetwork();
                generationLabel.Text = population.generation.ToString();
            }
        }
    }
}
