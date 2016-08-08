using System;
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
                MessageBox.Show("Debes seleccionar un test");
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
                MessageBox.Show("Error en el evento click boton test");
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
            bool a = validarRespuestas(false);
            if (a)
            {
                validarRespuestas(true);
                llenarGrafico();
                calcularResultado();
                MessageBox.Show("Proceso finalizado");
            }
            else
            {
                MessageBox.Show("Debe seleccionar una respuesta por cada pregunta");
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
                codigoTestRespuesta = getNextCodigoTest();
                resultado_txt.Clear();
                string cmd = "";
                string codigo_respuesta_seleccionada = "";
                int cont = 0;
                string nombreCarrera = "";
                DataSet dx = new DataSet();
                puntosLocales = 0;
                double sumaPuntosTest = getSumaPuntosTest();
                //MessageBox.Show("SumaTest: "+sumaPuntosTest.ToString());
                Puntaje = new List<double>();
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
                                            codigoCarrera=dx.Tables[0].Rows[0][1].ToString();
                                           
                                        }
                                    }
                                    if (!resultado_txt.Text.Contains( " Carrera:" + nombreCarrera + "-Pregunta:"+pregunta.Text+"- respuesta:" + respuesta.Tag.ToString() + "-puntos:" + (puntosLocales).ToString()));
                                    {
                                        /*create proc insert_test_resultado_respuestas
                                         @codTest int,@codCarrera int,@codPregunta int,@codRespuesta int,@puntos float,@codigo int*/
                                        string cx = "exec insert_test_resultado_respuestas '" + codigo_test_presionado.ToString() + "','" + codigoCarrera.ToString() + "','" + pregunta.Tag.ToString() + "','" + respuesta.Tag.ToString() + "','" + puntosLocales.ToString() + "','" + codigoTestRespuesta.ToString() + "'";
                                        utilidades.ejecutarcomando(cx);
                                        resultado_txt.Text += " Carrera:" + nombreCarrera + "-Pregunta:"+pregunta.Text+"- respuesta:" + codigo_respuesta_seleccionada.ToString() + "-puntos:" + (puntosLocales).ToString();
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
        public string getPuntosByTestCarrera(string cod_test,string codigo_carrera)
        {
            try
            {
                string sql = "select sum(puntos) from test_vs_preguntas where cod_test='"+cod_test.ToString()+"' and cod_carrera='"+codigo_carrera.ToString()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);
                if(ds.Tables[0].Rows[0][0].ToString()!="")
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error getPuntosByTestCarrera :"+ex.ToString());
                return null;
            }
        }
        public void calcularResultado()
        {
            double puntos = 0;
            double sumaPuntos = 0;
            resultado_txt.Text += Environment.NewLine;
            string sql = "select distinct cod_carrera from test_vs_preguntas where cod_test='"+codigo_test_presionado.ToString()+"'";
            DataSet ds = utilidades.ejecutarcomando(sql);
            DataSet ds2=utilidades.ejecutarcomando(sql);
            foreach(DataRow row1 in ds.Tables[0].Rows)
            {
                puntos = Convert.ToDouble(getPuntosByTestCarrera(codigo_test_presionado.ToString(), row1[0].ToString()));
                string sql2 = "select sum(puntos) from test_resultado_vs_respuestas where codigo='" + codigoTestRespuesta.ToString() + "' and cod_test='" + codigo_test_presionado.ToString() + "' and cod_carrera='" + row1[0].ToString() + "'";
                DataSet dx = utilidades.ejecutarcomando(sql2);
                if (dx.Tables[0].Rows[0][0].ToString() != "")
                {
                    sumaPuntos = Convert.ToDouble(dx.Tables[0].Rows[0][0].ToString());
                }
                resultado_txt.Text += "Carrera: " + getNombreCarreraById(row1[0].ToString()) + ", Porciento: " +Math.Round(((sumaPuntos / puntos) * 100),2).ToString() + "%";
                resultado_txt.Text += Environment.NewLine;
            }
        }
    }
}
