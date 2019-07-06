using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessEngine;
namespace UserInterface
{
    public partial class PawnPromotion : Form
    {
        public PieceType type;

        public PawnPromotion()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Name = "Pawn Promotion";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.type = PieceType.ROOK;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.LightBlue;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Goldenrod;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.type = PieceType.KNIGHT;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }
    
        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.Goldenrod;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.LightBlue;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.type = PieceType.BISHOP;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.LightBlue;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.BackColor = Color.Goldenrod;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.type = PieceType.QUEEN;
            this.Close();
            this.DialogResult = DialogResult.OK;
        }     

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.Goldenrod;
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            pictureBox4.BackColor = Color.LightBlue;
        }
    }
}
