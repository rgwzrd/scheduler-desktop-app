using scheduler_desktop_app;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scheduler_desktop_app
{
    public partial class MainForm : Form
    {
        private CustomerManagementForm _customers;
        private AppointmentManagementForm _appointments;
        private CalendarForm _calendar;
        private ReportsForm _reports;

        public MainForm()
        {
            InitializeComponent();

            IsMdiContainer = true;
            WindowState = FormWindowState.Maximized;

            var menu = new MenuStrip();

            var customersItem = new ToolStripMenuItem("Customers");
            customersItem.Click += (s, e) => OpenCustomers();

            var appointmentsItem = new ToolStripMenuItem("Appointments");
            appointmentsItem.Click += (s, e) => OpenAppointments();

            var calendarItem = new ToolStripMenuItem("Calendar");
            calendarItem.Click += (s, e) => OpenCalendar();

            var reportsItem = new ToolStripMenuItem("Reports");
            reportsItem.Click += (s, e) => OpenReports();

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) => Close();

            menu.Items.Add(customersItem);
            menu.Items.Add(appointmentsItem);
            menu.Items.Add(calendarItem);
            menu.Items.Add(reportsItem);
            menu.Items.Add(exitItem);

            menu.Dock = DockStyle.Top;
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private void OpenCustomers()
        {
            if (_customers == null || _customers.IsDisposed)
            {
                _customers = new CustomerManagementForm();
                _customers.MdiParent = this;
                _customers.Show();
                return;
            }

            _customers.Activate();
        }

        private void OpenAppointments()
        {
            if (_appointments == null || _appointments.IsDisposed)
            {
                _appointments = new AppointmentManagementForm();
                _appointments.MdiParent = this;
                _appointments.Show();
                return;
            }

            _appointments.Activate();
        }

        private void OpenCalendar()
        {
            if (_calendar == null || _calendar.IsDisposed)
            {
                _calendar = new CalendarForm();
                _calendar.MdiParent = this;
                _calendar.Show();
                return;
            }

            _calendar.Activate();
        }

        private void OpenReports()
        {
            if (_reports == null || _reports.IsDisposed)
            {
                _reports = new ReportsForm();
                _reports.MdiParent = this;
                _reports.Show();
                return;
            }

            _reports.Activate();
        }
    }
}
