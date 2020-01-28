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

        public Manager()
        {
            this.BackColor = Color.FromName("Black");
            this.Size = new Size(blockSize * width + blockSize / 3, blockSize * height + (int)(blockSize / 1.25));
            this.Text = "Snake";
            Block[,] blocks = game.GetBoard();

            for (int i = 0; i < height; i++)
            {
                board[i] = new Label[width];
                for (int j = 0; j < width; j++)
                {
                    Label tmp = new Label();
                    tmp.Location = new Point(j * blockSize, i * blockSize);
                    tmp.BackColor = blocks[i, j] == Block.Blank ? Color.FromName("Gray") : blocks[i, j] == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
                    //tmp.Text = i + ", " + j;
                    tmp.Size = new Size(blockSize, blockSize);
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

            Block[,] blocks = game.GetBoard();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    board[i][j].BackColor = blocks[i, j] == Block.Blank ? Color.FromName("Gray") : blocks[i, j] == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
                }
            }

            if (game.IsEnd())
                MessageBox.Show("You lost!");
        }
    }
}
