using System;
using System.Windows.Forms;
using SistemaAcademia.Models;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Services;

namespace SistemaAcademia.UI
{
    /// <summary>
    /// TELA DE EDIÇÃO DE ALUNO.
    /// 
    /// Permite ao administrador editar os dados de um aluno já cadastrado.
    /// Recebe um objeto Usuario no construtor com os dados atuais do aluno
    /// e preenche os campos automaticamente para edição.
    /// 
    /// Campos editáveis: Nome, Telefone, Email, CEP, Rua, Bairro, Cidade, Estado
    /// Campos NÃO editáveis: CPF e Login (são exibidos mas não atualizados no banco)
    /// 
    /// Possui integração com a API ViaCEP para atualizar o endereço pelo CEP.
    /// 
    /// Acessada a partir de: FormAdmin (botão "Editar")
    /// </summary>
    public partial class FormEditarAluno : Form
    {
        private AuthBLL _authBLL;              // Para atualizar o aluno no banco
        private ViaCepService _viaCepService;  // Para buscar endereço pelo CEP
        private Usuario _usuarioEdicao;        // Objeto com os dados do aluno sendo editado

        /// <summary>
        /// Construtor: Recebe o objeto Usuario com os dados atuais do aluno.
        /// Inicializa os serviços e preenche os campos da tela com os dados existentes.
        /// </summary>
        /// <param name="usuario">Objeto Usuario com os dados carregados do banco</param>
        public FormEditarAluno(Usuario usuario)
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
            _viaCepService = new ViaCepService();

            // Guarda referência ao objeto do aluno para usar na hora de salvar
            _usuarioEdicao = usuario;

            // Preenche os campos da tela com os dados atuais do aluno
            PreencherCampos();
        }

        /// <summary>
        /// Preenche todos os campos de texto da tela com os dados atuais do aluno.
        /// Chamado pelo construtor após a inicialização.
        /// </summary>
        private void PreencherCampos()
        {
            if (_usuarioEdicao != null)
            {
                txtNome.Text = _usuarioEdicao.Nome;
                txtCpf.Text = _usuarioEdicao.Cpf;
                txtTelefone.Text = _usuarioEdicao.Telefone;
                txtEmail.Text = _usuarioEdicao.Email;
                txtUsuarioLogin.Text = _usuarioEdicao.UsuarioLogin;
                txtCep.Text = _usuarioEdicao.Cep;
                txtRua.Text = _usuarioEdicao.Rua;
                txtBairro.Text = _usuarioEdicao.Bairro;
                txtCidade.Text = _usuarioEdicao.Cidade;
                txtEstado.Text = _usuarioEdicao.Estado;
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Buscar CEP" - Mesmo comportamento do FormRegistro.
        /// Consulta a API ViaCEP e preenche os campos de endereço automaticamente.
        /// </summary>
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

        /// <summary>
        /// EVENTO: BOTÃO "Salvar".
        /// 
        /// Atualiza os dados do aluno no banco de dados.
        /// 
        /// Fluxo:
        ///   1. Valida se o Nome está preenchido
        ///   2. Atualiza o objeto _usuarioEdicao com os novos valores da tela
        ///   3. Chama AuthBLL.Atualizar() que valida com Regex e salva via DAL
        ///   4. Define DialogResult = OK para que o FormAdmin saiba que houve alteração
        ///   5. Fecha a tela
        /// </summary>
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Atualiza o objeto em memória com os novos valores digitados na tela
                _usuarioEdicao.Nome = txtNome.Text;
                _usuarioEdicao.Telefone = txtTelefone.Text;
                _usuarioEdicao.Email = txtEmail.Text;
                _usuarioEdicao.Cep = txtCep.Text;
                _usuarioEdicao.Rua = txtRua.Text;
                _usuarioEdicao.Bairro = txtBairro.Text;
                _usuarioEdicao.Cidade = txtCidade.Text;
                _usuarioEdicao.Estado = txtEstado.Text;

                // Envia para a BLL validar e atualizar no banco
                _authBLL.Atualizar(_usuarioEdicao);

                MessageBox.Show("Cadastro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // DialogResult.OK: Sinaliza para o FormAdmin que a edição foi salva.
                // Quando o FormAdmin verifica "if (frm.ShowDialog() == DialogResult.OK)",
                // ele sabe que precisa recarregar a grid.
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao atualizar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
