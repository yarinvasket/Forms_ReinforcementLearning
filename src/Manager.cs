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
        private const int blockSize = 64;
        private const int height = 16, width = 16;
        private Label[][] board = new Label[height][];
        public static Random random = new Random();

        public Manager()
        {
            this.BackColor = Color.FromName("Black");
            this.Size = new Size(blockSize * width + blockSize / 3, blockSize * height + (int)(blockSize / 1.25));
            SnakeGame game = new SnakeGame(height, width);
            Block[,] blocks = game.GetBoard();

            for (int i = 0; i < height; i++)
            {
                board[i] = new Label[width];
                for (int j = 0; j < width; j++)
                {
                    Label tmp = new Label();
                    tmp.Location = new Point(j * blockSize, i * blockSize);
                    tmp.BackColor = blocks[i, j] == Block.Blank ? Color.FromName("Red") : Color.FromName("White");
                    //tmp.Text = i + ", " + j;
                    tmp.Size = new Size(blockSize, blockSize);
                    board[i][j] = tmp;
                    Controls.Add(tmp);
                }
            }
        }
    }
}
