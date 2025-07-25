using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // 1) Navega para a lista de vagas pendentes
    private async void OnVagasPendentesClicked(object sender, EventArgs e)
    {
        // rota absoluta para garantir que abra a aba correta
        await Shell.Current.GoToAsync(nameof(VagasPendentesPage));
    }

    // 2) Navega para o formulário de nova vaga
    private async void OnAdicionarVagaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NovaVagaPage));
    }

    // 3) Logout
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // limpa o token em memória (e no SecureStorage, se você implementou)
        FirebaseAuthService.Logout();
        // volta para a tela de login (pilha relativa)
        await Shell.Current.GoToAsync(nameof(LoginPage));
    }

    // 4) Deletar vagas antigas
    private async void OnGerenciarVagasClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(VagasPage));
    }
}
