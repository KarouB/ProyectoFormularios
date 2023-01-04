using ProyectoFormularios.Modulos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFormularios.Clases
{
    public class User
    {
        public User()
        {
            Reset();
        }


        #region Variables
        private int mId;
        private string mCodigo;
        private string mNombre;
        private string mApellido;
        private bool mEstado;
        private string mPassword;

        #endregion

        #region Propiedades
        public int Id { get => mId; set => mId = value; }
        public string Codigo { get => mCodigo; set => mCodigo = value; }
        public string Nombre { get => mNombre; set => mNombre = value; }
        public string Apellido { get => mApellido; set => mApellido = value; }
        public string Password { get => mPassword; set => mPassword = value; }
        public bool Estado { get => mEstado; set => mEstado = value; }

        #endregion

        public void Reset()
        {
            mCodigo = string.Empty;
            mNombre = string.Empty;
            mApellido = string.Empty;
            mEstado = false;
            mId = 0;
            mPassword = string.Empty;
        }
        internal bool GuardarDatos()
        {
            User obj = this;
            return Users.GuardarDatos(ref obj);
        }
        public bool ExisteCodigo(string valor, ref int id)
        {
            return Users.ExisteCodigo(valor, ref id);
        }

        internal bool LeerDatos()
        {
            User obj = this;
            return Users.LeerDatos(ref obj);
        }

        internal DataTable getDtUsers()
        {
            return Users.getDtUsers();
        }
    }
}
