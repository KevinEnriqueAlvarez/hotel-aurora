namespace Hotel.UI.Vistas
{
    partial class VistaListadoBase
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
            this.tarjetaFiltros = new Hotel.UI.Controles.PanelTarjeta();
            this.flpFiltros = new System.Windows.Forms.FlowLayoutPanel();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.pnlSeparadorBusqueda = new System.Windows.Forms.Panel();
            this.flpAcciones = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNuevo = new Hotel.UI.Controles.BotonModerno();
            this.btnEditar = new Hotel.UI.Controles.BotonModerno();
            this.btnEliminar = new Hotel.UI.Controles.BotonModerno();
            this.btnDetalle = new Hotel.UI.Controles.BotonModerno();
            this.pnlEspacio = new System.Windows.Forms.Panel();
            this.tarjetaTabla = new Hotel.UI.Controles.PanelTarjeta();
            this.tabla = new System.Windows.Forms.DataGridView();
            this.lblContador = new System.Windows.Forms.Label();
            this.tarjetaFiltros.SuspendLayout();
            this.flpAcciones.SuspendLayout();
            this.tarjetaTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabla)).BeginInit();
            this.SuspendLayout();
            //
            // tarjetaFiltros
            //
            this.tarjetaFiltros.Controls.Add(this.flpFiltros);
            this.tarjetaFiltros.Controls.Add(this.pnlSeparadorBusqueda);
            this.tarjetaFiltros.Controls.Add(this.txtBuscar);
            this.tarjetaFiltros.Controls.Add(this.flpAcciones);
            this.tarjetaFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.tarjetaFiltros.Location = new System.Drawing.Point(0, 0);
            this.tarjetaFiltros.Name = "tarjetaFiltros";
            this.tarjetaFiltros.Padding = new System.Windows.Forms.Padding(18, 14, 18, 12);
            this.tarjetaFiltros.Size = new System.Drawing.Size(900, 120);
            this.tarjetaFiltros.TabIndex = 0;
            //
            // flpFiltros
            //
            this.flpFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpFiltros.Location = new System.Drawing.Point(330, 14);
            this.flpFiltros.Name = "flpFiltros";
            this.flpFiltros.Size = new System.Drawing.Size(552, 36);
            this.flpFiltros.TabIndex = 3;
            this.flpFiltros.WrapContents = false;
            //
            // pnlSeparadorBusqueda
            //
            this.pnlSeparadorBusqueda.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSeparadorBusqueda.Location = new System.Drawing.Point(320, 14);
            this.pnlSeparadorBusqueda.Name = "pnlSeparadorBusqueda";
            this.pnlSeparadorBusqueda.Size = new System.Drawing.Size(10, 36);
            this.pnlSeparadorBusqueda.TabIndex = 2;
            //
            // txtBuscar
            //
            this.txtBuscar.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtBuscar.Location = new System.Drawing.Point(18, 14);
            this.txtBuscar.Margin = new System.Windows.Forms.Padding(0);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.PlaceholderText = "Buscar...";
            this.txtBuscar.Size = new System.Drawing.Size(302, 27);
            this.txtBuscar.TabIndex = 0;
            this.txtBuscar.TextChanged += new System.EventHandler(this.TxtBuscar_TextChanged);
            //
            // flpAcciones
            //
            this.flpAcciones.Controls.Add(this.btnNuevo);
            this.flpAcciones.Controls.Add(this.btnEditar);
            this.flpAcciones.Controls.Add(this.btnEliminar);
            this.flpAcciones.Controls.Add(this.btnDetalle);
            this.flpAcciones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpAcciones.Location = new System.Drawing.Point(18, 56);
            this.flpAcciones.Name = "flpAcciones";
            this.flpAcciones.Size = new System.Drawing.Size(864, 48);
            this.flpAcciones.TabIndex = 1;
            //
            // btnNuevo
            //
            this.btnNuevo.Icono = "";
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(110, 40);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Click += new System.EventHandler(this.BtnNuevo_Click);
            //
            // btnEditar
            //
            this.btnEditar.Icono = "";
            this.btnEditar.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(110, 40);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnEditar.Click += new System.EventHandler(this.BtnEditar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Icono = "";
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(130, 40);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Variante = Hotel.UI.Controles.VarianteBoton.Peligro;
            this.btnEliminar.Click += new System.EventHandler(this.BtnEliminar_Click);
            //
            // btnDetalle
            //
            this.btnDetalle.Icono = "";
            this.btnDetalle.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnDetalle.Name = "btnDetalle";
            this.btnDetalle.Size = new System.Drawing.Size(130, 40);
            this.btnDetalle.TabIndex = 3;
            this.btnDetalle.Text = "Ver detalle";
            this.btnDetalle.Variante = Hotel.UI.Controles.VarianteBoton.Secundario;
            this.btnDetalle.Click += new System.EventHandler(this.BtnDetalle_Click);
            //
            // pnlEspacio
            //
            this.pnlEspacio.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEspacio.Location = new System.Drawing.Point(0, 120);
            this.pnlEspacio.Name = "pnlEspacio";
            this.pnlEspacio.Size = new System.Drawing.Size(900, 8);
            this.pnlEspacio.TabIndex = 1;
            //
            // tarjetaTabla
            //
            this.tarjetaTabla.Controls.Add(this.tabla);
            this.tarjetaTabla.Controls.Add(this.lblContador);
            this.tarjetaTabla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjetaTabla.Location = new System.Drawing.Point(0, 128);
            this.tarjetaTabla.Name = "tarjetaTabla";
            this.tarjetaTabla.Padding = new System.Windows.Forms.Padding(14, 14, 14, 10);
            this.tarjetaTabla.Size = new System.Drawing.Size(900, 372);
            this.tarjetaTabla.TabIndex = 2;
            //
            // tabla
            //
            this.tabla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabla.Location = new System.Drawing.Point(14, 14);
            this.tabla.Name = "tabla";
            this.tabla.Size = new System.Drawing.Size(872, 318);
            this.tabla.TabIndex = 0;
            this.tabla.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Tabla_CellDoubleClick);
            this.tabla.SelectionChanged += new System.EventHandler(this.Tabla_SelectionChanged);
            //
            // lblContador
            //
            this.lblContador.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblContador.Location = new System.Drawing.Point(14, 332);
            this.lblContador.Name = "lblContador";
            this.lblContador.Size = new System.Drawing.Size(872, 30);
            this.lblContador.TabIndex = 1;
            this.lblContador.Text = "0 registros";
            this.lblContador.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // VistaListadoBase
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tarjetaTabla);
            this.Controls.Add(this.pnlEspacio);
            this.Controls.Add(this.tarjetaFiltros);
            this.Name = "VistaListadoBase";
            this.Size = new System.Drawing.Size(900, 500);
            this.tarjetaFiltros.ResumeLayout(false);
            this.tarjetaFiltros.PerformLayout();
            this.flpAcciones.ResumeLayout(false);
            this.tarjetaTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabla)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Hotel.UI.Controles.PanelTarjeta tarjetaFiltros;
        private System.Windows.Forms.FlowLayoutPanel flpFiltros;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Panel pnlSeparadorBusqueda;
        private System.Windows.Forms.FlowLayoutPanel flpAcciones;
        private Hotel.UI.Controles.BotonModerno btnNuevo;
        private Hotel.UI.Controles.BotonModerno btnEditar;
        private Hotel.UI.Controles.BotonModerno btnEliminar;
        private Hotel.UI.Controles.BotonModerno btnDetalle;
        private System.Windows.Forms.Panel pnlEspacio;
        private Hotel.UI.Controles.PanelTarjeta tarjetaTabla;
        private System.Windows.Forms.DataGridView tabla;
        private System.Windows.Forms.Label lblContador;
    }
}
