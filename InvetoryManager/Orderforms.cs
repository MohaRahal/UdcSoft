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
    public partial class Orderforms : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf"";Integrated Security=True;Connect Timeout=30;");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public Orderforms()
        {
            InitializeComponent();
            LoadUser();
        }

        public void LoadUser()
        {

            double total = 0;
            int i = 0;
            dgvOrder.Rows.Clear();

            cm = new SqlCommand("SELECT orderid, odate, O.pid, P.pname, O.cid, C.cname, qty, price, total " +
                                "FROM tbOrder AS O " +
                                "JOIN tbCustomer AS C ON O.cid = C.cid " +
                                "JOIN tbProduct AS P ON O.pid = P.pid " +
                                "WHERE CONVERT(VARCHAR, odate, 103) LIKE @search", con);
            cm.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvOrder.Rows.Add(i,
                                  dr[0].ToString(),
                                  Convert.ToDateTime(dr[1]).ToString("dd/MM/yyyy"),
                                  dr[2].ToString(),
                                  dr[3].ToString(),
                                  dr[4].ToString(),
                                  dr[5].ToString(),
                                  dr[6].ToString(),
                                  dr[7].ToString(),
                                  Convert.ToDecimal(dr[8]).ToString("N2"));
                total += Convert.ToDouble(dr[8]);
            }
            dr.Close();
            con.Close();

            lblQty.Text = i.ToString();
            lblTotal.Text = total.ToString();   


        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (OrderModuleForm moduleForm = new OrderModuleForm())
            {
                moduleForm.btnInsert.Enabled = true;
                moduleForm.buttonEdit.Enabled = false;
                moduleForm.buttonEdit.Visible = false;
                moduleForm.ShowDialog();
                LoadUser();
            }
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvOrder.Columns[e.ColumnIndex].Name;

            if (colName == "Delete")
            {
                if (MessageBox.Show("Você tem certeza que deseja excluir esse pedido?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();

                    // Obter a quantidade do pedido
                    var qtyCellValue = dgvOrder.Rows[e.RowIndex].Cells[7].Value?.ToString();
                    MessageBox.Show("Valor da célula de quantidade: " + qtyCellValue); // Diagnóstico

                    // Verifique se a célula não está vazia e se é um número
                    if (int.TryParse(qtyCellValue, out int qty))
                    {
                        // Deletar o pedido
                        cm = new SqlCommand("DELETE FROM tbOrder WHERE orderid = @orderId", con);
                        cm.Parameters.AddWithValue("@orderId", dgvOrder.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cm.ExecuteNonQuery();

                        // Atualizar a quantidade do produto
                        cm = new SqlCommand("UPDATE tbProduct SET pqty = pqty + @pqty WHERE pid = @productId", con);
                        cm.Parameters.AddWithValue("@pqty", qty);
                        cm.Parameters.AddWithValue("@productId", dgvOrder.Rows[e.RowIndex].Cells[3].Value.ToString());
                        cm.ExecuteNonQuery();

                        MessageBox.Show("Pedido deletado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Erro: Quantidade inválida. Valor lido: " + qtyCellValue);
                    }

                    con.Close();
                }
                LoadUser();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        
    }
}
