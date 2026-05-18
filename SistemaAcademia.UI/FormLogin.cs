using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Catraca;
using SistemaAcademia.Models;
using System;
using System.Windows.Forms;

namespace SistemaAcademia.UI
{
    /// <summary>
    /// TELA DE LOGIN - É a primeira tela que o usuário vê ao abrir o programa.
    /// 
    /// Responsabilidades:
    ///   - Receber login e senha do usuário
    ///   - Chamar a BLL para autenticar (verificar se login/senha estão corretos)
    ///   - Redirecionar conforme o tipo de usuário:
    ///       → Admin (IsAdmin = true): Abre a tela administrativa (FormAdmin)
    ///       → Aluno (IsAdmin = false): Verifica a catraca e mostra mensagem de liberado/bloqueado
    ///   - Permitir navegar para a tela de cadastro (FormRegistro)
    /// 
    /// Componentes visuais (definidos no FormLogin.Designer.cs):
    ///   - txtUsuario: Campo de texto para digitar o login
    ///   - txtSenha: Campo de texto para digitar a senha
    ///   - btnLogin: Botão que executa o login
    ///   - linkCadastro: Link que abre a tela de cadastro
    /// </summary>
    public partial class FormLogin : Form
    {
        // Instância da BLL de autenticação (não usada diretamente, mas mantida como campo)
        private AuthBLL _authBLL;

        /// <summary>
        /// Construtor: Inicializa os componentes visuais e cria a instância da BLL.
        /// InitializeComponent() é gerado automaticamente pelo Designer do Visual Studio
        /// e cria todos os botões, labels, textboxes, etc. definidos visualmente.
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();
            _authBLL = new AuthBLL();
        }

        /// <summary>
        /// EVENTO: Disparado quando o usuário CLICA NO BOTÃO LOGIN.
        /// 
        /// Fluxo completo:
        ///   1. Valida se os campos não estão vazios
        ///   2. Chama AuthBLL.Autenticar() para verificar login e senha no banco
        ///   3. Se o usuário é Admin → abre FormAdmin e esconde o login
        ///   4. Se o usuário é Aluno → verifica catraca:
        ///      - Pagamento em dia → "Catraca liberada. Bom treino!"
        ///      - Pagamento pendente → "Acesso barrado. Regularize o pagamento."
        ///   5. Se der erro (login/senha errados) → mostra mensagem de erro
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validação básica: campos não podem estar vazios
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Preencha usuário e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Para a execução aqui, não continua
            }

            try
            {
                // Cria uma nova instância da BLL de autenticação
                AuthBLL authBLL = new AuthBLL();

                // Tenta autenticar: busca no banco e compara os hashes de senha.
                // Se login ou senha estiverem errados, a BLL lança uma Exception.
                Usuario usuarioLogado = authBLL.Autenticar(txtUsuario.Text, txtSenha.Text);

                // Verifica o tipo de usuário retornado
                if (usuarioLogado.IsAdmin)
                {
                    // É ADMINISTRADOR: Abre a tela administrativa
                    FormAdmin telaAdmin = new FormAdmin();
                    telaAdmin.Show();         // Mostra a tela do admin
                    this.Hide();              // Esconde a tela de login (não fecha, para poder voltar)
                }
                else
                {
                    // É ALUNO: Simula a passagem pela catraca
                    CatracaBLL catracaBLL = new CatracaBLL();

                    // Verifica se o aluno tem pagamento no mês atual e registra o log
                    bool liberado = catracaBLL.VerificarAcessoCatraca(usuarioLogado.Cpf);

                    if (liberado)
                    {
                        // Pagamento em dia → Catraca liberada! ✅
                        MessageBox.Show($"Bem-vindo(a), {usuarioLogado.Nome}! Catraca liberada. Bom treino!", "Acesso Liberado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Sem pagamento → Catraca bloqueada! ❌
                        MessageBox.Show($"Acesso barrado para {usuarioLogado.Nome}. Você precisa regularizar o pagamento com a administração antes de acessar a academia.", "Acesso Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    // Limpa os campos para o próximo aluno usar
                    txtUsuario.Text = "";
                    txtSenha.Text = "";
                    // A tela de login permanece aberta para outro aluno poder logar
                }
            }
            catch (Exception ex)
            {
                // Se a BLL lançou exceção (ex: "Usuário não encontrado" ou "Senha incorreta"),
                // mostra a mensagem de erro em um MessageBox
                MessageBox.Show(ex.Message, "Falha no Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// EVENTO: Disparado quando o usuário CLICA NO LINK "Cadastre-se".
        /// Abre a tela de registro (FormRegistro) como um diálogo modal.
        /// ShowDialog() trava esta tela até o formulário de registro ser fechado.
        /// </summary>
        private void linkCadastro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRegistro telaRegistro = new FormRegistro();
            telaRegistro.ShowDialog(); // Modal: bloqueia a tela de login até fechar o registro
        }
    }
}
