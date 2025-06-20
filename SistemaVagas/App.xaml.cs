namespace SistemaVagas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CheckUserLogged();
        }

        private async void CheckUserLogged()
        {
            try
            {
                var token = await SecureStorage.GetAsync("user_token");

                if (!string.IsNullOrEmpty(token))
                {
                    // Usuário está logado
                    MainPage = new AppShell();
                }
                else
                {
                    // Não está logado
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception ex)
            {
                // Se deu erro no SecureStorage (emuladores antigos ou restrições)
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        /*protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }*/
    }
}