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
    public partial class CategoryModuleForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        
        public CategoryModuleForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar se a categoria já existe
                cm = new SqlCommand("SELECT COUNT(*) FROM tbCategory WHERE catname = @catname", com);
                cm.Parameters.AddWithValue("@catname", textCatName.Text);

                com.Open();
                int count = (int)cm.ExecuteScalar(); // Retorna o número de categorias com o mesmo nome
                com.Close();

                if (count > 0)
                {
                    MessageBox.Show("Esta categoria já existe. Tente um nome diferente.", "Categoria Duplicada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Cancelar o processo de salvar
                }

                // Se a categoria não existe, continuar com a inserção
                if (MessageBox.Show("Você tem certeza que deseja salvar esta Categoria?", "Salvar Categoria", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbCategory(catname) VALUES(@catname)", com);
                    cm.Parameters.AddWithValue("@catname", textCatName.Text);

                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();

                    MessageBox.Show("Categoria foi salva com sucesso.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Garantir que a conexão seja fechada, caso esteja aberta
                if (com.State == ConnectionState.Open)
                {
                    com.Close();
                }
            }

        }
        public void Clear()
        {
            textCatName.Clear();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {

            Clear();
            buttonSave.Enabled = true;
            buttonEdit.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Voce tem certeza que deseja atualizar a categoria ?", "Atualizar Categoria", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("UPDATE tbCategory SET catname = @catname WHERE catid LIKE '" + lblCatld.Text + "' ", com);
                    cm.Parameters.AddWithValue("@catname", textCatName.Text);
                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Categoria atualiza com Sucesso!");
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
