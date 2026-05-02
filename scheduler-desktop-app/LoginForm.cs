using scheduler_desktop_app.Services;
using scheduler_desktop_app.Data;
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
using System.Runtime.CompilerServices;


namespace scheduler_desktop_app
{
    public partial class LoginForm : Form
    {
        private readonly System.Windows.Forms.Timer _loginSuccessTimer = new System.Windows.Forms.Timer();

        public LoginForm()
        {
            InitializeComponent();

            _loginSuccessTimer.Interval = 2000;
            _loginSuccessTimer.Tick += LoginSuccessTimer_Tick;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbLanguage.SelectedIndex = 0;
            ApplyStrings();

            lblLocation.Text = LocationService.GetUserLocationSummary();

            lblError.Text = "";
            lblError.Visible = false;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.ForeColor = System.Drawing.Color.Red;
            lblError.Visible = true;
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            var culture = (cmbLanguage.SelectedItem?.ToString() == "Español")
                ? new CultureInfo("es")
                : new CultureInfo("en");

            Thread.CurrentThread.CurrentUICulture = culture;
            ApplyStrings();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                lblError.Visible = false;

                string u = (txtUsername.Text ?? "").Trim();
                string p = (txtPassword.Text ?? "").Trim();

                int? userId = AppState.UserRepo.GetUserIdByCredentials(u, p);

                if (userId != null)
                {
                    AppState.CurrentUserId = userId.Value;

                    lblError.Text = Localization.Strings.Login_Success;
                    lblError.Visible = true;
                    lblError.ForeColor = System.Drawing.Color.Green;
                    lblError.Refresh();

                    var next = AppointmentAlertService.GetNextAppointmentWithinMinutes(AppState.CurrentUserId, 15);
                    var msg = AppointmentAlertService.BuildAlertMessage(next);
                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        MessageBox.Show(msg, "Appointment Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoginHistoryService.Append(u);

                    btnLogin.Enabled = false;
                    txtUsername.Enabled = false;
                    txtPassword.Enabled = false;
                    cmbLanguage.Enabled = false;

                    _loginSuccessTimer.Start();
                    return;
                }
                else
                {
                    ShowError(Localization.Strings.Error_InvalidCredentials);

                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
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

        private void LoginSuccessTimer_Tick(object sender, EventArgs e)
        {
            _loginSuccessTimer.Stop();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
