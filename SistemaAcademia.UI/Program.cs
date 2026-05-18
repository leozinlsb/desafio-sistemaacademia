namespace SistemaAcademia.UI
{
    /// <summary>
    /// PONTO DE ENTRADA (Entry Point) da aplicação.
    /// 
    /// Esta é a classe que o Windows executa quando você clica em "Iniciar" no Visual Studio
    /// ou abre o .exe do programa. O método Main() é o PRIMEIRO código que roda.
    /// 
    /// Ele faz 3 coisas:
    ///   1. Configura o aplicativo (DPI, fontes, etc.)
    ///   2. Cria a tela de login (FormLogin)
    ///   3. Inicia o loop de mensagens do Windows Forms (mantém o programa rodando)
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Método Main: Ponto de entrada da aplicação Windows Forms.
        /// 
        /// [STAThread]: Atributo obrigatório para aplicações Windows Forms.
        /// Indica que o programa usa o modelo de threading STA (Single-Threaded Apartment),
        /// necessário para componentes visuais do Windows funcionarem corretamente.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Configura definições visuais do Windows Forms (DPI alto, fonte padrão, etc.)
            ApplicationConfiguration.Initialize();

            // Application.Run(): Inicia o loop principal do programa e abre o FormLogin.
            // O programa fica rodando enquanto esta janela (ou janelas filhas) estiver aberta.
            // Quando TODOS os formulários forem fechados, o programa encerra.
            Application.Run(new FormLogin());
        }
    }
}