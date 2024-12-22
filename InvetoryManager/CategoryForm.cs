using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvetoryManager
{
    public partial class CategoryForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dataCategory.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCategory", com);
            com.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            com.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CategoryModuleForm formModule = new CategoryModuleForm();
            formModule.buttonSave.Enabled = true;
            formModule.buttonEdit.Enabled = false;
            formModule.buttonEdit.Visible = false;
            formModule.ShowDialog();
            LoadCategory();
        }

        private void dataCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Editar")
            {
                CategoryModuleForm formModule = new CategoryModuleForm();             
                formModule.lblCatld.Text = dataCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                formModule.textCatName.Text = dataCategory.Rows[e.RowIndex].Cells[2].Value.ToString();
                formModule.buttonSave.Enabled = false;
                formModule.buttonSave.Visible = false;
                formModule.buttonEdit.Enabled = true;
                formModule.buttonEdit.Location = new Point(150, 170);
                formModule.buttonClear.Location = new Point(250, 170);

                formModule.ShowDialog();
            }
            else if (colName == "Deletar")
            {
                if (MessageBox.Show("Voce tem certeza que deseja excluir a categoria " + dataCategory.Rows[e.RowIndex].Cells[2].Value.ToString(), "Excluir Categoria", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {  
                        com.Open();                       
                        cm = new SqlCommand("DELETE FROM tbCategory WHERE catid = @catid", com);
                        cm.Parameters.AddWithValue("@catid", dataCategory.Rows[e.RowIndex].Cells[1].Value.ToString());               
                        cm.ExecuteNonQuery();
                        MessageBox.Show("Categoria Excluida com sucesso!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                    finally
                    {                
                        if (com.State == ConnectionState.Open)
                        {
                            com.Close();
                        }
                    }
                }
            }
            LoadCategory();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
