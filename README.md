# Sistema Academia - Desafio Técnico 🏋️‍♂️

Um sistema desktop de gestão de academia desenvolvido em C# (Windows Forms) utilizando a arquitetura em 3 camadas (UI, BLL, DAL). O projeto inclui funcionalidades completas de controle de acesso de alunos (catraca) baseadas em pagamento, registro de usuários, consumo de API externa para preenchimento de endereço e muito mais.

## 🚀 Funcionalidades

- **Autenticação Segura:** Login para administradores e alunos. As senhas são armazenadas de forma segura no banco de dados utilizando Hash e Salt (BCrypt/SHA-256).
- **Cadastro de Usuários:** Cadastro completo de alunos e administradores com validações robustas (Regex) para campos como CPF, E-mail e CEP.
- **Integração ViaCEP:** Preenchimento automático do endereço na tela de registro a partir do CEP informado.
- **Painel do Administrador (Dashboard):**
  - Visão geral de todos os alunos cadastrados.
  - Registro de pagamentos do mês vigente para os alunos.
  - Simulação de entrada na catraca, que verifica automaticamente a adimplência do aluno no mês atual e libera ou bloqueia a entrada, gerando um log no banco de dados.
  - CRUD Completo: Edição e Exclusão de alunos.
- **Regras de Negócio de Acesso:** Alunos inadimplentes são bloqueados na catraca.

## 🏗️ Arquitetura do Projeto (3 Camadas)

O projeto foi rigorosamente estruturado para separar as responsabilidades:

1. **`SistemaAcademia.UI` (Interface do Usuário):** Contém os formulários do Windows Forms (`FormLogin`, `FormRegistro`, `FormAdmin`, `FormEditarAluno`).
2. **`SistemaAcademia.BLL` (Business Logic Layer):** Camada responsável pelas regras de negócio, validações (Regex), comunicação com a API do ViaCEP e regras de liberação da catraca.
3. **`SistemaAcademia.DAL` (Data Access Layer):** Camada de persistência. Comunica-se diretamente com o SQL Server utilizando o `Microsoft.Data.SqlClient`.
4. **`SistemaAcademia.Models`:** Biblioteca de classes que representam as entidades do sistema (`Usuario`, `Pagamento`, `AcessoCatraca`).

## 💻 Tecnologias Utilizadas

- **Linguagem:** C# (.NET)
- **Interface:** Windows Forms (WinForms)
- **Banco de Dados:** Microsoft SQL Server
- **Acesso a Dados:** ADO.NET (`Microsoft.Data.SqlClient`)
- **API Externa:** [ViaCEP](https://viacep.com.br/) (para consulta de CEP)
- **Outros:** `System.Text.RegularExpressions` para validações.

## 🛠️ Como Executar o Projeto

### Pré-requisitos
- Visual Studio 2022 (ou superior) com o workload de "Desenvolvimento para desktop com .NET".
- SQL Server (LocalDB ou instância completa).

### Passos para Instalação

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/leozinlsb/desafio-sistemaacademia.git
   ```

2. **Configure o Banco de Dados:**
   - Abra o SQL Server Management Studio (SSMS).
   - Execute o script de criação das tabelas localizado na pasta `Database/CreateTables.sql`. Esse script criará o banco `AcademiaDB` e as tabelas `Usuario`, `Pagamento` e `AcessoCatraca`.

3. **Configure a String de Conexão:**
   - Vá até o projeto `SistemaAcademia.DAL`.
   - Na classe `ConnectionHelper.cs`, ajuste a Connection String para apontar para o seu servidor SQL local. Exemplo:
     ```csharp
     "Server=SEU_SERVIDOR;Database=AcademiaDB;Trusted_Connection=True;TrustServerCertificate=True;"
     ```

4. **Execute a Aplicação:**
   - Abra a solução `SistemaAcademia.slnx` no Visual Studio.
   - Defina o projeto `SistemaAcademia.UI` como Projeto de Inicialização.
   - Pressione `F5` ou clique em "Iniciar".

### Primeiro Acesso
- Crie um cadastro na tela de registro marcando a opção (se aplicável via banco) ou insira diretamente pelo banco de dados um usuário onde a flag `IsAdmin` seja `1` para acessar o Painel do Administrador.

## 🤝 Contribuindo

Este projeto foi desenvolvido como um desafio técnico. Contribuições, dicas e melhorias são sempre bem-vindas! Sinta-se à vontade para abrir uma *Issue* ou enviar um *Pull Request*.
