using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Catraca;
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
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha usuário e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AuthBLL authBLL = new AuthBLL();
                Usuario usuarioLogado = authBLL.Autenticar(txtUsuario.Text, txtSenha.Text);

                if (usuarioLogado.IsAdmin)
                {
                    FormAdmin telaAdmin = new FormAdmin();
                    telaAdmin.Show();
                    this.Hide();
                }
                else
                {
                    CatracaBLL catracaBLL = new CatracaBLL();
                    bool liberado = catracaBLL.VerificarAcessoCatraca(usuarioLogado.Cpf);

                    if (liberado)
                    {
                        MessageBox.Show($"Bem-vindo(a), {usuarioLogado.Nome}! Catraca liberada. Bom treino!", "Acesso Liberado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Acesso barrado para {usuarioLogado.Nome}. Você precisa regularizar o pagamento com a administração antes de acessar a academia.", "Acesso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

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
