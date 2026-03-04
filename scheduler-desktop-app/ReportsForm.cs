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

namespace scheduler_desktop_app
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void btnTypesByMonth_Click(object sender, EventArgs e)
        {
            var rows = ReportService.AppointmentTypeCountsByMonth();
            dgvReports.DataSource = null;
            dgvReports.DataSource = rows;
        }

        private void btnScheduleByUser_Click(object sender, EventArgs e)
        {
            var rows = ReportService.ScheduleForEachUser();
            dgvReports.DataSource = null;
            dgvReports.DataSource = rows;
        }

        private void btnCustomerReport_Click(object sender, EventArgs e)
        {
            var rows = ReportService.AppointmentCountsByCustomer();
            dgvReports.DataSource = null;
            dgvReports.DataSource = rows;
        }
    }
}
