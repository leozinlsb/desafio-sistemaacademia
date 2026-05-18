-- =================================================================================
-- SCRIPT DE CRIAÇÃO DO BANCO DE DADOS DA ACADEMIA
-- =================================================================================
-- Este script é responsável por criar toda a estrutura de tabelas necessárias
-- para o sistema funcionar. Ele deve ser executado no SQL Server Management Studio
-- (SSMS) ou em qualquer ferramenta que conecte ao SQL Server.
--
-- IMPORTANTE: Este script APAGA as tabelas existentes antes de recriá-las.
--             Isso significa que todos os dados serão perdidos ao executá-lo novamente.
-- =================================================================================

-- Criar banco de dados (Descomente se desejar criar o banco junto com as tabelas)
-- CREATE DATABASE AcademiaDB;
-- GO

-- USE: Seleciona o banco de dados "AcademiaDB" como banco ativo.
-- Todos os comandos abaixo serão executados dentro deste banco.
USE AcademiaDB;
GO

-- =================================================================================
-- LIMPEZA: Remove tabelas antigas caso já existam.
-- A ordem importa! Tabelas que possuem chave estrangeira (FK) precisam ser
-- apagadas ANTES da tabela que elas referenciam (Usuario).
-- Caso contrário, o SQL Server impediria a exclusão por causa da dependência.
-- =================================================================================

