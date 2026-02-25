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
    public partial class CustomerEditForm : Form
    {
        private readonly bool _isEditMode;
        private readonly int _customerId;

        public Customer CustomerResult { get; private set; }

        public CustomerEditForm()
        {
            InitializeComponent();
            _isEditMode = false;
            _customerId = 0;
            Text = "Add Customer";
        }

        public CustomerEditForm(Customer existing)
        {
            InitializeComponent();
            _isEditMode = true;
            _customerId = existing.CustomerId;

            Text = "Edit Customer";

            txtName.Text = existing.CustomerName;
            txtAddress.Text = existing.Address;
            txtPhone.Text = existing.Phone;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            var customer = new Customer
            {
                CustomerId = _customerId,
                CustomerName = txtName.Text,
                Address = txtAddress.Text,
                Phone = txtPhone.Text,
                Active = true
            };

            var errors = ValidationService.ValidateCustomer(customer);

            lblError.Text = "Errors found: " + errors.Count;
            lblError.Visible = true;

            if (errors.Any())
            {
                lblError.Text = string.Join(Environment.NewLine, errors);
                lblError.Visible = true;
                return;
            }

            CustomerResult = customer;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }
    }
}
