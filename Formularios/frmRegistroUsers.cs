using DevExpress.XtraEditors;
using ProyectoFormularios.Clases;
using ProyectoFormularios.Modulos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFormularios.Formularios
{
    public partial class frmRegistroUsers : DevExpress.XtraEditors.XtraForm
    {
        public frmRegistroUsers()
        {
            InitializeComponent();
        }

        #region Variables
        private bool mPuedeModificar;
        private bool mPuedeAgregar;
        private bool mPuedeEliminar;
        private int mId;
        private UsersWindows mParent;
        private Util.TipoABM mTipoABM;
        #endregion

        #region Propiedades
        public bool PuedeModificar { get => mPuedeModificar; set => mPuedeModificar = value; }
        public bool PuedeAgregar { get => mPuedeAgregar; set => mPuedeAgregar = value; }
        public bool PuedeEliminar { get => mPuedeEliminar; set => mPuedeEliminar = value; }
        public new UsersWindows Parent { get => mParent; set => mParent = value; }
        public int Id { get => mId; set => mId = value; }
        public Util.TipoABM TipoABM { get => mTipoABM; set => mTipoABM = value; }

        #endregion

        private void frmRegistroUsers_Load(object sender, EventArgs e)
        {
            BlanquearCampos();
            CargarDatos();
            ActivarCampos(mTipoABM == Util.TipoABM.Alta || mTipoABM == Util.TipoABM.Modificacion);
        }
        private void CargarDatos()
        {
            if (mId != 0)
            {
                User usu = new User();
                usu.Id = mId;
                if (usu.LeerDatos())
                {
                    txtCodigo.Text = usu.Codigo.Trim();
                    txtNombre.Text = usu.Nombre.Trim();
                    txtApellido.Text = usu.Apellido.Trim();
                    chkEstado.Checked = usu.Estado;
                }
            }
        }

        private void ActivarCampos(bool Activar = true)
        {
            txtCodigo.ReadOnly = !Activar || mTipoABM == Util.TipoABM.Modificacion;
            txtNombre.ReadOnly = !Activar;
            txtApellido.ReadOnly = !Activar;
            txtPassword.ReadOnly = !Activar;
            txtConfirmPassword.ReadOnly = !Activar;
            chkEstado.ReadOnly = !Activar;
        }

        private void BlanquearCampos()
        {
            txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            chkEstado.Checked = false;
        }

        private bool ValidarCampos(ref User obj)
        {
            int Id = 0;

            if (string.IsNullOrEmpty(txtCodigo.Text.Trim()))
            {
                txtCodigo.Focus();
                MessageBox.Show("Debe ingresar un codigo de usuario");
                return false;
            }

            if (obj.ExisteCodigo(txtCodigo.Text, ref Id) && mId == 0)
            {
                txtCodigo.Focus();
                MessageBox.Show("El código de usuario ya existe.");
                return false;
            }
            obj.Codigo = txtCodigo.Text;
            obj.Id = Id;
            mId = Id;

            if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
            {
                txtNombre.Focus();
                MessageBox.Show("Debe ingresar un nombre");
                return false;
            }

            txtPassword.Text = txtPassword.Text.Trim();
            txtConfirmPassword.Text = txtConfirmPassword.Text.Trim();

            if (!this.txtPassword.Text.Equals(this.txtConfirmPassword.Text))
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return false;
            }

            obj.Nombre = txtNombre.Text;
            obj.Apellido = txtApellido.Text;
            obj.Password = Security.Encriptar(txtPassword.Text);
            obj.Estado = chkEstado.Checked;

            return true;
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                User usua = new User();
                if (!ValidarCampos(ref usua)) throw new Exception();

                if (!usua.GuardarDatos()) throw new Exception("Hubo un error al guardar los datos del usuario");
                this.mId = usua.Id;
                MessageBox.Show("Usuario agregado correctamente");

                if (mParent != null) mParent.CallBack(false);

                BlanquearCampos();
                this.Close();

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}