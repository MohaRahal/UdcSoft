using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvetoryManager
{
    public partial class MainForm : Form
    {
    

        public MainForm()
        {
            InitializeComponent();
            this.Text = "© Controle de Estoque - UDCSOFT";
        }
        private Form activeForm = null;
        private void openChildForm(Form childform)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childform;
            childform.TopLevel = false;
            childform.FormBorderStyle = FormBorderStyle.None;
            childform.Dock = DockStyle.Fill;
            panelMain.Controls.Add(childform);
            panelMain.Tag = childform;
            childform.BringToFront();
            childform.Show();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            openChildForm(new CategoryForm());
        }

        private void btnProutos_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            openChildForm(new Orderforms());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
            {
                activeForm.Close();
                activeForm = null; // Definir activeForm como null para que não haja referência a um formulário fechado
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
