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

namespace InvetoryManager
{
    public partial class CustomerModuleForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void CustomerModuleForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
              
                cm = new SqlCommand("SELECT COUNT(*) FROM tbCustomer WHERE cphone = @cphone", com);
                cm.Parameters.AddWithValue("@cphone", textCNumber.Text);

                com.Open();
                int count = (int)cm.ExecuteScalar(); 
                com.Close();

                if (count > 0)
                {
                    MessageBox.Show("Este cliente já está cadastrado com o mesmo número de telefone.", "Cliente Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

               
                if (MessageBox.Show("Você tem certeza que deseja salvar este cliente?", "Salvar Cliente?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbCustomer(cname, cphone) VALUES(@cname, @cphone)", com);
                    cm.Parameters.AddWithValue("@cname", textCName.Text);
                    cm.Parameters.AddWithValue("@cphone", textCNumber.Text);

                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();

                    MessageBox.Show("Cliente cadastrado com sucesso!");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
               
                if (com.State == ConnectionState.Open)
                {
                    com.Close();
                }
            }

        }
        public void Clear()
        {
            textCName.Clear();
            textCNumber.Clear();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonSave.Enabled = true;
            buttonEdit.Enabled = false;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voce deseja sair da aba " + "Editar Clientes?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Voce tem certeza que deseja atualizar esse cliente?", "Atualizado!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("UPDATE tbCustomer SET cname = @cname,cphone=@cphone WHERE cid LIKE '" + lblCld.Text + "' ", com);
                    cm.Parameters.AddWithValue("@cname", textCName.Text);
                    cm.Parameters.AddWithValue("@cphone", textCNumber.Text);
                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Cliente Atualizado com Sucesso!");
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
