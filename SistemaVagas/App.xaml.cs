using Microsoft.Maui.Controls;
using SistemaVagas; // certifique-se de que LoginPage está nesse namespace
using System;
using System.Threading.Tasks;

namespace SistemaVagas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Página temporária para evitar erro antes do carregamento assíncrono
            MainPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };

            // Inicia a verificação do login de forma assíncrona
            CheckUserLogged();
        }

        private async void CheckUserLogged()
        {
            try
            {
                var token = await SecureStorage.GetAsync("user_token");

                if (!string.IsNullOrEmpty(token))
                {
                    // Usuário logado → vai para AppShell
                    MainPage = new AppShell();
                }
                else
                {
                    // Não logado → vai para LoginPage com navegação
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception)
            {
                // Qualquer erro no SecureStorage → vai para LoginPage
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
