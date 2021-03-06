﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace smart_carrer
{
    public partial class hacer_test : Form
    {
        public hacer_test()
        {
            InitializeComponent();
        }

        private void hacer_test_Load(object sender, EventArgs e)
        {
            cargar_tests();
            resultado_txt.ScrollBars = ScrollBars.Vertical;
        }
        int left;
        int top;
        Button boton = new Button();
        public void cargar_tests()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();//para limpiar el flow layaout
                top = 50;
                left = 5;
                string sql = "select codigo,nombre from test where estado='1'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                Button boton;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    boton = new Button();
                    boton.Tag = row[0].ToString();
                    boton.Top = top;
                    boton.Left = left;
                    boton.Width = 160;//ancho
                    boton.Height += 50;//alto
                    boton.BackColor = Color.White;
                    boton.Text = row[1].ToString() + "-" + row[0].ToString();
                    boton.ForeColor = Color.Tomato;
                    boton.Click += boton_test_click;//para cuando haga click en los botones de productos generados
                    flowLayoutPanel1.Controls.Add(boton);
                }
                left += 100;
            }
            catch (Exception)
            {
                MessageBox.Show("Error buscando los tests");
            }
        }
        string codigo_test_presionado = "0";
        void boton_test_click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                //MessageBox.Show(btn.Tag.ToString());
                codigo_test_presionado = btn.Tag.ToString(); //para tener el codigo
                //MessageBox.Show("codigo test presionado=" + btn.Tag.ToString());
                //btn.BackColor = Color.LightSkyBlue;
                cargar_preguntas();
                test_nombre_label_txt.Text = btn.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Error en el evento click boton test");
            }
        }
        public void cargar_preguntas()
        {
            flowLayoutPanel2.Controls.Clear();
            if (codigo_test_presionado.ToString() != "")
            {
                int top_pregunta = 50;
                int left_pregunta = 10;

                int top_respuesta = 20;
                int left_respuesta = 10;

                Label label_pregunta;
                string sql = "select distinct tp.cod_pregunta,pr.descripcion  from test_vs_preguntas tp join test t on t.codigo=tp.cod_test join preguntas pr on pr.codigo=tp.cod_pregunta where tp.cod_test='" + codigo_test_presionado.ToString() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);


                foreach (DataRow row1 in ds.Tables[0].Rows)//recorrer las preguntas
                {
                    top_respuesta = 20;
                    left_respuesta = 10;
                    label_pregunta = new Label();
                    label_pregunta.ForeColor = Color.Blue;
                    label_pregunta.Text = row1[1].ToString();
                    label_pregunta.Tag = row1[0].ToString();
                    label_pregunta.Top = top_pregunta;
                    label_pregunta.Left = left_pregunta;
                    label_pregunta.Width = 800;//ancho
                    label_pregunta.Height += 10;//alto
                    //flowLayoutPanel2.Controls.Add(label_pregunta);
                    string cmd = "select tp.cod_respuesta,rp.descripcion from test_vs_preguntas tp join respuestas rp on rp.codigo=tp.cod_respuesta where tp.cod_pregunta='" + row1[0].ToString() + "'";
                    DataSet dx = utilidades.ejecutarcomando(cmd);
                    CheckBox ck_respuesta;
                    RadioButton radio_respuesta;
                    GroupBox grupo = new GroupBox();
                    grupo.Tag = row1[0].ToString();//asignar codigo de la pregunta al groupbox
                    grupo.Click += groupBoxClick;
                    flowLayoutPanel2.Controls.Add(grupo);
                    grupo.Width = 800;//ancho

                    foreach (DataRow row2 in dx.Tables[0].Rows)//recorrer las respuestas
                    {

                        radio_respuesta = new RadioButton();

                        //radiobutton
                        radio_respuesta.ForeColor = Color.Tomato;
                        radio_respuesta.Text = row2[1].ToString();
                        radio_respuesta.Tag = row2[0].ToString();
                        radio_respuesta.Top = top_respuesta;
                        radio_respuesta.Left = left_respuesta;
                        radio_respuesta.Width = 700;//ancho
                        radio_respuesta.Height += 10;//alto
                        radio_respuesta.Click += radioRespuestaClick;

                        grupo.Text = label_pregunta.Text;
                        grupo.Controls.Add(radio_respuesta);

                        radio_respuesta.Location = new Point(10, top_respuesta);

                        top_respuesta += 40;
                        grupo.Height += 50;
                    }
                    flowLayoutPanel2.Controls.Add(grupo);
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un test", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void groupBoxClick(object sender, EventArgs e)
        {
            try
            {
                GroupBox grupo = (GroupBox)sender;
                //MessageBox.Show("Grupo " + grupo.Tag);
            }
            catch (Exception)
            {
                MessageBox.Show("Error en el evento click boton test", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string codigo_respuesta_seleccionada = "";
        void radioRespuestaClick(object sender, EventArgs e)
        {
            try
            {
                RadioButton radio = (RadioButton)sender;
                //MessageBox.Show(radio.Tag.ToString());
                codigo_respuesta_seleccionada = radio.Tag.ToString(); //para tener el codigo

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error click radio: " + ex.ToString());
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Desea guardar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                bool a = validarRespuestas(false);
                if (a)
                {
                    validarRespuestas(true);
                    llenarGrafico();
                    //calcularResultado();
                    calcularResultado2();
                    MessageBox.Show("Proceso finalizado", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  
                }
                else
                {
                    MessageBox.Show("Debes seleccionar una respuesta por cada preguna", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        List<double> Puntaje;
        public void llenarGrafico()
        {

            try
            {
                //vectores con los datos para el grafico
                string sql = "select codigo,nombre from facultades where estado='1'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                List<string> facultades;
                facultades = new List<string>();
                int cont = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    facultades.Add(row[1].ToString());
                    cont++;
                }
                List<double> Puntaje;
                Puntaje = new List<double>();
                int[] puntos = { 10, 40, 80 };
                //para cambiar la peleta de colores
                //grafico1.Palette = ChartColorPalette.Pastel;

                //titulo
                //grafico1.Titles.Add("Meses");
                //para agregar valores al grafico
                int f = 0;
                double puntaje = 0;
                Series ser = new Series();
                for (int c = 1; c <= facultades.Count; c++)
                {
                    puntaje = 0;
                    //titulos que salen en la derecha
                    //ser = chart1.Series.Add(facultades[f]);
                    f++;
                    //foreach (DataGridViewRow row in dataGridView1.Rows)
                    //{
                    //    monto += Convert.ToDouble(row.Cells[c].Value.ToString());
                    //}
                    ser.Points.Add(puntaje);

                    //para que aparezca encima de cada barra el monto que tiene
                    if (puntaje > 0)
                    {
                        ser.Label = puntaje.ToString("######");
                    }

                }
            }
            catch (Exception)
            {

            }
        }
        public double getSumaPuntosTest()
        {
            try
            {
                double sumaPuntosTotal = 0;
                string cmd = "select sum(puntos) from test_vs_preguntas where cod_test='" + codigo_test_presionado.ToString() + "'";
                DataSet dx = utilidades.ejecutarcomando(cmd);
                sumaPuntosTotal = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                return sumaPuntosTotal;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error suma puntos test: " + ex.ToString());
                return 0;
            }
        }
        public string getNombreCarreraById(string codigoCarrera)
        {
            try
            {
                string sql = "select top(1) nombre from carreras where codigo='" + codigoCarrera + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error tomando nombre carrera:" + ex.ToString());
                return null;
            }
        }
        public string getNextCodigoTest()
        {
            try
            {
                //sirve para conseguir el siguiente codigo secuencial del test
                string sql = "exec getNextCodigoTestResultadoRespuesta";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if (ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error nextCodigo :" + ex.ToString());
                return null;
            }
        }
        double puntosLocales = 0;
        string codigoTestRespuesta = "";
        string codigoCarrera = "";
        public Boolean validarRespuestas(Boolean procesar)
        {
            try
            {
                codigoTestRespuesta = getNextCodigoTest();//sera la secuencia de los numeros de test que se van realizando
                resultado_txt.Clear();
                string cmd = "";
                string codigo_respuesta_seleccionada = "";
                int cont = 0;
                string nombreCarrera = "";
                DataSet dx = new DataSet();
                puntosLocales = 0;
                //double sumaPuntosTest = getSumaPuntosTest();
                //MessageBox.Show("SumaTest: "+sumaPuntosTest.ToString());
                //Puntaje = new List<double>();
                Boolean todasRespuestas = false;//para saber una respuesta de cada pregunta esta marcada
                foreach (Control pregunta in flowLayoutPanel2.Controls)
                {
                    //MessageBox.Show("grupo->"+ pregunta.Tag);
                    Boolean marcada = false;
                    //MessageBox.Show(pregunta.Tag.ToString());                    
                    foreach (RadioButton respuesta in pregunta.Controls)
                    {
                        if (respuesta.Checked == true)
                        {
                            codigo_respuesta_seleccionada = respuesta.Tag.ToString();
                            if (procesar)
                            {
                                //MessageBox.Show("Pregunta;" + pregunta.Tag.ToString() + " respuesta: " + codigo_respuesta_seleccionada.ToString());
                                //crear un ciclo anterior que me traiga todas las carreras del test y que hag ala comparacion
                                string sql = "select distinct cod_carrera from test_vs_preguntas where cod_test='" + codigo_test_presionado.ToString() + "'";
                                DataSet ds = utilidades.ejecutarcomando(sql);
                                puntosLocales = 0;
                                foreach (DataRow row2 in ds.Tables[0].Rows)
                                {
                                    //create proc getPuntosByPreguntaRespuesta
                                    //@codTest int,@codPregunta int,@codRespuesta int
                                    //puntos-cod_carrera
                                    //MessageBox.Show("Codigo carrera "+row2[0].ToString());
                                    cmd = "exec getPuntosByPreguntaRespuesta '" + codigo_test_presionado.ToString() + "','" + pregunta.Tag.ToString() + "','" + respuesta.Tag.ToString() + "'";
                                    dx = utilidades.ejecutarcomando(cmd);
                                    if (row2[0].ToString() == dx.Tables[0].Rows[0][1].ToString())
                                    {
                                        puntosLocales = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                                        //Puntaje.Add(puntosLocales);
                                        nombreCarrera = getNombreCarreraById(dx.Tables[0].Rows[0][1].ToString());
                                        codigoCarrera = dx.Tables[0].Rows[0][1].ToString();
                                    }
                                }
                                if (!resultado_txt.Text.Contains(" Carrera:" + nombreCarrera + "-Pregunta:" + pregunta.Text + "- respuesta:" + respuesta.Tag.ToString() + "-puntos:" + (puntosLocales).ToString())) ;
                                {
                                    
                                    //primero sacar la aptitud que afecta esa respuesta de esta pregunta
                                    string ap = "select tp.cod_aptitud,ap.nombre  from test_vs_preguntas tp join preguntas pre on tp.cod_pregunta=pre.codigo join respuestas rep on tp.cod_respuesta=rep.codigo join aptitudes ap on ap.codigo=tp.cod_aptitud  where tp.cod_pregunta='" + pregunta.Tag.ToString() + "' and tp.cod_respuesta='" + codigo_respuesta_seleccionada.ToString() + "'";
                                    DataSet dp = utilidades.ejecutarcomando(ap);




                                    /*create proc insert_test_resultado_respuestas
                                    @codTest int,@codCarrera int,@codPregunta int,@codRespuesta int,@puntos float,@cod_aptitud int,@codigo int
                                    */
                                    string cx = "exec insert_test_resultado_respuestas '" + codigo_test_presionado.ToString() + "','" + codigoCarrera.ToString() + "','" + pregunta.Tag.ToString() + "','" + respuesta.Tag.ToString() + "','" + puntosLocales.ToString() + "','"+dp.Tables[0].Rows[0][0].ToString()+"','" + codigoTestRespuesta.ToString() + "'";
                                    utilidades.ejecutarcomando(cx);
                                    resultado_txt.Text += " Carrera:" + nombreCarrera + "-Pregunta:" + pregunta.Text + "- respuesta:" + codigo_respuesta_seleccionada.ToString()+"- aptitud: "+dp.Tables[0].Rows[0][1].ToString() +"-puntos:" + (puntosLocales).ToString();
                                    resultado_txt.Text += Environment.NewLine;
                                }
                            }
                            marcada = true;
                        }
                        //para saber que se estan recorriendo todas las preguntas y respuestass
                        //MessageBox.Show("Pregunta-> " + pregunta.Tag + "- respuesta->" + respuesta.Tag);  
                    }
                    if (marcada)
                    {
                        todasRespuestas = true;
                        cont++;
                    }
                    else
                    {
                        string cx = "delete from test_resultado_vs_respuestas where codigo='" + codigoTestRespuesta.ToString() + "'";
                        utilidades.ejecutarcomando(cx);
                        todasRespuestas = false;
                        resultado_txt.Clear();
                        cont = 0;
                    }
                }
                return todasRespuestas;
            }
            catch (Exception ex)
            {
                string cx = "delete from test_resultado_vs_respuestas where codigo='" + codigoTestRespuesta.ToString() + "'";
                utilidades.ejecutarcomando(cx);
                MessageBox.Show("Error validar: " + ex.ToString());
                return false;
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {
        }
        public string getPuntosByTestCarrera(string cod_test, string codigo_carrera)
        {
            try
            {
                string sql = "select sum(puntos) from test_vs_preguntas where cod_test='" + cod_test.ToString() + "' and cod_carrera='" + codigo_carrera.ToString() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if (ds.Tables[0].Rows[0][0].ToString() != "")
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getPuntosByTestCarrera :" + ex.ToString());
                return null;
            }
        }

        public void calcularResultado2()
        {
            try
            {
                string nombre_carrera="";
                double puntos = 0;
                double sumaPuntos = 0;
                double AptitudesObligatorias = 0;
                double AptitudesObligatoriasObtenidas = 0;
                double AptitudesNoObligatorias = 0;
                double AptitudesNoObligatoriasObtenidas = 0;
                resultado_txt.Text += Environment.NewLine;
                //se selecciona todas las carreras vinculadas al test para poder sacar la sumatoria total que afecta la carrera dentro del test
                string sql = "select distinct cod_carrera from test_vs_preguntas where cod_test='" + codigo_test_presionado.ToString() + "'";
                DataSet dataSetCarrera = utilidades.ejecutarcomando(sql);
                foreach (DataRow rowCarrera in dataSetCarrera.Tables[0].Rows)
                {

                    //sacando el nombre de la carrera en curso
                    string x = "select nombre from carreras where codigo='" + rowCarrera[0].ToString() + "'";
                    DataSet dx = utilidades.ejecutarcomando(x);
                    if (dx.Tables[0].Rows[0][0].ToString()!="")
                    {
                        nombre_carrera = dx.Tables[0].Rows[0][0].ToString();
                    }
                      
                    //sacar la cantidad de aptitudes obligatorias de la carrera en curso
                    x = "select count(*) from carrera_vs_aptirudes where cod_carrera='" + rowCarrera[0].ToString() + "' and obligatoria='1'";
                    dx = utilidades.ejecutarcomando(sql);
                    if(dx.Tables[0].Rows.Count>0)
                    {
                        AptitudesObligatorias = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                    }
                    //sacar la cantidad de aptitudes no obligatorias de la carrera en curso
                    x = "select count(*) from carrera_vs_aptirudes where cod_carrera='" + rowCarrera[0].ToString() + "' and obligatoria='0'";
                    dx = utilidades.ejecutarcomando(x);
                    if (dx.Tables[0].Rows.Count > 0)
                    {
                        AptitudesNoObligatorias = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                    }
                    //MessageBox.Show("Carrera:"+nombre_carrera.ToString() + "    -Aptitudes obligatorias:     " + AptitudesObligatorias.ToString());
                    //MessageBox.Show("Carrera:"+nombre_carrera.ToString() + "    -Aptitudes No obligatorias:   " + AptitudesNoObligatorias.ToString());

                    //debo de sumar todos los puntos correspondiendo a cada aptitud para entonces saber lo que respondio el y sacar
                    //un porcentaje en base a las respuestas
                }


                //fin de procesar las aptitudes obligatorias y no obligatorios
                //revisar toda esta parte
                sql = "select distinct cod_carrera from test_vs_preguntas where cod_test='" + codigo_test_presionado.ToString() + "'";
                dataSetCarrera = utilidades.ejecutarcomando(sql);
                //sacar las aptitudes vinculadas a la carrera
                //otravez recorriendo todas las carreras que afectan el test para obtener puntaje por cada aptitud
                foreach(DataRow rowCarrera in dataSetCarrera.Tables[0].Rows)
                {


                    //sacar las aptitudes que son obligatorias
                    sql = "select ca.cod_aptitud,ap.nombre,tr.cod_carrera,carr.nombre,tr.puntos from test_resultado_vs_respuestas tr join carrera_vs_aptirudes ca on tr.cod_aptitud=ca.cod_aptitud join aptitudes ap on ap.codigo=tr.cod_aptitud join carreras carr on carr.codigo=tr.cod_carrera where tr.cod_carrera='"+rowCarrera[0].ToString()+"' and ca.obligatoria='1' and tr.codigo='"+codigoTestRespuesta.ToString()+"'";
                    DataSet dataSetAptitudes = utilidades.ejecutarcomando(sql);
                    foreach (DataRow rowAptitudes in dataSetAptitudes.Tables[0].Rows)
                    {
                        //create proc insert_test_resultado_aptitudes
                        //@codigo int,@cod_test int,@cod_carrera int,@cod_aptitud int,@puntos int

                        string qw = "exec insert_test_resultado_aptitudes '"+codigoTestRespuesta.ToString()+"','"+codigo_test_presionado.ToString()+"','"+rowCarrera[0].ToString()+"','"+rowAptitudes[0].ToString()+"','"+rowAptitudes[4].ToString()+"'";
                        utilidades.ejecutarcomando(qw);
                    }

                    //sacar las aptitudes que no son obligatorias
                    sql = "select ca.cod_aptitud,ap.nombre,tr.cod_carrera,carr.nombre,tr.puntos from test_resultado_vs_respuestas tr join carrera_vs_aptirudes ca on tr.cod_aptitud=ca.cod_aptitud join aptitudes ap on ap.codigo=tr.cod_aptitud join carreras carr on carr.codigo=tr.cod_carrera where tr.cod_carrera='" + rowCarrera[0].ToString() + "' and ca.obligatoria='0' and tr.codigo='" + codigoTestRespuesta.ToString() + "'";
                    dataSetAptitudes = utilidades.ejecutarcomando(sql);
                    foreach (DataRow rowAptitudes in dataSetAptitudes.Tables[0].Rows)
                    {
                        //create proc insert_test_resultado_aptitudes
                        //@codigo int,@cod_test int,@cod_carrera int,@cod_aptitud int,@puntos int
                        string qw = "exec insert_test_resultado_aptitudes '" + codigoTestRespuesta.ToString() + "','" + codigo_test_presionado.ToString() + "','" + rowCarrera[0].ToString() + "','" + rowAptitudes[0].ToString() + "','" + rowAptitudes[4].ToString() + "'";
                        utilidades.ejecutarcomando(qw);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error procesando: " + ex.ToString());
            }
        }
























        /*public void calcularResultado()
        {
            try
            {
                double puntos = 0;
                double sumaPuntos = 0;
                resultado_txt.Text += Environment.NewLine;
                //se selecciona todas las carreras vinculadas al test para poder sacar la sumatoria total que afecta la carrera dentro del test
                string sql = "select distinct cod_carrera from test_vs_preguntas where cod_test='" + codigo_test_presionado.ToString() + "'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                DataSet ds2 = utilidades.ejecutarcomando(sql);
                foreach (DataRow row1 in ds.Tables[0].Rows)
                {
                    codigoCarrera = row1[0].ToString();
                    puntos = Convert.ToDouble(getPuntosByTestCarrera(codigo_test_presionado.ToString(), row1[0].ToString()));
                    //para sacar el total de puntos que se registro en el test 
                    string sql2 = "select sum(puntos) from test_resultado_vs_respuestas where codigo='" + codigoTestRespuesta.ToString() + "' and cod_test='" + codigo_test_presionado.ToString() + "' and cod_carrera='" + row1[0].ToString() + "'";
                    DataSet dx = utilidades.ejecutarcomando(sql2);
                    if (dx.Tables[0].Rows[0][0].ToString() != "")
                    {
                        sumaPuntos = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                    }
                    puntos = Math.Round(((sumaPuntos / puntos) * 100), 2);
                    resultado_txt.Text += "Carrera: " + getNombreCarreraById(row1[0].ToString()) + ", Porciento: " + puntos.ToString() + "%";
                    resultado_txt.Text += Environment.NewLine;
                    string cmd = "insert into test_resultado_final(codigo,fecha,cod_Carrera,puntuacion) values('" + codigoTestRespuesta.ToString() + "',GETDATE(),'" + codigoCarrera.ToString() + "','" + puntos.ToString() + "')";
                    utilidades.ejecutarcomando(cmd);
                }


                //parte final para decirle que carrera escoger siempre y cuando la puntuacion haya sido la mas alta
                sql = "select trf.codigo,c.nombre,trf.fecha,trf.cod_carrera,trf.puntuacion  from test_resultado_final trf join carreras c on trf.cod_carrera=c.codigo where trf.codigo='" + codigoTestRespuesta.ToString() + "' and trf.puntuacion=(select max(t.puntuacion) from test_resultado_final t where t.codigo='" + codigoTestRespuesta.ToString() + "')";
                ds = utilidades.ejecutarcomando(sql);
                resultado_txt.Text += Environment.NewLine;
                resultado_txt.Text += Environment.NewLine;
                resultado_txt.Text += "Carreras Recomendadas:";
                resultado_txt.Text += Environment.NewLine;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    resultado_txt.Text += "Carrera: " + row[1].ToString() + "- Puntos: " + row[4].ToString();
                    resultado_txt.Text += Environment.NewLine;
                }
                resultado_txt.Text += Environment.NewLine;
                resultado_txt.Text += Environment.NewLine;
                resultado_txt.Text += "Algunas aptitudes que debes poseer son:";
                resultado_txt.Text += Environment.NewLine;
                resultado_txt.Text += Environment.NewLine;
                //ahora darle algunas recomendaciones que deberia tener algunas cualidades que le pueden se mucha ayuda para determinar la carrera
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string query = "select ap.nombre from carrera_vs_aptirudes ca join carreras c on ca.cod_carrera=c.codigo join aptitudes ap on ca.cod_aptitud=ap.codigo where cod_carrera='" + row[3].ToString() + "'";
                    DataSet data = utilidades.ejecutarcomando(query);
                    foreach (DataRow aptitud in data.Tables[0].Rows)
                    {
                        resultado_txt.Text += "Carrera: " + row[1].ToString() + "-->" + aptitud[0].ToString();
                        resultado_txt.Text += Environment.NewLine;
                    }
                    resultado_txt.Text += Environment.NewLine;
                    resultado_txt.Text += Environment.NewLine;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error procesando: " + ex.ToString());
            }
        }*/

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Desea salir?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
