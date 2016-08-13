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
    public partial class aptitudes : Form
    {
        public aptitudes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
                create proc insert_aptitudes
                @nombre varchar(max),@descripcion varchar(150),@estado int,@codigo int
             */
            bool procesar = validarCampos();
            if(procesar)
            {
                int activo = 0;
                if (ck_estado.Checked == true)
                    activo = 1;
                else
                    activo = 0;

                if(codigo_txt.Text.Trim()=="")
                {
                    //guarda
                    string sql = "exec insert_aptitudes '"+nombre_txt.Text.Trim()+"','"+descripcion_txt.Text.Trim()+"','"+ck_estado.ToString()+"','0'";
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    if(ds.Tables[0].Rows.Count>0)
                    {
                        MessageBox.Show("Se agrego la aptitup", "", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    }
                    else
                    {
                        MessageBox.Show("No se agrego la aptitup", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    }
                    
                }
                else
                {
                    //modifica
                    string sql = "exec insert_aptitudes '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + ck_estado.ToString() + "','"+codigo_txt.Text.Trim()+"'";
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Se actualizo la aptitup", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se actualizo la aptitup", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }

            }

        }
        public Boolean validarCampos()
        {
            if(nombre_txt.Text.Trim()=="")
            {
                nombre_txt.Focus();
                MessageBox.Show("Campo del nombre esta vacio.","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
            

            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            busqueda_aptitudes ba = new busqueda_aptitudes();
            ba.pasado += new busqueda_aptitudes.pasar(ejecutar_codigo_aptitud);
            ba.ShowDialog();
        }
        public void ejecutar_codigo_aptitud(string dato)
        {
            codigo_txt.Text = dato.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            codigo_txt.Clear();
        }
    }
}
