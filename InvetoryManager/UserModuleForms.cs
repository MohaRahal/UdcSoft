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
    public partial class UserModuleForms : Form
    {
        //PASSAR O LOCAL DO AQRUIVO DO BANCO DE DADOS AQUI
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        public UserModuleForms()
        {
            InitializeComponent();
        }
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voce deseja sair da aba " + "Editar Usuarios?", "Confirmar,", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
            }
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPass.Text))
                {
                    MessageBox.Show("O campo de senha é obrigatório!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
               

                if (textPass.Text != textRepass.Text)
                {
                    MessageBox.Show("As senhas nao sao iguais!","Aviso",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(MessageBox.Show("Voce tem que certeza que deseja salvar este usuario?" , "Salvo",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbUser(username,fullname,password,phone)VALUES(@username,@fullname,@password,@phone)", com);
                    cm.Parameters.AddWithValue("@username",textUsername.Text);
                    cm.Parameters.AddWithValue("@fullname",textFullname.Text);
                    cm.Parameters.AddWithValue("@password", textPass.Text);
                    cm.Parameters.AddWithValue("@phone", textNumber.Text);
                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Usuario salvo com sucesso!");
                    Clear();
                    this.Dispose();

                }

            }
            catch (SqlException ex)
            {
                // Verificar se o erro é de violação de chave primária
                if (ex.Number == 2627) // Erro 2627 para chave duplicada (Primary Key ou Unique Key)
                {
                    MessageBox.Show(textUsername.Text +" já está registrado. Por favor, insira um  diferente.", "Erro de Duplicação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Tratar outros erros do SQL
                    MessageBox.Show("Erro ao salvar o usuário: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Certificar-se de que a conexão será fechada
                if (com.State == ConnectionState.Open)
                {
                    com.Close();
                }
            }
        }


        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonSave.Enabled = true;
            buttonEdit.Enabled = false;
        }

        public void Clear()
        {
            textUsername.Clear();
            textFullname.Clear();
            textPass.Clear();
            textNumber.Clear();
            textRepass.Clear();


        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPass.Text) || string.IsNullOrWhiteSpace(textRepass.Text))
                {
                    MessageBox.Show("As senhas não podem estar vazias!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (textPass.Text.Trim() != textRepass.Text.Trim())
                {
                    MessageBox.Show("As senhas não são iguais!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Voce tem que certeza que deseja editar este usuario?", "Editando", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbUser SET fullname =@fullname,password=@password,phone=@phone WHERE username LIKE'"+textUsername.Text+"' ", com);
                    cm.Parameters.AddWithValue("@fullname", textFullname.Text);
                    cm.Parameters.AddWithValue("@password", textPass.Text);
                    cm.Parameters.AddWithValue("@phone", textNumber.Text);
                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Usuario Atualizado com sucesso!");
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
