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
    public partial class busqueda_pregunta : Form
    {
        public busqueda_pregunta()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string codigo = "";
                    codigo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    pasado(codigo.ToString());
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No hay elementos para seleccionar");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error pasando variable hacia atras");
            }
        }
        public delegate void pasar(string dato);
        public event pasar pasado;
        public bool mantenimiento = false;

        private void busqueda_pregunta_Load(object sender, EventArgs e)
        {
            cargar_datos();
        }
        public void cargar_datos()
        {
            try
            {
                dataGridView1.Rows.Clear();
                if (mantenimiento == true)
                {
                    string sql = "select codigo,descripcion,estado from preguntas where codigo>0";
                    if (descripcion_txt.Text.Trim() != "")
                    {
                        sql += " and descripcion like '%" + descripcion_txt.Text.Trim() + "%'";
                    }
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataGridView1.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString());
                    }
                }
                if (mantenimiento == false)
                {

                    string sql = "select codigo,descripcion,estado from preguntas where estado='1'";
                    if (descripcion_txt.Text.Trim() != "")
                    {
                        sql += " and descripcion like '%" + descripcion_txt.Text.Trim() + "%'";
                    }
                    DataSet ds = utilidades.ejecutarcomando(sql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataGridView1.Rows.Add(row[0].ToString(), row[1].ToString(), row[2].ToString());
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Error buscando los datos");
            }
        }

        private void descripcion_txt_KeyUp(object sender, KeyEventArgs e)
        {
            cargar_datos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Desea salir?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                int fila = dataGridView1.CurrentRow.Index;
                textBox1.Text = dataGridView1.Rows[fila].Cells[1].Value.ToString();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
