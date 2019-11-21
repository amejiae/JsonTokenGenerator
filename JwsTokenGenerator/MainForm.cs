using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JwsTokenGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ExpirationPicker.Value = DateTime.UtcNow.AddHours(3);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void GenerateKeyButton_Click(object sender, EventArgs e)
        {
            var asymetricKey = new AsymmetricKeyPair(PrivateExponentTextBox.Text, 
                ExponentOneTextBox.Text,
                ExponentTwoTextBox.Text,
                PublicExponentTextBox.Text,
                CoeficientTextBox.Text,
                ModulusTextBox.Text,
                PrimeOneTextBox.Text,
                PrimeTwoTextBox.Text);

            var jsonToken = new JsonWebToken(EmailTextBox.Text, ExpirationPicker.Value, asymetricKey);
            var token = jsonToken.Serialize();

            var result = new ResultForm(token);
            result.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var jsonImport = new GetFromJsonForm();
            var result = jsonImport.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    var key = AsymmetricKeyPair.FromJson(JObject.Parse(jsonImport.Json));
                    SetKeyElementsFromJson(key);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"There was a problem converting the given json to private key elements: {exception.Message}");
                }
            }
        }

        private void SetKeyElementsFromJson(AsymmetricKeyPair key)
        {
            PrivateExponentTextBox.Text = key.PrivateExponent;
            ExponentOneTextBox.Text = key.ExponentOne;
            ExponentTwoTextBox.Text = key.ExponentTwo;
            PublicExponentTextBox.Text = key.PublicExponent;
            CoeficientTextBox.Text = key.Coefficient;
            ModulusTextBox.Text = key.Modulus;
            PrimeOneTextBox.Text = key.PrimeOne;
            PrimeTwoTextBox.Text = key.PrimeTwo;
        }
    }
}

