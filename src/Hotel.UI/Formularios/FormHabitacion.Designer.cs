namespace Hotel.UI.Formularios
{
    partial class FormHabitacion
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
            this.lblNumero = new System.Windows.Forms.Label();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.lblPiso = new System.Windows.Forms.Label();
            this.numPiso = new System.Windows.Forms.NumericUpDown();
            this.lblCapacidad = new System.Windows.Forms.Label();
            this.numCapacidad = new System.Windows.Forms.NumericUpDown();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.lblTarifa = new System.Windows.Forms.Label();
            this.numTarifa = new System.Windows.Forms.NumericUpDown();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numPiso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCapacidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTarifa)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblNumero);
            this.pnlContenido.Controls.Add(this.txtNumero);
            this.pnlContenido.Controls.Add(this.lblPiso);
            this.pnlContenido.Controls.Add(this.numPiso);
            this.pnlContenido.Controls.Add(this.lblCapacidad);
            this.pnlContenido.Controls.Add(this.numCapacidad);
            this.pnlContenido.Controls.Add(this.lblTipo);
            this.pnlContenido.Controls.Add(this.cmbTipo);
            this.pnlContenido.Controls.Add(this.lblTarifa);
            this.pnlContenido.Controls.Add(this.numTarifa);
            this.pnlContenido.Controls.Add(this.lblDescripcion);
            this.pnlContenido.Controls.Add(this.txtDescripcion);
            //
            // lblNumero
            //
            this.lblNumero.AutoSize = true;
            this.lblNumero.Location = new System.Drawing.Point(24, 8);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Text = "Número";
            //
            // txtNumero
            //
            this.txtNumero.Location = new System.Drawing.Point(24, 30);
            this.txtNumero.MaxLength = 10;
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(200, 27);
            this.txtNumero.TabIndex = 0;
            //
            // lblPiso
            //
            this.lblPiso.AutoSize = true;
            this.lblPiso.Location = new System.Drawing.Point(254, 8);
            this.lblPiso.Name = "lblPiso";
            this.lblPiso.Text = "Piso";
            //
            // numPiso
            //
            this.numPiso.Location = new System.Drawing.Point(254, 30);
            this.numPiso.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.numPiso.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPiso.Name = "numPiso";
            this.numPiso.Size = new System.Drawing.Size(100, 27);
            this.numPiso.TabIndex = 1;
            this.numPiso.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblCapacidad
            //
            this.lblCapacidad.AutoSize = true;
            this.lblCapacidad.Location = new System.Drawing.Point(374, 8);
            this.lblCapacidad.Name = "lblCapacidad";
            this.lblCapacidad.Text = "Capacidad";
            //
            // numCapacidad
            //
            this.numCapacidad.Location = new System.Drawing.Point(374, 30);
            this.numCapacidad.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numCapacidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numCapacidad.Name = "numCapacidad";
            this.numCapacidad.Size = new System.Drawing.Size(100, 27);
            this.numCapacidad.TabIndex = 2;
            this.numCapacidad.Value = new decimal(new int[] { 2, 0, 0, 0 });
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(24, 74);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Text = "Tipo";
            //
            // cmbTipo
            //
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Location = new System.Drawing.Point(24, 96);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(200, 28);
            this.cmbTipo.TabIndex = 3;
            //
            // lblTarifa
            //
            this.lblTarifa.AutoSize = true;
            this.lblTarifa.Location = new System.Drawing.Point(254, 74);
            this.lblTarifa.Name = "lblTarifa";
            this.lblTarifa.Text = "Tarifa por noche (Q)";
            //
            // numTarifa
            //
            this.numTarifa.DecimalPlaces = 2;
            this.numTarifa.Location = new System.Drawing.Point(254, 96);
            this.numTarifa.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numTarifa.Name = "numTarifa";
            this.numTarifa.Size = new System.Drawing.Size(220, 27);
            this.numTarifa.TabIndex = 4;
            this.numTarifa.ThousandsSeparator = true;
            this.numTarifa.Value = new decimal(new int[] { 500, 0, 0, 0 });
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(24, 140);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Text = "Descripción";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(24, 162);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(450, 100);
            this.txtDescripcion.TabIndex = 5;
            //
            // FormHabitacion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 440);
            this.Name = "FormHabitacion";
            this.Text = "Habitación";
            ((System.ComponentModel.ISupportInitialize)(this.numPiso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCapacidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTarifa)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.Label lblPiso;
        private System.Windows.Forms.NumericUpDown numPiso;
        private System.Windows.Forms.Label lblCapacidad;
        private System.Windows.Forms.NumericUpDown numCapacidad;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private System.Windows.Forms.Label lblTarifa;
        private System.Windows.Forms.NumericUpDown numTarifa;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
    }
}
