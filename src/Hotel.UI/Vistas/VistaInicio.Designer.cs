namespace Hotel.UI.Vistas
{
    partial class VistaInicio
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
            this.tblPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.tblKpi = new System.Windows.Forms.TableLayoutPanel();
            this.kpiOcupacion = new Hotel.UI.Controles.TarjetaKpi();
            this.kpiIngresos = new Hotel.UI.Controles.TarjetaKpi();
            this.kpiReservas = new Hotel.UI.Controles.TarjetaKpi();
            this.kpiLlegadas = new Hotel.UI.Controles.TarjetaKpi();
            this.tarjetaIngresos = new Hotel.UI.Controles.PanelTarjeta();
            this.grafico = new Hotel.UI.Controles.GraficoBarras();
            this.pnlIngresosEncabezado = new System.Windows.Forms.Panel();
            this.lblIngresosTotal = new System.Windows.Forms.Label();
            this.selector = new Hotel.UI.Controles.SelectorSegmentado();
            this.lblIngresosTitulo = new System.Windows.Forms.Label();
            this.tblDerecha = new System.Windows.Forms.TableLayoutPanel();
            this.tarjetaHabitaciones = new Hotel.UI.Controles.PanelTarjeta();
            this.dona = new Hotel.UI.Controles.GraficoDona();
            this.lblHabitacionesTitulo = new System.Windows.Forms.Label();
            this.tblCobros = new System.Windows.Forms.TableLayoutPanel();
            this.tarjetaCobros = new Hotel.UI.Controles.PanelTarjeta();
            this.barPendiente = new Hotel.UI.Controles.BarraProgreso();
            this.barCobrado = new Hotel.UI.Controles.BarraProgreso();
            this.lblCobrosTitulo = new System.Windows.Forms.Label();
            this.tarjetaSaldo = new Hotel.UI.Controles.PanelTarjeta();
            this.lblSaldoValor = new System.Windows.Forms.Label();
            this.lblSaldoTitulo = new System.Windows.Forms.Label();
            this.tblPrincipal.SuspendLayout();
            this.tblKpi.SuspendLayout();
            this.tarjetaIngresos.SuspendLayout();
            this.pnlIngresosEncabezado.SuspendLayout();
            this.tblDerecha.SuspendLayout();
            this.tarjetaHabitaciones.SuspendLayout();
            this.tblCobros.SuspendLayout();
            this.tarjetaCobros.SuspendLayout();
            this.tarjetaSaldo.SuspendLayout();
            this.SuspendLayout();
            //
            // tblPrincipal
            //
            this.tblPrincipal.ColumnCount = 2;
            this.tblPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 57F));
            this.tblPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43F));
            this.tblPrincipal.Controls.Add(this.tblKpi, 0, 0);
            this.tblPrincipal.Controls.Add(this.tarjetaIngresos, 0, 1);
            this.tblPrincipal.Controls.Add(this.tblDerecha, 1, 1);
            this.tblPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblPrincipal.Name = "tblPrincipal";
            this.tblPrincipal.RowCount = 2;
            this.tblPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tblPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblPrincipal.Size = new System.Drawing.Size(1200, 600);
            this.tblPrincipal.TabIndex = 0;
            //
            // tblKpi
            //
            this.tblKpi.ColumnCount = 4;
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblKpi.Controls.Add(this.kpiOcupacion, 0, 0);
            this.tblKpi.Controls.Add(this.kpiIngresos, 1, 0);
            this.tblKpi.Controls.Add(this.kpiReservas, 2, 0);
            this.tblKpi.Controls.Add(this.kpiLlegadas, 3, 0);
            this.tblPrincipal.SetColumnSpan(this.tblKpi, 2);
            this.tblKpi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblKpi.Margin = new System.Windows.Forms.Padding(0);
            this.tblKpi.Name = "tblKpi";
            this.tblKpi.RowCount = 1;
            this.tblKpi.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblKpi.TabIndex = 0;
            //
            // KPI
            //
            this.kpiOcupacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiOcupacion.Margin = new System.Windows.Forms.Padding(0);
            this.kpiOcupacion.Name = "kpiOcupacion";
            this.kpiIngresos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiIngresos.Margin = new System.Windows.Forms.Padding(0);
            this.kpiIngresos.Name = "kpiIngresos";
            this.kpiReservas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiReservas.Margin = new System.Windows.Forms.Padding(0);
            this.kpiReservas.Name = "kpiReservas";
            this.kpiLlegadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kpiLlegadas.Margin = new System.Windows.Forms.Padding(0);
            this.kpiLlegadas.Name = "kpiLlegadas";
            //
            // tarjetaIngresos
            //
            this.tarjetaIngresos.Controls.Add(this.grafico);
            this.tarjetaIngresos.Controls.Add(this.pnlIngresosEncabezado);
            this.tarjetaIngresos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaIngresos.Margin = new System.Windows.Forms.Padding(0);
            this.tarjetaIngresos.Name = "tarjetaIngresos";
            this.tarjetaIngresos.TabIndex = 1;
            //
            // grafico
            //
            this.grafico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grafico.Name = "grafico";
            this.grafico.TabIndex = 0;
            //
            // pnlIngresosEncabezado
            //
            this.pnlIngresosEncabezado.Controls.Add(this.lblIngresosTotal);
            this.pnlIngresosEncabezado.Controls.Add(this.selector);
            this.pnlIngresosEncabezado.Controls.Add(this.lblIngresosTitulo);
            this.pnlIngresosEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlIngresosEncabezado.Name = "pnlIngresosEncabezado";
            this.pnlIngresosEncabezado.Size = new System.Drawing.Size(600, 54);
            this.pnlIngresosEncabezado.TabIndex = 1;
            //
            // lblIngresosTotal
            //
            this.lblIngresosTotal.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblIngresosTotal.Name = "lblIngresosTotal";
            this.lblIngresosTotal.Size = new System.Drawing.Size(190, 54);
            this.lblIngresosTotal.TabIndex = 2;
            this.lblIngresosTotal.Tag = "personalizada";
            this.lblIngresosTotal.Text = "Q 0.00";
            this.lblIngresosTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // selector
            //
            this.selector.Location = new System.Drawing.Point(104, 8);
            this.selector.Name = "selector";
            this.selector.Size = new System.Drawing.Size(225, 38);
            this.selector.TabIndex = 1;
            this.selector.IndiceCambiado += new System.EventHandler(this.Selector_IndiceCambiado);
            //
            // lblIngresosTitulo
            //
            this.lblIngresosTitulo.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblIngresosTitulo.Name = "lblIngresosTitulo";
            this.lblIngresosTitulo.Size = new System.Drawing.Size(104, 54);
            this.lblIngresosTitulo.TabIndex = 0;
            this.lblIngresosTitulo.Tag = "personalizada";
            this.lblIngresosTitulo.Text = "Ingresos";
            this.lblIngresosTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tblDerecha
            //
            this.tblDerecha.ColumnCount = 1;
            this.tblDerecha.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblDerecha.Controls.Add(this.tarjetaHabitaciones, 0, 0);
            this.tblDerecha.Controls.Add(this.tblCobros, 0, 1);
            this.tblDerecha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDerecha.Margin = new System.Windows.Forms.Padding(0);
            this.tblDerecha.Name = "tblDerecha";
            this.tblDerecha.RowCount = 2;
            this.tblDerecha.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56F));
            this.tblDerecha.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tblDerecha.TabIndex = 2;
            //
            // tarjetaHabitaciones
            //
            this.tarjetaHabitaciones.Controls.Add(this.dona);
            this.tarjetaHabitaciones.Controls.Add(this.lblHabitacionesTitulo);
            this.tarjetaHabitaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaHabitaciones.Margin = new System.Windows.Forms.Padding(0);
            this.tarjetaHabitaciones.Name = "tarjetaHabitaciones";
            this.tarjetaHabitaciones.TabIndex = 0;
            //
            // dona
            //
            this.dona.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dona.Name = "dona";
            this.dona.TabIndex = 0;
            //
            // lblHabitacionesTitulo
            //
            this.lblHabitacionesTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHabitacionesTitulo.Name = "lblHabitacionesTitulo";
            this.lblHabitacionesTitulo.Size = new System.Drawing.Size(300, 38);
            this.lblHabitacionesTitulo.TabIndex = 1;
            this.lblHabitacionesTitulo.Tag = "personalizada";
            this.lblHabitacionesTitulo.Text = "Habitaciones";
            this.lblHabitacionesTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tblCobros
            //
            this.tblCobros.ColumnCount = 2;
            this.tblCobros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblCobros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblCobros.Controls.Add(this.tarjetaCobros, 0, 0);
            this.tblCobros.Controls.Add(this.tarjetaSaldo, 1, 0);
            this.tblCobros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblCobros.Margin = new System.Windows.Forms.Padding(0);
            this.tblCobros.Name = "tblCobros";
            this.tblCobros.RowCount = 1;
            this.tblCobros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblCobros.TabIndex = 1;
            //
            // tarjetaCobros
            //
            this.tarjetaCobros.Controls.Add(this.barPendiente);
            this.tarjetaCobros.Controls.Add(this.barCobrado);
            this.tarjetaCobros.Controls.Add(this.lblCobrosTitulo);
            this.tarjetaCobros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaCobros.Margin = new System.Windows.Forms.Padding(0);
            this.tarjetaCobros.Name = "tarjetaCobros";
            this.tarjetaCobros.TabIndex = 0;
            //
            // barPendiente
            //
            this.barPendiente.ColorBarra = System.Drawing.Color.FromArgb(59, 143, 217);
            this.barPendiente.Dock = System.Windows.Forms.DockStyle.Top;
            this.barPendiente.Etiqueta = "Pendiente";
            this.barPendiente.Name = "barPendiente";
            this.barPendiente.Size = new System.Drawing.Size(200, 50);
            this.barPendiente.TabIndex = 2;
            //
            // barCobrado
            //
            this.barCobrado.Dock = System.Windows.Forms.DockStyle.Top;
            this.barCobrado.Etiqueta = "Cobrado";
            this.barCobrado.Name = "barCobrado";
            this.barCobrado.Size = new System.Drawing.Size(200, 50);
            this.barCobrado.TabIndex = 1;
            //
            // lblCobrosTitulo
            //
            this.lblCobrosTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCobrosTitulo.Name = "lblCobrosTitulo";
            this.lblCobrosTitulo.Size = new System.Drawing.Size(200, 38);
            this.lblCobrosTitulo.TabIndex = 0;
            this.lblCobrosTitulo.Tag = "personalizada";
            this.lblCobrosTitulo.Text = "Cobros";
            this.lblCobrosTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tarjetaSaldo
            //
            this.tarjetaSaldo.Controls.Add(this.lblSaldoValor);
            this.tarjetaSaldo.Controls.Add(this.lblSaldoTitulo);
            this.tarjetaSaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaSaldo.Margin = new System.Windows.Forms.Padding(0);
            this.tarjetaSaldo.Name = "tarjetaSaldo";
            this.tarjetaSaldo.TabIndex = 1;
            //
            // lblSaldoValor
            //
            this.lblSaldoValor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSaldoValor.Name = "lblSaldoValor";
            this.lblSaldoValor.TabIndex = 1;
            this.lblSaldoValor.Tag = "personalizada";
            this.lblSaldoValor.Text = "Q 0.00";
            this.lblSaldoValor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSaldoTitulo
            //
            this.lblSaldoTitulo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSaldoTitulo.Name = "lblSaldoTitulo";
            this.lblSaldoTitulo.Size = new System.Drawing.Size(120, 30);
            this.lblSaldoTitulo.TabIndex = 0;
            this.lblSaldoTitulo.Tag = "personalizada";
            this.lblSaldoTitulo.Text = "Saldo por cobrar";
            this.lblSaldoTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // VistaInicio
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tblPrincipal);
            this.Name = "VistaInicio";
            this.Size = new System.Drawing.Size(1200, 600);
            this.tblPrincipal.ResumeLayout(false);
            this.tblKpi.ResumeLayout(false);
            this.tarjetaIngresos.ResumeLayout(false);
            this.pnlIngresosEncabezado.ResumeLayout(false);
            this.tblDerecha.ResumeLayout(false);
            this.tarjetaHabitaciones.ResumeLayout(false);
            this.tblCobros.ResumeLayout(false);
            this.tarjetaCobros.ResumeLayout(false);
            this.tarjetaSaldo.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblPrincipal;
        private System.Windows.Forms.TableLayoutPanel tblKpi;
        private Hotel.UI.Controles.TarjetaKpi kpiOcupacion;
        private Hotel.UI.Controles.TarjetaKpi kpiIngresos;
        private Hotel.UI.Controles.TarjetaKpi kpiReservas;
        private Hotel.UI.Controles.TarjetaKpi kpiLlegadas;
        private Hotel.UI.Controles.PanelTarjeta tarjetaIngresos;
        private Hotel.UI.Controles.GraficoBarras grafico;
        private System.Windows.Forms.Panel pnlIngresosEncabezado;
        private System.Windows.Forms.Label lblIngresosTotal;
        private Hotel.UI.Controles.SelectorSegmentado selector;
        private System.Windows.Forms.Label lblIngresosTitulo;
        private System.Windows.Forms.TableLayoutPanel tblDerecha;
        private Hotel.UI.Controles.PanelTarjeta tarjetaHabitaciones;
        private Hotel.UI.Controles.GraficoDona dona;
        private System.Windows.Forms.Label lblHabitacionesTitulo;
        private System.Windows.Forms.TableLayoutPanel tblCobros;
        private Hotel.UI.Controles.PanelTarjeta tarjetaCobros;
        private Hotel.UI.Controles.BarraProgreso barPendiente;
        private Hotel.UI.Controles.BarraProgreso barCobrado;
        private System.Windows.Forms.Label lblCobrosTitulo;
        private Hotel.UI.Controles.PanelTarjeta tarjetaSaldo;
        private System.Windows.Forms.Label lblSaldoValor;
        private System.Windows.Forms.Label lblSaldoTitulo;
    }
}
