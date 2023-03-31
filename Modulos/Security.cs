using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFormularios.Modulos
{
    public static class Security
    {
        private static int mUserActual; //id de usuario
     
        public static int UserActual { get => mUserActual; set => mUserActual = value; }

        public static bool IniciarSesion(string codigo, string pass = "")
        {
            bool Resultado = false;
            try
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = new OleDbConnection("Provider=SQLNCLI11;Data Source=localhost;Persist Security Info=True;User ID=sa;Initial Catalog=empleadoyoutub_db;Password=root");
                objCmd.CommandType = CommandType.Text;
                objCmd.CommandTimeout = 30000;
                objCmd.CommandText = "Select top 1 Usu_Id from Users where Usu_Codigo = ? and Usu_Password = ?";
                objCmd.Connection.Open();

                objCmd.Parameters.Add("Codigo", OleDbType.VarChar, 10).Value = codigo;
                objCmd.Parameters.Add("Password", OleDbType.VarChar, 100).Value = pass;

                OleDbDataReader dr = objCmd.ExecuteReader();
                if (dr.Read())
                {
                    Resultado = true;
                    mUserActual = Convert.ToInt32(dr["Usu_Id"]);
                }

                objCmd.Connection.Close();
            }
            catch (OleDbException olex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", olex.Source, Environment.NewLine, olex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", ex.Source, Environment.NewLine, ex.Message));
            }
            return Resultado;
        }

        public static void CerrarSesion()
        {
            mUserActual = 0;
        }
        public static string Encriptar(string password)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(password);
            result = Convert.ToBase64String(encryted);
            return result;
        }
    }
}
