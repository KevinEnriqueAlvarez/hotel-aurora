namespace Hotel.UI.Formularios
{
    partial class FormPrincipal
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
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblFecha = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlLateral = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new Hotel.UI.Controles.BotonNavegacion();
            this.btnUsuarios = new Hotel.UI.Controles.BotonNavegacion();
            this.btnFacturacion = new Hotel.UI.Controles.BotonNavegacion();
            this.btnRecepcion = new Hotel.UI.Controles.BotonNavegacion();
            this.btnReservas = new Hotel.UI.Controles.BotonNavegacion();
            this.btnHuespedes = new Hotel.UI.Controles.BotonNavegacion();
            this.btnHabitaciones = new Hotel.UI.Controles.BotonNavegacion();
            this.btnInicio = new Hotel.UI.Controles.BotonNavegacion();
            this.lblRol = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblHotel = new System.Windows.Forms.Label();
            this.pnlEncabezado.SuspendLayout();
            this.pnlLateral.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(220, 72);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Padding = new System.Windows.Forms.Padding(20, 0, 20, 20);
            this.pnlContenido.Size = new System.Drawing.Size(1060, 648);
            this.pnlContenido.TabIndex = 0;
            //
            // pnlEncabezado
            //
            this.pnlEncabezado.Controls.Add(this.lblFecha);
            this.pnlEncabezado.Controls.Add(this.lblTitulo);
            this.pnlEncabezado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado.Location = new System.Drawing.Point(220, 0);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Padding = new System.Windows.Forms.Padding(26, 0, 26, 0);
            this.pnlEncabezado.Size = new System.Drawing.Size(1060, 72);
            this.pnlEncabezado.TabIndex = 1;
            //
            // lblFecha
            //
            this.lblFecha.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblFecha.Location = new System.Drawing.Point(726, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(308, 72);
            this.lblFecha.TabIndex = 1;
            this.lblFecha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblTitulo
            //
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Location = new System.Drawing.Point(26, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(1008, 72);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Inicio";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlLateral
            //
            this.pnlLateral.Controls.Add(this.btnCerrarSesion);
            this.pnlLateral.Controls.Add(this.btnUsuarios);
            this.pnlLateral.Controls.Add(this.btnFacturacion);
            this.pnlLateral.Controls.Add(this.btnRecepcion);
            this.pnlLateral.Controls.Add(this.btnReservas);
            this.pnlLateral.Controls.Add(this.btnHuespedes);
            this.pnlLateral.Controls.Add(this.btnHabitaciones);
            this.pnlLateral.Controls.Add(this.btnInicio);
            this.pnlLateral.Controls.Add(this.lblRol);
            this.pnlLateral.Controls.Add(this.lblUsuario);
            this.pnlLateral.Controls.Add(this.lblHotel);
            this.pnlLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLateral.Location = new System.Drawing.Point(0, 0);
            this.pnlLateral.Name = "pnlLateral";
            this.pnlLateral.Size = new System.Drawing.Size(220, 720);
            this.pnlLateral.TabIndex = 2;
            //
            // btnCerrarSesion
            //
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCerrarSesion.Icono = "";
            this.btnCerrarSesion.Location = new System.Drawing.Point(0, 674);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(220, 46);
            this.btnCerrarSesion.TabIndex = 10;
            this.btnCerrarSesion.Text = "Cerrar sesión";
            this.btnCerrarSesion.Click += new System.EventHandler(this.BtnCerrarSesion_Click);
            //
            // btnUsuarios
            //
            this.btnUsuarios.Icono = "";
            this.btnUsuarios.Location = new System.Drawing.Point(0, 408);
            this.btnUsuarios.Name = "btnUsuarios";
            this.btnUsuarios.Size = new System.Drawing.Size(220, 46);
            this.btnUsuarios.TabIndex = 9;
            this.btnUsuarios.Text = "Usuarios";
            this.btnUsuarios.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnFacturacion
            //
            this.btnFacturacion.Icono = "";
            this.btnFacturacion.Location = new System.Drawing.Point(0, 362);
            this.btnFacturacion.Name = "btnFacturacion";
            this.btnFacturacion.Size = new System.Drawing.Size(220, 46);
            this.btnFacturacion.TabIndex = 8;
            this.btnFacturacion.Text = "Facturación";
            this.btnFacturacion.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnRecepcion
            //
            this.btnRecepcion.Icono = "";
            this.btnRecepcion.Location = new System.Drawing.Point(0, 316);
            this.btnRecepcion.Name = "btnRecepcion";
            this.btnRecepcion.Size = new System.Drawing.Size(220, 46);
            this.btnRecepcion.TabIndex = 7;
            this.btnRecepcion.Text = "Recepción";
            this.btnRecepcion.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnReservas
            //
            this.btnReservas.Icono = "";
            this.btnReservas.Location = new System.Drawing.Point(0, 270);
            this.btnReservas.Name = "btnReservas";
            this.btnReservas.Size = new System.Drawing.Size(220, 46);
            this.btnReservas.TabIndex = 6;
            this.btnReservas.Text = "Reservas";
            this.btnReservas.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnHuespedes
            //
            this.btnHuespedes.Icono = "";
            this.btnHuespedes.Location = new System.Drawing.Point(0, 224);
            this.btnHuespedes.Name = "btnHuespedes";
            this.btnHuespedes.Size = new System.Drawing.Size(220, 46);
            this.btnHuespedes.TabIndex = 5;
            this.btnHuespedes.Text = "Huéspedes";
            this.btnHuespedes.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnHabitaciones
            //
            this.btnHabitaciones.Icono = "";
            this.btnHabitaciones.Location = new System.Drawing.Point(0, 178);
            this.btnHabitaciones.Name = "btnHabitaciones";
            this.btnHabitaciones.Size = new System.Drawing.Size(220, 46);
            this.btnHabitaciones.TabIndex = 4;
            this.btnHabitaciones.Text = "Habitaciones";
            this.btnHabitaciones.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // btnInicio
            //
            this.btnInicio.Icono = "";
            this.btnInicio.Location = new System.Drawing.Point(0, 132);
            this.btnInicio.Name = "btnInicio";
            this.btnInicio.Size = new System.Drawing.Size(220, 46);
            this.btnInicio.TabIndex = 3;
            this.btnInicio.Text = "Inicio";
            this.btnInicio.Click += new System.EventHandler(this.BotonNavegacion_Click);
            //
            // lblRol
            //
            this.lblRol.Location = new System.Drawing.Point(22, 90);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(180, 22);
            this.lblRol.TabIndex = 2;
            this.lblRol.Text = "Rol";
            //
            // lblUsuario
            //
            this.lblUsuario.Location = new System.Drawing.Point(22, 62);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(190, 28);
            this.lblUsuario.TabIndex = 1;
            this.lblUsuario.Text = "Usuario";
            //
            // lblHotel
            //
            this.lblHotel.Location = new System.Drawing.Point(22, 20);
            this.lblHotel.Name = "lblHotel";
            this.lblHotel.Size = new System.Drawing.Size(190, 34);
            this.lblHotel.TabIndex = 0;
            this.lblHotel.Text = "Hotel Aurora";
            //
            // FormPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlContenido);
            this.Controls.Add(this.pnlEncabezado);
            this.Controls.Add(this.pnlLateral);
            this.MinimumSize = new System.Drawing.Size(1296, 759);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hotel Aurora";
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlLateral.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlContenido;
        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlLateral;
        private Hotel.UI.Controles.BotonNavegacion btnCerrarSesion;
        private Hotel.UI.Controles.BotonNavegacion btnUsuarios;
        private Hotel.UI.Controles.BotonNavegacion btnFacturacion;
        private Hotel.UI.Controles.BotonNavegacion btnRecepcion;
        private Hotel.UI.Controles.BotonNavegacion btnReservas;
        private Hotel.UI.Controles.BotonNavegacion btnHuespedes;
        private Hotel.UI.Controles.BotonNavegacion btnHabitaciones;
        private Hotel.UI.Controles.BotonNavegacion btnInicio;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblHotel;
    }
}
