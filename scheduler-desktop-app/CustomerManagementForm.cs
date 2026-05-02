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
        private readonly ICustomerRepository _repo = AppState.CustomerRepo;

        public CustomerManagementForm()
        {
            InitializeComponent();
        }

        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            ClearError();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            ClearError();

            dgvCustomers.DataSource = null;
            dgvCustomers.DataSource = _repo.GetAll();
        }

        private Customer GetSelectedCustomer()
        {
            return dgvCustomers.CurrentRow?.DataBoundItem as Customer;
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

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            ClearError();

            using (var form = new CustomerEditForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    _repo.Add(form.CustomerResult);
                    LoadCustomers();
                }
                catch (CustomerOperationException ex)
                {
                    ShowError(ex.Message);
                }
                catch (Exception)
                {
                    ShowError("Unexpected error while adding customer.");
                }
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            ClearError();

            Customer selected = GetSelectedCustomer();

            if (selected == null)
            {
                ShowError("Select a customer to edit.");
                return;
            }

            using (var form = new CustomerEditForm(selected))
            {
                if (form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    _repo.Update(form.CustomerResult);
                    LoadCustomers();
                }
                catch (CustomerOperationException ex)
                {
                    ShowError(ex.Message);
                }
                catch (Exception)
                {
                    ShowError("Unexpected error while updating customer.");
                }
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            ClearError();

            Customer selected = GetSelectedCustomer();

            if (selected == null)
            {
                ShowError("Select a customer to delete.");
                return;
            }

            try
            {
                int appointmentCount =
                    AppState.AppointmentRepo.CountByCustomer(selected.CustomerId);

                if (appointmentCount > 0)
                {
                    ShowError(
                        $"Cannot delete this customer because they have {appointmentCount} appointment(s). Delete the appointments first.");

                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    "Delete this customer?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm != DialogResult.Yes)
                {
                    return;
                }

                _repo.Delete(selected.CustomerId);
                LoadCustomers();
            }
            catch (CustomerOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (AppointmentOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception)
            {
                ShowError("Unexpected error while deleting customer.");
            }
        }
    }
}