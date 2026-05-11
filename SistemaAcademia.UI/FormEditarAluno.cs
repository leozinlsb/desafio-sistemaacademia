using System;
using System.Windows.Forms;
using SistemaAcademia.Models;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Services;

namespace SistemaAcademia.UI
{
    public partial class FormEditarAluno : Form
    {
        private AuthBLL _authBLL;
        private ViaCepService _viaCepService;
        private Usuario _usuarioEdicao;

        public FormEditarAluno(Usuario usuario)
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
            _viaCepService = new ViaCepService();
            _usuarioEdicao = usuario;

            PreencherCampos();
        }

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
                    MessageBox.Show("CEP não encontrado ou inválido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _usuarioEdicao.Nome = txtNome.Text;
                _usuarioEdicao.Telefone = txtTelefone.Text;
                _usuarioEdicao.Email = txtEmail.Text;
                _usuarioEdicao.Cep = txtCep.Text;
                _usuarioEdicao.Rua = txtRua.Text;
                _usuarioEdicao.Bairro = txtBairro.Text;
                _usuarioEdicao.Cidade = txtCidade.Text;
                _usuarioEdicao.Estado = txtEstado.Text;

                _authBLL.Atualizar(_usuarioEdicao);

                MessageBox.Show("Cadastro atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
