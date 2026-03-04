namespace scheduler_desktop_app
{
    partial class ReportsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.btnTypesByMonth = new System.Windows.Forms.Button();
            this.btnScheduleByUser = new System.Windows.Forms.Button();
            this.btnCustomerReport = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReports
            // 
            this.dgvReports.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Location = new System.Drawing.Point(260, 80);
            this.dgvReports.MultiSelect = false;
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.ReadOnly = true;
            this.dgvReports.RowHeadersWidth = 51;
            this.dgvReports.RowTemplate.Height = 24;
            this.dgvReports.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReports.Size = new System.Drawing.Size(650, 381);
            this.dgvReports.TabIndex = 0;
            // 
            // btnTypesByMonth
            // 
            this.btnTypesByMonth.Location = new System.Drawing.Point(119, 106);
            this.btnTypesByMonth.Name = "btnTypesByMonth";
            this.btnTypesByMonth.Size = new System.Drawing.Size(118, 38);
            this.btnTypesByMonth.TabIndex = 1;
            this.btnTypesByMonth.Text = "Types By Month";
            this.btnTypesByMonth.UseVisualStyleBackColor = true;
            this.btnTypesByMonth.Click += new System.EventHandler(this.btnTypesByMonth_Click);
            // 
            // btnScheduleByUser
            // 
            this.btnScheduleByUser.Location = new System.Drawing.Point(119, 169);
            this.btnScheduleByUser.Name = "btnScheduleByUser";
            this.btnScheduleByUser.Size = new System.Drawing.Size(118, 41);
            this.btnScheduleByUser.TabIndex = 2;
            this.btnScheduleByUser.Text = "Schedule By User";
            this.btnScheduleByUser.UseVisualStyleBackColor = true;
            this.btnScheduleByUser.Click += new System.EventHandler(this.btnScheduleByUser_Click);
            // 
            // btnCustomerReport
            // 
            this.btnCustomerReport.Location = new System.Drawing.Point(119, 235);
            this.btnCustomerReport.Name = "btnCustomerReport";
            this.btnCustomerReport.Size = new System.Drawing.Size(118, 57);
            this.btnCustomerReport.TabIndex = 3;
            this.btnCustomerReport.Text = "Customer Appointment Counts";
            this.btnCustomerReport.UseVisualStyleBackColor = true;
            this.btnCustomerReport.Click += new System.EventHandler(this.btnCustomerReport_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(267, 47);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(55, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Reports";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(267, 485);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 16);
            this.lblError.TabIndex = 5;
            this.lblError.Visible = false;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 600);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCustomerReport);
            this.Controls.Add(this.btnScheduleByUser);
            this.Controls.Add(this.btnTypesByMonth);
            this.Controls.Add(this.dgvReports);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.Button btnTypesByMonth;
        private System.Windows.Forms.Button btnScheduleByUser;
        private System.Windows.Forms.Button btnCustomerReport;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblError;
    }
}