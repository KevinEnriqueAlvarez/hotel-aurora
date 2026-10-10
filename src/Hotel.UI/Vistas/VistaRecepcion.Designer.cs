namespace Hotel.UI.Vistas
{
    partial class VistaRecepcion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tblRecepcion = new System.Windows.Forms.TableLayoutPanel();
            this.tarjetaLlegadas = new Hotel.UI.Controles.PanelTarjeta();
            this.tablaLlegadas = new System.Windows.Forms.DataGridView();
            this.pnlLlegadas = new System.Windows.Forms.Panel();
            this.btnCheckIn = new Hotel.UI.Controles.BotonModerno();
            this.lblLlegadas = new System.Windows.Forms.Label();
            this.tarjetaEnCasa = new Hotel.UI.Controles.PanelTarjeta();
            this.tablaEnCasa = new System.Windows.Forms.DataGridView();
            this.pnlEnCasa = new System.Windows.Forms.Panel();
            this.btnCargo = new Hotel.UI.Controles.BotonModerno();
            this.btnCheckOut = new Hotel.UI.Controles.BotonModerno();
            this.lblEnCasa = new System.Windows.Forms.Label();
            this.tarjetaLimpieza = new Hotel.UI.Controles.PanelTarjeta();
            this.tablaLimpieza = new System.Windows.Forms.DataGridView();
            this.pnlLimpieza = new System.Windows.Forms.Panel();
            this.btnLiberar = new Hotel.UI.Controles.BotonModerno();
            this.lblLimpieza = new System.Windows.Forms.Label();
            this.tblRecepcion.SuspendLayout();
            this.tarjetaLlegadas.SuspendLayout();
            this.tarjetaEnCasa.SuspendLayout();
            this.tarjetaLimpieza.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLlegadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaEnCasa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLimpieza)).BeginInit();
            this.pnlLlegadas.SuspendLayout();
            this.pnlEnCasa.SuspendLayout();
            this.pnlLimpieza.SuspendLayout();
            this.SuspendLayout();
            //
            // tblRecepcion
            //
            this.tblRecepcion.ColumnCount = 3;
            this.tblRecepcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblRecepcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.tblRecepcion.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblRecepcion.Controls.Add(this.tarjetaLlegadas, 0, 0);
            this.tblRecepcion.Controls.Add(this.tarjetaEnCasa, 1, 0);
            this.tblRecepcion.Controls.Add(this.tarjetaLimpieza, 2, 0);
            this.tblRecepcion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRecepcion.Location = new System.Drawing.Point(0, 0);
            this.tblRecepcion.Name = "tblRecepcion";
            this.tblRecepcion.RowCount = 1;
            this.tblRecepcion.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblRecepcion.Size = new System.Drawing.Size(1200, 560);
            this.tblRecepcion.TabIndex = 0;
            //
            // tarjetaLlegadas
            //
            this.tarjetaLlegadas.Controls.Add(this.tablaLlegadas);
            this.tarjetaLlegadas.Controls.Add(this.pnlLlegadas);
            this.tarjetaLlegadas.Controls.Add(this.lblLlegadas);
            this.tarjetaLlegadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaLlegadas.Name = "tarjetaLlegadas";
            this.tarjetaLlegadas.Padding = new System.Windows.Forms.Padding(18, 16, 18, 14);
            this.tarjetaLlegadas.TabIndex = 0;
            //
            // tablaLlegadas
            //
            this.tablaLlegadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaLlegadas.Name = "tablaLlegadas";
            this.tablaLlegadas.TabIndex = 0;
            this.tablaLlegadas.SelectionChanged += new System.EventHandler(this.Tabla_SelectionChanged);
            //
            // pnlLlegadas
            //
            this.pnlLlegadas.Controls.Add(this.btnCheckIn);
            this.pnlLlegadas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLlegadas.Name = "pnlLlegadas";
            this.pnlLlegadas.Size = new System.Drawing.Size(300, 54);
            this.pnlLlegadas.TabIndex = 1;
            //
            // btnCheckIn
            //
            this.btnCheckIn.Icono = "";
            this.btnCheckIn.Location = new System.Drawing.Point(0, 10);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(180, 40);
            this.btnCheckIn.TabIndex = 0;
            this.btnCheckIn.Text = "Realizar check-in";
            this.btnCheckIn.Click += new System.EventHandler(this.BtnCheckIn_Click);
            //
            // lblLlegadas
            //
            this.lblLlegadas.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLlegadas.Name = "lblLlegadas";
            this.lblLlegadas.Size = new System.Drawing.Size(300, 40);
            this.lblLlegadas.TabIndex = 2;
            this.lblLlegadas.Tag = "personalizada";
            this.lblLlegadas.Text = "Llegadas de hoy";
            this.lblLlegadas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tarjetaEnCasa
            //
            this.tarjetaEnCasa.Controls.Add(this.tablaEnCasa);
            this.tarjetaEnCasa.Controls.Add(this.pnlEnCasa);
            this.tarjetaEnCasa.Controls.Add(this.lblEnCasa);
            this.tarjetaEnCasa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaEnCasa.Name = "tarjetaEnCasa";
            this.tarjetaEnCasa.Padding = new System.Windows.Forms.Padding(18, 16, 18, 14);
            this.tarjetaEnCasa.TabIndex = 1;
            //
            // tablaEnCasa
            //
            this.tablaEnCasa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaEnCasa.Name = "tablaEnCasa";
            this.tablaEnCasa.TabIndex = 0;
            this.tablaEnCasa.SelectionChanged += new System.EventHandler(this.Tabla_SelectionChanged);
            //
            // pnlEnCasa
            //
            this.pnlEnCasa.Controls.Add(this.btnCargo);
            this.pnlEnCasa.Controls.Add(this.btnCheckOut);
            this.pnlEnCasa.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEnCasa.Name = "pnlEnCasa";
            this.pnlEnCasa.Size = new System.Drawing.Size(300, 54);
            this.pnlEnCasa.TabIndex = 1;
            //
            // btnCargo
            //
            this.btnCargo.Icono = "";
            this.btnCargo.Location = new System.Drawing.Point(0, 10);
            this.btnCargo.Name = "btnCargo";
            this.btnCargo.Size = new System.Drawing.Size(170, 40);
            this.btnCargo.TabIndex = 0;
            this.btnCargo.Text = "Agregar cargo";
            this.btnCargo.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnCargo.Click += new System.EventHandler(this.BtnCargo_Click);
            //
            // btnCheckOut
            //
            this.btnCheckOut.Icono = "";
            this.btnCheckOut.Location = new System.Drawing.Point(180, 10);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(160, 40);
            this.btnCheckOut.TabIndex = 1;
            this.btnCheckOut.Text = "Check-out";
            this.btnCheckOut.Click += new System.EventHandler(this.BtnCheckOut_Click);
            //
            // lblEnCasa
            //
            this.lblEnCasa.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEnCasa.Name = "lblEnCasa";
            this.lblEnCasa.Size = new System.Drawing.Size(300, 40);
            this.lblEnCasa.TabIndex = 2;
            this.lblEnCasa.Tag = "personalizada";
            this.lblEnCasa.Text = "Huéspedes en casa";
            this.lblEnCasa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tarjetaLimpieza
            //
            this.tarjetaLimpieza.Controls.Add(this.tablaLimpieza);
            this.tarjetaLimpieza.Controls.Add(this.pnlLimpieza);
            this.tarjetaLimpieza.Controls.Add(this.lblLimpieza);
            this.tarjetaLimpieza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaLimpieza.Name = "tarjetaLimpieza";
            this.tarjetaLimpieza.Padding = new System.Windows.Forms.Padding(18, 16, 18, 14);
            this.tarjetaLimpieza.TabIndex = 2;
            //
            // tablaLimpieza
            //
            this.tablaLimpieza.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablaLimpieza.Name = "tablaLimpieza";
            this.tablaLimpieza.TabIndex = 0;
            this.tablaLimpieza.SelectionChanged += new System.EventHandler(this.Tabla_SelectionChanged);
            //
            // pnlLimpieza
            //
            this.pnlLimpieza.Controls.Add(this.btnLiberar);
            this.pnlLimpieza.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlLimpieza.Name = "pnlLimpieza";
            this.pnlLimpieza.Size = new System.Drawing.Size(300, 54);
            this.pnlLimpieza.TabIndex = 1;
            //
            // btnLiberar
            //
            this.btnLiberar.Icono = "";
            this.btnLiberar.Location = new System.Drawing.Point(0, 10);
            this.btnLiberar.Name = "btnLiberar";
            this.btnLiberar.Size = new System.Drawing.Size(180, 40);
            this.btnLiberar.TabIndex = 0;
            this.btnLiberar.Text = "Liberar habitación";
            this.btnLiberar.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnLiberar.Click += new System.EventHandler(this.BtnLiberar_Click);
            //
            // lblLimpieza
            //
            this.lblLimpieza.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLimpieza.Name = "lblLimpieza";
            this.lblLimpieza.Size = new System.Drawing.Size(300, 40);
            this.lblLimpieza.TabIndex = 2;
            this.lblLimpieza.Tag = "personalizada";
            this.lblLimpieza.Text = "Por limpiar";
            this.lblLimpieza.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // VistaRecepcion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tblRecepcion);
            this.Name = "VistaRecepcion";
            this.Size = new System.Drawing.Size(1200, 560);
            this.tblRecepcion.ResumeLayout(false);
            this.tarjetaLlegadas.ResumeLayout(false);
            this.tarjetaEnCasa.ResumeLayout(false);
            this.tarjetaLimpieza.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablaLlegadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaEnCasa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaLimpieza)).EndInit();
            this.pnlLlegadas.ResumeLayout(false);
            this.pnlEnCasa.ResumeLayout(false);
            this.pnlLimpieza.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblRecepcion;
        private Hotel.UI.Controles.PanelTarjeta tarjetaLlegadas;
        private System.Windows.Forms.DataGridView tablaLlegadas;
        private System.Windows.Forms.Panel pnlLlegadas;
        private Hotel.UI.Controles.BotonModerno btnCheckIn;
        private System.Windows.Forms.Label lblLlegadas;
        private Hotel.UI.Controles.PanelTarjeta tarjetaEnCasa;
        private System.Windows.Forms.DataGridView tablaEnCasa;
        private System.Windows.Forms.Panel pnlEnCasa;
        private Hotel.UI.Controles.BotonModerno btnCargo;
        private Hotel.UI.Controles.BotonModerno btnCheckOut;
        private System.Windows.Forms.Label lblEnCasa;
        private Hotel.UI.Controles.PanelTarjeta tarjetaLimpieza;
        private System.Windows.Forms.DataGridView tablaLimpieza;
        private System.Windows.Forms.Panel pnlLimpieza;
        private Hotel.UI.Controles.BotonModerno btnLiberar;
        private System.Windows.Forms.Label lblLimpieza;
    }
}
