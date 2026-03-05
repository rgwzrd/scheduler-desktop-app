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
        private readonly int _customerId;

        public Customer CustomerResult { get; private set; }

        public CustomerEditForm()
        {
            InitializeComponent();
            _customerId = 0;
            Text = "Add Customer";
        }

        private void ShowErrorText()
        {
            lblError.ForeColor = Color.Red;
            lblError.Visible = true;
        }
        public CustomerEditForm(Customer existing)
        {
            InitializeComponent();
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


            if (errors.Any())
            {
                lblError.Text = string.Join(Environment.NewLine, errors);
                ShowErrorText();
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
    }
}
