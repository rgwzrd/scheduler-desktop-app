using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
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
    public partial class CustomerManagementForm : Form
    {
        private readonly ICustomerRepository _repo = new InMemoryCustomerRepository();
        public CustomerManagementForm()
        {
            InitializeComponent();
        }

        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            lblError.Text = "";
            lblError.Visible = false;

            var customers = _repo.GetAll();
            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = customers;
        }

        private Customer GetSelectedCustomer()
        {
            if (dgvCustomers.CurrentRow == null)
                return null;

            return dgvCustomers.CurrentRow.DataBoundItem as Customer;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            using (var form = new CustomerEditForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repo.Add(form.CustomerResult);
                    LoadCustomers();
                }
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                lblError.Text = "Select a customer to edit.";
                lblError.ForeColor = Color.Red;
                lblError.Visible = true;
                return;
            }

            using (var form = new CustomerEditForm(selected))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _repo.Update(form.CustomerResult);
                    LoadCustomers();
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            lblError.Visible = false;

            var selected = GetSelectedCustomer();
            if (selected == null)
            {
                lblError.Text = "Select a customer to delete.";
                lblError.ForeColor = Color.Red;
                lblError.Visible = true;
                return;
            }

            _repo.Delete(selected.CustomerId);
            LoadCustomers();
        }

    }
}
