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
    public partial class ProductModuleForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"); //trocar para o local da pasta que estara o database do usuario
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        CategoryForm CategoryForm = new CategoryForm();
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboQty.Items.Clear();
            cm = new SqlCommand("SELECT catname FROM tbCategory", com);
            com.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboQty.Items.Add(dr[0].ToString());
            }
            dr.Close();
            com.Close();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Voce tem certeza que deseja Adicionar este Produto?", "Salvar Produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("INSERT INTO tbProduct(pname,pqty,pprice,pdescription,pcategory)VALUES(@pname, @pqty, @pprice, @pdescription, @pcategory)", com);
                    cm.Parameters.AddWithValue("@pname", textPNAME.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(textPQTY.Text));
                    cm.Parameters.AddWithValue("@pprice",textPPRICE.Text);
                    cm.Parameters.AddWithValue("@pdescription", textPDESC.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboQty.Text);

                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Produto Salvo com Sucesso.");
                    Clear();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            textPNAME.Clear();
            textPQTY.Clear();
            textPPRICE.Clear();
            textPDESC.Clear();
            comboQty.Text = "";
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonSave.Enabled = true;
            buttonEdit.Enabled = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Voce tem certeza que deseja editar esse produto?", "Editar Produto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("UPDATE tbProduct SET pname = @pname, pqty=@pqty, pprice=@pprice, pdescription=@pdescription, pcategory=@pcategory WHERE pid LIKE '" + labelID.Text + "' ", com);
                    cm.Parameters.AddWithValue("@pname", textPNAME.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(textPQTY.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToDecimal(textPPRICE.Text));
                    cm.Parameters.AddWithValue("@pdescription", textPDESC.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboQty.Text);
                    com.Open();
                    cm.ExecuteNonQuery();
                    com.Close();
                    MessageBox.Show("Produto Atualizado!");
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
