using SistemaAcademia.BLL.Auth;
using SistemaAcademia.Models;
using System;
using System.Windows.Forms;

namespace SistemaAcademia.UI
{
    public partial class FormLogin : Form
    {
        private AuthBLL _authBLL;

        public FormLogin()
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validação básica da tela
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha usuário e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AuthBLL authBLL = new AuthBLL();

                // Tenta autenticar.
                Usuario usuarioLogado = authBLL.Autenticar(txtUsuario.Text, txtSenha.Text);

                // Opcional: Se tiver telas diferentes para Admin e Cliente
                if (usuarioLogado.IsAdmin)
                {
                    FormAdmin telaAdmin = new FormAdmin();
                    telaAdmin.Show();
                    this.Hide(); // Esconde a tela de login apenas para admin
                }
                else
                {
                    MessageBox.Show($"Bem-vindo(a), {usuarioLogado.Nome}! Vá para a catraca.", "Acesso Liberado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Aqui você poderia abrir um FormCliente, se tiver.
                    txtUsuario.Text = "";
                    txtSenha.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Falha no Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRegistro telaRegistro = new FormRegistro();
            telaRegistro.ShowDialog();
        }
    }
}
