using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvetoryManager
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        /// PARA USAR O APP EM OUTRO COMPUTADOR LEMBRAR DE TROCAR OS ENDERECOS DO DATABASE  DBIMS.MDF FILE , DEVE SER PASSADO O LOCAL DO ARQUIVO
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}



