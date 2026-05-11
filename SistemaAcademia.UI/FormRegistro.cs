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
            // Validação simples de interface
            if (string.IsNullOrWhiteSpace(txtCep.Text))
            {
                MessageBox.Show("Por favor, digite um CEP válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnBuscarCep.Enabled = false;

                // Chama a camada de Serviço (BLL)
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
            // Evita enviar dados completamente em branco para a BLL
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios (Nome e Senha).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Monta o objeto Model com os dados da tela
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

                // Envia para a BLL validar as Regras (Regex) e depois salvar na DAL
                _authBLL.Registrar(usuario, senha);

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Fecha a tela após cadastrar
            }
            catch (Exception ex)
            {
                // Qualquer erro de Regex (ex: "E-mail inválido") lançado pela BLL vai cair aqui
                MessageBox.Show("Falha no cadastro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}