namespace Hotel.UI.Formularios
{
    partial class FormLogin
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
            this.tarjeta = new Hotel.UI.Controles.PanelTarjeta();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblHotel = new System.Windows.Forms.Label();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblClave = new System.Windows.Forms.Label();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.btnMostrar = new Hotel.UI.Controles.BotonModerno();
            this.lblError = new System.Windows.Forms.Label();
            this.btnIngresar = new Hotel.UI.Controles.BotonModerno();
            this.tarjeta.SuspendLayout();
            this.SuspendLayout();
            //
            // tarjeta
            //
            this.tarjeta.Controls.Add(this.lblLogo);
            this.tarjeta.Controls.Add(this.lblHotel);
            this.tarjeta.Controls.Add(this.lblSubtitulo);
            this.tarjeta.Controls.Add(this.lblUsuario);
            this.tarjeta.Controls.Add(this.txtUsuario);
            this.tarjeta.Controls.Add(this.lblClave);
            this.tarjeta.Controls.Add(this.txtClave);
            this.tarjeta.Controls.Add(this.btnMostrar);
            this.tarjeta.Controls.Add(this.lblError);
            this.tarjeta.Controls.Add(this.btnIngresar);
            this.tarjeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tarjeta.Location = new System.Drawing.Point(0, 0);
            this.tarjeta.Name = "tarjeta";
            this.tarjeta.Size = new System.Drawing.Size(404, 481);
            this.tarjeta.TabIndex = 0;
            //
            // lblLogo
            //
            this.lblLogo.Location = new System.Drawing.Point(30, 34);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(344, 52);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblHotel
            //
            this.lblHotel.Location = new System.Drawing.Point(30, 90);
            this.lblHotel.Name = "lblHotel";
            this.lblHotel.Size = new System.Drawing.Size(344, 40);
            this.lblHotel.TabIndex = 1;
            this.lblHotel.Text = "Hotel Aurora";
            this.lblHotel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.Location = new System.Drawing.Point(30, 130);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(344, 24);
            this.lblSubtitulo.TabIndex = 2;
            this.lblSubtitulo.Text = "Sistema de reservas — inicie sesión";
            this.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblUsuario
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(36, 180);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(55, 19);
            this.lblUsuario.TabIndex = 3;
            this.lblUsuario.Text = "Usuario";
            //
            // txtUsuario
            //
            this.txtUsuario.Location = new System.Drawing.Point(36, 204);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(332, 27);
            this.txtUsuario.TabIndex = 0;
            this.txtUsuario.TextChanged += new System.EventHandler(this.Campo_TextChanged);
            //
            // lblClave
            //
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new System.Drawing.Point(36, 244);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(83, 19);
            this.lblClave.TabIndex = 4;
            this.lblClave.Text = "Contraseña";
            //
            // txtClave
            //
            this.txtClave.Location = new System.Drawing.Point(36, 268);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(288, 27);
            this.txtClave.TabIndex = 1;
            this.txtClave.UseSystemPasswordChar = true;
            this.txtClave.TextChanged += new System.EventHandler(this.Campo_TextChanged);
            //
            // btnMostrar
            //
            this.btnMostrar.Icono = "";
            this.btnMostrar.Location = new System.Drawing.Point(330, 263);
            this.btnMostrar.Name = "btnMostrar";
            this.btnMostrar.Size = new System.Drawing.Size(38, 36);
            this.btnMostrar.TabIndex = 2;
            this.btnMostrar.TabStop = false;
            this.btnMostrar.Variante = Hotel.UI.Controles.VarianteBoton.Fantasma;
            this.btnMostrar.Click += new System.EventHandler(this.BtnMostrar_Click);
            //
            // lblError
            //
            this.lblError.Location = new System.Drawing.Point(36, 310);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(332, 44);
            this.lblError.TabIndex = 5;
            //
            // btnIngresar
            //
            this.btnIngresar.Location = new System.Drawing.Point(36, 366);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(332, 46);
            this.btnIngresar.TabIndex = 3;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.Click += new System.EventHandler(this.BtnIngresar_Click);
            //
            // FormLogin
            //
            this.AcceptButton = this.btnIngresar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(404, 481);
            this.Controls.Add(this.tarjeta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hotel Aurora — Iniciar sesión";
            this.tarjeta.ResumeLayout(false);
            this.tarjeta.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Hotel.UI.Controles.PanelTarjeta tarjeta;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblHotel;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.TextBox txtClave;
        private Hotel.UI.Controles.BotonModerno btnMostrar;
        private System.Windows.Forms.Label lblError;
        private Hotel.UI.Controles.BotonModerno btnIngresar;
    }
}
