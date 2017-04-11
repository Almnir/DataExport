using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
                        LoadTablesList();
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
                    LoadTablesList();
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

        private void LoadTablesList()
        {
            List<string> tables = DatabaseHelper.GetTables(Globals.GetConnectionString());
            resultList = new List<string>();
            foreach (var table in tables)
            {
                if (Globals.TABLES_NAMES.Contains(table) && !resultList.Contains(table))
                {
                    resultList.Add(table);
                }
            }
            foreach (var table in resultList)
            {
                if (Globals.TABLES_NAMES.Contains(table))
                {
                    ListViewItem lvi = new ListViewItem(table);
                    tablesList.Items.Add(lvi);
                }
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
            if (string.IsNullOrEmpty(Globals.frmSettings.ServerText) || string.IsNullOrEmpty(Globals.frmSettings.DatabaseText)
                || string.IsNullOrEmpty(Globals.frmSettings.LoginText) || string.IsNullOrEmpty(Globals.frmSettings.PasswordText))
            {
                MessageBox.Show("Подключение не задано!", "Внимание!");
                return;
            }
            if (this.tablesList.CheckedItems.Count == 0)
            {
                MessageBox.Show("Ни одной таблицы не выбрано!", "Внимание!");
                return;
            }
            List<string> checkedTables = new List<string>();
            foreach (ListViewItem ct in this.tablesList.CheckedItems)
            {
                checkedTables.Add(ct.Text);
            }
            long estimateSize = 0;
            List<string> nonemptyTables = DatabaseHelper.ExcludeEmptyTablesAndGetSize(checkedTables, out estimateSize);
            if (nonemptyTables.Count == 0)
            {
                MessageBox.Show("Таблицы пусты!", "Внимание!");
                return;
            }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".zip";
            DialogResult userClicked = dlg.ShowDialog();
            if (userClicked == DialogResult.OK)
            {
                if (DataExportHelper.GetTotalFreeSpace(Path.GetDirectoryName(Path.GetFullPath(dlg.FileName))) <= estimateSize)
                {
                    MessageBox.Show(string.Format("Мало свободного места для сохранения архива!\n Требуется примерно {0} байт.", estimateSize), "Внимание!");
                    return;
                }
                ProgressWindow pw = new ProgressWindow();
                ExportProcess eprocess = new ExportProcess(pw, nonemptyTables, dlg.FileName);
                eprocess.ExportProcessRun();
            }
        }
    }
}
