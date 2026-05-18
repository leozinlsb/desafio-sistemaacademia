# Sistema Academia - Desafio Técnico 🏋️‍♂️

Um sistema desktop de gestão de academia desenvolvido em **C# (Windows Forms)** utilizando a **arquitetura em 3 camadas** (UI, BLL, DAL) com banco de dados **SQL Server**. O projeto inclui controle de acesso de alunos via catraca baseado em pagamento, cadastro com validações Regex, consumo da API ViaCEP e painel administrativo completo.

---

## 🚀 Funcionalidades

- **Autenticação Segura:** Login para administradores e alunos com senhas criptografadas usando Hash SHA-256.
- **Cadastro de Usuários:** Registro completo de alunos com validações Regex para E-mail, Telefone e CEP.
- **Integração ViaCEP:** Preenchimento automático do endereço (Rua, Bairro, Cidade, Estado) a partir do CEP informado, consumindo a API pública [ViaCEP](https://viacep.com.br/).
- **Controle de Catraca:** Alunos com pagamento em dia passam; inadimplentes são bloqueados. Cada tentativa gera um log no banco.
- **Painel Administrativo (Dashboard):**
  - Visão geral de todos os alunos com status de pagamento e último acesso.
  - Registro de pagamentos do mês vigente.
  - Simulação de entrada na catraca.
  - CRUD Completo: Edição e Exclusão de alunos.

---

## 🏗️ Arquitetura em 3 Camadas

O projeto separa rigorosamente as responsabilidades em camadas independentes. A regra de ouro é: **a UI nunca acessa a DAL diretamente** — sempre passa pela BLL.

```
┌─────────────────────────────────────────────────────────────────┐
│                    🖥️  UI (Interface)                           │
│  FormLogin · FormRegistro · FormAdmin · FormEditarAluno         │
│  Responsabilidade: Exibir telas, capturar eventos do usuário    │
├─────────────────────────────────────────────────────────────────┤
│                         ▼ chama ▼                               │
├─────────────────────────────────────────────────────────────────┤
│                ⚙️  BLL (Regras de Negócio)                      │
│  AuthBLL · CatracaBLL · PagamentoBLL · DashboardBLL             │
│  RegexValidator · ViaCepService                                 │
│  Responsabilidade: Validar dados, criptografar senhas,          │
│  aplicar regras, consumir API externa                           │
├─────────────────────────────────────────────────────────────────┤
│                         ▼ chama ▼                               │
├─────────────────────────────────────────────────────────────────┤
│               🗄️  DAL (Acesso a Dados)                         │
│  UsuarioDAL · CatracaDAL · PagamentoDAL · DashboardDAL          │
│  ConnectionHelper                                               │
│  Responsabilidade: Executar comandos SQL no banco de dados       │
├─────────────────────────────────────────────────────────────────┤
│                         ▼ conecta ▼                             │
├─────────────────────────────────────────────────────────────────┤
│              🛢️  SQL Server (AcademiaDB)                        │
│  Tabelas: Usuario · Pagamento · AcessoCatraca                   │
└─────────────────────────────────────────────────────────────────┘

📦 Models (SistemaAcademia.Models) — Usados por TODAS as camadas
   Usuario · Pagamento · AcessoCatraca · EnderecoViaCep
```

### Detalhamento de cada camada

| Camada | Projeto | O que faz |
|--------|---------|-----------|
| **UI** | `SistemaAcademia.UI` | Formulários Windows Forms. Captura cliques de botões, exibe mensagens (MessageBox) e repassa dados para a BLL |
| **BLL** | `SistemaAcademia.BLL` | Valida formatos com Regex (email, telefone, CEP), criptografa senhas com SHA256, verifica regras de catraca, consome API ViaCEP |
| **DAL** | `SistemaAcademia.DAL` | Monta e executa comandos SQL (INSERT, SELECT, UPDATE, DELETE) usando ADO.NET com parâmetros para prevenir SQL Injection |
| **Models** | `SistemaAcademia.Models` | Classes C# que espelham as tabelas do banco. Transportam dados entre as camadas |

---

## 🔄 Fluxo Principal do Sistema

### 1. Login do Aluno (Catraca)

```
Aluno abre o programa
       │
       ▼
┌─────────────────┐
│   FormLogin     │  ← Digita usuário e senha
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│    AuthBLL      │  ← Criptografa a senha digitada com SHA256
│  .Autenticar()  │  ← Busca o usuário no banco pelo login
│                 │  ← Compara os hashes (banco vs digitado)
└────────┬────────┘
         │
         ▼ IsAdmin = false (é aluno)
┌─────────────────┐
│   CatracaBLL    │  ← Verifica se existe pagamento no mês atual
│ .VerificarAcesso│  ← Registra o log de tentativa no banco
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
    ▼         ▼
 ✅ Pago    ❌ Sem Pagamento
"Catraca   "Acesso barrado.
liberada!   Regularize o
Bom treino!" pagamento."
```

### 2. Login do Admin (Painel)

```
Admin abre o programa
       │
       ▼
┌─────────────────┐
│   FormLogin     │  ← Digita "admin" + senha
└────────┬────────┘
         │
         ▼ IsAdmin = true (é admin)
┌─────────────────────────────────────────────┐
│              FormAdmin                       │
│                                              │
│  ┌────────────────────────────────────────┐  │
│  │  DataGridView (tabela com alunos)      │  │
│  │  Id | Nome | CPF | Status | Últ.Acesso │  │
│  └────────────────────────────────────────┘  │
│                                              │
│  [Registrar Pagamento] [Simular Catraca]     │
│  [Editar Aluno]        [Excluir Aluno]       │
│  [Voltar ao Login]                           │
└──────────────────────────────────────────────┘
```

### 3. Cadastro de Novo Aluno

```
Tela de Login → clica "Cadastre-se"
       │
       ▼
┌───────────────────┐
│   FormRegistro    │  ← Preenche: Nome, CPF, Telefone, Email,
│                   │     Login, Senha, CEP
│  [Buscar CEP] ────┼──→ API ViaCEP → Preenche Rua, Bairro,
│                   │                  Cidade, Estado
│  [Cadastrar] ─────┼──→ BLL valida Regex → Criptografa senha
│                   │    → DAL salva no banco
└───────────────────┘
```

---

## 📂 Estrutura de Arquivos

```
SistemaAcademia/
│
├── Database/
│   └── CreateTables.sql          ← Script SQL: cria tabelas + admin padrão
│
├── SistemaAcademia.Models/       ← 📦 Classes que representam as tabelas
│   ├── Usuario.cs                   Alunos e administradores
│   ├── Pagamento.cs                 Pagamentos de mensalidade
│   ├── AcessoCatraca.cs             Registros de passagem na catraca
│   └── EnderecoViaCep.cs            Resposta da API ViaCEP
│
├── SistemaAcademia.DAL/          ← 🗄️ Acesso ao banco de dados
│   ├── ConnectionHelper.cs          Centraliza a connection string
│   ├── UsuarioDAL.cs                CRUD de usuários (INSERT, SELECT, UPDATE, DELETE)
│   ├── CatracaDAL.cs                Log de acessos + verificação de pagamento por CPF
│   ├── PagamentoDAL.cs              Inserção e consulta de pagamentos
│   └── DashboardDAL.cs              Query com LEFT JOIN para a grid do admin
│
├── SistemaAcademia.BLL/          ← ⚙️ Regras de negócio
│   ├── Auth/
│   │   └── AuthBLL.cs               Login, registro, edição, exclusão + SHA256
│   ├── Catraca/
│   │   └── CatracaBLL.cs            Lógica de verificação da catraca
│   ├── Pagamento/
│   │   └── PagamentoBLL.cs          Registro de pagamento do mês vigente
│   ├── Services/
│   │   └── ViaCepService.cs         Consulta HTTP à API ViaCEP
│   ├── Validations/
│   │   └── RegexValidator.cs        Validação de email, telefone e CEP com Regex
│   └── DashboardBLL.cs              Ponte entre UI e DAL do dashboard
│
├── SistemaAcademia.UI/           ← 🖥️ Telas (Windows Forms)
│   ├── Program.cs                   Ponto de entrada da aplicação
│   ├── FormLogin.cs                 Tela de login (primeira tela)
│   ├── FormRegistro.cs              Tela de cadastro de novos alunos
│   ├── FormAdmin.cs                 Painel administrativo com grid
│   └── FormEditarAluno.cs           Tela de edição de dados do aluno
│
└── SistemaAcademia.slnx          ← Arquivo da solução (abre no Visual Studio)
```

---

## 🗄️ Banco de Dados

O sistema utiliza **3 tabelas** no banco **AcademiaDB**:

### Tabela `Usuario`
Armazena alunos e administradores. A diferença é o campo `IsAdmin` (0 = aluno, 1 = admin).

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | INT (PK, auto-incremento) | Identificador único |
| UsuarioLogin | VARCHAR(50) UNIQUE | Nome de login |
| SenhaHash | NVARCHAR(256) | Hash SHA256 da senha |
| Nome | VARCHAR(150) | Nome completo |
| Cpf | VARCHAR(14) UNIQUE | CPF com máscara |
| Telefone | VARCHAR(15) | Telefone com máscara |
| Email | VARCHAR(150) UNIQUE | E-mail |
| Cep, Rua, Bairro, Cidade, Estado | VARCHAR | Endereço (preenchido via ViaCEP) |
| IsAdmin | BIT | 0 = Aluno, 1 = Admin |
| DataCadastro | DATETIME | Data de criação |

### Tabela `Pagamento`
Registra pagamentos mensais. Vinculada ao `Usuario` por chave estrangeira com `ON DELETE CASCADE`.

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | INT (PK) | Identificador único |
| UsuarioId | INT (FK → Usuario) | Aluno que pagou |
| DataPagamento | DATETIME | Data do pagamento |
| Valor | DECIMAL(10,2) | Valor em reais |
| MesReferencia | INT | Mês do pagamento (1-12) |
| AnoReferencia | INT | Ano do pagamento |

### Tabela `AcessoCatraca`
Log de todas as tentativas de passagem na catraca. Também com `ON DELETE CASCADE`.

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | INT (PK) | Identificador único |
| UsuarioId | INT (FK → Usuario) | Aluno que tentou acessar |
| DataAcesso | DATETIME | Data/hora da tentativa |
| Liberado | BIT | 1 = Liberado, 0 = Bloqueado |
| MotivoBloqueio | VARCHAR(100) NULL | Motivo do bloqueio (se houver) |

### Relacionamento entre tabelas

```
┌──────────────┐       ┌──────────────┐
│   Usuario    │──1:N──│  Pagamento   │
│              │       │              │
│  Id (PK)     │◄──────│ UsuarioId(FK)│
│  Nome        │       │ Valor        │
│  Cpf         │       │ MesReferencia│
│  IsAdmin     │       └──────────────┘
│  SenhaHash   │
│  ...         │       ┌──────────────┐
│              │──1:N──│AcessoCatraca │
│              │       │              │
│              │◄──────│ UsuarioId(FK)│
└──────────────┘       │ Liberado     │
                       │ DataAcesso   │
                       └──────────────┘
```

> **ON DELETE CASCADE:** Quando um aluno é excluído, todos os seus pagamentos e logs de catraca são apagados automaticamente pelo banco.

---

## 🔐 Segurança

- **Senhas nunca são salvas em texto puro.** O sistema usa o algoritmo **SHA-256** para gerar um hash irreversível da senha antes de salvar no banco.
- **Parâmetros SQL (@Nome, @Cpf, etc.)** são usados em todos os comandos SQL para prevenir **SQL Injection**.
- **Validação com Regex** garante que e-mail, telefone e CEP estejam no formato correto antes de salvar.

---

## 💻 Tecnologias Utilizadas

| Tecnologia | Uso |
|------------|-----|
| C# (.NET) | Linguagem principal |
| Windows Forms | Interface gráfica desktop |
| SQL Server (LocalDB) | Banco de dados relacional |
| ADO.NET (Microsoft.Data.SqlClient) | Acesso ao banco via código |
| API ViaCEP | Consulta de endereço por CEP |
| SHA-256 | Criptografia de senhas |
| Regex | Validação de formatos (email, telefone, CEP) |

---

## 🛠️ Como Executar o Projeto

### Pré-requisitos
- **Visual Studio 2022** (ou superior) com o workload "Desenvolvimento para desktop com .NET".
- **SQL Server** (LocalDB ou instância completa).

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/leozinlsb/desafio-sistemaacademia.git
   ```

2. **Configure o Banco de Dados:**
   - Abra o SQL Server Management Studio (SSMS).
   - Crie o banco de dados `AcademiaDB` (ou descomente a linha no script).
   - Execute o script `Database/CreateTables.sql`. Ele criará as 3 tabelas e o usuário admin padrão.

3. **Configure a Connection String:**
   - Abra o arquivo `SistemaAcademia.DAL/ConnectionHelper.cs`.
   - Ajuste a string de conexão para o seu servidor SQL local:
     ```csharp
     "Server=SEU_SERVIDOR;Database=AcademiaDB;Integrated Security=True;TrustServerCertificate=True;"
     ```

4. **Execute a Aplicação:**
   - Abra `SistemaAcademia.slnx` no Visual Studio.
   - Defina `SistemaAcademia.UI` como Projeto de Inicialização.
   - Pressione `F5` para rodar.

### Primeiro Acesso (Admin padrão)
O script SQL já cria um administrador para você acessar o painel:
- **Login:** `admin`
- **Senha:** `admin123`

---
