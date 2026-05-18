using SistemaAcademia.BLL;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Catraca;
using SistemaAcademia.BLL.Pagamento;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaAcademia.UI
{
    /// <summary>
    /// TELA ADMINISTRATIVA (PAINEL DO ADMIN).
    /// 
    /// É a tela principal do administrador da academia. Exibe uma DataGridView (tabela)
    /// com todos os alunos cadastrados e permite:
    ///   - Registrar pagamento de mensalidade
    ///   - Simular passagem na catraca
    ///   - Editar dados de um aluno
    ///   - Excluir um aluno
    ///   - Voltar para a tela de login
    /// 
    /// A grid mostra: Id, Nome, CPF, Telefone, Status do Mês (Pago/Sem Pagamento)
    /// e Último Acesso na catraca.
    /// 
    /// Acessada quando: um usuário com IsAdmin = true faz login no FormLogin.
    /// </summary>
    public partial class FormAdmin : Form
    {
        // Instâncias das BLLs necessárias para as operações desta tela
        private PagamentoBLL _pagamentoBLL;     // Para registrar pagamentos
        private CatracaBLL _catracaBLL;         // Para simular a catraca
        private DashboardBLL _dashboardBLL;     // Para carregar os dados da grid

        /// <summary>
        /// Construtor: Inicializa componentes visuais e cria as instâncias das BLLs.
        /// </summary>
        public FormAdmin()
        {
            InitializeComponent();
            _pagamentoBLL = new PagamentoBLL();
            _catracaBLL = new CatracaBLL();
            _dashboardBLL = new DashboardBLL();
        }

        /// <summary>
        /// EVENTO: Disparado automaticamente quando o formulário é CARREGADO pela primeira vez.
        /// Chama CarregarDados() para preencher a grid com os alunos.
        /// </summary>
        private void FormAdmin_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        /// <summary>
        /// MÉTODO AUXILIAR: Carrega (ou recarrega) os dados dos alunos na DataGridView.
        /// 
        /// É chamado:
        ///   - No FormAdmin_Load (abertura da tela)
        ///   - Após registrar pagamento (para atualizar o status)
        ///   - Após simular catraca (para atualizar o último acesso)
        ///   - Após editar ou excluir um aluno
        /// 
        /// Fluxo: UI → DashboardBLL → DashboardDAL → SQL Server → DataTable → DataGridView
        /// </summary>
        private void CarregarDados()
        {
            try
            {
                // Pede para a BLL buscar os dados (respeitando a arquitetura em 3 camadas)
                // O retorno é um DataTable que é atribuído diretamente como DataSource da grid
                dgvAlunos.DataSource = _dashboardBLL.ListarVisaoGeralAlunos();

                // Configurações visuais da DataGridView
                dgvAlunos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // Colunas preenchem toda a largura
                dgvAlunos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;     // Seleciona a linha inteira ao clicar
                dgvAlunos.MultiSelect = false;    // Permite selecionar apenas 1 linha por vez
                dgvAlunos.ReadOnly = true;        // Impede edição direta na grid (só pelo botão Editar)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados dos alunos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Registrar Pagamento".
        /// 
        /// Registra o pagamento da mensalidade do mês atual para o aluno selecionado na grid.
        /// O valor é fixo em R$ 100,00.
        /// 
        /// Fluxo:
        ///   1. Verifica se tem um aluno selecionado na grid
        ///   2. Verifica se o aluno já está pago (evita duplicação)
        ///   3. Pede confirmação ao admin
        ///   4. Chama PagamentoBLL para registrar no banco
        ///   5. Recarrega a grid para mostrar o status atualizado
        /// </summary>
        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            // Verifica se o admin selecionou algum aluno na grid
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno na grid primeiro clicando na linha inteira.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pega o Id e Nome do aluno selecionado na grid
            // Cells["NomeColuna"].Value acessa o valor de uma coluna específica da linha selecionada
            int usuarioId = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
            string nome = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString();
            string status = dgvAlunos.SelectedRows[0].Cells["StatusMesAtual"].Value.ToString();

            // Se já está pago, avisa e não registra novamente (evita duplicação)
            if (status == "Pago")
            {
                MessageBox.Show($"O aluno {nome} já consta como pago para o mês atual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Pede confirmação antes de registrar o pagamento
            var confirmResult = MessageBox.Show($"Deseja registrar o pagamento do mês atual para {nome} no valor de R$ 100,00?",
                                     "Confirmar Pagamento", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    // Registra o pagamento via BLL → DAL → banco de dados
                    // 100m = 100 em decimal (o "m" indica tipo decimal no C#)
                    _pagamentoBLL.RegistrarPagamentoMesVigente(usuarioId, 100m);
                    MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recarrega a grid para mostrar "Pago" no StatusMesAtual
                    CarregarDados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Simular Catraca".
        /// 
        /// Simula a passagem de um aluno pela catraca da academia.
        /// Verifica se o aluno tem pagamento no mês e registra o log de acesso.
        /// 
        /// Fluxo:
        ///   1. Verifica se tem um aluno selecionado
        ///   2. Pega o CPF e Nome do aluno
        ///   3. Chama CatracaBLL para verificar pagamento e registrar log
        ///   4. Mostra mensagem de LIBERADA ou BLOQUEADA
        ///   5. Recarrega a grid (atualiza UltimoAcesso)
        /// </summary>
        private void btnSimularCatraca_Click(object sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno na grid para simular a catraca.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cpf = dgvAlunos.SelectedRows[0].Cells["Cpf"].Value.ToString();
            string nome = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString();

            try
            {
                // Verifica pagamento e registra log de acesso no banco
                bool liberado = _catracaBLL.VerificarAcessoCatraca(cpf);
                if (liberado)
                {
                    MessageBox.Show($"Catraca LIBERADA para {nome}! O log de acesso foi salvo no banco.", "Catraca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Catraca BLOQUEADA para {nome}. Motivo: Inadimplência do mês atual.", "Catraca", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // Recarrega para mostrar a data do último acesso atualizada
                CarregarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na catraca: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Editar".
        /// 
        /// Abre a tela de edição (FormEditarAluno) com os dados do aluno selecionado.
        /// 
        /// Fluxo:
        ///   1. Verifica se tem aluno selecionado
        ///   2. Busca os dados completos do aluno pelo Id (via AuthBLL → UsuarioDAL)
        ///   3. Abre FormEditarAluno passando o objeto Usuario como parâmetro
        ///   4. ShowDialog(): Aguarda o formulário ser fechado
        ///   5. Se o resultado foi OK (salvo com sucesso) → recarrega a grid
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno na grid primeiro clicando na linha inteira.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idSelecionado = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);

            try
            {
                // Busca os dados completos do aluno no banco pelo Id
                AuthBLL bll = new AuthBLL();
                var usuario = bll.ObterPorId(idSelecionado);
                if (usuario != null)
                {
                    // Cria o formulário de edição passando o usuário como parâmetro
                    FormEditarAluno frm = new FormEditarAluno(usuario);

                    // ShowDialog: Abre como modal (trava a tela admin até fechar)
                    // Se o formulário retornou DialogResult.OK, significa que salvou com sucesso
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        CarregarDados(); // Recarrega a grid com os dados atualizados
                    }
                }
                else
                {
                    MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar edição: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Excluir".
        /// 
        /// Exclui permanentemente o aluno selecionado do banco de dados.
        /// Por causa do ON DELETE CASCADE nas tabelas, os pagamentos e logs
        /// de catraca do aluno também são apagados automaticamente.
        /// 
        /// Fluxo:
        ///   1. Verifica se tem aluno selecionado
        ///   2. Pede CONFIRMAÇÃO ao admin (para evitar exclusões acidentais)
        ///   3. Chama AuthBLL.ExcluirUsuario() → UsuarioDAL.Excluir()
        ///   4. Recarrega a grid (o aluno desaparece da lista)
        /// </summary>
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            // Verifica se tem alguma linha selecionada na Grid
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno na lista para excluir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Pega o ID e o Nome da linha selecionada
            int idSelecionado = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
            string nomeSelecionado = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString();

            // Pergunta de segurança: MessageBoxIcon.Stop mostra um ícone vermelho de alerta
            var confirmacao = MessageBox.Show($"Tem certeza que deseja excluir permanentemente o aluno {nomeSelecionado}?",
                                              "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    AuthBLL bll = new AuthBLL();
                    bll.ExcluirUsuario(idSelecionado);

                    MessageBox.Show("Aluno excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Recarrega a Grid para o aluno sumir da tela
                    CarregarDados();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao excluir: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// EVENTO: BOTÃO "Voltar ao Login".
        /// 
        /// Fecha o painel administrativo e mostra novamente a tela de login.
        /// Procura o FormLogin entre os formulários abertos (ele foi apenas escondido
        /// com Hide() no momento do login do admin).
        /// </summary>
        private void btnVoltarLogin_Click(object sender, EventArgs e)
        {
            // Fecha o FormAdmin
            this.Close();

            // Procura o FormLogin entre os formulários que estão abertos (mas ocultos)
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormLogin)
                {
                    form.Show(); // Mostra a tela de login novamente
                    break;       // Para o loop ao encontrar
                }
            }
        }
    }
}