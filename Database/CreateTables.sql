-- Criar banco de dados (Descomente se desejar criar o banco junto com as tabelas)
-- CREATE DATABASE AcademiaDB;
-- GO
USE AcademiaDB;
GO

-- ATENÇÃO: As tabelas antigas (se existirem) serão apagadas para criar a estrutura correta.
IF OBJECT_ID('dbo.AcessoCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessoCatraca;
IF OBJECT_ID('dbo.Pagamento', 'U') IS NOT NULL DROP TABLE dbo.Pagamento;
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
-- Também limpa as tabelas erradas que o script antigo pode ter criado
IF OBJECT_ID('dbo.AcessosCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessosCatraca;
IF OBJECT_ID('dbo.Pagamentos', 'U') IS NOT NULL DROP TABLE dbo.Pagamentos;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
GO

-- 1. Tabela de Usuarios (Clientes e Admins)
CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioLogin VARCHAR(50) NOT NULL UNIQUE,
    SenhaHash NVARCHAR(256) NOT NULL,
    Nome VARCHAR(150) NOT NULL,
    Cpf VARCHAR(14) NOT NULL UNIQUE,
    Telefone VARCHAR(15) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    Cep VARCHAR(9) NOT NULL,
    Rua VARCHAR(150) NOT NULL,
    Bairro VARCHAR(100) NOT NULL,
    Cidade VARCHAR(100) NOT NULL,
    Estado CHAR(2) NOT NULL,
    IsAdmin BIT NOT NULL DEFAULT 0,
    DataCadastro DATETIME DEFAULT GETDATE()
);
GO

-- 2. Tabela de Pagamentos
CREATE TABLE Pagamento (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    DataPagamento DATETIME NOT NULL DEFAULT GETDATE(),
    Valor DECIMAL(10,2) NOT NULL,
    MesReferencia INT NOT NULL,
    AnoReferencia INT NOT NULL,
    
    CONSTRAINT FK_Pagamento_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);
GO

-- 3. Tabela de Acessos da Catraca
CREATE TABLE AcessoCatraca (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    DataAcesso DATETIME NOT NULL DEFAULT GETDATE(),
    Liberado BIT NOT NULL, -- 1 = Sim, 0 = Não
    MotivoBloqueio VARCHAR(100) NULL,
    
    CONSTRAINT FK_AcessoCatraca_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);
GO

-- =================================================================================
-- 4. Criação do Usuário Administrador (admin / admin123)
-- Hash SHA256 puro em formato hexadecimal sem salt
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
    1 -- 1 indica que é Admin
);
GO
