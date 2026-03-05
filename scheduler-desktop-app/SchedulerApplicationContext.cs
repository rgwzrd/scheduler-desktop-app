using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace scheduler_desktop_app
{
    internal class SchedulerApplicationContext : ApplicationContext
    {
        private LoginForm _loginForm;
        private MainForm _mainForm;

        public SchedulerApplicationContext()
        {
            ShowLogin();
        }

        private void ShowLogin()
        {
            _loginForm = new LoginForm();
            _loginForm.FormClosed += LoginClosed;
            _loginForm.Show();
        }

        private void LoginClosed(object sender, FormClosedEventArgs e)
        {
            if (_loginForm.DialogResult == DialogResult.OK)
            {
                _mainForm = new MainForm();
                _mainForm.FormClosed += (s, args) => ExitThread();
                _mainForm.Show();
            }
            else
            {
                ExitThread();
            }
        }
    }
}
