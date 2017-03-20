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
    public partial class frmAlumno : Form
    {
        private DataTable tablaCurso;
        private Alumno alumno;

        public Alumno Alumno
        {
            get { return alumno; }
        }
        public frmAlumno(Alumno alum,string queEs):this()
        {
            alumno = new Alumno();
            alumno = alum;
            textLegajo.Enabled = false;
            if (queEs.Equals("borrar"))
            {
                textApellido.Enabled = false;
                comboCurso.Enabled = false;
            }
        }
        
        public frmAlumno()
        {
            //alumno = new Alumno();
            InitializeComponent();
           

        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            alumno = new Alumno();
            alumno.Apellido = textApellido.Text;
            alumno.Legajo = int.Parse(textLegajo.Text);
            int codigo = Convert.ToInt32(((DataRowView)comboCurso.SelectedItem)[0]);
            alumno.CodCurso = codigo;
             this.DialogResult = DialogResult.OK;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmAlumno_Load(object sender, EventArgs e)
        {
            CargarComboBox();
            CargarAlumno();
        }

        private void CargarComboBox()
        {
             tablaCurso = ((frmPrincipal)Owner).CrearDataTableCursos();

            comboCurso.DataSource = tablaCurso;
            comboCurso.DisplayMember = "nombre";

            comboCurso.BindingContext = this.BindingContext;
        }
        private void SeleccionarComboBox(int codCurso)
        {
            //DataTable tablaCurso = ((frmPrincipal)Owner).CrearDataTableCursos();
            for (int i = 0; i < tablaCurso.Rows.Count; i++)
            {
                if (Convert.ToInt32(tablaCurso.Rows[i][0]) == codCurso)
                {
                    comboCurso.SelectedIndex = i;
                    return;
                }
            }

        }
        private void CargarAlumno()
        {
            if (alumno == null)
            {
                return;
            }
            textApellido.Text = alumno.Apellido;
            textLegajo.Text = alumno.Legajo.ToString();
            SeleccionarComboBox(alumno.CodCurso);
        }
    }
}
