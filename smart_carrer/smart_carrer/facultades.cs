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
    public partial class facultades : Form
    {
        public facultades()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int estado = 0;
                if (nombre_txt.Text.Trim() != "")
                {
                    if (codigo_txt.Text.Trim() == "")
                    {
                        //guardar
                        /*
                         create proc insert_facultad
                         @nombre varchar(max),@descripcion varchar(max),@estado int,@codigo int
                        */
                        if (ck_estado.Checked == true)
                        {
                            estado = 1;
                        }
                        else
                        {
                            estado = 0;
                        }
                        string sql = "exec insert_facultad '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + estado.ToString() + "','0'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se agrego!");
                            codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se agrego!");
                        }
                    }
                    else
                    {
                        //actualizar
                        /*
                         create proc insert_facultad
                         @nombre varchar(max),@descripcion varchar(max),@estado int,@codigo int
                        */
                        if (ck_estado.Checked == true)
                        {
                            estado = 1;
                        }
                        else
                        {
                            estado = 0;
                        }
                        string sql = "exec insert_facultad '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + estado.ToString() + "','" + codigo_txt.Text.Trim() + "'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se actualizo!");
                            codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se actualizo!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Falta el nombre");
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error agregando o ya existe");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            busqueda_facultad bf = new busqueda_facultad();
            bf.mantenimiento = true;
            bf.pasado += new busqueda_facultad.pasar(ejecutar_codigo_facultad);
            bf.ShowDialog();
            cargar_datos();
        }
        public void cargar_datos()
        {
            if(codigo_txt.Text.Trim()!="")
            {
                string sql = "select nombre,descripcion,estado from facultades where codigo='"+codigo_txt.Text.Trim()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if(ds.Tables[0].Rows[0][0].ToString()!="")
                {
                    nombre_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                    descripcion_txt.Text = ds.Tables[0].Rows[0][1].ToString();
                    if(ds.Tables[0].Rows[0][2].ToString()=="1")
                    {
                        ck_estado.Checked = true;
                    }
                    else
                    {
                        ck_estado.Checked = false;
                    }
                }
            }
        }
        public void ejecutar_codigo_facultad(string dato)
        {
            codigo_txt.Text = dato.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            codigo_txt.Clear();
            nombre_txt.Clear();
            descripcion_txt.Clear();
            
            ck_estado.Checked = true;
        }

        private void facultades_Load(object sender, EventArgs e)
        {

        }
    }
}
