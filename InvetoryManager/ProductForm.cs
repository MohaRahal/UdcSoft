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
    public partial class ProductForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
         
            
        }
        public void LoadProduct()
        {
            int i = 0;
            dgvProducts.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%" + textSearch.Text + "%'", com);
            com.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProducts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            com.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm formModule = new ProductModuleForm();
            formModule.buttonSave.Enabled = true;
            formModule.buttonEdit.Enabled = false;
            formModule.buttonEdit.Visible = false;
            formModule.ShowDialog();
            LoadProduct();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProducts.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();
                productModule.labelID.Text = dgvProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.textPNAME.Text = dgvProducts.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.textPQTY.Text = dgvProducts.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.textPPRICE.Text = dgvProducts.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.textPDESC.Text = dgvProducts.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.comboQty.Text = dgvProducts.Rows[e.RowIndex].Cells[6].Value.ToString();

                productModule.buttonSave.Visible = false;
                productModule.buttonEdit.Enabled = true;
                productModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Voce tem certeza que deseja Excluir este produto?", "Excluir produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    com.Open();
                    cm = new SqlCommand("DELETE FROM tbProduct WHERE pid LIKE '" + dgvProducts.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", com);
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Produto Excluido!");
                }
            }
            LoadProduct();
        }

      

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
                LoadProduct();
        }
    }
}
