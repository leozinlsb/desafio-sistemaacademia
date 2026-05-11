using SistemaAcademia.BLL;
using SistemaAcademia.BLL.Auth;
using SistemaAcademia.BLL.Catraca;
using SistemaAcademia.BLL.Pagamento;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaAcademia.UI
{
    public partial class FormAdmin : Form
    {
        private PagamentoBLL _pagamentoBLL;
        private CatracaBLL _catracaBLL;
        private DashboardBLL _dashboardBLL; // Adicionado para manter a arquitetura

        public FormAdmin()
        {
            InitializeComponent();
            _pagamentoBLL = new PagamentoBLL();
            _catracaBLL = new CatracaBLL();
            _dashboardBLL = new DashboardBLL();
        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            try
            {
                // A UI pede para a BLL, respeitando as 3 camadas
                dgvAlunos.DataSource = _dashboardBLL.ListarVisaoGeralAlunos();

                // Estilização opcional para a grid não ficar "espremida"
                dgvAlunos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvAlunos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAlunos.MultiSelect = false;
                dgvAlunos.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados dos alunos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            if (dgvAlunos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um aluno na grid primeiro clicando na linha inteira.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int usuarioId = Convert.ToInt32(dgvAlunos.SelectedRows[0].Cells["Id"].Value);
            string nome = dgvAlunos.SelectedRows[0].Cells["Nome"].Value.ToString();
            string status = dgvAlunos.SelectedRows[0].Cells["StatusMesAtual"].Value.ToString();

            if (status == "Pago")
            {
                MessageBox.Show($"O aluno {nome} já consta como pago para o mês atual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show($"Deseja registrar o pagamento do mês atual para {nome} no valor de R$ 100,00?",
                                     "Confirmar Pagamento", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    _pagamentoBLL.RegistrarPagamentoMesVigente(usuarioId, 100m);
                    MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarDados(); // Recarrega a grid para mostrar a nova data
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

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
                bool liberado = _catracaBLL.VerificarAcessoCatraca(cpf);
                if (liberado)
                {
                    MessageBox.Show($"Catraca LIBERADA para {nome}! O log de acesso foi salvo no banco.", "Catraca", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"Catraca BLOQUEADA para {nome}. Motivo: Inadimplência do mês atual.", "Catraca", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                CarregarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na catraca: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
                AuthBLL bll = new AuthBLL();
                var usuario = bll.ObterPorId(idSelecionado);
                if (usuario != null)
                {
                    FormEditarAluno frm = new FormEditarAluno(usuario);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        CarregarDados();
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

            // Pergunta de segurança para não apagar sem querer
            var confirmacao = MessageBox.Show($"Tem certeza que deseja excluir permanentemente o aluno {nomeSelecionado}?",
                                              "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    // O ideal seria instanciar isso no construtor do Form, como fizemos com as outras BLLs
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
        private void btnVoltarLogin_Click(object sender, EventArgs e)
        {
            // Oculta o FormAdmin e mostra as instâncias abertas de FormLogin
            this.Close();
            foreach (Form form in Application.OpenForms)
            {
                if (form is FormLogin)
                {
                    form.Show();
                    break;
                }
            }
        }
    }
}