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
    public partial class tests : Form
    {
        public tests()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Desea guardar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
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
                             create proc insert_test
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
                            string sql = "exec insert_test '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + estado.ToString() + "','0'";
                            DataSet ds = utilidades.ejecutarcomando(sql);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                                actualiza_preguntas();
                                MessageBox.Show("Se agrego!");

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
                             create proc insert_test
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
                            string sql = "exec insert_test '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + estado.ToString() + "','" + codigo_txt.Text.Trim() + "'";
                            DataSet ds = utilidades.ejecutarcomando(sql);
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                codigo_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                                actualiza_preguntas();
                                MessageBox.Show("Se actualizo!");
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
                catch (Exception)
                {
                    MessageBox.Show("Error agregando o ya existe");
                }
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            codigo_txt.Clear();
            nombre_txt.Clear();
            descripcion_txt.Clear();
            ck_estado.Checked = true;
        }
        public void cargar_preguntas()
        {
            string sql = "";
            DataSet ds = utilidades.ejecutarcomando(sql);
            foreach(DataRow row in ds.Tables[0].Rows)
            {

            }
        }
        public void cargar_preguntas_test()
        {
            dataGridView1.Rows.Clear();
            string sql="select tp.cod_pregunta,pr.descripcion,tp.cod_carrera,c.nombre,tp.cod_respuesta,rp.descripcion,tp.cod_aptitud,ap.nombre,tp.puntos from test_vs_preguntas tp join test t on t.codigo=tp.cod_test join preguntas pr on pr.codigo=tp.cod_pregunta join  respuestas rp on rp.codigo=tp.cod_respuesta join carreras c on c.codigo=tp.cod_carrera join aptitudes ap  on ap.codigo=tp.cod_aptitud where tp.cod_test='" + codigo_txt.Text.Trim() + "'";
            DataSet ds = utilidades.ejecutarcomando(sql);
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                dataGridView1.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString(), row[7].ToString(), row[8].ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            busqueda_tests bt = new busqueda_tests();
            bt.mantenimiento = true;
            bt.pasado += new busqueda_tests.pasar(ejecutar_codigo_test);
            bt.ShowDialog();
            cargar_datos();
            cargar_preguntas_test();
        }
        public void ejecutar_codigo_test(string dato)
        {
            codigo_txt.Text = dato.ToString();
        }
        public void cargar_datos()
        {
            if(codigo_txt.Text.Trim()!="")
            {
                if (codigo_txt.Text.Trim() != "")
                {
                    string sql = "select nombre,descripcion,estado from test where codigo='" + codigo_txt.Text.Trim() + "'";
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    if (ds.Tables[0].Rows[0][0].ToString() != "")
                    {
                        nombre_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                        descripcion_txt.Text = ds.Tables[0].Rows[0][1].ToString();
                        if (ds.Tables[0].Rows[0][2].ToString() == "1")
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
        }

        private void tests_Load(object sender, EventArgs e)
        {

        }
        public void actualiza_preguntas()
        {
            try
            {
                string sql = "delete from test_vs_preguntas where cod_test='" + codigo_txt.Text.Trim() + "'";
                utilidades.ejecutarcomando(sql);
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    /*
                       alter proc insert_test_vs_preguntas
                      @cod_test int,@cod_pregunta int,@cod_carrera int,@cod_respuesta int,@cod_aptitud int,@puntos float
                     */
                    sql = "exec insert_test_vs_preguntas '" + codigo_txt.Text.Trim() + "','" + row.Cells[0].Value.ToString() + "','" + row.Cells[2].Value.ToString() + "','" + row.Cells[4].Value.ToString() + "','" + row.Cells[6].Value.ToString() + "','"+row.Cells[8].Value.ToString()+"'";
                    utilidades.ejecutarcomando(sql);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error actualizando las preguntas");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (codigo_carrera_txt.Text.Trim() != "")
            {
                busqueda_pregunta bp = new busqueda_pregunta();
                bp.pasado += new busqueda_pregunta.pasar(ejecutar_codigo_pregunta);
                bp.ShowDialog();
                cargar_pregunta();
            }
            else
            {
                MessageBox.Show("Seleccione la carrera");
            }
        }
        public void ejecutar_codigo_pregunta(string dato)
        {
            codigo_pregunta_txt.Text = dato.ToString();
        }
        public void cargar_pregunta()
        {
            if(codigo_pregunta_txt.Text.Trim()!="")
            {
                string sql = "select descripcion from preguntas where codigo='"+codigo_pregunta_txt.Text.Trim()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if(ds.Tables[0].Rows[0][0].ToString()!="")
                {
                    descripcion_pregunta_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int cont = 0;
            try
            {
                if (codigo_txt.Text.Trim() != "")
                {
                    if (codigo_carrera_txt.Text.Trim() != "")
                    {
                        if (codigo_pregunta_txt.Text.Trim() != "")
                        {
                            if (codigo_respuesta_txt.Text.Trim() != "")
                            {
                                if (puntos_txt.Text.Trim() != "")
                                {
                                    if (codigo_aptitud_txt.Text.Trim() != "")
                                    {
                                        foreach (DataGridViewRow row in dataGridView1.Rows)
                                        {
                                            if (row.Cells[0].Value.ToString() == codigo_pregunta_txt.Text.Trim() && row.Cells[2].Value.ToString() == codigo_carrera_txt.Text.Trim() && row.Cells[4].Value.ToString() == codigo_respuesta_txt.Text.Trim() && codigo_aptitud_txt.Text == row.Cells[6].Value.ToString())
                                            {
                                                cont++;
                                            }
                                        }
                                        if (cont == 0)
                                        {
                                            dataGridView1.Rows.Add(codigo_pregunta_txt.Text.Trim(), descripcion_pregunta_txt.Text.Trim(), codigo_carrera_txt.Text.Trim(), nombre_carrera_txt.Text.Trim(), codigo_respuesta_txt.Text.Trim(), descripcion_respuesta_txt.Text.Trim(), codigo_aptitud_txt.Text.Trim(), nombre_aptitud_txt.Text.Trim(), puntos_txt.Text.Trim());
                                        }
                                        else
                                        {
                                            MessageBox.Show("Esa opcion que intenta poner ya se encuentra seleccionada");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Falta la aptitud");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Faltan los puntos");
                                    puntos_txt.Focus();
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
                    else
                    {
                        MessageBox.Show("Falta la carrera");
                    }
                }
                else
                {
                    MessageBox.Show("Falta seleccionar el test");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error agregando el numero");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            codigo_pregunta_txt.Clear();
            descripcion_pregunta_txt.Clear();
        }

        private void puntos_txt_KeyUp(object sender, KeyEventArgs e)
        {
            if (utilidades.numero_entero(puntos_txt.Text.Trim()) != true)
            {
                puntos_txt.Clear();
                MessageBox.Show("los puntos deben de ser numeros enteros");
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

        private void button5_Click(object sender, EventArgs e)
        {
            busqueda_carrera bc = new busqueda_carrera();
            bc.pasado += new busqueda_carrera.pasar(ejecutar_codigo_carrera);
            bc.ShowDialog();
            cargar_nombre_carrera();
        }
        public void ejecutar_codigo_carrera(string dato)
        {
            codigo_carrera_txt.Text = dato.ToString();
        }
        public void cargar_nombre_carrera()
        {
            if(codigo_carrera_txt.Text.Trim()!="")
            {
                string sql = "select nombre from carreras where codigo='"+codigo_carrera_txt.Text.Trim()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                nombre_carrera_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(codigo_pregunta_txt.Text.Trim()!="")
            {
                busqueda_respuesta bc = new busqueda_respuesta();
                bc.pasado += new busqueda_respuesta.pasar(ejecutar_codigo_respuesta);
                bc.ShowDialog();
                cargar_respuesta();
            }
        }
        public void ejecutar_codigo_respuesta(string dato)
        {
            codigo_respuesta_txt.Text = dato.ToString();
        }
        public void cargar_respuesta()
        {
            if(codigo_respuesta_txt.Text.Trim()!="")
            {
                string sql = "select descripcion from respuestas where codigo='"+codigo_respuesta_txt.Text.Trim()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                descripcion_respuesta_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            busqueda_aptitudes ba = new busqueda_aptitudes();
            ba.pasado += new busqueda_aptitudes.pasar(ejecutar_codigo_aptitud);
            ba.ShowDialog();
            cargar_datos_aptitudes();
        }
        public void ejecutar_codigo_aptitud(string dato)
        {
            codigo_aptitud_txt.Text = dato.ToString();
        }

        public void cargar_datos_aptitudes()
        {
            if (codigo_aptitud_txt.Text.Trim() != "")
            {
                string sql = "select nombre,descripcion from aptitudes where codigo='" + codigo_aptitud_txt.Text.Trim() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    nombre_aptitud_txt.Text = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            codigo_aptitud_txt.Clear();
            nombre_aptitud_txt.Clear();
        }
    }
}
