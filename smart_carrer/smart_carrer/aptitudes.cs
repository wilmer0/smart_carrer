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


            DialogResult result = MessageBox.Show("Desea guardar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                /*
                    create proc insert_aptitudes
                    @nombre varchar(max),@descripcion varchar(150),@estado int,@codigo int
                 */
                bool procesar = validarCampos();
                if (procesar)
                {
                    int activo = 0;
                    if (ck_estado.Checked == true)
                        activo = 1;
                    else
                        activo = 0;

                    if (codigo_txt.Text.Trim() == "")
                    {
                        //guarda
                        string sql = "exec insert_aptitudes '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + activo.ToString() + "','0'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se agrego la aptitud", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            codigo_txt.Clear();
                            nombre_txt.Clear();
                            ck_estado.Checked = true;
                        }
                        else
                        {
                            MessageBox.Show("No se agrego la aptitud", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        //modifica
                        string sql = "exec insert_aptitudes '" + nombre_txt.Text.Trim() + "','" + descripcion_txt.Text.Trim() + "','" + activo.ToString() + "','" + codigo_txt.Text.Trim() + "'";
                        DataSet ds = utilidades.ejecutarcomando(sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Se actualizo la aptitud", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            codigo_txt.Clear();
                            nombre_txt.Clear();
                            ck_estado.Checked = true;
                        }
                        else
                        {
                            MessageBox.Show("No se actualizo la aptitud", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                   
                }
                actualiza_carrera_vs_aptitudes();
            }
           
        }
        public void cargar_datos()
        {
            try
            {
                if (codigo_txt.Text.Trim() != "")
                {
                    string sql = "select nombre,descripcion,estado from aptitudes where codigo='" + codigo_txt.Text.Trim() + "'";
                    DataSet ds = utilidades.ejecutarcomando(sql);
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
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
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
            ba.pasado += new busqueda_aptitudes.pasar(ejecutar_codigo_aptitud_mantenimiento);
            ba.mantenimiento = true;
            ba.ShowDialog();
            cargar_datos();
        }
        public void ejecutar_codigo_aptitud_mantenimiento(string dato)
        {
            codigo_txt.Text = dato;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            codigo_txt.Clear();
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
                catch (Exception ex)
                {
                    MessageBox.Show("Error eliminando la fila seleccionada: "+ex.ToString());
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            busqueda_carrera bc = new busqueda_carrera();
            bc.pasado += new busqueda_carrera.pasar(ejecutar_codigo_carrera);
            bc.ShowDialog();
            cargar_datos_carrera();
            cargar_aptitudes_carrera();
        }
        public void cargar_aptitudes_carrera()
        {
            dataGridView1.Rows.Clear();
            if(codigo_carrera_txt.Text.Trim()!="")
            {
                string sql = "select ca.cod_carrera,c.nombre,ca.cod_aptitud,ap.nombre from carrera_vs_aptirudes ca join carreras c on ca.cod_carrera=c.codigo join aptitudes ap on ca.cod_aptitud=ap.codigo where cod_carrera='"+codigo_carrera_txt.Text.Trim()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if(ds.Tables[0].Rows.Count>0)
                { 
                    foreach(DataRow row in ds.Tables[0].Rows)
                    {
                        dataGridView1.Rows.Add(row[0].ToString(),row[1].ToString(),row[2].ToString(),row[3].ToString());
                    }
                }
            }
        }
        public void ejecutar_codigo_carrera(string dato)
        {
            codigo_carrera_txt.Text = dato.ToString();
        }
        public void cargar_datos_carrera()
        {
            string sql = "select nombre,descripcion from carreras where codigo='" + codigo_carrera_txt.Text.Trim() + "'";
            DataSet ds = utilidades.ejecutarcomando(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                nombre_carrera_txt.Text = ds.Tables[0].Rows[0][0].ToString();   
            }
        }

        private void button4_Click(object sender, EventArgs e)
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
            string sql = "select nombre,descripcion from aptitudes where codigo='" + codigo_aptitud_txt.Text.Trim() + "'";
            DataSet ds = utilidades.ejecutarcomando(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
               nombre_aptitud_txt.Text = ds.Tables[0].Rows[0][0].ToString();
            }
        }

        public void actualiza_carrera_vs_aptitudes()
        {
            if(dataGridView1.Rows.Count>0)
            {
                string sql = "delete from carreras_vs_aptirudes";
                utilidades.ejecutarcomando(sql);
                foreach(DataGridViewRow row in dataGridView1.Rows)
                {
                    sql = "exec insert_carrera_aptitud  '"+row.Cells[0].Value.ToString()+"','"+row.Cells[2].Value.ToString()+"'";
                    utilidades.ejecutarcomando(sql);
                }
                MessageBox.Show("Se agregaron las aptitudes por carreras","",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                //MessageBox.Show("No hay elementos","",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int cont = 0;
            try
            {
                if (codigo_carrera_txt.Text.Trim() != "")
                {
                    if (codigo_aptitud_txt.Text.Trim() != "")
                    {

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            if (row.Cells[0].Value.ToString() == codigo_carrera_txt.Text.Trim() && row.Cells[2].Value.ToString() == codigo_aptitud_txt.Text.Trim())
                            {
                                cont++;
                            }
                        }
                        if (cont == 0)
                        {
                            dataGridView1.Rows.Add(codigo_carrera_txt.Text.Trim(), nombre_carrera_txt.Text.Trim(), codigo_aptitud_txt.Text.Trim(), nombre_aptitud_txt.Text.Trim());
                        }
                        else
                        {
                            MessageBox.Show("La aptitud se encuentra seleccionada con relacion de la carrera","",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Falta la aptitud", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Falta la carreara", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.ToString());
            }
        }
    }
}
