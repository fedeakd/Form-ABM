using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidad;
namespace Modelo.SegundoParcial.LABIII
{
    public partial class FrmMostrar : Form
    {
        private EnumMostrar queMostrar;
        private DataSet tablaSet;
        public FrmMostrar(EnumMostrar mostrar):this()
        {
            this.queMostrar = mostrar;
        }
        public FrmMostrar()
        {
            InitializeComponent();
        }

        private void FrmMostrar_Load(object sender, EventArgs e)
        {
            this.tablaSet = ((frmPrincipal)this.Owner).DataSetAlumno;
            if (tablaSet.Tables.Count < 2)
            {
                this.tablaSet.Tables.Add(((frmPrincipal)this.Owner).CrearDataTableCursos());
                RelacionDeTablas();
            }
           
            ConfigurarDataGridView();
            CargarTabla();
        }
        private void ConfigurarDataGridView()
        {
            dataGriew.MultiSelect = false; //multiselección No permitida
            dataGriew.AllowUserToAddRows = false;
            dataGriew.RowHeadersVisible = false;
            dataGriew.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGriew.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataTable tabla = queMostrar == EnumMostrar.Curso ? this.tablaSet.Tables[1] : this.tablaSet.Tables[0];
            int cantidadDeColumnas =tabla.Columns.Count;
            List<string> nombreColumnas = new List<string>();
            foreach (DataColumn item in tabla.Columns)
            {
                nombreColumnas.Add(item.ColumnName);
            }
           
            this.dataGriew.ColumnCount = cantidadDeColumnas;

            for (int i = 0; i < cantidadDeColumnas; i++)
            {
                this.dataGriew.Columns[i].Name = nombreColumnas[i];
            }
        }
        private void CargarTabla()
        {
            DataTable tabla = queMostrar == EnumMostrar.Curso ? this.tablaSet.Tables[1] : this.tablaSet.Tables[0];
            if (queMostrar != EnumMostrar.AlumnoASPNET) {
                foreach (DataRow item in tabla.Rows)
                {
                    if (item.RowState != DataRowState.Deleted)
                    {
                        switch (queMostrar)
                        {
                            case EnumMostrar.Alumno:
                                dataGriew.Rows.Add(item.ItemArray);
                                break;
                            case EnumMostrar.Curso:
                                dataGriew.Rows.Add(item.ItemArray);
                                break;
                            case EnumMostrar.AlumnoCurso:
                                dataGriew.Rows.Add(new object[] { item[0],item[1],
                             item.GetParentRow("CursosAlumno")[2] });

                                break;

                            default:
                                break;
                        }
                    }
                }
                  
            }
            else
            {
                DataRow[] filas = tablaSet.Tables[0].Select(string.Format("Curso=1005"));
                foreach (DataRow fila in filas)
                {
                    dataGriew.Rows.Add(new object[] { fila[0],fila[1],
                    fila.GetParentRow("CursosAlumno")[2]});
                }
            }
        }
        private void RelacionDeTablas()
        {
            DataRelation relacion = new DataRelation("CursosAlumno", tablaSet.Tables[1].Columns[0],
                tablaSet.Tables[0].Columns[2]);
            tablaSet.Relations.Add(relacion);

        }
    }
}
