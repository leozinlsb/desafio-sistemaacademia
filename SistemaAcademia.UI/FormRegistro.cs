using System;
using System.Windows.Forms;
using SistemaAcademia.Models;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Services;

namespace SistemaAcademia.UI
{
    /// <summary>
    /// TELA DE REGISTRO (CADASTRO) DE NOVOS ALUNOS.
    /// 
    /// Permite que um novo aluno se cadastre no sistema preenchendo seus dados pessoais.
    /// Possui integração com a API ViaCEP para preencher o endereço automaticamente
    /// a partir do CEP digitado.
    /// 
    /// Componentes visuais (definidos no FormRegistro.Designer.cs):
    ///   - txtNome, txtCpf, txtTelefone, txtEmail: Dados pessoais
    ///   - txtUsuarioLogin, txtSenha: Credenciais de acesso
    ///   - txtCep, txtRua, txtBairro, txtCidade, txtEstado: Endereço
    ///   - btnBuscarCep: Botão que consulta a API ViaCEP
    ///   - btnCadastrar: Botão que efetiva o cadastro
    /// 
    /// Acessada a partir do: FormLogin (link "Cadastre-se")
    /// </summary>
    public partial class FormRegistro : Form
    {
        // Instâncias das camadas de negócio utilizadas nesta tela
        private AuthBLL _authBLL;              // Para registrar o usuário (validação + gravação)
        private ViaCepService _viaCepService;  // Para buscar endereço pelo CEP via API

        /// <summary>
        /// Construtor: Inicializa componentes visuais e cria as instâncias da BLL e do Serviço.
        /// </summary>
        public FormRegistro()
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
            _viaCepService = new ViaCepService();
        }

        /// <summary>
        /// EVENTO: Disparado quando o usuário CLICA NO BOTÃO "Buscar CEP".
        /// 
        /// Método ASSÍNCRONO (async): Faz uma chamada HTTP para a API ViaCEP
        /// sem travar a interface. Enquanto espera a resposta da internet,
        /// a tela continua responsiva (o usuário pode clicar em outros campos).
        /// 
        /// Fluxo:
        ///   1. Valida se o CEP foi preenchido
        ///   2. Desabilita o botão (evita cliques duplos)
        ///   3. Chama a API ViaCEP de forma assíncrona (await)
        ///   4. Se encontrou → preenche Rua, Bairro, Cidade e Estado automaticamente
        ///   5. Se não encontrou → mostra "CEP não existente"
        ///   6. Reabilita o botão no finally (sempre executa, com ou sem erro)
        /// </summary>
        private async void btnBuscarCep_Click(object sender, EventArgs e)
        {
            // Validação simples: CEP não pode estar vazio
            if (string.IsNullOrWhiteSpace(txtCep.Text))
            {
                MessageBox.Show("Por favor, digite um CEP válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Desabilita o botão para evitar múltiplos cliques enquanto busca
                btnBuscarCep.Enabled = false;

                // Chama o serviço ViaCEP (requisição HTTP assíncrona)
                // O "await" pausa este método até a resposta chegar, mas a tela não trava
                var endereco = await _viaCepService.BuscarEnderecoPorCepAsync(txtCep.Text);

                // Verifica se a API retornou dados válidos
                if (endereco != null && !endereco.Erro)
                {
                    // CEP encontrado! Preenche os campos de endereço automaticamente
                    txtRua.Text = endereco.Rua;
                    txtBairro.Text = endereco.Bairro;
                    txtCidade.Text = endereco.Cidade;
                    txtEstado.Text = endereco.Estado;
                }
                else
                {
                    // CEP não existe na base dos Correios
                    MessageBox.Show("CEP não existente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Erro de conexão com a API (sem internet, timeout, etc.)
                MessageBox.Show("Erro ao buscar CEP: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // finally: SEMPRE executa, independente de sucesso ou erro.
                // Reabilita o botão para permitir nova busca.
                btnBuscarCep.Enabled = true;
            }
        }

        /// <summary>
        /// EVENTO: Disparado quando o usuário CLICA NO BOTÃO "Cadastrar".
        /// 
        /// Fluxo:
        ///   1. Valida campos obrigatórios (Nome e Senha)
        ///   2. Monta um objeto Usuario com os dados da tela
        ///   3. Envia para a BLL, que:
        ///      a) Valida os formatos com Regex (email, telefone, CEP)
        ///      b) Criptografa a senha com SHA256
        ///      c) Salva no banco via DAL
        ///   4. Se tudo deu certo → mostra "Cadastro realizado com sucesso!" e fecha a tela
        ///   5. Se deu erro → mostra a mensagem de erro (ex: "E-mail inválido")
        /// </summary>
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            // Validação mínima na UI: Nome e Senha são obrigatórios
            if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha os campos obrigatórios (Nome e Senha).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Monta o objeto Model com todos os dados preenchidos na tela.
                // Este objeto será transportado pela BLL até a DAL para ser salvo no banco.
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
                    IsAdmin = false,         // Novo cadastro é sempre ALUNO (nunca admin)
                    DataCadastro = DateTime.Now
                };

                // Pega a senha em texto puro (será criptografada pela BLL)
                string senha = txtSenha.Text;

                // Envia para a BLL: ela valida (Regex), criptografa (SHA256) e salva (DAL)
                _authBLL.Registrar(usuario, senha);

                MessageBox.Show("Cadastro realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Fecha a tela de registro e volta para o login
            }
            catch (Exception ex)
            {
                // Qualquer erro da BLL (ex: "E-mail inválido", "CEP inválido") cai aqui
                MessageBox.Show("Falha no cadastro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}