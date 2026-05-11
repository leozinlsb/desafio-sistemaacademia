namespace SistemaAcademia.UI
{
    partial class FormRegistro
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblCpf;
        private System.Windows.Forms.MaskedTextBox txtCpf;
        private System.Windows.Forms.Label lblTelefone;
        private System.Windows.Forms.MaskedTextBox txtTelefone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblUsuarioLogin;
        private System.Windows.Forms.TextBox txtUsuarioLogin;
        private System.Windows.Forms.Label lblSenha;
        private System.Windows.Forms.TextBox txtSenha;
        
        private System.Windows.Forms.Label lblCep;
        private System.Windows.Forms.MaskedTextBox txtCep;
        private System.Windows.Forms.Button btnBuscarCep;
        private System.Windows.Forms.Label lblRua;
        private System.Windows.Forms.TextBox txtRua;
        private System.Windows.Forms.Label lblBairro;
        private System.Windows.Forms.TextBox txtBairro;
        private System.Windows.Forms.Label lblCidade;
        private System.Windows.Forms.TextBox txtCidade;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.TextBox txtEstado;

        
        private System.Windows.Forms.Button btnCadastrar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblCpf = new System.Windows.Forms.Label();
            this.txtCpf = new System.Windows.Forms.MaskedTextBox();
            this.lblTelefone = new System.Windows.Forms.Label();
            this.txtTelefone = new System.Windows.Forms.MaskedTextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblUsuarioLogin = new System.Windows.Forms.Label();
            this.txtUsuarioLogin = new System.Windows.Forms.TextBox();
            this.lblSenha = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.lblCep = new System.Windows.Forms.Label();
            this.txtCep = new System.Windows.Forms.MaskedTextBox();
            this.btnBuscarCep = new System.Windows.Forms.Button();
            this.lblRua = new System.Windows.Forms.Label();
            this.txtRua = new System.Windows.Forms.TextBox();
            this.lblBairro = new System.Windows.Forms.Label();
            this.txtBairro = new System.Windows.Forms.TextBox();
            this.lblCidade = new System.Windows.Forms.Label();
            this.txtCidade = new System.Windows.Forms.TextBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.txtEstado = new System.Windows.Forms.TextBox();

            this.btnCadastrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            
            // Coluna 1
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(20, 20);
            this.lblNome.Name = "lblNome";
            this.lblNome.Text = "Nome:";
            
            this.txtNome.Location = new System.Drawing.Point(20, 40);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(200, 23);

            this.lblCpf.AutoSize = true;
            this.lblCpf.Location = new System.Drawing.Point(20, 70);
            this.lblCpf.Name = "lblCpf";
            this.lblCpf.Text = "CPF:";
            
            this.txtCpf.Location = new System.Drawing.Point(20, 90);
            this.txtCpf.Name = "txtCpf";
            this.txtCpf.Mask = "000\\.000\\.000-00";
            this.txtCpf.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtCpf.Size = new System.Drawing.Size(200, 23);

            this.lblTelefone.AutoSize = true;
            this.lblTelefone.Location = new System.Drawing.Point(20, 120);
            this.lblTelefone.Name = "lblTelefone";
            this.lblTelefone.Text = "Telefone (ex: (11) 99999-9999):";
            
            this.txtTelefone.Location = new System.Drawing.Point(20, 140);
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.Mask = "(00) 00000-0000";
            this.txtTelefone.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtTelefone.Size = new System.Drawing.Size(200, 23);

            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(20, 170);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Text = "E-mail:";
            
            this.txtEmail.Location = new System.Drawing.Point(20, 190);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.MaxLength = 150;
            this.txtEmail.Size = new System.Drawing.Size(200, 23);

            this.lblUsuarioLogin.AutoSize = true;
            this.lblUsuarioLogin.Location = new System.Drawing.Point(20, 220);
            this.lblUsuarioLogin.Name = "lblUsuarioLogin";
            this.lblUsuarioLogin.Text = "Login:";
            
            this.txtUsuarioLogin.Location = new System.Drawing.Point(20, 240);
            this.txtUsuarioLogin.Name = "txtUsuarioLogin";
            this.txtUsuarioLogin.Size = new System.Drawing.Size(200, 23);

            this.lblSenha.AutoSize = true;
            this.lblSenha.Location = new System.Drawing.Point(20, 270);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Text = "Senha:";
            
            this.txtSenha.Location = new System.Drawing.Point(20, 290);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(200, 23);

            // Coluna 2
            this.lblCep.AutoSize = true;
            this.lblCep.Location = new System.Drawing.Point(250, 20);
            this.lblCep.Name = "lblCep";
            this.lblCep.Text = "CEP (ex: 12345-678):";
            
            this.txtCep.Location = new System.Drawing.Point(250, 40);
            this.txtCep.Name = "txtCep";
            this.txtCep.Mask = "00000-000";
            this.txtCep.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludeLiterals;
            this.txtCep.Size = new System.Drawing.Size(100, 23);

            this.btnBuscarCep.Location = new System.Drawing.Point(360, 39);
            this.btnBuscarCep.Name = "btnBuscarCep";
            this.btnBuscarCep.Size = new System.Drawing.Size(90, 25);
            this.btnBuscarCep.Text = "Buscar CEP";
            this.btnBuscarCep.Click += new System.EventHandler(this.btnBuscarCep_Click);

            this.lblRua.AutoSize = true;
            this.lblRua.Location = new System.Drawing.Point(250, 70);
            this.lblRua.Name = "lblRua";
            this.lblRua.Text = "Rua:";
            
            this.txtRua.Location = new System.Drawing.Point(250, 90);
            this.txtRua.Name = "txtRua";
            this.txtRua.Size = new System.Drawing.Size(200, 23);

            this.lblBairro.AutoSize = true;
            this.lblBairro.Location = new System.Drawing.Point(250, 120);
            this.lblBairro.Name = "lblBairro";
            this.lblBairro.Text = "Bairro:";
            
            this.txtBairro.Location = new System.Drawing.Point(250, 140);
            this.txtBairro.Name = "txtBairro";
            this.txtBairro.Size = new System.Drawing.Size(200, 23);

            this.lblCidade.AutoSize = true;
            this.lblCidade.Location = new System.Drawing.Point(250, 170);
            this.lblCidade.Name = "lblCidade";
            this.lblCidade.Text = "Cidade:";
            
            this.txtCidade.Location = new System.Drawing.Point(250, 190);
            this.txtCidade.Name = "txtCidade";
            this.txtCidade.Size = new System.Drawing.Size(200, 23);

            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(250, 220);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Text = "Estado (UF):";
            this.txtEstado.Location = new System.Drawing.Point(250, 240);
            this.txtEstado.Name = "txtEstado";
            this.txtEstado.Size = new System.Drawing.Size(200, 23);


            // Botao Cadastrar
            this.btnCadastrar.Location = new System.Drawing.Point(135, 340);
            this.btnCadastrar.Name = "btnCadastrar";
            this.btnCadastrar.Size = new System.Drawing.Size(200, 40);
            this.btnCadastrar.Text = "CADASTRAR";
            this.btnCadastrar.Click += new System.EventHandler(this.btnCadastrar_Click);

            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 400);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblCpf);
            this.Controls.Add(this.txtCpf);
            this.Controls.Add(this.lblTelefone);
            this.Controls.Add(this.txtTelefone);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblUsuarioLogin);
            this.Controls.Add(this.txtUsuarioLogin);
            this.Controls.Add(this.lblSenha);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.lblCep);
            this.Controls.Add(this.txtCep);
            this.Controls.Add(this.btnBuscarCep);
            this.Controls.Add(this.lblRua);
            this.Controls.Add(this.txtRua);
            this.Controls.Add(this.lblBairro);
            this.Controls.Add(this.txtBairro);
            this.Controls.Add(this.lblCidade);
            this.Controls.Add(this.txtCidade);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.txtEstado);

            this.Controls.Add(this.btnCadastrar);

            this.Name = "FormRegistro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cadastro de Cliente";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
