using System;
using System.Collections.ObjectModel;
using SistemaVagas.Models;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas
{
    // 1) Wrapper que adiciona o ID ao modelo Vaga
    public class VagaItem : Vaga
    {
        public string Id { get; set; } = string.Empty;
    }

    public partial class VagasPage : ContentPage
    {
        // 2) ObservableCollection de VagaItem em vez de Vaga
        public ObservableCollection<VagaItem> Vagas { get; } = new();

        public VagasPage()
        {
            InitializeComponent();
            BindingContext = this;
            CarregarVagas();
        }

        // 3) Carrega o dicionário do Firebase, converte para VagaItem e popula a coleção
        private async void CarregarVagas()
        {
            var settings = await ConfigManager.LoadSettingsAsync();

            // Método que retorna Dictionary<id, Vaga>
            var vagasDict = await FirebaseVagasService
                .ObterVagasComIdsAsync(settings.Firebase.DatabaseUrl);

            Vagas.Clear();
            foreach (var kvp in vagasDict)
            {
                Vagas.Add(new VagaItem
                {
                    Id = kvp.Key,
                    Nome = kvp.Value.Nome,
                    Nivel = kvp.Value.Nivel,
                    Modalidade = kvp.Value.Modalidade,
                    Url = kvp.Value.Url,
                    Descricao = kvp.Value.Descricao
                });
            }
        }

        // 4) Handler do swipe “Excluir”
        private async void OnExcluirVaga(object sender, EventArgs e)
        {
            var swipeItem = (SwipeItem)sender;
            var id = swipeItem.CommandParameter?.ToString();
            if (string.IsNullOrEmpty(id))
                return;

            bool confirma = await DisplayAlert(
                "Confirmar", "Deseja excluir esta vaga?", "Sim", "Não");
            if (!confirma) return;

            var settings = await ConfigManager.LoadSettingsAsync();
            bool sucesso = await FirebaseVagasService
                .DeletarVagaAsync(id, settings.Firebase.DatabaseUrl);

            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Vaga excluída.", "OK");
                CarregarVagas();
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao excluir vaga.", "OK");
            }
        }

        // 5) Mantém seu OnVagaSelected se você quiser abrir o detalhe ao tocar
        async void OnVagaSelected(object sender, SelectionChangedEventArgs e)
        {
            var vaga = e.CurrentSelection.FirstOrDefault() as VagaItem;
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
}
