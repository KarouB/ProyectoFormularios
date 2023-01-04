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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFormularios.Formularios
{
    public partial class frmLogin : DevExpress.XtraEditors.XtraForm
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private string mPassword;
        private bool ValidarCampos()
        {
            mPassword = "";

            txtUser.Text = txtUser.Text.Trim();

            if (string.IsNullOrEmpty(txtUser.Text))
            {
                txtUser.Focus();
                MessageBox.Show("Debe ingresar un nombre de usuario valido");
                return false;
            }

            txtPassword.Text = txtPassword.Text.Trim();

            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                mPassword = Security.Encriptar(txtPassword.Text.Trim());
            }

            return true;
        }

        private void LimpiarCampos()
        {
            txtUser.Text = string.Empty;
            txtPassword.Text = string.Empty;

        }
        private void ShowUsers()
        {
            try
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f is frmUsers)
                    {
                        ((frmUsers)f).BringToFront();
                        return;
                    }
                }
                frmUsers fUsers = new frmUsers();

                this.Hide();
                fUsers.ShowDialog();
                if (fUsers.CerrarSistema) this.Close();
                fUsers.Dispose();
                LimpiarCampos();
                this.Show();
            }
            catch (ThreadAbortException) { }
            catch (OutOfMemoryException)
            {
                MessageBox.Show(string.Format("{0}{1}{2}", "No hay suficiente memoria para mostrar la pantalla.", Environment.NewLine, "Intente liberar memoria para poder ejecutar el proceso."), "Login", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}{1}{2}", ex.Source, Environment.NewLine, ex.Message), "Login", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnSesion_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarCampos()) throw new Exception();
                if (!Security.IniciarSesion(txtUser.Text, mPassword)) throw new Exception("Usuario y/o contraseña incorrectos");

                ShowUsers();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnRegistrarte_Click(object sender, EventArgs e)
        {
            //Se crea la clase que levanta la ventana de ABM usuarios
            UsersWindows UserW = new UsersWindows();
            //Se setean los parametros necesarios para levantar la ventana.
            UserW.Id = 0;
            UserW.Owner = this;
            UserW.Titulo = "Alta de usuario";
            UserW.ShowRegisUsers(Util.TipoABM.Alta, false);
        }


    }


    
}