using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;           // para SecureStorage
using SistemaVagas.Services;            // para FirebaseAuthService
using System;

namespace SistemaVagas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Tela temporária de loading
            MainPage = new ContentPage
            {
                Content = new ActivityIndicator
                {
                    IsRunning = true,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                }
            };

            // Verifica login e define a página raiz
            CheckUserLogged();
        }

        private async void CheckUserLogged()
        {
            try
            {
                // <-- Aqui: trocamos "user_token" por "auth_token"
                var token = await SecureStorage.GetAsync("auth_token");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    // Se você tiver um método para restaurar em memória:
                    // FirebaseAuthService.SetToken(token);

                    // Usuário logado → mostra o AppShell (dashboard)
                    MainPage = new AppShell();
                }
                else
                {
                    // Não logado → vai para a tela de Login
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch
            {
                // Qualquer falha no SecureStorage → Login
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
