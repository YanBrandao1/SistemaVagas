using SistemaVagas.Models;
using SistemaVagas.Services;
using SistemaVagas.Settings;

namespace SistemaVagas
{
    public partial class NovaVagaPage : ContentPage
    {
        public NovaVagaPage()
        {
            InitializeComponent();
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
            // 1) Cria o objeto Vaga com os valores do formulário
            var vaga = new Vaga
            {
                Nome = nomeEntry.Text,
                Nivel = nivelEntry.Text,
                Modalidade = modalidadeEntry.Text,
                Url = urlEntry.Text,
                Descricao = descricaoEditor.Text
            };

            // 2) Carrega a URL do Firebase e chama o serviço
            var settings = await ConfigManager.LoadSettingsAsync();
            bool sucesso = await FirebaseVagasService
                .AdicionarVagaAsync(vaga, settings.Firebase.DatabaseUrl);

            // 3) Feedback para o usuário
            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Vaga enviada para aprovação.", "OK");
                // volta para a página anterior (lista de vagas pendentes, por exemplo)
                await Shell.Current.GoToAsync($"//{nameof(VagasPage)}");
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível cadastrar a vaga. Tente novamente.", "OK");
            }
        }
    }
}
