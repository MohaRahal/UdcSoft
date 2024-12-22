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
using System.Xml.Linq;

namespace InvetoryManager
{
    public partial class LoginForms : Form
    {
        /// <summary>
        /// AQUI FAZENDO LIGACAO COM O FATABASE
        /// </summary>
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public LoginForms()
        {
            InitializeComponent();
            this.Cursor = Cursors.Default;
        }

        private void LoginForms_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPass.Checked == false)
            {
                Textpass.UseSystemPasswordChar = true;
            }
            else
            {
                Textpass.UseSystemPasswordChar = false;
            }
        }

        private void Textpass_TextChanged(object sender, EventArgs e)
        {

        }

       
        private void pictureBoxClose_MouseEnter(object sender, EventArgs e)
        {
            // Altera o cursor para "Mão" quando o mouse entra no PictureBox
            this.Cursor = Cursors.Hand;
        }

        private void pictureBoxClose_MouseLeave(object sender, EventArgs e)
        {
            // Restaura o cursor padrão quando o mouse sai do PictureBox
            this.Cursor = Cursors.Default;
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Sair da Aplicação","Confirmar",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ///
            /// REALIZACAO DO LOGIN E COMPARANDO AS INFORMACOES COM O BANCO DE DADOS
            ///
            try
            {
                cm = new SqlCommand("SELECT * FROM tbUser WHERE username=@username AND password=@password", con);
                cm.Parameters.AddWithValue("@username", textName.Text);
                cm.Parameters.AddWithValue("@password", Textpass.Text);
                con.Open();
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    MessageBox.Show("Bem-Vindo " + dr["username"].ToString() , "Acesso Aprovado!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    this.Hide();
                    main.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Senha ou Usuario Incorretos!", "Acesso Negado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textName.Clear();
            Textpass.Clear();
        }
    }
}
            
        
    

