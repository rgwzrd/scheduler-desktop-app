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
            lblError.Text = "";
            lblError.Visible = false;

            dgvAppointments.ReadOnly = true;
            dgvAppointments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAppointments.MultiSelect = false;

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            var appts = AppState.AppointmentRepo.GetByUser(AppState.CurrentUserId);

            dgvAppointments.DataSource = null;
            dgvAppointments.DataSource = appts;

            if (dgvAppointments.Columns["StartUtc"] != null)
                dgvAppointments.Columns["StartUtc"].Visible = false;

            if (dgvAppointments.Columns["EndUtc"] != null)
                dgvAppointments.Columns["EndUtc"].Visible = false;

            if (dgvAppointments.Columns["StartLocal"] != null)
                dgvAppointments.Columns["StartLocal"].HeaderText = "Start";

            if (dgvAppointments.Columns["EndLocal"] != null)
                dgvAppointments.Columns["EndLocal"].HeaderText = "End";
        }

        private Appointment GetSelected()
        {
            return dgvAppointments.CurrentRow?.DataBoundItem as Appointment;
        }
        private void ShowErrorText()
        {
            lblError.ForeColor = Color.Red;
            lblError.Visible = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            var customers = AppState.CustomerRepo.GetAll().ToArray();
            if (customers.Length == 0)
            {
                lblError.Text = "Add a customer first.";
                ShowErrorText();
                return;
            }

            using (var f = new AppointmentEditForm(AppState.AppointmentRepo, customers, AppState.CurrentUserId))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        AppState.AppointmentRepo.Add(f.Result);
                        LoadAppointments();
                    }
                    catch (AppointmentOperationException ex)
                    {
                        lblError.Text = ex.Message;
                        ShowErrorText();
                    }
                    catch (Exception)
                    {
                        lblError.Text = "Unexpected error while adding appointment.";
                        ShowErrorText();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            var selected = GetSelected();
            if (selected == null)
            {
                lblError.Text = "Select an appointment to edit.";
                ShowErrorText();
                return;
            }

            var customers = AppState.CustomerRepo.GetAll().ToArray();

            using (var f = new AppointmentEditForm(AppState.AppointmentRepo, customers, AppState.CurrentUserId, selected))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        AppState.AppointmentRepo.Update(f.Result);
                        LoadAppointments();
                    }
                    catch (AppointmentOperationException ex)
                    {
                        lblError.Text = ex.Message;
                        ShowErrorText();
                    }
                    catch (Exception)
                    {
                        lblError.Text = "Unexpected error while updating appointment.";
                        ShowErrorText();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;

            var selected = GetSelected();
            if (selected == null)
            {
                lblError.Text = "Select an appointment to delete.";
                ShowErrorText();
                return;
            }

            try
            {
                AppState.AppointmentRepo.Delete(selected.AppointmentId);
                LoadAppointments();
            }
            catch (AppointmentOperationException ex)
            {
                lblError.Text = ex.Message;
                ShowErrorText();
            }
            catch (Exception)
            {
                lblError.Text = "Unexpected error while deleting appointment.";
                ShowErrorText();
            }
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            using (var f = new CalendarForm())
            {
                f.ShowDialog();
            }
        }
    }
}
