using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface
{
    public partial class Menu : Form
    {
        string[] modes = { "", "1P vs 1P", "1P vs COM" };
        string [] levels = {"", "" , "Easy", "Medium", "Hard"};
        int selectedLevel;
        int selectedMode;
        public Menu()
        {
            InitializeComponent();
            this.selectedLevel = 3;
            this.selectedMode = 1;
            label5.Text = levels[this.selectedLevel];
            label5.Visible = false;
            label4.Visible = false;
            label3.Visible = false;
            label6.Visible = false;
            label9.Text = modes[this.selectedMode];
            
        }
      
        private void Form1_Load(object sender, EventArgs e)
        {            
                             
        }

        private void Menu_Paint(object sender, PaintEventArgs e)
        {
            Image image = Image.FromFile(Application.StartupPath + "\\images\\Background.jpg");
            e.Graphics.DrawImage(image, 0, 0);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Game game = new Game(selectedLevel, selectedMode, this);                       
            this.Hide();
            game.ShowDialog();
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Red;
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.Purple;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (selectedLevel - 1 > 1)
                label5.Text = this.levels[--this.selectedLevel];
            else
            {
                this.selectedLevel = 4;
                label5.Text = this.levels[this.selectedLevel];
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (selectedLevel + 1 < 5 )
                label5.Text = this.levels[++this.selectedLevel];
            else
            {
                this.selectedLevel = 2;
                label5.Text = this.levels[this.selectedLevel];
                
            }
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Red;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Purple;
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Red;
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            label6.ForeColor = Color.Purple;
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Red;
        }

        private void label7_MouseLeave(object sender, EventArgs e)
        {
            label7.ForeColor = Color.Purple;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (this.selectedMode - 1 > 0)
                label9.Text = this.modes[--this.selectedMode];
            else
            {
                this.selectedMode = 2;
                label9.Text = this.modes[this.selectedMode];
            }
            if (this.selectedMode == 2)
            {
                label5.Visible = true;
                label4.Visible = true;
                label3.Visible = true;
                label6.Visible = true;
                this.selectedLevel = 2;
                label5.Text = this.levels[this.selectedLevel];
            }
            else
            {
                label5.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            if (this.selectedMode + 1 < 3)
                label9.Text = this.modes[++this.selectedMode];
            else
            {
                this.selectedMode = 1;
                label9.Text = this.modes[this.selectedMode];
            }
            if (this.selectedMode == 2)
            {
                label5.Visible = true;
                label4.Visible = true;
                label3.Visible = true;
                label6.Visible = true;
                this.selectedLevel = 2;
                label5.Text = this.levels[this.selectedLevel];
            }
            else
            {
                label5.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                label6.Visible = false;
            }
        }

        private void label10_MouseEnter(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Red;
        }

        private void label10_MouseLeave(object sender, EventArgs e)
        {
            label10.ForeColor = Color.Purple;
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Red;
        }

        private void label8_MouseLeave(object sender, EventArgs e)
        {
            label8.ForeColor = Color.Purple;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
