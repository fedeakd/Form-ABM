using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Libreria;
using Microsoft.VisualBasic;
using Entidad;
namespace Modelo.SegundoParcial.LABIII
{
    public partial class frmPrincipal : Form
    {

        private DataSet dataSetAlumno;
        public DataSet DataSetAlumno
        {
            get
            {
                return dataSetAlumno;
            }

        }
        private SqlDataAdapter dataAdapter;
        private Conexion conectar;
        private frmAlumno frmAlumno;
        private FrmMostrar frmMostrar;
        public frmPrincipal()
        {
            dataSetAlumno = new DataSet();
            DataTable tablaAlumno = TraerDatos();
            this.dataSetAlumno.Tables.Add(tablaAlumno);
            InitializeComponent();

         // string algo=  Interaction.InputBox("asdsa","algo", "akgi", 400, 400);
         // MessageBox.Show(algo);
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            CrearDataTableCursos();
        }
        public DataTable CrearDataTableCursos() {
            DataTable tablaCurso = new DataTable("Curso");
            if (File.Exists("CursoDatos.xml") == false)
            {
                tablaCurso.Columns.Add("codigo", typeof(Int32));
                tablaCurso.Columns.Add("duracion", typeof(Int32));
                tablaCurso.Columns.Add("nombre", typeof(String));
                tablaCurso.Columns[0].AutoIncrement = true;
                tablaCurso.Columns[0].AutoIncrementSeed = 1000;
                tablaCurso.Columns[0].AutoIncrementStep = 5;
                tablaCurso.PrimaryKey = new DataColumn[] { tablaCurso.Columns[0] };

                tablaCurso.Rows.Add(new Object[] { null, 1, "Java" });
                tablaCurso.Rows.Add(new Object[] { null, 2, "ASPNET" });
                tablaCurso.Rows.Add(new Object[] { null, 3, "C#" });

                tablaCurso.WriteXml("CursoDatos.xml");
                tablaCurso.WriteXmlSchema("CursoEsquema.xml");
                //MessageBox.Show("Datos creados");
            }
            else {
                tablaCurso.ReadXmlSchema("CursoEsquema.xml");
                tablaCurso.ReadXml("CursoDatos.xml");
                
                //MessageBox.Show("Datos cargados");
            }
            return tablaCurso;
        }

        private void altaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.frmAlumno = new frmAlumno();
            if (frmAlumno.ShowDialog(this) == DialogResult.OK)
            {
                dataSetAlumno.Tables[0].Rows.Add(new object[] {
                    frmAlumno.Alumno.Legajo,
                    frmAlumno.Alumno.Apellido,
                    frmAlumno.Alumno.CodCurso

                });
            }
        }
  
        private void bajaToolStripMenuItem_Click(object sender, EventArgs e)
        {

           int algo= int.Parse( Interaction.InputBox("Ingrese el alumno que deseas borrar","Que hacer", "", 400, 200));
           Alumno alu= BuscarAlumno(algo);
            frmAlumno = new frmAlumno(alu, "borrar");
            if (alu != null)
            {

                if (frmAlumno.ShowDialog(this) == DialogResult.OK)
                {

                    this.dataSetAlumno.Tables[0].Select(string.Format("legajo={0}", algo))[0].Delete();
                }
            }
            else
            {
                MessageBox.Show("No se ah encontrado ese legajo");
            }
        }

        private Alumno BuscarAlumno(int legajo)
        {
            DataRow[] filas= this.dataSetAlumno.Tables[0].Select(string.Format("legajo={0}", legajo));
             DataRow fila=  filas.Length > 0 ? filas[0] : null;
            if (fila != null)
            {
                Alumno alu = new Alumno();
                alu.Apellido = fila[1].ToString();
                alu.Legajo = Convert.ToInt32(fila[0]);
                alu.CodCurso = Convert.ToInt32(fila[2]);
                return alu;
            }
            return null;

        }

        private void modificacionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int algo = int.Parse(Interaction.InputBox("Ingrese el alumno que deseas borrar", "Que hacer", "", 400, 200));
            Alumno alu = BuscarAlumno(algo);
            frmAlumno = new frmAlumno(alu,"");
            if (alu != null)
            {
                if (frmAlumno.ShowDialog(this) == DialogResult.OK)
                {
                    DataRow fila = this.dataSetAlumno.Tables[0].Select(string.Format("legajo={0}", algo))[0];
                    fila[0] = frmAlumno.Alumno.Legajo;
                    fila[1] = frmAlumno.Alumno.Apellido;
                    fila[2] = frmAlumno.Alumno.CodCurso;
                }
            }
            else
            {
                MessageBox.Show("No se ah encontrado ese legajo");

            }
        }
        private void alumnoToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            this.frmMostrar = new FrmMostrar(EnumMostrar.Alumno);
            if (frmMostrar.ShowDialog(this) == DialogResult.OK)
            {

            }
        }
        private void cursoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.frmMostrar = new FrmMostrar(EnumMostrar.Curso);
            frmMostrar.ShowDialog(this);
        }

        private void alumnoToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.frmMostrar = new FrmMostrar(EnumMostrar.AlumnoCurso);
            frmMostrar.ShowDialog(this);
        }

        private void mostrarAlumnoASPNETToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.frmMostrar = new FrmMostrar(EnumMostrar.AlumnoASPNET);
            frmMostrar.ShowDialog(this);
        }
        private DataTable TraerDatos()
        {
 
            SqlDataReader dataRead;
       
            DataTable tablaAlumno = new DataTable("Alumnos");
            conectar = new Conexion("USUARIO-PC", "facultadUtn");
            conectar.Conectar.Open();
            dataRead = new SqlCommand("SELECT * from Alumnos", conectar.Conectar).ExecuteReader();
            tablaAlumno.Load(dataRead);
            conectar.Conectar.Close();
            return tablaAlumno;

        }
        private SqlDataAdapter ConfigurarDataAdapter()
        {
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = new SqlCommand("Select * FROM Alumnos ",this.conectar.Conectar);
            da.InsertCommand = new SqlCommand("insert into Alumnos (legajo,apellido,curso) values" +
                "(@legajo,@apellido,@curso)",conectar.Conectar);
            da.UpdateCommand = new SqlCommand("update Alumnos set " +
                "legajo=@legajo, " +
                "apellido=@apellido, " +
                "curso=@curso "+
                "where legajo=@legajo",conectar.Conectar);
            da.DeleteCommand = new SqlCommand("DELETE from Alumnos where legajo=@legajo",conectar.Conectar);

            da.InsertCommand.Parameters.Add("@legajo", SqlDbType.Int,4,"legajo");
            da.InsertCommand.Parameters.Add("@apellido", SqlDbType.VarChar, 50,"apellido");
            da.InsertCommand.Parameters.Add("@curso", SqlDbType.Int,4,"curso");

            da.UpdateCommand.Parameters.Add("@legajo", SqlDbType.Int,4,"legajo");
            da.UpdateCommand.Parameters.Add("@apellido", SqlDbType.VarChar, 50,"apellido");
            da.UpdateCommand.Parameters.Add("@curso", SqlDbType.Int,4,"curso");

            da.DeleteCommand.Parameters.Add("@legajo", SqlDbType.Int,4,"legajo");



            return da;
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataAdapter = ConfigurarDataAdapter();
            dataAdapter.Update(dataSetAlumno.Tables[0]);
        }
    }
}
