namespace Hotel.UI.Formularios
{
    partial class FormPago
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
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblMetodo = new System.Windows.Forms.Label();
            this.cmbMetodo = new System.Windows.Forms.ComboBox();
            this.lblMonto = new System.Windows.Forms.Label();
            this.numMonto = new System.Windows.Forms.NumericUpDown();
            this.pnlEfectivo = new System.Windows.Forms.Panel();
            this.lblRecibido = new System.Windows.Forms.Label();
            this.numRecibido = new System.Windows.Forms.NumericUpDown();
            this.lblCambio = new System.Windows.Forms.Label();
            this.pnlTarjeta = new System.Windows.Forms.Panel();
            this.lblDigitos = new System.Windows.Forms.Label();
            this.txtDigitos = new System.Windows.Forms.TextBox();
            this.lblAutorizacion = new System.Windows.Forms.Label();
            this.txtAutorizacion = new System.Windows.Forms.TextBox();
            this.pnlTransferencia = new System.Windows.Forms.Panel();
            this.lblBanco = new System.Windows.Forms.Label();
            this.txtBanco = new System.Windows.Forms.TextBox();
            this.lblReferencia = new System.Windows.Forms.Label();
            this.txtReferencia = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numMonto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRecibido)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.pnlEfectivo.SuspendLayout();
            this.pnlTarjeta.SuspendLayout();
            this.pnlTransferencia.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblSaldo);
            this.pnlContenido.Controls.Add(this.lblMetodo);
            this.pnlContenido.Controls.Add(this.cmbMetodo);
            this.pnlContenido.Controls.Add(this.lblMonto);
            this.pnlContenido.Controls.Add(this.numMonto);
            this.pnlContenido.Controls.Add(this.pnlEfectivo);
            this.pnlContenido.Controls.Add(this.pnlTarjeta);
            this.pnlContenido.Controls.Add(this.pnlTransferencia);
            //
            // lblSaldo
            //
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblSaldo.Location = new System.Drawing.Point(24, 4);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(450, 24);
            this.lblSaldo.Tag = "personalizada";
            //
            // lblMetodo
            //
            this.lblMetodo.AutoSize = true;
            this.lblMetodo.Location = new System.Drawing.Point(24, 42);
            this.lblMetodo.Name = "lblMetodo";
            this.lblMetodo.Text = "Método de pago";
            //
            // cmbMetodo
            //
            this.cmbMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMetodo.Location = new System.Drawing.Point(24, 64);
            this.cmbMetodo.Name = "cmbMetodo";
            this.cmbMetodo.Size = new System.Drawing.Size(210, 28);
            this.cmbMetodo.TabIndex = 0;
            this.cmbMetodo.SelectedIndexChanged += new System.EventHandler(this.Metodo_Cambiado);
            //
            // lblMonto
            //
            this.lblMonto.AutoSize = true;
            this.lblMonto.Location = new System.Drawing.Point(264, 42);
            this.lblMonto.Name = "lblMonto";
            this.lblMonto.Text = "Monto a pagar (Q)";
            //
            // numMonto
            //
            this.numMonto.DecimalPlaces = 2;
            this.numMonto.Location = new System.Drawing.Point(264, 64);
            this.numMonto.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numMonto.Name = "numMonto";
            this.numMonto.Size = new System.Drawing.Size(210, 27);
            this.numMonto.TabIndex = 1;
            this.numMonto.ThousandsSeparator = true;
            this.numMonto.ValueChanged += new System.EventHandler(this.Metodo_Cambiado);
            //
            // pnlEfectivo
            //
            this.pnlEfectivo.Controls.Add(this.lblRecibido);
            this.pnlEfectivo.Controls.Add(this.numRecibido);
            this.pnlEfectivo.Controls.Add(this.lblCambio);
            this.pnlEfectivo.Location = new System.Drawing.Point(24, 112);
            this.pnlEfectivo.Name = "pnlEfectivo";
            this.pnlEfectivo.Size = new System.Drawing.Size(450, 130);
            this.pnlEfectivo.TabIndex = 2;
            this.lblRecibido.AutoSize = true;
            this.lblRecibido.Location = new System.Drawing.Point(0, 0);
            this.lblRecibido.Name = "lblRecibido";
            this.lblRecibido.Text = "Monto recibido (Q)";
            this.numRecibido.DecimalPlaces = 2;
            this.numRecibido.Location = new System.Drawing.Point(0, 24);
            this.numRecibido.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numRecibido.Name = "numRecibido";
            this.numRecibido.Size = new System.Drawing.Size(210, 27);
            this.numRecibido.TabIndex = 0;
            this.numRecibido.ThousandsSeparator = true;
            this.numRecibido.ValueChanged += new System.EventHandler(this.Metodo_Cambiado);
            this.lblCambio.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblCambio.Location = new System.Drawing.Point(0, 66);
            this.lblCambio.Name = "lblCambio";
            this.lblCambio.Size = new System.Drawing.Size(440, 26);
            this.lblCambio.Tag = "personalizada";
            //
            // pnlTarjeta
            //
            this.pnlTarjeta.Controls.Add(this.lblDigitos);
            this.pnlTarjeta.Controls.Add(this.txtDigitos);
            this.pnlTarjeta.Controls.Add(this.lblAutorizacion);
            this.pnlTarjeta.Controls.Add(this.txtAutorizacion);
            this.pnlTarjeta.Location = new System.Drawing.Point(24, 112);
            this.pnlTarjeta.Name = "pnlTarjeta";
            this.pnlTarjeta.Size = new System.Drawing.Size(450, 130);
            this.pnlTarjeta.TabIndex = 3;
            this.pnlTarjeta.Visible = false;
            this.lblDigitos.AutoSize = true;
            this.lblDigitos.Location = new System.Drawing.Point(0, 0);
            this.lblDigitos.Name = "lblDigitos";
            this.lblDigitos.Text = "Últimos 4 dígitos";
            this.txtDigitos.Location = new System.Drawing.Point(0, 24);
            this.txtDigitos.MaxLength = 4;
            this.txtDigitos.Name = "txtDigitos";
            this.txtDigitos.Size = new System.Drawing.Size(210, 27);
            this.txtDigitos.TabIndex = 0;
            this.lblAutorizacion.AutoSize = true;
            this.lblAutorizacion.Location = new System.Drawing.Point(230, 0);
            this.lblAutorizacion.Name = "lblAutorizacion";
            this.lblAutorizacion.Text = "Número de autorización";
            this.txtAutorizacion.Location = new System.Drawing.Point(230, 24);
            this.txtAutorizacion.Name = "txtAutorizacion";
            this.txtAutorizacion.Size = new System.Drawing.Size(210, 27);
            this.txtAutorizacion.TabIndex = 1;
            //
            // pnlTransferencia
            //
            this.pnlTransferencia.Controls.Add(this.lblBanco);
            this.pnlTransferencia.Controls.Add(this.txtBanco);
            this.pnlTransferencia.Controls.Add(this.lblReferencia);
            this.pnlTransferencia.Controls.Add(this.txtReferencia);
            this.pnlTransferencia.Location = new System.Drawing.Point(24, 112);
            this.pnlTransferencia.Name = "pnlTransferencia";
            this.pnlTransferencia.Size = new System.Drawing.Size(450, 130);
            this.pnlTransferencia.TabIndex = 4;
            this.pnlTransferencia.Visible = false;
            this.lblBanco.AutoSize = true;
            this.lblBanco.Location = new System.Drawing.Point(0, 0);
            this.lblBanco.Name = "lblBanco";
            this.lblBanco.Text = "Banco";
            this.txtBanco.Location = new System.Drawing.Point(0, 24);
            this.txtBanco.Name = "txtBanco";
            this.txtBanco.Size = new System.Drawing.Size(210, 27);
            this.txtBanco.TabIndex = 0;
            this.lblReferencia.AutoSize = true;
            this.lblReferencia.Location = new System.Drawing.Point(230, 0);
            this.lblReferencia.Name = "lblReferencia";
            this.lblReferencia.Text = "Número de referencia";
            this.txtReferencia.Location = new System.Drawing.Point(230, 24);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Size = new System.Drawing.Size(210, 27);
            this.txtReferencia.TabIndex = 1;
            //
            // FormPago
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 420);
            this.Name = "FormPago";
            this.Text = "Pago";
            ((System.ComponentModel.ISupportInitialize)(this.numMonto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRecibido)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.pnlEfectivo.ResumeLayout(false);
            this.pnlEfectivo.PerformLayout();
            this.pnlTarjeta.ResumeLayout(false);
            this.pnlTarjeta.PerformLayout();
            this.pnlTransferencia.ResumeLayout(false);
            this.pnlTransferencia.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblMetodo;
        private System.Windows.Forms.ComboBox cmbMetodo;
        private System.Windows.Forms.Label lblMonto;
        private System.Windows.Forms.NumericUpDown numMonto;
        private System.Windows.Forms.Panel pnlEfectivo;
        private System.Windows.Forms.Label lblRecibido;
        private System.Windows.Forms.NumericUpDown numRecibido;
        private System.Windows.Forms.Label lblCambio;
        private System.Windows.Forms.Panel pnlTarjeta;
        private System.Windows.Forms.Label lblDigitos;
        private System.Windows.Forms.TextBox txtDigitos;
        private System.Windows.Forms.Label lblAutorizacion;
        private System.Windows.Forms.TextBox txtAutorizacion;
        private System.Windows.Forms.Panel pnlTransferencia;
        private System.Windows.Forms.Label lblBanco;
        private System.Windows.Forms.TextBox txtBanco;
        private System.Windows.Forms.Label lblReferencia;
        private System.Windows.Forms.TextBox txtReferencia;
    }
}
