using System.Collections.ObjectModel;
using SistemaVagas.Models;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class VagasPage : ContentPage
{
    public ObservableCollection<Vaga> Vagas { get; } = new();

    public VagasPage()
    {
        InitializeComponent();
        BindingContext = this;

        CarregarVagas();
    }

    private async void CarregarVagas()
    {
        var settings = await ConfigManager.LoadSettingsAsync();
        var vagasFirebase = await FirebaseVagasService.ObterVagasAsync(settings.Firebase.DatabaseUrl);

        Vagas.Clear();

        foreach (var vaga in vagasFirebase)
        {
            Vagas.Add(vaga);
        }
    }

    async void OnVagaSelected(object sender, SelectionChangedEventArgs e)
    {
        var vaga = e.CurrentSelection.FirstOrDefault() as Vaga;
        if (vaga == null) return;

        var rota = $"{nameof(VagaDetailPage)}" +
                   $"?nome={Uri.EscapeDataString(vaga.Nome)}" +
                   $"&nivel={Uri.EscapeDataString(vaga.Nivel)}" +
                   $"&modalidade={Uri.EscapeDataString(vaga.Modalidade)}" +
                   $"&descricao={Uri.EscapeDataString(vaga.Descricao)}" +
                   $"&url={Uri.EscapeDataString(vaga.Url)}";

        await Shell.Current.GoToAsync(rota);
        ((CollectionView)sender).SelectedItem = null;
    }
}
