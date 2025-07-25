using SistemaVagas.Models;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class VagaPendenteDetailPage : ContentPage
{
    private string _vagaId;
    private Vaga _vaga;

    public VagaPendenteDetailPage(string id, Vaga vaga)
    {
        InitializeComponent();
        _vagaId = id;
        _vaga = vaga;
        BindingContext = vaga;
    }

    private async void OnAbrirLinkClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_vaga?.Url))
            await Launcher.Default.OpenAsync(_vaga.Url);
    }

    private async void OnAprovarClicked(object sender, EventArgs e)
    {
        var settings = await ConfigManager.LoadSettingsAsync();
        await FirebaseVagasService.AprovarVagaAsync(_vagaId, settings.Firebase.DatabaseUrl);

        await DisplayAlert("Aprovada", "Vaga aprovada com sucesso.", "OK");
        await Shell.Current.GoToAsync("..");
    }

    private async void OnRejeitarClicked(object sender, EventArgs e)
    {
        var settings = await ConfigManager.LoadSettingsAsync();
        await FirebaseVagasService.RejeitarVagaAsync(_vagaId, settings.Firebase.DatabaseUrl);

        await DisplayAlert("Removida", "Vaga rejeitada com sucesso.", "OK");
        await Shell.Current.GoToAsync("..");
    }
}
