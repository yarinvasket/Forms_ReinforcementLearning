using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reinforcement
{
    public partial class Manager : Form
    {
        private const int blockSize = 48;
        private const int height = 16, width = 16;
        private Label[][] board = new Label[height][];
        public static Random random = new Random();
        private SnakeGame game = new SnakeGame(height, width);
        private bool areCheatsEnabled;

        public Manager()
        {
            this.BackColor = Color.FromArgb(0, 0, 100);
            this.Size = new Size(blockSize * width + blockSize / 3, blockSize * height + (int)(blockSize / 1.25));
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
                    tmp.BackColor = blocks[i, j] == Block.Blank ? Color.FromName("Black") : blocks[i, j] == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
                    //tmp.Text = i + ", " + j;
                    tmp.Size = new Size(blockSize, blockSize);
                    tmp.Tag = new SPoint(j, i);
                    tmp.Click += LabelClick;
                    board[i][j] = tmp;
                    Controls.Add(tmp);
                }
            }

            Button[] buttons = new Button[3];
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

            Button toggleCheats = new Button();
            toggleCheats.Location = new Point(6 * blockSize, width * blockSize);
            toggleCheats.Size = new Size(blockSize * 2, blockSize);
            toggleCheats.BackColor = Color.FromName("White");
            toggleCheats.Click += toggleCheat;
            toggleCheats.Text = "Toggle Cheats";
            Controls.Add(toggleCheats);
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

            if (game.IsEnd() && !areCheatsEnabled)
            {
                MessageBox.Show("You lost!");
                game = new SnakeGame(height, width);
            }

            Block[,] blocks = game.GetBoard();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    board[i][j].BackColor = blocks[i, j] == Block.Blank ? Color.FromName("Black") : blocks[i, j] == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
                }
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
                    blocks[point.y, point.x] = Block.Snake;
                }
                else blocks[point.y, point.x]++;

                block = blocks[point.y, point.x];
                board[point.y][point.x].BackColor = block == Block.Blank ? Color.FromName("Black") : block == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
            }
        }
    }
}
