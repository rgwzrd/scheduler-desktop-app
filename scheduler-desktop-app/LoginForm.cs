using scheduler_desktop_app.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;


namespace scheduler_desktop_app
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbLanguage.SelectedIndex = 0;

            lblLocation.Text = LocationService.GetUserLocationSummary();

            lblError.Text = "";
            lblError.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = (cmbLanguage.SelectedItem?.ToString() == "Español")
                ? new CultureInfo("es")
                : new CultureInfo("en");

            Thread.CurrentThread.CurrentUICulture = culture;
            ApplyStrings();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            string u = (txtUsername.Text ?? "").Trim();
            string p = (txtPassword.Text ?? "").Trim();

            if (u == "test" && p == "test")
            {
                // success condition
            }
            else
            {
                lblError.Text = Localization.Strings.Error_InvalidCredentials;
                lblError.Visible = true;
                lblError.MaximumSize = new Size(250, 0);
                lblError.AutoSize = true;
            }
        }

        private void ApplyStrings()
        {
            Text = Localization.Strings.Login_Title;
            lblUsername.Text = Localization.Strings.Username_Label;
            lblPassword.Text = Localization.Strings.Password_Label;
            btnLogin.Text = Localization.Strings.Login_Button;
            lblLanguage.Text = Localization.Strings.Language_Label;
            lblLocationTitle.Text = Localization.Strings.Location_Label;
        }
    }
}
