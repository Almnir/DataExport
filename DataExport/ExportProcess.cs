using Ionic.Zip;
using NLog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DataExport
{
    class ExportProcess
    {

        private static Logger log = LogManager.GetCurrentClassLogger();

        private BackgroundWorker backgroundWorker;
        private ProgressWindow progressWindow;
        private ProgressBar progressBar;
        private Label progressLabel;
        private List<string> checkedTables;
        private string errorString;
        private string zipName;

        /// <summary>
        /// Состояние прогресса
        /// </summary>
        class ProgressState
        {
            public int Value { get; set; }
            public string Content { get; set; }
        }

        public ExportProcess(ProgressWindow progressWindow, List<string> checkedTables, string zipName)
        {
            this.progressWindow = progressWindow;
            this.checkedTables = checkedTables;
            this.errorString = string.Empty;
            this.zipName = zipName;
            this.progressBar = progressWindow.GetProgressBar();
            this.progressLabel = progressWindow.GetProgressLabel();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!this.progressWindow.IsDisposed)
            {
                ProgressState state = e.UserState as ProgressState;
                this.progressBar.Value = state.Value;
                this.progressLabel.Text = state.Content;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.progressBar.Value = 100;
            this.progressWindow.Close();
            if (e.Cancelled)
            {
                MessageBox.Show("Выгрузка отменена!");
                return;
            }
            if (e.Error == null)
            {
                MessageBox.Show("Выгрузка завершена!");
                return;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
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
                    int percents = (i + 1) * 100 / this.checkedTables.Count;
                    this.backgroundWorker.ReportProgress(percents, new ProgressState { Value = percents, Content = tableName });
                }
            }
            catch (System.Exception ex)
            {
                log.Error(ex.ToString());
                MessageBox.Show(ex.ToString(), "Ошибка!");
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
