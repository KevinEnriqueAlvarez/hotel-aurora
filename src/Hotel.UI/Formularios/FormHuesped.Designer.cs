namespace Hotel.UI.Formularios
{
    partial class FormHuesped
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
            this.lblTipoDocumento = new System.Windows.Forms.Label();
            this.cmbTipoDocumento = new System.Windows.Forms.ComboBox();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblCorreo = new System.Windows.Forms.Label();
            this.txtCorreo = new System.Windows.Forms.TextBox();
            this.lblNacionalidad = new System.Windows.Forms.Label();
            this.txtNacionalidad = new System.Windows.Forms.TextBox();
            this.lblNacimiento = new System.Windows.Forms.Label();
            this.dtpNacimiento = new System.Windows.Forms.DateTimePicker();
            this.lblNit = new System.Windows.Forms.Label();
            this.txtNit = new System.Windows.Forms.TextBox();
            this.lblNotas = new System.Windows.Forms.Label();
            this.txtNotas = new System.Windows.Forms.TextBox();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblNombres);
            this.pnlContenido.Controls.Add(this.txtNombres);
            this.pnlContenido.Controls.Add(this.lblApellidos);
            this.pnlContenido.Controls.Add(this.txtApellidos);
            this.pnlContenido.Controls.Add(this.lblTipoDocumento);
            this.pnlContenido.Controls.Add(this.cmbTipoDocumento);
            this.pnlContenido.Controls.Add(this.lblDocumento);
            this.pnlContenido.Controls.Add(this.txtDocumento);
            this.pnlContenido.Controls.Add(this.lblTelefono);
            this.pnlContenido.Controls.Add(this.txtTelefono);
            this.pnlContenido.Controls.Add(this.lblCorreo);
            this.pnlContenido.Controls.Add(this.txtCorreo);
            this.pnlContenido.Controls.Add(this.lblNacionalidad);
            this.pnlContenido.Controls.Add(this.txtNacionalidad);
            this.pnlContenido.Controls.Add(this.lblNacimiento);
            this.pnlContenido.Controls.Add(this.dtpNacimiento);
            this.pnlContenido.Controls.Add(this.lblNit);
            this.pnlContenido.Controls.Add(this.txtNit);
            this.pnlContenido.Controls.Add(this.lblNotas);
            this.pnlContenido.Controls.Add(this.txtNotas);
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
            this.lblTipoDocumento.AutoSize = true;
            this.lblTipoDocumento.Location = new System.Drawing.Point(24, 66);
            this.lblTipoDocumento.Name = "lblTipoDocumento";
            this.lblTipoDocumento.Text = "Documento";
            this.cmbTipoDocumento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoDocumento.Location = new System.Drawing.Point(24, 88);
            this.cmbTipoDocumento.Name = "cmbTipoDocumento";
            this.cmbTipoDocumento.Size = new System.Drawing.Size(130, 28);
            this.cmbTipoDocumento.TabIndex = 2;
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Location = new System.Drawing.Point(174, 66);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Text = "Número de documento";
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
            this.lblNacionalidad.AutoSize = true;
            this.lblNacionalidad.Location = new System.Drawing.Point(24, 190);
            this.lblNacionalidad.Name = "lblNacionalidad";
            this.lblNacionalidad.Text = "Nacionalidad";
            this.txtNacionalidad.Location = new System.Drawing.Point(24, 212);
            this.txtNacionalidad.Name = "txtNacionalidad";
            this.txtNacionalidad.Size = new System.Drawing.Size(215, 27);
            this.txtNacionalidad.TabIndex = 6;
            this.txtNacionalidad.Text = "Guatemalteca";
            this.lblNacimiento.AutoSize = true;
            this.lblNacimiento.Location = new System.Drawing.Point(259, 190);
            this.lblNacimiento.Name = "lblNacimiento";
            this.lblNacimiento.Text = "Fecha de nacimiento";
            this.dtpNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNacimiento.Location = new System.Drawing.Point(259, 212);
            this.dtpNacimiento.Name = "dtpNacimiento";
            this.dtpNacimiento.Size = new System.Drawing.Size(215, 27);
            this.dtpNacimiento.TabIndex = 7;
            //
            // fila 5
            //
            this.lblNit.AutoSize = true;
            this.lblNit.Location = new System.Drawing.Point(24, 252);
            this.lblNit.Name = "lblNit";
            this.lblNit.Text = "NIT (CF = consumidor final)";
            this.txtNit.Location = new System.Drawing.Point(24, 274);
            this.txtNit.MaxLength = 20;
            this.txtNit.Name = "txtNit";
            this.txtNit.Size = new System.Drawing.Size(215, 27);
            this.txtNit.TabIndex = 8;
            this.txtNit.Text = "CF";
            //
            // notas
            //
            this.lblNotas.AutoSize = true;
            this.lblNotas.Location = new System.Drawing.Point(24, 314);
            this.lblNotas.Name = "lblNotas";
            this.lblNotas.Text = "Notas";
            this.txtNotas.Location = new System.Drawing.Point(24, 336);
            this.txtNotas.Multiline = true;
            this.txtNotas.Name = "txtNotas";
            this.txtNotas.Size = new System.Drawing.Size(450, 64);
            this.txtNotas.TabIndex = 9;
            //
            // FormHuesped
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(520, 560);
            this.Name = "FormHuesped";
            this.Text = "Huésped";
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.TextBox txtApellidos;
        private System.Windows.Forms.Label lblTipoDocumento;
        private System.Windows.Forms.ComboBox cmbTipoDocumento;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblCorreo;
        private System.Windows.Forms.TextBox txtCorreo;
        private System.Windows.Forms.Label lblNacionalidad;
        private System.Windows.Forms.TextBox txtNacionalidad;
        private System.Windows.Forms.Label lblNacimiento;
        private System.Windows.Forms.DateTimePicker dtpNacimiento;
        private System.Windows.Forms.Label lblNit;
        private System.Windows.Forms.TextBox txtNit;
        private System.Windows.Forms.Label lblNotas;
        private System.Windows.Forms.TextBox txtNotas;
    }
}
