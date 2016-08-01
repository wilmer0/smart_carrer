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
            DialogResult dr = MessageBox.Show("Desea salir?", "Saliendo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Desea eliminar?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                    }
                    else
                    {
                        dr = MessageBox.Show("No hay elementos para eliminar", "Eliminando", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error eliminando la fila seleccionada");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int cont = 0;
            try
            {
                if (codigo_pregunta_txt.Text.Trim() != "")
                {
                    if (codigo_respuesta_pregunta_txt.Text.Trim() != "")
                    {

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == codigo_pregunta_txt.Text.Trim() && row.Cells[2].Value.ToString()==codigo_respuesta_pregunta_txt.Text.Trim())
                            {
                                cont++;
                            }
                        }
                        if (cont == 0)
                        {
                            dataGridView1.Rows.Add(codigo_pregunta_txt.Text.Trim(), descripcion_pregunta_txt.Text.Trim(), codigo_respuesta_pregunta_txt.Text.Trim(), descripcion_respuesta_pregunta_txt.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("La pregunta con esa respuesta ya se encuentra seleccionada");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Falta la respuesta");
                    }
                }
                else
                {
                    MessageBox.Show("Falta la pregunta");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error agregando la respuesta a la pregunta");
            }
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
                string sql = "select descripcion from respuestas where codigo='" + codigo_respuesta_txt.Text.Trim() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                descripcion_respuesta_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //try
            //{
                
                int estado = 0;
                if(descripcion_respuesta_txt.Text.Trim()!="")
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
                        string sql = "exec insert_respuesta '"+descripcion_respuesta_txt.Text.Trim()+"','"+estado.ToString()+"','0'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if(ds.Tables[0].Rows.Count>0)
                        {
                            MessageBox.Show("Se agrego");
                        }
                        else
                        {
                            MessageBox.Show("No se agrego");
                        }
                    }
                    else
                    {
                        //actualiza
                        /*
                  create proc insert_respuesta
                  @descripcion varchar(max),@estado int,@codigo int
                 */

                        string sql = "exec insert_respuesta '" + descripcion_respuesta_txt.Text.Trim() + "','" + estado.ToString() + "','"+codigo_respuesta_txt.Text.Trim()+"'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se actualizo");
                        }
                        else
                        {
                            MessageBox.Show("No se actualizo");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Falta la respuesta");
                }
                
            //}
            //catch(Exception)
            //{
            //    MessageBox.Show("Error");
            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
