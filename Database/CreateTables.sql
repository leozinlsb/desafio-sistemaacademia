USE AcademiaDB;
GO

-- Limpa tabelas existentes (ordem importa por causa das FKs)
IF OBJECT_ID('dbo.AcessoCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessoCatraca;
IF OBJECT_ID('dbo.Pagamento', 'U') IS NOT NULL DROP TABLE dbo.Pagamento;
IF OBJECT_ID('dbo.Usuario', 'U') IS NOT NULL DROP TABLE dbo.Usuario;
IF OBJECT_ID('dbo.AcessosCatraca', 'U') IS NOT NULL DROP TABLE dbo.AcessosCatraca;
IF OBJECT_ID('dbo.Pagamentos', 'U') IS NOT NULL DROP TABLE dbo.Pagamentos;
IF OBJECT_ID('dbo.Clientes', 'U') IS NOT NULL DROP TABLE dbo.Clientes;
GO

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

CREATE TABLE AcessoCatraca (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    DataAcesso DATETIME NOT NULL DEFAULT GETDATE(),
    Liberado BIT NOT NULL,
    MotivoBloqueio VARCHAR(100) NULL,
    CONSTRAINT FK_AcessoCatraca_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id) ON DELETE CASCADE
);
GO

-- Admin padrão (Login: admin | Senha: admin123)
INSERT INTO Usuario (
    UsuarioLogin, SenhaHash, Nome, Cpf, Telefone, Email,
    Cep, Rua, Bairro, Cidade, Estado, IsAdmin
)
VALUES (
    'admin', 
    '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9',
    'Administrador do Sistema', 
    '000.000.000-00', 
    '(00) 00000-0000', 
    'admin@academia.com', 
    '00000-000', 
    'Rua', 
    'Bairro', 
    'Cidade', 
    'SP',
    1
);
GO
