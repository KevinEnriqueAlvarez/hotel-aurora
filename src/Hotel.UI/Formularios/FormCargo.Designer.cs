namespace Hotel.UI.Formularios
{
    partial class FormCargo
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
            this.lblServicio = new System.Windows.Forms.Label();
            this.cmbServicio = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.numCantidad = new System.Windows.Forms.NumericUpDown();
            this.lblTotal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblServicio);
            this.pnlContenido.Controls.Add(this.cmbServicio);
            this.pnlContenido.Controls.Add(this.lblCantidad);
            this.pnlContenido.Controls.Add(this.numCantidad);
            this.pnlContenido.Controls.Add(this.lblTotal);
            //
            // lblServicio
            //
            this.lblServicio.AutoSize = true;
            this.lblServicio.Location = new System.Drawing.Point(24, 4);
            this.lblServicio.Name = "lblServicio";
            this.lblServicio.Text = "Servicio";
            //
            // cmbServicio
            //
            this.cmbServicio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServicio.Location = new System.Drawing.Point(24, 28);
            this.cmbServicio.Name = "cmbServicio";
            this.cmbServicio.Size = new System.Drawing.Size(440, 28);
            this.cmbServicio.TabIndex = 0;
            this.cmbServicio.SelectedIndexChanged += new System.EventHandler(this.Campo_Cambiado);
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(24, 74);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Text = "Cantidad";
            //
            // numCantidad
            //
            this.numCantidad.Location = new System.Drawing.Point(24, 98);
            this.numCantidad.Maximum = new decimal(new int[] { 99, 0, 0, 0 });
            this.numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numCantidad.Name = "numCantidad";
            this.numCantidad.Size = new System.Drawing.Size(120, 27);
            this.numCantidad.TabIndex = 1;
            this.numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
            this.numCantidad.ValueChanged += new System.EventHandler(this.Campo_Cambiado);
            //
            // lblTotal
            //
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.lblTotal.Location = new System.Drawing.Point(24, 150);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(440, 30);
            this.lblTotal.Tag = "personalizada";
            //
            // FormCargo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 400);
            this.Name = "FormCargo";
            this.Text = "Cargo adicional";
            ((System.ComponentModel.ISupportInitialize)(this.numCantidad)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblServicio;
        private System.Windows.Forms.ComboBox cmbServicio;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.NumericUpDown numCantidad;
        private System.Windows.Forms.Label lblTotal;
    }
}
