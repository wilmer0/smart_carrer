using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace smart_carrer
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Desea salir?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            mantenimientos m = new mantenimientos();
            m.ShowDialog();
        }

        private void menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void menu_Load(object sender, EventArgs e)
        {
          
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void menu_Click(object sender, EventArgs e)
        {
            hacer_test ht = new hacer_test();
            ht.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            hacer_test ht = new hacer_test();
            ht.ShowDialog();
        }
    }
}
