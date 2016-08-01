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
    public partial class splashscreen : Form
    {
        public splashscreen()
        {
            InitializeComponent();
        }

        int cont = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
        
            if (cont == 2)//si dura x segundos
            {
                timer1.Stop();
                timer1.Enabled = false;
                this.Hide();
                menu me = new menu();
                me.ShowDialog();
               
            }
            else
            {
                cont++;
            }
            
        
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
          
        }

        private void splashscreen_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }
    }
}
