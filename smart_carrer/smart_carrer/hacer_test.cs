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
    public partial class hacer_test : Form
    {
        public hacer_test()
        {
            InitializeComponent();
        }

        private void hacer_test_Load(object sender, EventArgs e)
        {
            cargar_tests();
        }
        int left;
        int top;
        Button boton=new Button();
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
                btn.BackColor = Color.LightSkyBlue;
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
            if(codigo_test_presionado.ToString()!="")
            {
                int top_pregunta=50;
                int left_pregunta = 10;
                
                int top_respuesta=20;
                int left_respuesta=10;
                
                Label label_pregunta;
                string sql = "select distinct tp.cod_pregunta,pr.descripcion  from test_vs_preguntas tp join test t on t.codigo=tp.cod_test join preguntas pr on pr.codigo=tp.cod_pregunta where tp.cod_test='"+codigo_test_presionado.ToString()+"'";
                DataSet ds = utilidades.ejecutarcomando(sql);

                
                foreach(DataRow row1 in ds.Tables[0].Rows)//recorrer las preguntas
                {
                    top_respuesta = 10;
                    left_respuesta = 10;
                  
                    label_pregunta = new Label();
                    label_pregunta.ForeColor = Color.Blue;
                    label_pregunta.Text = row1[1].ToString();
                    label_pregunta.Tag = row1[0].ToString();
                    label_pregunta.Top = top_pregunta;
                    label_pregunta.Left = left_pregunta;
                    label_pregunta.Width = 800;//ancho
                    label_pregunta.Height += 5;//alto
                    flowLayoutPanel2.Controls.Add(label_pregunta);
                    string cmd = "select tp.cod_respuesta,rp.descripcion from test_vs_preguntas tp join respuestas rp on rp.codigo=tp.cod_respuesta where tp.cod_pregunta='"+row1[0].ToString()+"'";
                    DataSet dx = utilidades.ejecutarcomando(cmd);
                    CheckBox ck_respuesta;
                    RadioButton radio_respuesta;
                    GroupBox grupo = new GroupBox();
                    flowLayoutPanel2.Controls.Add(grupo);
                    grupo.Width = 800;//ancho
                   
                    
                    foreach (DataRow row2 in dx.Tables[0].Rows)//recorrer las respuestas
                    {
                        //grupo = new GroupBox();
                        ck_respuesta = new CheckBox();
                        radio_respuesta = new RadioButton();
                        
                        //radiobutton
                        radio_respuesta.ForeColor = Color.Tomato;
                        radio_respuesta.Text = row2[1].ToString();
                        radio_respuesta.Tag = row2[0].ToString();
                        radio_respuesta.Top = top_respuesta;
                        radio_respuesta.Left = left_respuesta;
                        radio_respuesta.Width = 700;//ancho
                        radio_respuesta.Height +=10;//alto
                        
                        //checkbox
                        //ck_respuesta.ForeColor = Color.Tomato;
                        //ck_respuesta.Text = row2[1].ToString();
                        //ck_respuesta.Tag = row2[0].ToString();
                        //ck_respuesta.Top = 10;
                        //ck_respuesta.Left = 10;
                        //ck_respuesta.Width = 700;//ancho
                        //ck_respuesta.Height += 10;//alto
                        
                        //grupo.Controls.Add(ck_respuesta);

                        grupo.Controls.Add(radio_respuesta);

                        radio_respuesta.Location=new Point(10, top_respuesta);
                        //flowLayoutPanel2.Controls.Add(grupo);
                       // left_respuesta += 20;
                        //grupo.Height += 20;//alto

                        top_respuesta += 50;
                        grupo.Height += 30;
                    }
                    flowLayoutPanel2.Controls.Add(grupo);
                    //left_pregunta += 100;
                }
            }
            else
            {
                MessageBox.Show("Debes seleccionar un test");
            }
        }
    }
}
