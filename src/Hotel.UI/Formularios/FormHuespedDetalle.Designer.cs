namespace Hotel.UI.Formularios
{
    partial class FormHuespedDetalle
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
            this.lblNombre = new System.Windows.Forms.Label();
            this.insignia = new Hotel.UI.Controles.InsigniaEstado();
            this.selector = new Hotel.UI.Controles.SelectorSegmentado();
            this.tblDatos = new System.Windows.Forms.TableLayoutPanel();
            this.tablaHistorial = new System.Windows.Forms.DataGridView();
            this.lblResumen = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tablaHistorial)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.tblDatos.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.tablaHistorial);
            this.pnlContenido.Controls.Add(this.lblResumen);
            this.pnlContenido.Controls.Add(this.tblDatos);
            this.pnlContenido.Controls.Add(this.selector);
            this.pnlContenido.Controls.Add(this.insignia);
            this.pnlContenido.Controls.Add(this.lblNombre);
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblNombre.Location = new System.Drawing.Point(24, 2);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Tag = "personalizada";
            this.lblNombre.Text = "Nombre";
            //
            // insignia
            //
            this.insignia.Location = new System.Drawing.Point(620, 0);
            this.insignia.Name = "insignia";
            this.insignia.Size = new System.Drawing.Size(110, 30);
            this.insignia.TabIndex = 1;
            //
            // selector
            //
            this.selector.Location = new System.Drawing.Point(24, 42);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(400, 38);
            this.selector.TabIndex = 2;
            this.selector.IndiceCambiado += new System.EventHandler(this.Selector_IndiceCambiado);
            //
            // tblDatos
            //
            this.tblDatos.ColumnCount = 2;
            this.tblDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblDatos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblDatos.Location = new System.Drawing.Point(24, 98);
            this.tblDatos.Name = "tblDatos";
            this.tblDatos.Size = new System.Drawing.Size(706, 320);
            this.tblDatos.TabIndex = 3;
            //
            // tablaHistorial
            //
            this.tablaHistorial.Location = new System.Drawing.Point(24, 98);
            this.tablaHistorial.Name = "tablaHistorial";
            this.tablaHistorial.Size = new System.Drawing.Size(706, 284);
            this.tablaHistorial.TabIndex = 4;
            this.tablaHistorial.Visible = false;
            //
            // lblResumen
            //
            this.lblResumen.Location = new System.Drawing.Point(24, 390);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(706, 28);
            this.lblResumen.TabIndex = 5;
            this.lblResumen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblResumen.Visible = false;
            //
            // FormHuespedDetalle
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(780, 560);
            this.Name = "FormHuespedDetalle";
            this.Text = "Detalle de huésped";
            ((System.ComponentModel.ISupportInitialize)(this.tablaHistorial)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.tblDatos.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblNombre;
        private Hotel.UI.Controles.InsigniaEstado insignia;
        private Hotel.UI.Controles.SelectorSegmentado selector;
        private System.Windows.Forms.TableLayoutPanel tblDatos;
        private System.Windows.Forms.DataGridView tablaHistorial;
        private System.Windows.Forms.Label lblResumen;
    }
}
