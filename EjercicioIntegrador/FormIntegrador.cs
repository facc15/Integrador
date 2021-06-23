using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entidades;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.Sql;
using System.Drawing.Imaging;

namespace EjercicioIntegrador
{
    public partial class FormIntegrador : Form
    {
        private SqlConnection cn;
        private DataTable dt;
        private SqlDataAdapter da;

        public FormIntegrador()
        {
            InitializeComponent();

            this.cn = new SqlConnection(Properties.Settings.Default.Conexion);

            this.ConfigurarGrilla();

            this.ConfigurarDataAdapter();

            this.ConfigurarDataTable();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void ConfigurarGrilla()
        {
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;

            this.dataGridView1.BackgroundColor = Color.LightCyan;
            this.dataGridView1.ForeColor = Color.Black;
            
        }

        private void ConfigurarDataAdapter()
        {
            try
            {
                this.da = new SqlDataAdapter();
                
                this.da.SelectCommand = new SqlCommand("SELECT id,nombre,satelites,gravedad FROM planetas", this.cn);
                this.da.InsertCommand = new SqlCommand("INSERT INTO planetas(nombre,satelites,gravedad) VALUES(@nombre,@satelites,@gravedad)", this.cn);
                this.da.UpdateCommand = new SqlCommand("UPDATE planetas SET nombre=@nombre,satelites=@satelites,gravedad=@gravedad WHERE id=@id", this.cn);
                this.da.DeleteCommand = new SqlCommand("DELETE FROM planetas WHERE id=@id", this.cn);

                this.da.InsertCommand.Parameters.Add("@nombre", SqlDbType.VarChar, 50, "nombre");
                this.da.InsertCommand.Parameters.Add("@satelites", SqlDbType.Int, 50, "satelites");
                this.da.InsertCommand.Parameters.Add("@gravedad", SqlDbType.Float, 50, "gravedad");

                this.da.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 10, "id");
                this.da.UpdateCommand.Parameters.Add("@nombre", SqlDbType.VarChar, 50, "nombre");
                this.da.UpdateCommand.Parameters.Add("@satelites", SqlDbType.Int, 10, "satelites");
                this.da.UpdateCommand.Parameters.Add("@gravedad", SqlDbType.Float, 50, "gravedad");

                this.da.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 10, "id");
                


            }
            catch (Exception e)
            {

                throw e;
            }


        }

        private void ConfigurarDataTable()
        {
            this.dt = new DataTable("planetas");
            this.dt.Columns.Add("id", typeof(int));
            this.dt.Columns.Add("nombre", typeof(string));
            this.dt.Columns.Add("satelites", typeof(int));
            this.dt.Columns.Add("gravedad", typeof(double));

            this.dt.Columns["id"].AutoIncrement = true;
            this.dt.Columns["id"].AutoIncrementSeed = 1;
            this.dt.Columns["id"].AutoIncrementStep = 1;

        }


        private void btn1_Click(object sender, EventArgs e)
        {
            Planeta planeta = new Planeta("Marte",9,13.4);

            if(planeta.SerializarXml())
                MessageBox.Show("Se serializó exitosamente!!!");
            else
                MessageBox.Show("Error al serializar");


            this.richTxtBox.Text=planeta.DeserializarXml();

        }

        private void btn2_Click(object sender, EventArgs e)
        {
            Planeta planeta = new Planeta("Mercurio", 4, 23.4);
            Planeta planeta2 = new Planeta("Marte", 9, 245.4);
            Planeta planeta3 = new Planeta("Venus", 2, 333.4);

            SistemaSolar<Planeta> sistema = new SistemaSolar<Planeta>(5);

            
            sistema.Agregar(planeta);
            sistema.Agregar(planeta2);
            sistema.Agregar(planeta3);

            string planetas = "";

            foreach(Planeta aux in sistema.lista)
            {
                planetas += aux.ToString();
            }
            this.richTxtBox.Text = planetas;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            Planeta planeta = new Planeta("Mercurio", 4, 23.4);
            Planeta planeta2 = new Planeta("Marte", 9, 245.4);
            Planeta planeta3 = new Planeta("Venus", 2, 333.4);
            Planeta planeta4 = new Planeta("Tierra", 1, 222.1);

            SistemaSolar<Planeta> sistema = new SistemaSolar<Planeta>(3);

            sistema.Agregar(planeta);
            sistema.Agregar(planeta2);
            sistema.Agregar(planeta3);
            try
            {
                sistema.Agregar(planeta4);
            }
            catch (NoHayLugarException ex)
            {

                MessageBox.Show(ex.Message);
            }
            


        }

        private void btnTraer_Click(object sender, EventArgs e)
        {
           
            this.dt = this.TraerPlanetas();

            this.dataGridView1.DataSource = this.dt;
        }

        public DataTable TraerPlanetas()
        {

            try
            {
                this.da.Fill(this.dt);
            }
            catch (Exception e)
            {

                throw e;
            }

            return this.dt;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FormAlta alta = new FormAlta();

            if(alta.ShowDialog()==DialogResult.OK)
            {
                DataRow fila = this.dt.NewRow();

                fila[1] = alta.planeta.nombre;
                fila[2] = alta.planeta.satelites;
                fila[3] = alta.planeta.gravedad;

                this.dt.Rows.Add(fila);

                this.dataGridView1.DataSource = this.dt;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int i = this.dataGridView1.SelectedRows[0].Index;

            DataRow fila = this.dt.Rows[i];

            Planeta p = new Planeta(fila[1].ToString(), int.Parse(fila[2].ToString()), double.Parse(fila[3].ToString()));

            FormAlta planetaModificado = new FormAlta(p);

            if (planetaModificado.ShowDialog() == DialogResult.OK)
            {
                this.dt.Rows[i][1] = planetaModificado.planeta.nombre;
                this.dt.Rows[i][2] = planetaModificado.planeta.satelites;
                this.dt.Rows[i][3] = planetaModificado.planeta.gravedad;

                this.dataGridView1.DataSource = this.dt;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int i = this.dataGridView1.SelectedRows[0].Index;

            DataRow fila = this.dt.Rows[i];

            Planeta p = new Planeta(fila[1].ToString(), int.Parse(fila[2].ToString()), double.Parse(fila[3].ToString()));

            FormAlta planetaEliminado = new FormAlta(p);

            if (planetaEliminado.ShowDialog() == DialogResult.OK)
            {
                this.dt.Rows[i].Delete();

                this.dataGridView1.DataSource = this.dt;
            }

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (this.GuardarDatos())
            {
                MessageBox.Show("Se guardo exitosamente", "GUARDAR", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
            }else
            {
                MessageBox.Show("Error en guardar");
            }
        }

        private bool GuardarDatos()
        {
            try
            {
                this.da.Update(this.dt);
            }
            catch (Exception e)
            {

                return false;
            }
            return true;

        }
    }
}
