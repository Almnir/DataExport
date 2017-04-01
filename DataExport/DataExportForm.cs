using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DataExport
{
    public partial class DataExportForm : Form
    {

        List<string> resultList;

        public DataExportForm()
        {
            InitializeComponent();
            Load += new EventHandler(LoadForm);
        }

        private void LoadForm(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Globals.frmSettings.ServerText) || string.IsNullOrEmpty(Globals.frmSettings.DatabaseText)
                || string.IsNullOrEmpty(Globals.frmSettings.LoginText) || string.IsNullOrEmpty(Globals.frmSettings.PasswordText))
            {
                SettingsForm sf = new SettingsForm();
                DialogResult dr = sf.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (DatabaseHelper.CheckConnection())
                    {
                        PopulateTablesList();
                    }
                    else
                    {
                        MessageBox.Show("Нет соединения!", "Внимание!");
                    }
                }
            }
            else
            {
                if (DatabaseHelper.CheckConnection())
                {
                    PopulateTablesList();
                }
                else
                {
                    MessageBox.Show("Нет соединения!", "Внимание!");
                }
            }
            this.Focus();
        }

        private void PopulateTablesList()
        {
            List<string> tables = DatabaseHelper.GetTables(Globals.GetConnectionString());
            List<string> patternsAll = new List<string>()
            {
                { "ac_" },
                { "dats_" },
                { "prnf_" },
                { "rbd_" },
                { "res_" },
                { "sht_" }
            };
            resultList = tables.FindAll(delegate (string s) 
            {
                bool flag = false;
                foreach (var pattern in patternsAll)
                {
                    flag |= s.StartsWith(pattern);
                    if (flag == true) break;
                }
                return flag;
            });
            foreach (var table in resultList)
            {
                ListViewItem lvi = new ListViewItem(table);
                tablesList.Items.Add(lvi);
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }

        private void allLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in tablesList.Items)
            {
                item.Checked = true;
            }
        }

        private void clearAllLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in tablesList.Items)
            {
                item.Checked = false;
            }
        }

        private void properLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (ListViewItem item in tablesList.Items)
            {
                foreach (var tableName in Globals.TABLES_NAMES)
                {
                    if (item.Text.Equals(tableName))
                    {
                        item.Checked = true;
                        break;
                    }
                }
            }
        }

        private void startExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".zip";
            DialogResult userClicked = dlg.ShowDialog();
            if (userClicked == DialogResult.OK)
            {
                foreach (ListViewItem item in this.tablesList.CheckedItems)
                {
                    using (SqlConnection connection = new SqlConnection(Globals.GetConnectionString()))
                    {
                        connection.Open();

                        string query = string.Format(@";WITH XMLNAMESPACES ('http://rustest.ru/giadbset' AS ns1)
                                            SELECT *
                                            FROM {0}
                                            FOR XML PATH('{0}'), ROOT('GIADBSet'); ", item.Text);
                        SqlCommand command = new SqlCommand(query, connection);
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.Encoding = Encoding.UTF8;
                        settings.CloseOutput = true;
                        using (XmlReader reader = command.ExecuteXmlReader())
                        using (XmlWriter writer = XmlWriter.Create(item.Text + ".xml", settings))
                        {
                            writer.WriteNode(reader, true);
                        }
                        using (ZipFile zip = new ZipFile(dlg.FileName + ".zip"))
                        {
                            zip.AddFile(item.Text + ".xml");
                            zip.Save();
                        }
                    }
                }
                MessageBox.Show("Успешно!");
            }
        }
    }
}
