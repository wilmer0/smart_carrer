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
    public partial class respuestas : Form
    {
        public respuestas()
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

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
                busqueda_respuesta bc = new busqueda_respuesta();
                bc.mantenimiento = true;
                bc.pasado += new busqueda_respuesta.pasar(ejecutar_codigo_respuesta);
                bc.ShowDialog();
                cargar_respuesta();
            
        }
        public void ejecutar_codigo_respuesta(string dato)
        {
            codigo_respuesta_txt.Text = dato.ToString();
        }
        public void cargar_respuesta()
        {
            if (codigo_respuesta_txt.Text.Trim() != "")
            {
                string sql = "select descripcion,estado from respuestas where codigo='" + codigo_respuesta_txt.Text.Trim() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                descripcion_respuesta.Text = ds.Tables[0].Rows[0][0].ToString();
                if(ds.Tables[0].Rows[0][1].ToString()=="1")
                {
                    ck_estado_respuesta.Checked = true;
                }
                else
                {
                    ck_estado_respuesta.Checked = false;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                
                int estado = 0;
                if(descripcion_respuesta.Text.Trim()!="")
                {
                    if(ck_estado_respuesta.Checked==true)
                    {
                        estado = 1;
                    }
                    else
                    {
                        estado = 0;
                    }
                    if(codigo_respuesta_txt.Text.Trim()=="")
                    {
                        //guardar
                        /*
                  create proc insert_respuesta
                  @descripcion varchar(max),@estado int,@codigo int
                 */
                        string sql = "exec insert_respuesta '"+descripcion_respuesta.Text.Trim()+"','"+estado.ToString()+"','0'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if(ds.Tables[0].Rows.Count>0)
                        {
                            MessageBox.Show("Se agrego la respuesta", "", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        }
                        else
                        {
                            MessageBox.Show("No se agrego la respuesta", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        }
                    }
                    else
                    {
                        //actualiza
                        /*
                  create proc insert_respuesta
                  @descripcion varchar(max),@estado int,@codigo int
                 */

                        string sql = "exec insert_respuesta '" + descripcion_respuesta.Text.Trim() + "','" + estado.ToString() + "','"+codigo_respuesta_txt.Text.Trim()+"'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se actualizo la respuesta", "", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                        }
                        else
                        {
                            MessageBox.Show("No se actualizo la respuesta", "", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Falta la respuesta", "", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error :"+ex.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
