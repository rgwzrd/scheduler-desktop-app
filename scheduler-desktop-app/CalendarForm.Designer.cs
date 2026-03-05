namespace scheduler_desktop_app
{
    partial class CalendarForm
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
            this.monthCalendar = new System.Windows.Forms.MonthCalendar();
            this.dgvDayAppointments = new System.Windows.Forms.DataGridView();
            this.lblSelectedDay = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).BeginInit();
            this.SuspendLayout();
            // 
            // monthCalendar
            // 
            this.monthCalendar.Location = new System.Drawing.Point(183, 29);
            this.monthCalendar.Name = "monthCalendar";
            this.monthCalendar.TabIndex = 0;
            this.monthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar_DateSelected);
            // 
            // dgvDayAppointments
            // 
            this.dgvDayAppointments.AllowUserToAddRows = false;
            this.dgvDayAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDayAppointments.Location = new System.Drawing.Point(32, 316);
            this.dgvDayAppointments.Name = "dgvDayAppointments";
            this.dgvDayAppointments.RowHeadersWidth = 51;
            this.dgvDayAppointments.RowTemplate.Height = 46;
            this.dgvDayAppointments.Size = new System.Drawing.Size(556, 251);
            this.dgvDayAppointments.TabIndex = 1;
            // 
            // lblSelectedDay
            // 
            this.lblSelectedDay.AutoSize = true;
            this.lblSelectedDay.Location = new System.Drawing.Point(110, 269);
            this.lblSelectedDay.Name = "lblSelectedDay";
            this.lblSelectedDay.Size = new System.Drawing.Size(90, 16);
            this.lblSelectedDay.TabIndex = 2;
            this.lblSelectedDay.Text = "Selected day:";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(130, 253);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 16);
            this.lblError.TabIndex = 3;
            this.lblError.Visible = false;
            // 
            // CalendarForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(627, 601);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblSelectedDay);
            this.Controls.Add(this.dgvDayAppointments);
            this.Controls.Add(this.monthCalendar);
            this.Name = "CalendarForm";
            this.Text = "Calendar Form";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayAppointments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar monthCalendar;
        private System.Windows.Forms.DataGridView dgvDayAppointments;
        private System.Windows.Forms.Label lblSelectedDay;
        private System.Windows.Forms.Label lblError;
    }
}