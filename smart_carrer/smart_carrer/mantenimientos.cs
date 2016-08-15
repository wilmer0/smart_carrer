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
    public partial class mantenimientos : Form
    {
        public mantenimientos()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Desea salir?", "Saliendo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            facultades fa = new facultades();
            fa.ShowDialog();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            carreras ca = new carreras();
            ca.ShowDialog();
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            tests te = new tests();
            te.ShowDialog();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            preguntas pre = new preguntas();
            pre.ShowDialog();
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            respuestas re = new respuestas();
            re.ShowDialog();
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            aptitudes ap = new aptitudes();
            ap.ShowDialog();
        }
    }
}
