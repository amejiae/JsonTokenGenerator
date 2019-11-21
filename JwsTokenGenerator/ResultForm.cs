using System.Windows.Forms;

namespace JwsTokenGenerator
{
    public partial class ResultForm : Form
    {
        public ResultForm(string token)
        {
            InitializeComponent();
            TokenTextBox.Text = token;
        }
    }
}
