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

namespace scheduler_desktop_app
{
    public partial class CalendarForm : Form
    {

        public CalendarForm()
        {
            InitializeComponent();
        }
        

        private void CalendarForm_Load(object sender, EventArgs e)
        {
            lblError.Visible = false;

            dgvDayAppointments.ReadOnly = true;
            dgvDayAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDayAppointments.MultiSelect = false;

            LoadAppointmentsForLocalDay(DateTime.Today);
        }

        private void monthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            lblError.Visible = false;

            LoadAppointmentsForLocalDay(e.Start.Date);
        }

        private void LoadAppointmentsForLocalDay(DateTime localDay)
        {
            lblSelectedDay.Text = $"Selected day: {localDay:yyyy-MM-dd}";

            var all = AppState.AppointmentRepo.GetByUser(AppState.CurrentUserId);

            var dayAppts = all
                .Where(a => a.StartLocal.Date == localDay.Date)
                .OrderBy(a => a.StartLocal)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.CustomerName,
                    a.Type,
                    Start = a.StartLocal,
                    End = a.EndLocal
                })
                .ToList();

            dgvDayAppointments.DataSource = null;
            dgvDayAppointments.DataSource = dayAppts;

        }
       
        }
    }

