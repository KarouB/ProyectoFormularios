using ProyectoFormularios.Formularios;
using ProyectoFormularios.Modulos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFormularios.Clases
{
    public class UsersWindows
    {
        public UsersWindows()
        {
            Reset();
        }

        #region Variables
        private int mId;
        private dynamic mOwner;
        private bool mCancel;
        private frmRegistroUsers fUsuR;
        private frmModificarUsuario fUsuM;
        private string mTitulo;
        #endregion

        #region Propiedades
        public int Id { get => mId; set => mId = value; }
        public dynamic Owner { get => mOwner; set => mOwner = value; }
        public bool Cancel { get => mCancel; set => mCancel = value; }


        public string Titulo { get => mTitulo; set => mTitulo = value; }
        #endregion

        private void Reset()
        {
            mId = 0;
            mCancel = false;
        }

        public void CallBack(bool Cancelar)
        {
            if (!Cancelar)
            {
                if (fUsuR != null)
                {
                    mId = fUsuR.Id;
                    fUsuR.Dispose();
                }

                if (mOwner != null) mOwner.CallBack(mId);
            }
        }

        public void ShowRegisUsers(Util.TipoABM tipoABM = Util.TipoABM.Consulta, bool Modal = false)
        {
            try
            {
                mCancel = true;
                foreach (Form f in Application.OpenForms)
                {
                    if (f is frmRegistroUsers)
                    {
                        if ((f as frmRegistroUsers).Id == mId)
                        {
                            ((frmRegistroUsers)f).BringToFront();
                            return;
                        }
                    }
                }
                fUsuR = new frmRegistroUsers();
                fUsuR.Id = mId;
                fUsuR.Text = mTitulo;
                fUsuR.TipoABM = tipoABM;

                if (!Modal)
                    fUsuR.Show();
                else
                    fUsuR.ShowDialog();
            }
            catch (ThreadAbortException) { }
            catch (OutOfMemoryException)
            {
                MessageBox.Show(string.Format("{0}{1}{2}", "No hay suficiente memoria para mostrar la pantalla.", Environment.NewLine, "Intente liberar memoria para poder ejecutar el proceso."), "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}{1}{2}", ex.Source, Environment.NewLine, ex.Message), "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        public void ShowEditor(Util.TipoABM tipoABM = Util.TipoABM.Modificacion, bool Modal = false)
        {
            try
            {
                mCancel = true;
                foreach (Form f in Application.OpenForms)
                {
                    if (f is frmModificarUsuario)
                    {
                        if ((f as frmModificarUsuario).Id == mId)
                        {
                            ((frmModificarUsuario)f).BringToFront();
                            return;
                        }
                    }
                }
                fUsuM = new frmModificarUsuario();
                fUsuM.Id = mId;
                fUsuM.Text = mTitulo;
                fUsuM.TipoABM = tipoABM;

                if (!Modal)
                    fUsuM.Show();
                else
                    fUsuM.ShowDialog();
            }
            catch (ThreadAbortException) { }
            catch (OutOfMemoryException)
            {
                MessageBox.Show(string.Format("{0}{1}{2}", "No hay suficiente memoria para mostrar la pantalla.", Environment.NewLine, "Intente liberar memoria para poder ejecutar el proceso."), "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}{1}{2}", ex.Source, Environment.NewLine, ex.Message), "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

    }
}
