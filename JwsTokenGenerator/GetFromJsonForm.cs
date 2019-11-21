using System;
using System.Windows.Forms;

namespace JwsTokenGenerator
{
    public partial class GetFromJsonForm : Form
    {
        public GetFromJsonForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Json = jsonTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        public string Json { get; private set; }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
