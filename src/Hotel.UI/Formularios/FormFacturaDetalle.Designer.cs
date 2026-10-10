namespace Hotel.UI.Formularios
{
    partial class FormFacturaDetalle
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
            this.lblEncabezado = new System.Windows.Forms.Label();
            this.insignia = new Hotel.UI.Controles.InsigniaEstado();
            this.tablaLineas = new System.Windows.Forms.DataGridView();
            this.tablaPagos = new System.Windows.Forms.DataGridView();
            this.lblTotales = new System.Windows.Forms.Label();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.btnPago = new Hotel.UI.Controles.BotonModerno();
            this.btnAnular = new Hotel.UI.Controles.BotonModerno();
            this.btnVistaPrevia = new Hotel.UI.Controles.BotonModerno();
            this.btnImprimir = new Hotel.UI.Controles.BotonModerno();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPagos)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblEncabezado);
            this.pnlContenido.Controls.Add(this.insignia);
            this.pnlContenido.Controls.Add(this.tablaLineas);
            this.pnlContenido.Controls.Add(this.tablaPagos);
            this.pnlContenido.Controls.Add(this.lblTotales);
            this.pnlContenido.Controls.Add(this.lblSaldo);
            this.pnlContenido.Controls.Add(this.btnPago);
            this.pnlContenido.Controls.Add(this.btnAnular);
            this.pnlContenido.Controls.Add(this.btnVistaPrevia);
            this.pnlContenido.Controls.Add(this.btnImprimir);
            //
            // lblEncabezado
            //
            this.lblEncabezado.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblEncabezado.Location = new System.Drawing.Point(24, 0);
            this.lblEncabezado.Name = "lblEncabezado";
            this.lblEncabezado.Size = new System.Drawing.Size(650, 48);
            this.lblEncabezado.Tag = "personalizada";
            //
            // insignia
            //
            this.insignia.Location = new System.Drawing.Point(700, 4);
            this.insignia.Name = "insignia";
            this.insignia.Size = new System.Drawing.Size(120, 30);
            this.insignia.TabIndex = 1;
            //
            // tablaLineas
            //
            this.tablaLineas.Location = new System.Drawing.Point(24, 56);
            this.tablaLineas.Name = "tablaLineas";
            this.tablaLineas.Size = new System.Drawing.Size(796, 150);
            this.tablaLineas.TabIndex = 2;
            //
            // tablaPagos
            //
            this.tablaPagos.Location = new System.Drawing.Point(24, 220);
            this.tablaPagos.Name = "tablaPagos";
            this.tablaPagos.Size = new System.Drawing.Size(470, 140);
            this.tablaPagos.TabIndex = 3;
            //
            // lblTotales
            //
            this.lblTotales.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.lblTotales.Location = new System.Drawing.Point(510, 220);
            this.lblTotales.Name = "lblTotales";
            this.lblTotales.Size = new System.Drawing.Size(310, 128);
            this.lblTotales.Tag = "personalizada";
            this.lblTotales.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // lblSaldo
            //
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblSaldo.Location = new System.Drawing.Point(24, 372);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(796, 30);
            this.lblSaldo.Tag = "personalizada";
            //
            // btnPago
            //
            this.btnPago.Icono = "";
            this.btnPago.Location = new System.Drawing.Point(24, 412);
            this.btnPago.Name = "btnPago";
            this.btnPago.Size = new System.Drawing.Size(180, 42);
            this.btnPago.TabIndex = 4;
            this.btnPago.Text = "Registrar pago";
            this.btnPago.Click += new System.EventHandler(this.BtnPago_Click);
            //
            // btnAnular
            //
            this.btnAnular.Icono = "";
            this.btnAnular.Location = new System.Drawing.Point(214, 412);
            this.btnAnular.Name = "btnAnular";
            this.btnAnular.Size = new System.Drawing.Size(170, 42);
            this.btnAnular.TabIndex = 5;
            this.btnAnular.Text = "Anular factura";
            this.btnAnular.Variante = Hotel.UI.Controles.VarianteBoton.Peligro;
            this.btnAnular.Click += new System.EventHandler(this.BtnAnular_Click);
            //
            // btnVistaPrevia
            //
            this.btnVistaPrevia.Icono = "";
            this.btnVistaPrevia.Location = new System.Drawing.Point(510, 412);
            this.btnVistaPrevia.Name = "btnVistaPrevia";
            this.btnVistaPrevia.Size = new System.Drawing.Size(160, 42);
            this.btnVistaPrevia.TabIndex = 6;
            this.btnVistaPrevia.Text = "Vista previa";
            this.btnVistaPrevia.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnVistaPrevia.Click += new System.EventHandler(this.BtnVistaPrevia_Click);
            //
            // btnImprimir
            //
            this.btnImprimir.Icono = "";
            this.btnImprimir.Location = new System.Drawing.Point(680, 412);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(140, 42);
            this.btnImprimir.TabIndex = 7;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnImprimir.Click += new System.EventHandler(this.BtnImprimir_Click);
            //
            // FormFacturaDetalle
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(870, 620);
            this.Name = "FormFacturaDetalle";
            this.Text = "Factura";
            ((System.ComponentModel.ISupportInitialize)(this.tablaLineas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaPagos)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblEncabezado;
        private Hotel.UI.Controles.InsigniaEstado insignia;
        private System.Windows.Forms.DataGridView tablaLineas;
        private System.Windows.Forms.DataGridView tablaPagos;
        private System.Windows.Forms.Label lblTotales;
        private System.Windows.Forms.Label lblSaldo;
        private Hotel.UI.Controles.BotonModerno btnPago;
        private Hotel.UI.Controles.BotonModerno btnAnular;
        private Hotel.UI.Controles.BotonModerno btnVistaPrevia;
        private Hotel.UI.Controles.BotonModerno btnImprimir;
    }
}
