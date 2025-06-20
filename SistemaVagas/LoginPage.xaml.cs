using Microsoft.Maui.ApplicationModel.Communication;
using System.Text.RegularExpressions;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Carregar as configurações do arquivo appsettings.json
        var settings = await ConfigManager.LoadSettingsAsync();

        // Pega a API Key dentro da configuração
        var apiKey = settings.Firebase.ApiKey;

        // Cria o serviço de autenticação passando a API Key
        var authService = new FirebaseAuthService(apiKey);

        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        try
        {
            var auth = await authService.LoginAsync(EmailEntry.Text, PasswordEntry.Text);

            await SecureStorage.SetAsync("auth_token", auth.IdToken);

            statusLabel.Text = $"Bem-vindo, {auth.Email}";

            await SecureStorage.SetAsync("user_token", auth.IdToken);

            // Navegar para a página principal após login
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            statusLabel.Text = "Erro: " + ex.Message;
        }
    }

    public void OnRegisterClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new RegisterPage();
    }

}