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
    public partial class preguntas : Form
    {
        public preguntas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int estado = 0;
               
                    if (descripcion_txt.Text.Trim() != "")
                    {
                        if (codigo_txt.Text.Trim() == "")
                        {
                            //guardar
                            /*
                            create proc insert_pregunta
                            @descripcion varchar(max),@cod_test int,@estado int,@codigo int
                             */
                            if (ck_estado.Checked == true)
                            {
                                estado = 1;
                            }
                            else
                            {
                                estado = 0;
                            }
                            string sql = "exec insert_pregunta '" + descripcion_txt.Text.Trim() +"','" + estado.ToString() + "','0'";
                            DataSet ds = utilidades.ejecutarcomando(sql);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                MessageBox.Show("Se guardo!");
                                codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se guardo");
                            }
                        }
                        else
                        {
                            //actualiza
                            /*
                                create proc insert_pregunta
                                @descripcion varchar(max),@cod_test int,@estado int,@codigo int
                             */
                            if (ck_estado.Checked == true)
                            {
                                estado = 1;
                            }
                            else
                            {
                                estado = 0;
                            }
                            string sql = "exec insert_pregunta '" + descripcion_txt.Text.Trim() + "','" + estado.ToString() + "','" + codigo_txt.Text.Trim() + "'";
                            DataSet ds = utilidades.ejecutarcomando(sql);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                MessageBox.Show("Se actualizo!");
                                codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se actualizo");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Falta establecer la pregunta");
                    }
               
            }
            catch(Exception)
            {
                MessageBox.Show("Error agregando o ya existe la pregunta");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            busqueda_pregunta bp = new busqueda_pregunta();
            bp.mantenimiento = true;
            bp.pasado += new busqueda_pregunta.pasar(ejecutar_codigo_pregunta);
            bp.ShowDialog();
            cargar_datos_pregunta();
        }
        public void ejecutar_codigo_pregunta(string dato)
        {
            codigo_txt.Text = dato.ToString();
        }
        public void cargar_datos_pregunta()
        {
            if(codigo_txt.Text.Trim()!="")
            {
                string sql="select descripcion from preguntas where codigo='"+codigo_txt.Text.Trim()+"'";
                DataSet ds=utilidades.ejecutarcomando(sql);
                descripcion_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }
       

        private void label5_Click(object sender, EventArgs e)
        {
           
        }

        private void label4_Click(object sender, EventArgs e)
        {
            codigo_txt.Clear();
            descripcion_txt.Clear();
            ck_estado.Checked = true;
        }

        private void preguntas_Load(object sender, EventArgs e)
        {

        }
    }
}
