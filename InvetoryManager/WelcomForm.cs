using System;
using System.Windows.Forms;

namespace InvetoryManager
{
    public partial class WelcomForm : Form
    {
        // Intervalo em milissegundos para o Timer
        private const int TimerInterval = 50;
        private const int ProgressBarIncrement = 2;

        public WelcomForm()
        {
            InitializeComponent();

            // Configurações da ProgressBar
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;

            // Configurações do Timer
            timer1.Interval = TimerInterval;
            timer1.Tick += Timer1_Tick;

            // Inicie o Timer
            timer1.Start();
        }

        private async void Timer1_Tick(object sender, EventArgs e)
        {
            // Aumenta o valor da ProgressBar
            progressBar1.Value += ProgressBarIncrement;

            // Verifica se a ProgressBar atingiu o valor máximo
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
                timer1.Stop();// Para o Timer
                OpenLoginForm(); // Abre o formulário de login
            }
        }

        private void OpenLoginForm()
        {
            // Cria e exibe o formulário de login
            LoginForms loginForms = new LoginForms();
            this.Hide(); // Oculta o formulário atual
            loginForms.ShowDialog(); // Exibe o formulário de login
            this.Close(); // Fecha o formulário atual após o fechamento do formulário de login
        }

    }
}
