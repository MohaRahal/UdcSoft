using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection.Emit;

namespace InvetoryManager
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;

        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            if (com.State == ConnectionState.Open) com.Close();

        }

        public void LoadCustomer()
        {
            int i = 0;
            dataCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid, cname FROM tbCustomer WHERE CONCAT(cid, cname) LIKE '%" + txtSearchCust.Text + "%'", com);

            if (com.State == ConnectionState.Closed) com.Open();

            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            com.Close();
        }

        public void LoadProduct()
        {

            int i = 0;
            dgvProducts.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%" + txtSearchProd.Text + "%'", com);

            if (com.State == ConnectionState.Closed) com.Open();

            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProducts.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            com.Close();
        }

        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void txtSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void dataCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCid.Text = dataCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCName.Text = dataCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
            
        }


        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal price;
            int quantity;

            if (decimal.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out price) &&
                int.TryParse(numericUpDown1.Value.ToString(), out quantity))
            {
                if (quantity > qty)
                {
                    MessageBox.Show("Estoque Insuficiente!", "Alerta!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numericUpDown1.Value = numericUpDown1.Value - 1;
                    return;
                }

                decimal total = price * quantity;
                txtTotal.Text = total.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                txtTotal.Text = "0.00";
            }
        }


        private void dataCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCid.Text = dataCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCName.Text = dataCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPid.Text = dgvProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPName.Text = dgvProducts.Rows[e.RowIndex].Cells[2].Value.ToString();

            // Ler o preço como decimal
            decimal price;
            if (decimal.TryParse(dgvProducts.Rows[e.RowIndex].Cells[4].Value.ToString(), out price))
            {
                txtPrice.Text = price.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                txtPrice.Text = "0.00";
            }

            qty = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells[3].Value.ToString());
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {

            try
            {
                string dateFormat = "dd/MM/yyyy";
                if (!DateTime.TryParseExact(dtOrder.Text, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime orderDate))
                {
                    MessageBox.Show("Erro: O valor da data do pedido não é válido. Por favor, use o formato " + dateFormat, "Erro de Conversão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(txtCid.Text))
                {
                    MessageBox.Show("Selecione um Cliente!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtPid.Text))
                {
                    MessageBox.Show("Selecione um Produto!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Você tem certeza que deseja salvar este pedido?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (SqlConnection com = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;"))
                    {
                        com.Open();
                        using (SqlCommand cm = new SqlCommand("INSERT INTO tbOrder (odate, pid, cid, qty, price, total) VALUES (@odate, @pid, @cid, @qty, @price, @total)", com))
                        {
                            cm.Parameters.AddWithValue("@odate", dtOrder.Value);
                            cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtPid.Text));
                            cm.Parameters.AddWithValue("@cid", Convert.ToInt32(txtCid.Text));
                            cm.Parameters.AddWithValue("@qty", Convert.ToInt32(numericUpDown1.Value));
                            cm.Parameters.AddWithValue("@price", Decimal.Parse(txtPrice.Text, NumberStyles.Any, CultureInfo.InvariantCulture));
                            cm.Parameters.AddWithValue("@total", Decimal.Parse(txtTotal.Text, NumberStyles.Any, CultureInfo.InvariantCulture));

                            cm.ExecuteNonQuery();
                        }

                        using (SqlCommand updateCmd = new SqlCommand("UPDATE tbProduct SET pqty = pqty - @pqty WHERE pid = @pid", com))
                        {
                            updateCmd.Parameters.AddWithValue("@pqty", Convert.ToInt16(numericUpDown1.Value));
                            updateCmd.Parameters.AddWithValue("@pid", Convert.ToInt32(txtPid.Text));

                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    LoadProduct();
                    Clear();
                }

                if (MessageBox.Show("Deseja cadastrar mais pedidos?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao salvar o Pedido: " + ex.Message);
            }
        }



        public void Clear()
        {
            txtCid.Clear();
            txtCName.Clear();
            txtPid.Clear();
            txtPName.Clear();
            txtPrice.Clear();
            numericUpDown1.Value = 1;
            txtTotal.Clear();
            dtOrder.Value = DateTime.Now;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnInsert.Enabled = true;
            buttonEdit.Enabled = false;
        }

       
    }
}
