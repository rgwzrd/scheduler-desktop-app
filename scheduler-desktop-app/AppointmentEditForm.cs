using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
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
    public partial class AppointmentEditForm : Form
    {
        private readonly IAppointmentRepository _apptRepo;
        private readonly int _userId;
        private readonly bool _isEdit;
        private readonly int _apptId;

        public Appointment Result { get; private set; }

        public AppointmentEditForm(IAppointmentRepository apptRepo, Customer[] customers, int userId)
        {
            InitializeComponent();
            _apptRepo = apptRepo;
            _userId = userId;
            _isEdit = false;
            _apptId = 0;

            BindCustomers(customers);
            InitPickers();
        }

        public AppointmentEditForm(IAppointmentRepository apptRepo, Customer[] customers, int userId, Appointment existing)
        {
            InitializeComponent();
            _apptRepo = apptRepo;
            _userId = userId;
            _isEdit = true;
            _apptId = existing.AppointmentId;

            BindCustomers(customers);
            InitPickers();

            txtType.Text = existing.Type;

            dtpStart.Value = TimeService.UtcToLocal(existing.StartUtc);
            dtpEnd.Value = TimeService.UtcToLocal(existing.EndUtc);

            cmbCustomer.SelectedValue = existing.CustomerId;
        }

        private void InitPickers()
        {
            dtpStart.Format = DateTimePickerFormat.Custom;
            dtpStart.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpStart.ShowUpDown = true;

            dtpEnd.Format = DateTimePickerFormat.Custom;
            dtpEnd.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpEnd.ShowUpDown = true;

            lblError.Text = "";
            lblError.Visible = false;
        }

        private void BindCustomers(Customer[] customers)
        {
            cmbCustomer.DisplayMember = "CustomerName";
            cmbCustomer.ValueMember = "CustomerId";
            cmbCustomer.DataSource = customers.ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            if (cmbCustomer.SelectedItem == null)
            {
                lblError.Text = "Customer selection is required.";
                lblError.Visible = true;
                return;
            }

            var selectedCustomer = (Customer)cmbCustomer.SelectedItem;

            var startLocal = dtpStart.Value;
            var endLocal = dtpEnd.Value;

            var startUtc = TimeService.LocalToUtc(startLocal);
            var endUtc = TimeService.LocalToUtc(endLocal);

            string type = txtType.Text ?? "";

            bool overlaps = _apptRepo.Overlaps(_userId, startUtc, endUtc, _isEdit ? _apptId : 0);

            var errors = AppointmentValidationService.Validate(
                startUtc,
                endUtc,
                type,
                () => overlaps
            );

            if (errors.Any())
            {
                lblError.Text = string.Join(Environment.NewLine, errors);
                lblError.Visible = true;
                return;
            }

            Result = new Appointment
            {
                AppointmentId = _apptId,
                UserId = _userId,
                CustomerId = selectedCustomer.CustomerId,
                CustomerName = selectedCustomer.CustomerName,
                Type = type.Trim(),
                StartUtc = startUtc,
                EndUtc = endUtc
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
