using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
namespace smart_carrer
{
    class utilidades
    {
        public static DataSet ejecutarcomando(string query)
        {
            try
            {
                //string cmd = "select ip_server,base_datos,base_datos_usuario,base_datos_clave from sistema where ip_server!='' ";
                //DataSet dx = Utilidades.ejecutarcomando(cmd);
                //if(dx.Tables[0].Rows[0][0].ToString()!="")
                //{
                //    MessageBox.Show("Ip server tiene dato");
                //}

                //funaciona nitido para conecciones desde otra maquina porq se especifica el user y password de la bd
                //SqlConnection conn = new SqlConnection("Data Source=dlr-laptop.ddns.net,31164;" + "Initial Catalog=punto_venta;" + "User id=dextroyex;" + "Password=wilmerlomas1;");
                //SqlConnection conn = new SqlConnection("Data Source=dlrsoft.ddns.net,31164;" + "Initial Catalog=punto_venta;" + "User id=dextroyex;" + "Password=wilmerlomas1;");
                SqlConnection conn = new SqlConnection("Data Source=.;" + "Initial Catalog=smart_carrer;" + "User id=dextroyex;" + "Password=wilmerlomas1;");

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch
            {
                return null;
                MessageBox.Show("Fallo conectando al server");

            }
        }
        public static bool numero_decimal(string cadena)
        {
            try
            {
                Convert.ToDecimal(cadena.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool numero_entero(string cadena)
        {
            try
            {
                Convert.ToInt64(cadena.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
