using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
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
    public partial class frmUsers : DevExpress.XtraEditors.XtraForm
    {
        public bool CerrarSistema;
        public frmUsers()
        {
            InitializeComponent();
        }
        #region Variables
        private DataTable mDtUsers;
        private bool mPuedeModificar;
        private bool mPuedeAgregar;
        private bool mPuedeEliminar;
        #endregion

        #region Propiedades
        public bool PuedeModificar { get => mPuedeModificar; set => mPuedeModificar = value; }
        public bool PuedeAgregar { get => mPuedeAgregar; set => mPuedeAgregar = value; }
        public bool PuedeEliminar { get => mPuedeEliminar; set => mPuedeEliminar = value; }

        #endregion
        private void frmUsers_Load(object sender, EventArgs e)
        {
            mPuedeAgregar = true;
            mPuedeModificar = true;
            mPuedeEliminar = true;

            configuraGrilla();
            configuraDT();

            CerrarSistema = true;
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            gvUsers.CloseEditor();
            gvUsers.UpdateCurrentRow();
        }
        private void configuraGrilla()
        {
            //Se configura Columna por columna especificando nombre, tamaño, si es editable o no y otras opciones.
            GridColumn Col;

            RepositoryItemCheckEdit chk = new RepositoryItemCheckEdit();
            chk.AllowGrayed = false;
            chk.ValueChecked = 1;
            chk.ValueUnchecked = 0;
            chk.CheckedChanged += Chk_CheckedChanged;
            gcUsers.RepositoryItems.Add(chk);

            Col = new GridColumn()
            {
                Name = "CHECK",
                FieldName = "CHECK",
                Caption = " ",
                Width = 40,
                MinWidth = 40,
                MaxWidth = 60,
                Visible = true,
                ColumnEdit = chk
            };

            gvUsers.Columns.Add(Col);


            Col = new GridColumn()
            {
                Name = "Usu_Codigo",
                FieldName = "Usu_Codigo",
                Caption = "Nombre de Usuario",
                Width = 70,
                MinWidth = 70,
                MaxWidth = 150,
                Visible = true
            };
            Col.OptionsColumn.AllowEdit = false;
            Col.OptionsColumn.ReadOnly = true;
            gvUsers.Columns.Add(Col);



            Col = new GridColumn()
            {
                Name = "Usu_Nombre",
                FieldName = "Usu_Nombre",
                Caption = "Nombre",
                Width = 70,
                MinWidth = 70,
                MaxWidth = 150,
                Visible = true
            };
            Col.OptionsColumn.AllowEdit = false;
            Col.OptionsColumn.ReadOnly = true;
            gvUsers.Columns.Add(Col);

            Col = new GridColumn()
            {
                Name = "Usu_Apellido",
                FieldName = "Usu_Apellido",
                Caption = "Apellido",
                Width = 70,
                MinWidth = 70,
                MaxWidth = 150,
                Visible = true
            };
            Col.OptionsColumn.AllowEdit = false;
            Col.OptionsColumn.ReadOnly = true;
            gvUsers.Columns.Add(Col);
          

            Col = new GridColumn()
            {
                Name = "Usu_Estado",
                FieldName = "Usu_Estado",
                Caption = "Estado",
                Width = 90,
                MinWidth = 90,
                MaxWidth = 150,
                Visible = false
            };
            Col.OptionsColumn.AllowEdit = false;
            Col.OptionsColumn.ReadOnly = true;
            gvUsers.Columns.Add(Col);


            Col = new GridColumn()
            {
                Name = "USU_ID",
                FieldName = "USU_ID",
                Visible = false
            };
            gvUsers.Columns.Add(Col);

            gvUsers.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        }
        private void configuraDT()
        {
            mDtUsers = new DataTable();

            DataColumn col;
            col = new DataColumn("CHECK", typeof(int));
            col.DefaultValue = 0;
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Codigo", typeof(string));
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Nombre", typeof(string));
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Apellido", typeof(string));
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Password", typeof(string));
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Estado", typeof(bool));
            mDtUsers.Columns.Add(col);

            col = new DataColumn("Usu_Id", typeof(int));
            col.DefaultValue = 0;
            mDtUsers.Columns.Add(col);

            gvUsers.BeginUpdate();
            gcUsers.DataSource = mDtUsers;
            gvUsers.EndUpdate();
        }

        private bool hayMarcados()
        {
            foreach (DataRow item in mDtUsers.Rows)
            {
                if (item.Field<int>("CHECK") == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private int cantidadMarcados()
        {
            int cantidad = 0;

            foreach (DataRow item in mDtUsers.Rows)
            {
                if (item.Field<int>("CHECK") == 1)
                {
                    cantidad++;
                }
            }
            return cantidad;
        }

        private void gvUsers_ShowingEditor(object sender, CancelEventArgs e)
        {
            string col = gvUsers.FocusedColumn.FieldName;

            if (!mPuedeModificar && col == "Usu_Estado")
            {
                e.Cancel = true;
                return;
            }
        }

        private void btnMostrarUsers_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mDtUsers.Clear();

            User usu = new User();

            DataTable dt = usu.getDtUsers();

            mDtUsers.Merge(dt, true, MissingSchemaAction.Ignore);
        }

        private void btnAgregar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Se crea la clase que levanta la ventana de ABM usuarios
            UsersWindows usuW = new UsersWindows();
            //Se setean los parametros necesarios para levantar la ventana.
            usuW.Id = 0;
            usuW.Owner = this;
            usuW.Titulo = "Alta de usuario";
            usuW.ShowRegisUsers(Util.TipoABM.Alta, false);
        }

        private void btnModificar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvUsers.FocusedRowHandle < 0) return;
            UsersWindows usuW = new UsersWindows();
            usuW.Id = Convert.ToInt32(gvUsers.GetRowCellValue(gvUsers.FocusedRowHandle, "Usu_Id"));
            usuW.Owner = this;
            usuW.Titulo = "Modificación de usuario";
            usuW.ShowEditor(Util.TipoABM.Modificacion, false);
        }

        private void btnEliminar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!hayMarcados())
            {
                MessageBox.Show("Seleccione uno o más registros para eliminar");
                return;
            }
            int cant = cantidadMarcados();

            if (MessageBox.Show("¿Esta seguro que desea eliminar los " + cant + " registros?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            foreach (DataRow item in mDtUsers.Rows)
            {
                if (item.Field<int>("CHECK") == 1)
                {
                    User usu = new User();
                    usu.Id = item.Field<int>("USU_ID");
                    usu.LeerDatos();
                    usu.Estado = false;
                    usu.GuardarDatos();
                    item.Delete();
                    MessageBox.Show("Usuario eliminado correctamente");
                }
            }
            mDtUsers.AcceptChanges();
        }

        private void btnSalir_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


    }
}