using ProyectoFormularios.Clases;
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
    internal static class Users
    {
        internal static bool GuardarDatos(ref User obj)
        {
            try
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = new OleDbConnection("Provider=SQLNCLI11;Data Source=localhost;Persist Security Info=True;User ID=sa;Initial Catalog=empleadoyoutub_db;Password=root");
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandTimeout = 30000;

                if (obj.Id == 0)
                {
                    objCmd.CommandText = "Users_Create";
                }
                else
                {
                    objCmd.CommandText = "Users_Update";

                }

                OleDbParameter par = objCmd.Parameters.Add("@Usu_Id", OleDbType.Integer, 4);
                par.Direction = ParameterDirection.InputOutput;
                par.Value = obj.Id;
                                         
                objCmd.Parameters.Add("@Usu_Codigo", OleDbType.VarChar, 10).Value = obj.Codigo;
                objCmd.Parameters.Add("@Usu_Nombre", OleDbType.VarChar, 100).Value = obj.Nombre;
                objCmd.Parameters.Add("@Usu_Apellido", OleDbType.VarChar, 100).Value = obj.Apellido;
                objCmd.Parameters.Add("@Usu_Password", OleDbType.VarChar, 100).Value = obj.Password;
                objCmd.Parameters.Add("@Usu_Estado", OleDbType.Boolean, 1).Value = obj.Estado;


                objCmd.Connection.Open();

                objCmd.ExecuteNonQuery();

                if (obj.Id == 0) obj.Id = Convert.ToInt32(objCmd.Parameters[0].Value);
                objCmd.Connection.Close();

                return true;
            }
            catch (OleDbException olex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", olex.Source, Environment.NewLine, olex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", ex.Source, Environment.NewLine, ex.Message));
            }
            return false;
        }

        internal static DataTable getDtUsers()
        {
            DataTable dt = new DataTable();
            try
            {

                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = new OleDbConnection("Provider=SQLNCLI11;Data Source=localhost;Persist Security Info=True;User ID=sa;Initial Catalog=empleadoyoutub_db;Password=root");
                objCmd.CommandType = CommandType.Text;
                objCmd.CommandTimeout = 30000;
                objCmd.CommandText = "Select * from Users where usu_estado = 1";
                objCmd.Connection.Open();

                using (OleDbDataAdapter da = new OleDbDataAdapter(objCmd))
                {
                    da.Fill(dt);
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
            return dt;
        }
        internal static bool LeerDatos(ref User obj)
        {
            DataTable dt = new DataTable();
            try
            {
                if (obj.Id != 0)
                {
                    OleDbCommand objCmd = new OleDbCommand();
                    objCmd.Connection = new OleDbConnection("Provider=SQLNCLI11;Data Source=localhost;Persist Security Info=True;User ID=sa;Initial Catalog=empleadoyoutub_db;Password=root");
                    objCmd.CommandType = CommandType.Text;
                    objCmd.CommandTimeout = 30000;
                    objCmd.CommandText = "Select * from Users where Usu_Id = ?";
                    objCmd.Connection.Open();

                    objCmd.Parameters.Add("", OleDbType.Integer, 4).Value = obj.Id;

                    using (OleDbDataAdapter da = new OleDbDataAdapter(objCmd))
                    {
                        da.Fill(dt);
                    }
                    objCmd.Connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        FetchFields(ref obj, dt.Rows[0]);
                        return true;
                    }
                }
            }
            catch (OleDbException olex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", olex.Source, Environment.NewLine, olex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0} {1} {2}", ex.Source, Environment.NewLine, ex.Message));
            }
            return false;
        }

        private static void FetchFields(ref User obj, DataRow row)
        {
            obj.Reset();

            if (!row.IsNull("USU_CODIGO")) obj.Codigo = row.Field<string>("USU_CODIGO");
            if (!row.IsNull("USU_NOMBRE")) obj.Nombre = row.Field<string>("USU_NOMBRE");
            if (!row.IsNull("USU_APELLIDO")) obj.Apellido = row.Field<string>("USU_APELLIDO");
            if (!row.IsNull("USU_ESTADO")) obj.Estado = row.Field<bool>("USU_ESTADO");
            if (!row.IsNull("USU_ID")) obj.Id = row.Field<int>("USU_ID");
            //if (!row.IsNull("USU_PASS")) obj.Password = row.Field<string>("USU_PASS");
        }

        internal static bool ExisteCodigo(string valor, ref int id)
        {

            bool Existe = false;
            try
            {
                OleDbCommand objCmd = new OleDbCommand();
                objCmd.Connection = new OleDbConnection("Provider=SQLNCLI11;Data Source=localhost;Persist Security Info=True;User ID=sa;Initial Catalog=empleadoyoutub_db;Password=root");
                objCmd.CommandType = CommandType.Text;
                objCmd.CommandTimeout = 30000;
                objCmd.CommandText = "Select top 1 Usu_Id from Users where Usu_Codigo = ?";
                objCmd.Connection.Open();

                objCmd.Parameters.Add("Codigo", OleDbType.VarChar, 10).Value = valor;

                OleDbDataReader dr = objCmd.ExecuteReader();
                if (dr.Read())
                {
                    Existe = true;
                    id = Convert.ToInt32(dr["Usu_Id"]);
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
            return Existe;
        }

    }
}
