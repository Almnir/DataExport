using NLog;
using System;
using System.Windows.Forms;

namespace DataExport
{
    public partial class SettingsForm : Form
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public SettingsForm()
        {
            InitializeComponent();
            Load += SettingsWindowLoad;
        }

        private void SettingsWindowLoad(object sender, EventArgs e)
        {
            serverText.Text = Globals.frmSettings.ServerText == null ? "" : Globals.frmSettings.ServerText;
            databaseText.Text = Globals.frmSettings.DatabaseText == null ? "" : Globals.frmSettings.DatabaseText;
            loginText.Text = Globals.frmSettings.LoginText == null ? "" : Globals.frmSettings.LoginText;
            passwordText.Text = Globals.frmSettings.PasswordText == null ? "" : Globals.frmSettings.PasswordText;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (SaveSettings())
            {
                this.Close();
            }
        }

        private bool SaveSettings()
        {
            Globals.frmSettings.ServerText = serverText.Text;
            Globals.frmSettings.DatabaseText = databaseText.Text;
            Globals.frmSettings.LoginText = loginText.Text;
            Globals.frmSettings.PasswordText = passwordText.Text;
            Globals.frmSettings.Save();
            return true;
        }

        private void checkConnectionButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            if (DatabaseHelper.CheckConnection())
            {
                MessageBox.Show("Соединение успешно!", "Проверено");
            }
            else
            {
                MessageBox.Show("Нет соединения!", "Внимание!");
            }
        }
    }
}
