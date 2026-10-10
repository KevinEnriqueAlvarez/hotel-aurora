namespace Hotel.UI.Formularios
{
    partial class FormUsuario
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
            this.lblNombres = new System.Windows.Forms.Label();
            this.txtNombres = new System.Windows.Forms.TextBox();
            this.lblApellidos = new System.Windows.Forms.Label();
            this.txtApellidos = new System.Windows.Forms.TextBox();
            this.cmbTipoDocumento = new System.Windows.Forms.ComboBox();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblRol = new System.Windows.Forms.Label();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.lblClave = new System.Windows.Forms.Label();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblNombres);
            this.pnlContenido.Controls.Add(this.txtNombres);
            this.pnlContenido.Controls.Add(this.lblApellidos);
            this.pnlContenido.Controls.Add(this.txtApellidos);
            this.pnlContenido.Controls.Add(this.cmbTipoDocumento);
            this.pnlContenido.Controls.Add(this.lblDocumento);
            this.pnlContenido.Controls.Add(this.txtDocumento);
            this.pnlContenido.Controls.Add(this.lblTelefono);
            this.pnlContenido.Controls.Add(this.txtTelefono);
            this.pnlContenido.Controls.Add(this.lblCorreo);
            this.pnlContenido.Controls.Add(this.txtCorreo);
            this.pnlContenido.Controls.Add(this.lblUsuario);
            this.pnlContenido.Controls.Add(this.txtUsuario);
            this.pnlContenido.Controls.Add(this.lblRol);
            this.pnlContenido.Controls.Add(this.cmbRol);
            this.pnlContenido.Controls.Add(this.lblClave);
            this.pnlContenido.Controls.Add(this.txtClave);
            this.pnlContenido.Controls.Add(this.chkActivo);
            //
            // fila 1
            //
            this.lblNombres.AutoSize = true;
            this.lblNombres.Location = new System.Drawing.Point(24, 4);
            this.lblNombres.Name = "lblNombres";
            this.lblNombres.Text = "Nombres";
            this.txtNombres.Location = new System.Drawing.Point(24, 26);
            this.txtNombres.MaxLength = 60;
            this.txtNombres.Name = "txtNombres";
            this.txtNombres.Size = new System.Drawing.Size(215, 27);
            this.txtNombres.TabIndex = 0;
            this.lblApellidos.AutoSize = true;
            this.lblApellidos.Location = new System.Drawing.Point(259, 4);
            this.lblApellidos.Name = "lblApellidos";
            this.lblApellidos.Text = "Apellidos";
            this.txtApellidos.Location = new System.Drawing.Point(259, 26);
            this.txtApellidos.MaxLength = 60;
            this.txtApellidos.Name = "txtApellidos";
            this.txtApellidos.Size = new System.Drawing.Size(215, 27);
            this.txtApellidos.TabIndex = 1;
            //
            // fila 2
            //
            this.cmbTipoDocumento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDocumento.Location = new System.Drawing.Point(24, 88);
            this.cmbTipoDocumento.Name = "cmbTipoDocumento";
            this.cmbTipoDocumento.Size = new System.Drawing.Size(130, 28);
            this.cmbTipoDocumento.TabIndex = 2;
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Location = new System.Drawing.Point(24, 66);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Text = "Documento";
            this.txtDocumento.Location = new System.Drawing.Point(174, 88);
            this.txtDocumento.MaxLength = 20;
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(300, 27);
            this.txtDocumento.TabIndex = 3;
            //
            // fila 3
            //
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.Location = new System.Drawing.Point(24, 128);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Text = "Teléfono";
            this.txtTelefono.Location = new System.Drawing.Point(24, 150);
            this.txtTelefono.MaxLength = 16;
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(215, 27);
            this.txtTelefono.TabIndex = 4;
            this.lblCorreo.AutoSize = true;
            this.lblCorreo.Location = new System.Drawing.Point(259, 128);
            this.lblCorreo.Name = "lblCorreo";
            this.lblCorreo.Text = "Correo electrónico";
            this.txtCorreo.Location = new System.Drawing.Point(259, 150);
            this.txtCorreo.Name = "txtCorreo";
            this.txtCorreo.Size = new System.Drawing.Size(215, 27);
            this.txtCorreo.TabIndex = 5;
            //
            // fila 4
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(24, 190);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Text = "Nombre de usuario";
            this.txtUsuario.Location = new System.Drawing.Point(24, 212);
            this.txtUsuario.MaxLength = 30;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(215, 27);
            this.txtUsuario.TabIndex = 6;
            this.lblRol.AutoSize = true;
            this.lblRol.Location = new System.Drawing.Point(259, 190);
            this.lblRol.Name = "lblRol";
            this.lblRol.Text = "Rol";
            this.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRol.Location = new System.Drawing.Point(259, 212);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(215, 28);
            this.cmbRol.TabIndex = 7;
            //
            // fila 5
            //
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new System.Drawing.Point(24, 252);
            this.lblClave.Name = "lblClave";
            this.lblClave.Text = "Contraseña";
            this.txtClave.Location = new System.Drawing.Point(24, 274);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(215, 27);
            this.txtClave.TabIndex = 8;
            this.txtClave.UseSystemPasswordChar = true;
            this.chkActivo.AutoSize = true;
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivo.Location = new System.Drawing.Point(24, 276);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.TabIndex = 9;
            this.chkActivo.Text = "Usuario activo";
            //
            // FormUsuario
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 500);
            this.Name = "FormUsuario";
            this.Text = "Usuario";
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.TextBox txtApellidos;
        private System.Windows.Forms.ComboBox cmbTipoDocumento;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.CheckBox chkActivo;
    }
}
