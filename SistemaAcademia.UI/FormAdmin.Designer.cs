namespace SistemaAcademia.UI
{
    partial class FormAdmin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvAlunos;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnRegistrarPagamento;
        private System.Windows.Forms.Button btnSimularCatraca;
        private System.Windows.Forms.Button btnAtualizar;

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
            dgvAlunos = new DataGridView();
            lblTitulo = new Label();
            btnRegistrarPagamento = new Button();
            btnSimularCatraca = new Button();
            btnAtualizar = new Button();
            btnVoltarLogin = new Button();
            btnEditar = new Button();
            btnExcluir = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvAlunos).BeginInit();
            SuspendLayout();
            // 
            // dgvAlunos
            // 
            dgvAlunos.AllowUserToAddRows = false;
            dgvAlunos.AllowUserToDeleteRows = false;
            dgvAlunos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAlunos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAlunos.Location = new Point(20, 70);
            dgvAlunos.Name = "dgvAlunos";
            dgvAlunos.ReadOnly = true;
            dgvAlunos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAlunos.Size = new Size(740, 320);
            dgvAlunos.TabIndex = 1;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitulo.Location = new Point(20, 20);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(264, 30);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Painel do Administrador";
            // 
            // btnRegistrarPagamento
            // 
            btnRegistrarPagamento.Location = new Point(20, 410);
            btnRegistrarPagamento.Name = "btnRegistrarPagamento";
            btnRegistrarPagamento.Size = new Size(200, 35);
            btnRegistrarPagamento.TabIndex = 2;
            btnRegistrarPagamento.Text = "Registrar Pagamento do Aluno";
            btnRegistrarPagamento.Click += btnRegistrarPagamento_Click;
            // 
            // btnSimularCatraca
            // 
            btnSimularCatraca.Location = new Point(226, 410);
            btnSimularCatraca.Name = "btnSimularCatraca";
            btnSimularCatraca.Size = new Size(200, 35);
            btnSimularCatraca.TabIndex = 3;
            btnSimularCatraca.Text = "Simular Entrada na Catraca";
            btnSimularCatraca.Click += btnSimularCatraca_Click;
            // 
            // btnAtualizar
            // 
            btnAtualizar.Location = new Point(660, 20);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.Size = new Size(100, 30);
            btnAtualizar.TabIndex = 4;
            btnAtualizar.Text = "Atualizar Grid";
            btnAtualizar.Click += FormAdmin_Load;
            // 
            // btnVoltarLogin
            // 
            btnVoltarLogin.Location = new Point(540, 20);
            btnVoltarLogin.Name = "btnVoltarLogin";
            btnVoltarLogin.Size = new Size(110, 30);
            btnVoltarLogin.TabIndex = 7;
            btnVoltarLogin.Text = "Voltar ao Login";
            btnVoltarLogin.Click += btnVoltarLogin_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(432, 410);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(200, 35);
            btnEditar.TabIndex = 5;
            btnEditar.Text = "Editar Aluno";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Location = new Point(638, 410);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.Size = new Size(134, 35);
            btnExcluir.TabIndex = 6;
            btnExcluir.Text = "Excluir Aluno";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // FormAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(btnExcluir);
            Controls.Add(btnEditar);
            Controls.Add(btnAtualizar);
            Controls.Add(btnVoltarLogin);
            Controls.Add(btnSimularCatraca);
            Controls.Add(btnRegistrarPagamento);
            Controls.Add(dgvAlunos);
            Controls.Add(lblTitulo);
            Name = "FormAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vista do Administrador";
            Load += FormAdmin_Load;
            ((System.ComponentModel.ISupportInitialize)dgvAlunos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button btnEditar;
        private Button btnExcluir;
        private Button btnVoltarLogin;
    }
}

