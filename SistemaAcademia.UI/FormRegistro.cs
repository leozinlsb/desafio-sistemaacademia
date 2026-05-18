using System;
using System.Windows.Forms;
using SistemaAcademia.Models;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Services;

namespace SistemaAcademia.UI
{
    public partial class FormRegistro : Form
    {
        private AuthBLL _authBLL;
        private ViaCepService _viaCepService;

        public FormRegistro()
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
            _viaCepService = new ViaCepService();
        }

        private async void btnBuscarCep_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCep.Text))
            {
                MessageBox.Show("Por favor, digite um CEP válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnBuscarCep.Enabled = false;
                var endereco = await _viaCepService.BuscarEnderecoPorCepAsync(txtCep.Text);

                if (endereco != null && !endereco.Erro)
                {
                    txtRua.Text = endereco.Rua;
                    txtBairro.Text = endereco.Bairro;
                    txtCidade.Text = endereco.Cidade;
                    txtEstado.Text = endereco.Estado;
                }
                else
                {
                    MessageBox.Show("CEP não existente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar CEP: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscarCep.Enabled = true;
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios (Nome e Senha).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuario = new Usuario
                {
                    Nome = txtNome.Text,
                    Cpf = txtCpf.Text,
                    Telefone = txtTelefone.Text,
                    Email = txtEmail.Text,
                    UsuarioLogin = txtUsuarioLogin.Text,
                    Cep = txtCep.Text,
                    Rua = txtRua.Text,
                    Bairro = txtBairro.Text,
                    Cidade = txtCidade.Text,
                    Estado = txtEstado.Text,
                    IsAdmin = false,
                    DataCadastro = DateTime.Now
                };

                string senha = txtSenha.Text;
                _authBLL.Registrar(usuario, senha);

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha no cadastro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}