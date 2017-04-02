using System;
using System.Windows.Forms;

namespace DataExport
{
    public partial class ProgressWindow : Form
    {
        public event Action Canceled;

        public ProgressWindow()
        {
            InitializeComponent();
        }

        public ProgressBar GetProgressBar()
        {
            return progressBar;
        }

        public void SetMaximum(int max)
        {
            this.progressBar.Maximum = max;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            if (Canceled != null)
                Canceled();
        }
    }
}
