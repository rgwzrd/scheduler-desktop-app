using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using scheduler_desktop_app.Exceptions;

namespace scheduler_desktop_app
{
    public partial class AppointmentManagementForm : Form
    {
        public AppointmentManagementForm()
        {
            InitializeComponent();
        }

        private void AppointmentManagementForm_Load(object sender, EventArgs e)
        {
            ClearError();

            dgvAppointments.ReadOnly = true;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.MultiSelect = false;

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            dgvAppointments.DataSource = null;
            dgvAppointments.DataSource =
                AppState.AppointmentRepo.GetByUser(AppState.CurrentUserId);

            if (dgvAppointments.Columns["StartUtc"] != null)
            {
                dgvAppointments.Columns["StartUtc"].Visible = false;
            }

            if (dgvAppointments.Columns["EndUtc"] != null)
            {
                dgvAppointments.Columns["EndUtc"].Visible = false;
            }

            if (dgvAppointments.Columns["StartLocal"] != null)
            {
                dgvAppointments.Columns["StartLocal"].HeaderText = "Start";
            }

            if (dgvAppointments.Columns["EndLocal"] != null)
            {
                dgvAppointments.Columns["EndLocal"].HeaderText = "End";
            }
        }

        private Appointment GetSelected()
        {
            return dgvAppointments.CurrentRow?.DataBoundItem as Appointment;
        }

        private void ClearError()
        {
            lblError.Text = string.Empty;
            lblError.Visible = false;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.ForeColor = Color.Red;
            lblError.Visible = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearError();

            Customer[] customers = AppState.CustomerRepo.GetAll().ToArray();

            if (customers.Length == 0)
            {
                ShowError("Add a customer first.");
                return;
            }

            using (var form = new AppointmentEditForm(
                AppState.AppointmentRepo,
                customers,
                AppState.CurrentUserId))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    AppState.AppointmentRepo.Add(form.Result);
                    LoadAppointments();
                }
                catch (AppointmentOperationException ex)
                {
                    ShowError(ex.Message);
                }
                catch (Exception)
                {
                    ShowError("Unexpected error while adding appointment.");
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ClearError();

            Appointment selected = GetSelected();

            if (selected == null)
            {
                ShowError("Select an appointment to edit.");
                return;
            }

            Customer[] customers = AppState.CustomerRepo.GetAll().ToArray();

            using (var form = new AppointmentEditForm(
                AppState.AppointmentRepo,
                customers,
                AppState.CurrentUserId,
                selected))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    AppState.AppointmentRepo.Update(form.Result);
                    LoadAppointments();
                }
                catch (AppointmentOperationException ex)
                {
                    ShowError(ex.Message);
                }
                catch (Exception)
                {
                    ShowError("Unexpected error while updating appointment.");
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ClearError();

            Appointment selected = GetSelected();

            if (selected == null)
            {
                ShowError("Select an appointment to delete.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Delete this appointment?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                AppState.AppointmentRepo.Delete(selected.AppointmentId);
                LoadAppointments();
            }
            catch (AppointmentOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception)
            {
                ShowError("Unexpected error while deleting appointment.");
            }
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            using (var form = new CalendarForm())
            {
                form.ShowDialog();
            }
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            using (var form = new ReportsForm())
            {
                form.ShowDialog();
            }
        }
    }
}
