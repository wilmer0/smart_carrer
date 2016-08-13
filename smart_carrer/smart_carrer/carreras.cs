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
    public partial class carreras : Form
    {
        public carreras()
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
                    if (codigo_facultad_txt.Text.Trim() != "")
                    {
                        if (codigo_txt.Text.Trim() == "")
                        {
                            //guardar
                            /*
                            create proc insert_carrera
                            @nombre varchar(max),@descripcion varchar(max),@cod_facultad int,@estado int,@codigo int

                            */
                            if (ck_estado.Checked == true)
                            {
                                estado = 1;
                            }
                            else
                            {
                                estado = 0;
                            }
                            string sql = "exec insert_carrera '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','"+codigo_facultad_txt.Text.Trim() +"','"+ estado.ToString() + "','0'";
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
                             create proc insert_carrera
                                @nombre varchar(max),@descripcion varchar(max),@cod_facultad int,@estado int,@codigo int

                            */
                            if (ck_estado.Checked == true)
                            {
                                estado = 1;
                            }
                            else
                            {
                                estado = 0;
                            }
                            string sql = "exec insert_carrera '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + codigo_facultad_txt.Text.Trim() + "','" + estado.ToString() + "','" + codigo_txt.Text.Trim() + "'";
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
                        MessageBox.Show("Falta la facultad");
                    }
                }
                else
                {
                    MessageBox.Show("Falta el nombre");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error agregando o ya existe");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            busqueda_facultad bf = new busqueda_facultad();
            bf.mantenimiento = true;
            bf.pasado += new busqueda_facultad.pasar(ejecutar_codigo_facultad);
            bf.ShowDialog();
            cargar_datos_facultad();
        }
        public void ejecutar_codigo_facultad(string dato)
        {
            codigo_facultad_txt.Text = dato.ToString();
        }
        public void cargar_datos_facultad()
        {
            try
            {
                if (codigo_facultad_txt.Text.Trim() != "")
                {
                    string sql = "select nombre from facultades where codigo='" + codigo_facultad_txt.Text.Trim() + "'";
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    if (ds.Tables[0].Rows[0][0].ToString() != "")
                    {
                        nombre_facultad_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error cargando los datos de la facultad");
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            codigo_facultad_txt.Clear();
            nombre_facultad_txt.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            busqueda_carrera bc = new busqueda_carrera();
            bc.mantenimiento = true;
            bc.pasado += new busqueda_carrera.pasar(ejecutar_codigo_carrera);
            bc.ShowDialog();
            cargar_datos_carrera();
            cargar_nombre_facultad();
        }
        public void ejecutar_codigo_carrera(string dato)
        {
            codigo_txt.Text = dato.ToString();
        }
        public void cargar_datos_carrera()
        {
            if (codigo_txt.Text.Trim() != "")
            {
                string sql = "select codigo,nombre,cod_facultad,descripcion,estado from carreras";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if (ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    nombre_txt.Text = ds.Tables[0].Rows[0][1].ToString();
                    codigo_facultad_txt.Text = ds.Tables[0].Rows[0][2].ToString();
                    descripcion_txt.Text = ds.Tables[0].Rows[0][3].ToString();
                    if (ds.Tables[0].Rows[0][4].ToString() == "1")
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
        public void cargar_nombre_facultad()
        {
            if(codigo_facultad_txt.Text.Trim()!="")
            {
                string sql = "select nombre from facultades where codigo='" + codigo_facultad_txt.Text.Trim() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                nombre_facultad_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }
    }
}