-- OBJECT_ID verifica se a tabela existe. O segundo parâmetro 'U' significa "User Table".
-- Se a tabela existir, o DROP TABLE a remove do banco.
IF OBJECT_ID('dbo.AcessoCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessoCatraca;
IF OBJECT_ID('dbo.Pagamento', 'U') IS NOT NULL DROP TABLE dbo.Pagamento;
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
-- Também limpa as tabelas erradas que o script antigo pode ter criado
-- (nomes no plural que foram usados em versões anteriores do projeto)
IF OBJECT_ID('dbo.AcessosCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessosCatraca;
IF OBJECT_ID('dbo.Pagamentos', 'U') IS NOT NULL DROP TABLE dbo.Pagamentos;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
GO

-- =================================================================================
-- 1. TABELA: Usuario
-- =================================================================================
-- Esta é a tabela principal do sistema. Armazena tanto ALUNOS quanto ADMINISTRADORES.
-- A diferença entre eles é o campo "IsAdmin":
--   IsAdmin = 0 → Aluno (usuário comum da academia)
--   IsAdmin = 1 → Administrador (acessa o painel de gerenciamento)
-- =================================================================================
CREATE TABLE Usuario (
    -- Id: Chave primária com auto-incremento (IDENTITY).
    -- O SQL Server gera automaticamente 1, 2, 3... para cada novo registro.
    Id INT IDENTITY(1,1) PRIMARY KEY,

    -- UsuarioLogin: Nome de login único (ex: "joao123"). 
    -- UNIQUE impede que dois usuários tenham o mesmo login.
    UsuarioLogin VARCHAR(50) NOT NULL UNIQUE,

    -- SenhaHash: A senha do usuário NÃO é guardada em texto puro por segurança.
    -- Em vez disso, guardamos o "hash SHA256" da senha (uma sequência de 64 caracteres hexadecimais).
    -- Exemplo: a senha "admin123" vira "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9"
    -- Isso impede que alguém que acesse o banco leia a senha real.
    SenhaHash NVARCHAR(256) NOT NULL,

    -- Nome completo do aluno/admin
    Nome VARCHAR(150) NOT NULL,

    -- Cpf: O CPF é armazenado com máscara (ex: "123.456.789-00").
    -- UNIQUE garante que o mesmo CPF não seja cadastrado duas vezes.
    Cpf VARCHAR(14) NOT NULL UNIQUE,

    -- Telefone com máscara: (XX) XXXXX-XXXX
    Telefone VARCHAR(15) NOT NULL,

    -- Email: também é UNIQUE (um email por cadastro).
    Email VARCHAR(150) NOT NULL UNIQUE,

    -- Dados de endereço (preenchidos automaticamente via API ViaCEP no sistema)
    Cep VARCHAR(9) NOT NULL,        -- Formato: XXXXX-XXX
    Rua VARCHAR(150) NOT NULL,
    Bairro VARCHAR(100) NOT NULL,
    Cidade VARCHAR(100) NOT NULL,
    Estado CHAR(2) NOT NULL,         -- Sigla do estado: SP, RJ, MG, etc.

    -- IsAdmin: BIT funciona como um booleano no SQL Server.
    -- 0 = false (aluno), 1 = true (admin). DEFAULT 0 significa que, se não
    -- for informado, o padrão é ser aluno.
    IsAdmin BIT NOT NULL DEFAULT 0,

    -- DataCadastro: Data/hora em que o registro foi criado.
    -- GETDATE() preenche automaticamente com a data/hora atual do servidor SQL.
    DataCadastro DATETIME DEFAULT GETDATE()
);
GO

-- =================================================================================
-- 2. TABELA: Pagamento
-- =================================================================================
-- Registra os pagamentos mensais dos alunos.
-- Cada linha representa UM pagamento feito por UM aluno em UM mês específico.
-- O admin registra o pagamento pela tela administrativa.
-- =================================================================================
CREATE TABLE Pagamento (
    Id INT IDENTITY(1,1) PRIMARY KEY,

    -- UsuarioId: Liga este pagamento a um aluno específico da tabela Usuario.
    -- É uma chave estrangeira (FK = Foreign Key).
    UsuarioId INT NOT NULL,

    -- Data em que o pagamento foi registrado no sistema
    DataPagamento DATETIME NOT NULL DEFAULT GETDATE(),

    -- Valor do pagamento (ex: 100.00). DECIMAL(10,2) permite até 10 dígitos,
    -- sendo 2 casas decimais (centavos).
    Valor DECIMAL(10,2) NOT NULL,

    -- MesReferencia e AnoReferencia: indicam A QUAL MÊS este pagamento se refere.
    -- Ex: MesReferencia = 5 e AnoReferencia = 2026 → pagamento referente a Maio/2026.
    -- O sistema usa esses campos para verificar se o aluno está em dia no mês atual.
    MesReferencia INT NOT NULL,
    AnoReferencia INT NOT NULL,
    
    -- CONSTRAINT (restrição) de chave estrangeira:
    -- Garante que o UsuarioId precisa existir na tabela Usuario.
    -- ON DELETE CASCADE: Se o aluno for excluído da tabela Usuario, todos os 
    -- pagamentos dele são apagados automaticamente (efeito cascata).
    CONSTRAINT FK_Pagamento_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);
GO

-- =================================================================================
-- 3. TABELA: AcessoCatraca
-- =================================================================================
-- Registra o HISTÓRICO de passagens na catraca da academia (log de acessos).
-- Cada vez que um aluno tenta entrar (via login ou simulação do admin), 
-- uma linha é inserida aqui informando se foi liberado ou bloqueado.
-- =================================================================================
CREATE TABLE AcessoCatraca (
    Id INT IDENTITY(1,1) PRIMARY KEY,

    -- UsuarioId: Liga este registro de acesso a um aluno da tabela Usuario.
    UsuarioId INT NOT NULL,

    -- Data/hora exata da tentativa de acesso
    DataAcesso DATETIME NOT NULL DEFAULT GETDATE(),

    -- Liberado: BIT (booleano) → 1 = Sim (catraca liberou), 0 = Não (catraca bloqueou)
    Liberado BIT NOT NULL,

    -- MotivoBloqueio: Texto opcional que informa o motivo caso o acesso seja negado.
    -- NULL significa que não há motivo (acesso foi liberado).
    MotivoBloqueio VARCHAR(100) NULL,
    
    -- FK com CASCADE: Se o aluno for excluído, seus logs de acesso também somem.
    CONSTRAINT FK_AcessoCatraca_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);
GO

-- =================================================================================
-- 4. INSERÇÃO DO USUÁRIO ADMINISTRADOR PADRÃO
-- =================================================================================
-- Cria um admin inicial para que você consiga acessar o painel administrativo
-- logo após rodar o script, sem precisar cadastrar ninguém antes.
--
-- Login: admin
-- Senha: admin123
-- Hash SHA256 de "admin123": 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
--
-- O hash é gerado pelo algoritmo SHA256 SEM salt (sal criptográfico).
-- O programa C# usa o mesmo algoritmo para comparar na hora do login.
-- =================================================================================
INSERT INTO Usuario (
    UsuarioLogin, 
    SenhaHash, 
    Nome, 
    Cpf, 
    Telefone, 
    Email, 
    Cep, 
    Rua, 
    Bairro, 
    Cidade, 
    Estado,
    IsAdmin
)
VALUES (
    'admin', 
    '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', -- Hash SHA256 de "admin123"
    'Administrador do Sistema', 
    '000.000.000-00', 
    '(00) 00000-0000', 
    'admin@academia.com', 
    '00000-000', 
    'Rua', 
    'Bairro', 
    'Cidade', 
    'SP',
    1 -- 1 indica que é Admin (IsAdmin = true)
);
GO
