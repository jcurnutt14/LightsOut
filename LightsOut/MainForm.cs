using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {

        private const int GridOffset = 25;      // Distance from upper-left side of window
        private const int GridLength = 200;     // Size in pixels of grid
        private int CellLength;
        private LightsOutGame lightsOutGame;

        public MainForm()
        {
            InitializeComponent();

            lightsOutGame = new LightsOutGame();
            CellLength = GridLength / lightsOutGame.GridSize;
           
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < lightsOutGame.GridSize; r++)
            {
                for (int c = 0; c < lightsOutGame.GridSize; c++)
                {
                    // Get proper pen and brush for on/off
                    // grid section
                    Brush brush;
                    Pen pen;
                    if (lightsOutGame.GetGridValue(r, c))
                    {
                        pen = Pens.Black;
                        brush = Brushes.White; // On
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black; // Off
                    }
                    // Determine (x,y) coord of row and col to draw rectangle
                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;
                    // Draw outline and inner rectangle
                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, x + 1, y + 1, CellLength - 1, CellLength - 1);
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Make sure click was inside the grid
            if (e.X < GridOffset || e.X > CellLength * lightsOutGame.GridSize + GridOffset ||
            e.Y < GridOffset || e.Y > CellLength * lightsOutGame.GridSize + GridOffset)
                return;

            // Find row, col of mouse press
            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            // Invert selected box and all surrounding boxes
            lightsOutGame.Move(r, c);

            // Redraw grid
            this.Invalidate();
            // Check to see if puzzle has been solved
            if (PlayerWon())
            {
                // Display winner dialog box
                MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool PlayerWon()
        {
            return lightsOutGame.IsGameOver();
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            lightsOutGame.NewGame();

            // Redraw grid
            this.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void about_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
    }
}
