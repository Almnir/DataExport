using Ionic.Zip;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DataExport
{
    class ExportProcess
    {
        private BackgroundWorker backgroundWorker;
        private ProgressWindow progressWindow;
        private ProgressBar progressBar;
        private List<string> checkedTables;
        private string errorString;
        private string zipName;

        public ExportProcess(ProgressWindow progressWindow, List<string> checkedTables, string zipName)
        {
            this.progressWindow = progressWindow;
            this.checkedTables = checkedTables;
            this.errorString = string.Empty;
            this.zipName = zipName;
            this.progressBar = progressWindow.GetProgressBar();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = 100;
            this.progressWindow.Close();
            if (e.Cancelled)
            {
                MessageBox.Show("Выгрузка отменена!");
            }
            if (e.Error == null)
            {
                MessageBox.Show("Выгрузка завершена!");
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            for (; i < this.checkedTables.Count; i++)
            {
                string tableName = this.checkedTables[i];
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                using (SqlConnection connection = new SqlConnection(Globals.GetConnectionString()))
                {
                    connection.Open();
                    string query = Globals.EXPORTS_QUERIES[tableName];
                    SqlCommand command = new SqlCommand(query, connection);
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Encoding.UTF8;
                    settings.CloseOutput = true;
                    string path = Path.GetDirectoryName(Path.GetFullPath(this.zipName));
                    string filePath = Path.Combine(path, tableName + ".xml");
                    using (XmlReader reader = command.ExecuteXmlReader())
                    using (XmlWriter writer = XmlWriter.Create(filePath, settings))
                    {
                        writer.WriteNode(reader, true);
                        writer.Flush();
                    }
                    using (ZipFile zip = new ZipFile(this.zipName))
                    {
                        zip.AddFile(filePath, "");
                        zip.Save();
                    }
                    File.Delete(filePath);
                }
                int percents = (i+1) * 100 / this.checkedTables.Count;
                this.backgroundWorker.ReportProgress(percents);
            }
        }

        public void ExportProcessRun()
        {
            progressWindow.Canceled += () => this.backgroundWorker.CancelAsync();
            this.backgroundWorker.RunWorkerAsync();
            this.progressWindow.ShowDialog();
        }
    }
}
