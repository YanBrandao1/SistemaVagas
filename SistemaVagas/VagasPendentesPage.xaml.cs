using System.Collections.ObjectModel;
using SistemaVagas.Models;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas;

public partial class VagasPendentesPage : ContentPage
{
    public ObservableCollection<Vaga> Vagas { get; } = new();

    public VagasPendentesPage()
    {
        InitializeComponent();
        BindingContext = this;

        CarregarVagasPendentes();
    }

    private async void CarregarVagasPendentes()
    {
        var settings = await ConfigManager.LoadSettingsAsync();
        var vagasDict = await FirebaseVagasService.ObterVagasPendentesAsync(settings.Firebase.DatabaseUrl);

        Vagas.Clear();

        foreach (var vaga in vagasDict.Values)
        {
            Vagas.Add(vaga);
        }
    }

    async void OnVagaSelected(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as Vaga;
        if (selected == null) return;

        var settings = await ConfigManager.LoadSettingsAsync();
        var vagasDict = await FirebaseVagasService.ObterVagasPendentesAsync(settings.Firebase.DatabaseUrl);

        // Localiza o ID correspondente
        var entry = vagasDict.FirstOrDefault(x => x.Value.Nome == selected.Nome &&
                                                  x.Value.Modalidade == selected.Modalidade &&
                                                  x.Value.Nivel == selected.Nivel);

        if (entry.Key != null)
        {
            await Shell.Current.Navigation.PushAsync(new VagaPendenteDetailPage(entry.Key, entry.Value));
        }

    ((CollectionView)sender).SelectedItem = null;
    }

}
