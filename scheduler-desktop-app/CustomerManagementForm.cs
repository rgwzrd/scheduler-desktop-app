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
using scheduler_desktop_app.Exceptions;

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
                    try
                    {
                        _repo.Add(form.CustomerResult);
                        LoadCustomers();
                    }

                    catch (CustomerOperationException ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Unexpected error while adding customer.";
                        lblError.Visible = true;
                    }
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
                    try
                    {
                        _repo.Update(form.CustomerResult);
                        LoadCustomers();
                    }
                    catch (CustomerOperationException ex)
                    {
                        lblError.Text = ex.Message;
                        lblError.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "Unexpected error while updating customer.";
                        lblError.Visible = true;
                    }

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
            try
            {
                _repo.Delete(selected.CustomerId);
                LoadCustomers();
            }

            catch (CustomerOperationException ex)
            {
                lblError.Text = ex.Message;
                lblError.ForeColor = Color.Red;
                lblError.Visible = true;
            }

            catch (Exception ex)
            {
                lblError.Text = "Unexpected error while deleting customer.";
                lblError.Visible = true;
            }
        }

    }
}
