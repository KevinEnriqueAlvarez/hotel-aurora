namespace Hotel.UI.Formularios
{
    partial class FormCheckOut
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
            this.tablaLineas = new System.Windows.Forms.DataGridView();
            this.lblDescuento = new System.Windows.Forms.Label();
            this.numDescuento = new System.Windows.Forms.NumericUpDown();
            this.lblTotales = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblInfo);
            this.pnlContenido.Controls.Add(this.tablaLineas);
            this.pnlContenido.Controls.Add(this.lblDescuento);
            this.pnlContenido.Controls.Add(this.numDescuento);
            this.pnlContenido.Controls.Add(this.lblTotales);
            //
            // lblInfo
            //
            this.lblInfo.Location = new System.Drawing.Point(24, 4);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(692, 50);
            //
            // tablaLineas
            //
            this.tablaLineas.Location = new System.Drawing.Point(24, 60);
            this.tablaLineas.Name = "tablaLineas";
            this.tablaLineas.Size = new System.Drawing.Size(692, 170);
            this.tablaLineas.TabIndex = 0;
            //
            // lblDescuento
            //
            this.lblDescuento.AutoSize = true;
            this.lblDescuento.Location = new System.Drawing.Point(24, 246);
            this.lblDescuento.Name = "lblDescuento";
            this.lblDescuento.Text = "Descuento (%)";
            //
            // numDescuento
            //
            this.numDescuento.DecimalPlaces = 1;
            this.numDescuento.Location = new System.Drawing.Point(24, 270);
            this.numDescuento.Name = "numDescuento";
            this.numDescuento.Size = new System.Drawing.Size(140, 27);
            this.numDescuento.TabIndex = 1;
            this.numDescuento.ValueChanged += new System.EventHandler(this.NumDescuento_ValueChanged);
            //
            // lblTotales
            //
            this.lblTotales.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblTotales.Location = new System.Drawing.Point(400, 240);
            this.lblTotales.Name = "lblTotales";
            this.lblTotales.Size = new System.Drawing.Size(316, 130);
            this.lblTotales.TabIndex = 2;
            this.lblTotales.Tag = "personalizada";
            this.lblTotales.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // FormCheckOut
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(760, 540);
            this.Name = "FormCheckOut";
            this.Text = "Check-out";
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDescuento)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridView tablaLineas;
        private System.Windows.Forms.Label lblDescuento;
        private System.Windows.Forms.NumericUpDown numDescuento;
        private System.Windows.Forms.Label lblTotales;
    }
}
