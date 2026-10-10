namespace Hotel.UI.Formularios
{
    partial class FormReserva
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
            this.lblEntrada = new System.Windows.Forms.Label();
            this.dtpEntrada = new System.Windows.Forms.DateTimePicker();
            this.lblSalida = new System.Windows.Forms.Label();
            this.dtpSalida = new System.Windows.Forms.DateTimePicker();
            this.lblPersonas = new System.Windows.Forms.Label();
            this.numPersonas = new System.Windows.Forms.NumericUpDown();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new Hotel.UI.Controles.BotonModerno();
            this.lblDisponibles = new System.Windows.Forms.Label();
            this.tablaHabitaciones = new System.Windows.Forms.DataGridView();
            this.lblHuesped = new System.Windows.Forms.Label();
            this.txtBuscarHuesped = new System.Windows.Forms.TextBox();
            this.btnNuevoHuesped = new Hotel.UI.Controles.BotonModerno();
            this.tablaHuespedes = new System.Windows.Forms.DataGridView();
            this.lblResumen = new System.Windows.Forms.Label();
            this.btnConfirmar = new Hotel.UI.Controles.BotonModerno();
            ((System.ComponentModel.ISupportInitialize)(this.numPersonas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaHabitaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaHuespedes)).BeginInit();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblEntrada);
            this.pnlContenido.Controls.Add(this.dtpEntrada);
            this.pnlContenido.Controls.Add(this.lblSalida);
            this.pnlContenido.Controls.Add(this.dtpSalida);
            this.pnlContenido.Controls.Add(this.lblPersonas);
            this.pnlContenido.Controls.Add(this.numPersonas);
            this.pnlContenido.Controls.Add(this.lblTipo);
            this.pnlContenido.Controls.Add(this.cmbTipo);
            this.pnlContenido.Controls.Add(this.btnBuscar);
            this.pnlContenido.Controls.Add(this.lblDisponibles);
            this.pnlContenido.Controls.Add(this.tablaHabitaciones);
            this.pnlContenido.Controls.Add(this.lblHuesped);
            this.pnlContenido.Controls.Add(this.txtBuscarHuesped);
            this.pnlContenido.Controls.Add(this.btnNuevoHuesped);
            this.pnlContenido.Controls.Add(this.tablaHuespedes);
            this.pnlContenido.Controls.Add(this.lblResumen);
            this.pnlContenido.Controls.Add(this.btnConfirmar);
            //
            // 1. fechas, personas y tipo
            //
            this.lblEntrada.AutoSize = true;
            this.lblEntrada.Location = new System.Drawing.Point(24, 2);
            this.lblEntrada.Name = "lblEntrada";
            this.lblEntrada.Text = "1. Entrada";
            this.dtpEntrada.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEntrada.Location = new System.Drawing.Point(24, 24);
            this.dtpEntrada.Name = "dtpEntrada";
            this.dtpEntrada.Size = new System.Drawing.Size(150, 27);
            this.dtpEntrada.TabIndex = 0;
            this.dtpEntrada.ValueChanged += new System.EventHandler(this.FechaEntrada_ValueChanged);
            this.lblSalida.AutoSize = true;
            this.lblSalida.Location = new System.Drawing.Point(190, 2);
            this.lblSalida.Name = "lblSalida";
            this.lblSalida.Text = "Salida";
            this.dtpSalida.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSalida.Location = new System.Drawing.Point(190, 24);
            this.dtpSalida.Name = "dtpSalida";
            this.dtpSalida.Size = new System.Drawing.Size(150, 27);
            this.dtpSalida.TabIndex = 1;
            this.dtpSalida.ValueChanged += new System.EventHandler(this.Criterio_Cambiado);
            this.lblPersonas.AutoSize = true;
            this.lblPersonas.Location = new System.Drawing.Point(356, 2);
            this.lblPersonas.Name = "lblPersonas";
            this.lblPersonas.Text = "Personas";
            this.numPersonas.Location = new System.Drawing.Point(356, 24);
            this.numPersonas.Maximum = new decimal(new int[] { 12, 0, 0, 0 });
            this.numPersonas.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPersonas.Name = "numPersonas";
            this.numPersonas.Size = new System.Drawing.Size(80, 27);
            this.numPersonas.TabIndex = 2;
            this.numPersonas.Value = new decimal(new int[] { 2, 0, 0, 0 });
            this.numPersonas.ValueChanged += new System.EventHandler(this.Criterio_Cambiado);
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(452, 2);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Text = "Tipo de habitación";
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.Location = new System.Drawing.Point(452, 24);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(170, 28);
            this.cmbTipo.TabIndex = 3;
            this.cmbTipo.SelectedIndexChanged += new System.EventHandler(this.Criterio_Cambiado);
            this.btnBuscar.Icono = "";
            this.btnBuscar.Location = new System.Drawing.Point(642, 16);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(250, 40);
            this.btnBuscar.TabIndex = 4;
            this.btnBuscar.Text = "Buscar disponibilidad";
            this.btnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            //
            // 2. habitaciones disponibles
            //
            this.lblDisponibles.AutoSize = true;
            this.lblDisponibles.Location = new System.Drawing.Point(24, 66);
            this.lblDisponibles.Name = "lblDisponibles";
            this.lblDisponibles.Text = "2. Pulse «Buscar disponibilidad» para ver las habitaciones.";
            this.tablaHabitaciones.Location = new System.Drawing.Point(24, 90);
            this.tablaHabitaciones.Name = "tablaHabitaciones";
            this.tablaHabitaciones.Size = new System.Drawing.Size(868, 168);
            this.tablaHabitaciones.TabIndex = 5;
            this.tablaHabitaciones.SelectionChanged += new System.EventHandler(this.TablaHabitaciones_SelectionChanged);
            //
            // 3. huésped
            //
            this.lblHuesped.AutoSize = true;
            this.lblHuesped.Location = new System.Drawing.Point(24, 268);
            this.lblHuesped.Name = "lblHuesped";
            this.lblHuesped.Text = "3. Huésped (buscar por nombre o documento)";
            this.txtBuscarHuesped.Location = new System.Drawing.Point(24, 292);
            this.txtBuscarHuesped.Name = "txtBuscarHuesped";
            this.txtBuscarHuesped.Size = new System.Drawing.Size(300, 27);
            this.txtBuscarHuesped.TabIndex = 6;
            this.txtBuscarHuesped.TextChanged += new System.EventHandler(this.TxtBuscarHuesped_TextChanged);
            this.btnNuevoHuesped.Icono = "";
            this.btnNuevoHuesped.Location = new System.Drawing.Point(24, 328);
            this.btnNuevoHuesped.Name = "btnNuevoHuesped";
            this.btnNuevoHuesped.Size = new System.Drawing.Size(180, 40);
            this.btnNuevoHuesped.TabIndex = 7;
            this.btnNuevoHuesped.Text = "Nuevo huésped";
            this.btnNuevoHuesped.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnNuevoHuesped.Click += new System.EventHandler(this.BtnNuevoHuesped_Click);
            this.tablaHuespedes.Location = new System.Drawing.Point(344, 268);
            this.tablaHuespedes.Name = "tablaHuespedes";
            this.tablaHuespedes.Size = new System.Drawing.Size(548, 118);
            this.tablaHuespedes.TabIndex = 8;
            this.tablaHuespedes.SelectionChanged += new System.EventHandler(this.TablaHuespedes_SelectionChanged);
            //
            // 4. resumen
            //
            this.lblResumen.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lblResumen.Location = new System.Drawing.Point(24, 402);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(660, 80);
            this.lblResumen.TabIndex = 9;
            this.lblResumen.Tag = "personalizada";
            this.btnConfirmar.Icono = "";
            this.btnConfirmar.Location = new System.Drawing.Point(700, 402);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(192, 44);
            this.btnConfirmar.TabIndex = 10;
            this.btnConfirmar.Text = "Guardar y confirmar";
            this.btnConfirmar.Variante = Hotel.UI.Controles.VarianteBoton.Primario;
            this.btnConfirmar.Click += new System.EventHandler(this.BtnConfirmar_Click);
            //
            // FormReserva
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 640);
            this.Name = "FormReserva";
            this.Text = "Reserva";
            ((System.ComponentModel.ISupportInitialize)(this.numPersonas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaHabitaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablaHuespedes)).EndInit();
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblEntrada;
        private System.Windows.Forms.DateTimePicker dtpEntrada;
        private System.Windows.Forms.Label lblSalida;
        private System.Windows.Forms.DateTimePicker dtpSalida;
        private System.Windows.Forms.Label lblPersonas;
        private System.Windows.Forms.NumericUpDown numPersonas;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cmbTipo;
        private Hotel.UI.Controles.BotonModerno btnBuscar;
        private System.Windows.Forms.Label lblDisponibles;
        private System.Windows.Forms.DataGridView tablaHabitaciones;
        private System.Windows.Forms.Label lblHuesped;
        private System.Windows.Forms.TextBox txtBuscarHuesped;
        private Hotel.UI.Controles.BotonModerno btnNuevoHuesped;
        private System.Windows.Forms.DataGridView tablaHuespedes;
        private System.Windows.Forms.Label lblResumen;
        private Hotel.UI.Controles.BotonModerno btnConfirmar;
    }
}
