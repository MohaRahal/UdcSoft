using System;
using System.IO;

namespace BackupDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            // Caminho do arquivo .mdf e .ldf
            string databaseFile = @"D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS.mdf";
            string logFile = @"D:\InventoryManagementSystem\InventoryManagementSystem\Tutorial Database\dbIMS_log.ldf";

            // Caminho do diretório de backup
            string backupDirectory = @"D:\TESTE\InvetoryManager\InvetoryManager\backup-files";

            // Nome do arquivo de backup com data e hora para identificar
            string backupFileName = "dbIMS_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".mdf";
            string logBackupFileName = "dbIMS_log_backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".ldf";

            try
            {
                // Cria o diretório de backup, se ele não existir
                if (!Directory.Exists(backupDirectory))
                {
                    Directory.CreateDirectory(backupDirectory);
                }

                // Caminhos completos de destino para os arquivos de backup
                string databaseBackupPath = Path.Combine(backupDirectory, backupFileName);
                string logBackupPath = Path.Combine(backupDirectory, logBackupFileName);
                Console.ForegroundColor = ConsoleColor.Green;

                // Copia o arquivo .mdf para o diretório de backup
                File.Copy(databaseFile, databaseBackupPath, true);
                Console.WriteLine("Arquivo de banco de dados .mdf copiado com sucesso!");

                // Copia o arquivo .ldf para o diretório de backup
                File.Copy(logFile, logBackupPath, true);
                Console.WriteLine("Arquivo de log .ldf copiado com sucesso!");
                Console.ResetColor();
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao realizar backup: " + ex.Message);
            }
        }
    }
}
