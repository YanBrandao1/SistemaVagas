using System.Text.RegularExpressions;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class RegisterPage : ContentPage
{
    private FirebaseAuthService authService;

    public RegisterPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private async void LoadSettings()
    {
        var settings = await ConfigManager.LoadSettingsAsync();
        authService = new FirebaseAuthService(settings.Firebase.ApiKey);
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@ufba\.br$");

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Erro", "Preencha todos os campos.", "OK");
            return;
        }

        if(!regex.IsMatch(email))
        {
            await DisplayAlert("Erro","Só podem ser cadastradas contas com domínio @ufba.br", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Erro", "As senhas não conferem.", "OK");
            return;
        }

        try
        {
            var result = await authService.RegisterAsync(email, password);

            // Salvar token no dispositivo
            await SecureStorage.SetAsync("user_token", result.IdToken);

            // Redirecionar para a página principal
            Application.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro no cadastro", ex.Message, "OK");
        }
    }

    private void OnGoToLoginClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new LoginPage());
    }
}
