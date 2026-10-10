namespace Hotel.UI.Formularios
{
    partial class FormEntradaTexto
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
            this.lblEtiqueta = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.pnlContenido.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlContenido
            //
            this.pnlContenido.Controls.Add(this.lblEtiqueta);
            this.pnlContenido.Controls.Add(this.txtValor);
            //
            // lblEtiqueta
            //
            this.lblEtiqueta.Location = new System.Drawing.Point(24, 4);
            this.lblEtiqueta.Name = "lblEtiqueta";
            this.lblEtiqueta.Size = new System.Drawing.Size(410, 44);
            this.lblEtiqueta.Text = "Motivo";
            //
            // txtValor
            //
            this.txtValor.Location = new System.Drawing.Point(24, 52);
            this.txtValor.Multiline = true;
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(410, 90);
            this.txtValor.TabIndex = 0;
            //
            // FormEntradaTexto
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(460, 290);
            this.Name = "FormEntradaTexto";
            this.Text = "Entrada de texto";
            this.pnlContenido.ResumeLayout(false);
            this.pnlContenido.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblEtiqueta;
        private System.Windows.Forms.TextBox txtValor;
    }
}
