namespace Hotel.UI.Formularios
{
    partial class FormCheckIn
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblHabitacion = new System.Windows.Forms.Label();
            this.cmbHabitacion = new System.Windows.Forms.ComboBox();
            this.lblAviso = new System.Windows.Forms.Label();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblInfo);
            this.pnlContenido.Controls.Add(this.lblHabitacion);
            this.pnlContenido.Controls.Add(this.cmbHabitacion);
            this.pnlContenido.Controls.Add(this.lblAviso);
            //
            // lblInfo
            //
            this.lblInfo.Location = new System.Drawing.Point(24, 4);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(500, 56);
            //
            // lblHabitacion
            //
            this.lblHabitacion.AutoSize = true;
            this.lblHabitacion.Location = new System.Drawing.Point(24, 74);
            this.lblHabitacion.Name = "lblHabitacion";
            this.lblHabitacion.Text = "Habitación a asignar (puede cambiarla)";
            //
            // cmbHabitacion
            //
            this.cmbHabitacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHabitacion.Location = new System.Drawing.Point(24, 98);
            this.cmbHabitacion.Name = "cmbHabitacion";
            this.cmbHabitacion.Size = new System.Drawing.Size(500, 28);
            this.cmbHabitacion.TabIndex = 0;
            //
            // lblAviso
            //
            this.lblAviso.ForeColor = System.Drawing.Color.FromArgb(226, 92, 92);
            this.lblAviso.Location = new System.Drawing.Point(24, 140);
            this.lblAviso.Name = "lblAviso";
            this.lblAviso.Size = new System.Drawing.Size(500, 48);
            this.lblAviso.Tag = "personalizada";
            //
            // FormCheckIn
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(560, 340);
            this.Name = "FormCheckIn";
            this.Text = "Check-in";
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblHabitacion;
        private System.Windows.Forms.ComboBox cmbHabitacion;
        private System.Windows.Forms.Label lblAviso;
    }
}
