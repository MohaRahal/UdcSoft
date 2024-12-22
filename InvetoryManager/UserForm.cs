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
    public partial class UserForm : Form
    {                                         //PASSAR O LOCAL DO AQRUIVO DO BANCO DE DADOS AQUI
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;");
        SqlCommand cm = new SqlCommand();
        SqlDataReader reader;
        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }

        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser", con);
            con.Open();
            reader = cm.ExecuteReader();
            while (reader.Read())
            {
                i++;

                // Substitui a senha por bolinhas (●), assumindo que a senha está em reader[2]
                string passwordMasked = new string('●', reader[2].ToString().Length);

                // Adiciona a linha no DataGridView, com a senha mascarada
                dgvUser.Rows.Add(i, reader[0].ToString(), reader[1].ToString(), passwordMasked, reader[3].ToString());
            }
            reader.Close();
            con.Close();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModuleForms userModule = new UserModuleForms();
            userModule.buttonSave.Enabled = true;
            userModule.buttonEdit.Enabled = false;
            userModule.buttonEdit.Visible = false;
            userModule.buttonSave.Location = new Point(200, 300);
            userModule.buttonClear.Location = new Point(375, 300);
            userModule.ShowDialog();
            LoadUser();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name; 
          


            if (colName == "Edit")
            {
                UserModuleForms userModule = new UserModuleForms();

                // Preencher os campos do formulário com os valores da linha selecionada
                userModule.textUsername.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.textFullname.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();           
                userModule.textNumber.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.buttonSave.Enabled = false;
                userModule.buttonSave.Visible = false;
                userModule.buttonEdit.Enabled = true;
                userModule.textUsername.Enabled = false;
                userModule.ShowDialog();
                LoadUser();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Você tem certeza que deseja deletar o usuário " + dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString() + " ?", "Apagar Usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        con.Open();
                        cm = new SqlCommand("DELETE FROM tbUser WHERE username LIKE '" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                        cm.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Usuario Deletado com Sucesso!");
                     
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir usuário: " + ex.Message);
                    }
                    finally
                    {
                        con.Close();
                    }

                    // Recarregar a lista de usuários
                    LoadUser();
                }
            }
        }

    }
}
