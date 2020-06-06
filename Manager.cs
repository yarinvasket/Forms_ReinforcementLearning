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

namespace Reinforcement
{
    public partial class Manager : Form
    {
        protected const int blockSize = 25;
        protected const int height = 20, width = 20;
        protected Label[][] board = new Label[height][];
        public static Random random = new Random();
        protected SnakeGame game = new SnakeGame(height, width);
        protected bool areCheatsEnabled;
        protected Button[] buttons;
        protected Button toggleCheats;

        public Manager()
        {
            this.BackColor = Color.FromArgb(0, 0, 100);
            this.Size = new Size(blockSize * width + blockSize, blockSize * height + blockSize * 2);
            this.Text = "Snake";
            Block[,] blocks = game.GetBoard();
            areCheatsEnabled = false;

            for (int i = 0; i < height; i++)
            {
                board[i] = new Label[width];
                for (int j = 0; j < width; j++)
                {
                    Label tmp = new Label();
                    tmp.Location = new Point(j * blockSize, i * blockSize);
                    tmp.BackColor = GetBlockColor(blocks[i, j]);
                    //tmp.Text = i + ", " + j;
                    tmp.Size = new Size(blockSize, blockSize);
                    tmp.Tag = new SPoint(j, i);
                    tmp.Click += LabelClick;
                    board[i][j] = tmp;
                    Controls.Add(tmp);
                }
            }

            buttons = new Button[3];
            for (int i = 0; i < 3; i++)
            {
                buttons[i] = new Button();
                buttons[i].Location = new Point(i * blockSize * 2, width * blockSize);
                buttons[i].Size = new Size(blockSize * 2, blockSize);
                buttons[i].BackColor = Color.FromName("White");
                buttons[i].Tag = i;
                buttons[i].Click += buttonClick;
                Controls.Add(buttons[i]);
            }
            buttons[0].Text = "Up";
            buttons[1].Text = "Left";
            buttons[2].Text = "Right";

            toggleCheats = new Button();
            toggleCheats.Location = new Point(6 * blockSize, width * blockSize);
            toggleCheats.Size = new Size(blockSize * 2, blockSize);
            toggleCheats.BackColor = Color.FromName("White");
            toggleCheats.Click += toggleCheat;
            toggleCheats.Text = "Toggle Cheats";
            Controls.Add(toggleCheats);

            FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                Environment.Exit(Environment.ExitCode);
            };
        }

        public void buttonClick(object sender, EventArgs e)
        {
            Button snder = (Button)sender;
            int active = (int)snder.Tag;
            float[] input = new float[3];
            for (int i = 0; i < 3; i++)
            {
                input[i] = 0;
            }
            input[active] = 1;
            game.Tick(input);

            if (game.isEnd && !areCheatsEnabled)
            {
                MessageBox.Show("You lost!", "Game Over!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                game = new SnakeGame(height, width);
                Block[,] blocks = game.GetBoard();
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        board[i][j].BackColor = GetBlockColor(blocks[i, j]);
                    }
                }
                return;
            }

            SPoint[] diffs = game.GetDiff();
            foreach (SPoint diff in diffs)
            {
                if (diff.y >= height || diff.x >= width || diff.x < 0 || diff.y < 0)
                    continue;
                board[diff.y][diff.x].BackColor = GetBlockColor(game.GetBoard()[diff.y, diff.x]);
            }
        }

        public void toggleCheat(object sender, EventArgs e)
        {
            areCheatsEnabled = !areCheatsEnabled;
        }

        public void LabelClick(object sender, EventArgs e)
        {
            if (areCheatsEnabled)
            {
                Label label = (Label)sender;
                SPoint point = (SPoint)label.Tag;
                Block[,] blocks = game.GetBoard();
                Block block = blocks[point.y, point.x];
                if (block == Block.Food)
                {
                    blocks[point.y, point.x] = Block.Blank;
                }
                else blocks[point.y, point.x]++;

                block = blocks[point.y, point.x];
                board[point.y][point.x].BackColor = GetBlockColor(block);
            }
        }

        public Color GetBlockColor(Block block)
        {
            return block == Block.Blank ? Color.FromName("Black") : block == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
        }
    }
}
