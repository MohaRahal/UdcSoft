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
    public partial class CustomerForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomer();
        }
        public void LoadCustomer()
        {
            int i = 0;
            dataCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCustomer", com);
            com.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            com.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm moduleForm = new CustomerModuleForm();
            moduleForm.buttonSave.Enabled = true;
            moduleForm.buttonEdit.Enabled = false;
            moduleForm.buttonEdit.Visible = false;
            moduleForm.buttonSave.Location = new Point(150, 200);
            moduleForm.buttonClear.Location = new Point(350, 200);
            moduleForm.ShowDialog();
            LoadCustomer();
        }

        private void dataCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataCustomer.Columns[e.ColumnIndex].Name;
            

            

            if (colName == "Editar")
            {
                
                CustomerModuleForm customerModule = new CustomerModuleForm();

                customerModule.lblCld.Text = dataCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.textCName.Text = dataCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.textCNumber.Text = dataCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();

                customerModule.buttonSave.Enabled = false;
                customerModule.buttonSave.Visible = false;
                customerModule.buttonEdit.Enabled = true;
                customerModule.buttonEdit.Location = new Point(150, 200);
                customerModule.buttonClear.Location = new Point(250, 200);
                customerModule.ShowDialog();
                LoadCustomer();
            }
            else if (colName == "Deletar")
            {
                string cid = dataCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (MessageBox.Show("Voce tem certeza que deseja excluir o cliente " + dataCustomer.Rows[e.RowIndex].Cells[2].Value.ToString() + " ?", "Excluir Cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        com.Open();
                        cm = new SqlCommand("DELETE FROM tbCustomer WHERE cid = @cid", com);
                        cm.Parameters.AddWithValue("@cid", cid);
                        int result = cm.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Registro excluído com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Nenhuma linha foi afetada!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro: " + ex.Message);
                    }
                    finally
                    {
                        com.Close();  
                    }
                }
                LoadCustomer();
            }

        }
    }
}
