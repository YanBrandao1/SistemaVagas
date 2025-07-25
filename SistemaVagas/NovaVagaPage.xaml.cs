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
            // 1) Cria o objeto Vaga com os valores do formul�rio
            var vaga = new Vaga
            {
                Nome = nomeEntry.Text,
                Nivel = nivelEntry.Text,
                Modalidade = modalidadeEntry.Text,
                Url = urlEntry.Text,
                Descricao = descricaoEditor.Text
            };

            // 2) Carrega a URL do Firebase e chama o servi�o
            var settings = await ConfigManager.LoadSettingsAsync();
            bool sucesso = await FirebaseVagasService
                .AdicionarVagaAsync(vaga, settings.Firebase.DatabaseUrl);

            // 3) Feedback para o usu�rio
            if (sucesso)
            {
                await DisplayAlert("Sucesso", "Vaga enviada para aprova��o.", "OK");
                // volta para a p�gina anterior (lista de vagas pendentes, por exemplo)
                await Shell.Current.GoToAsync($"//{nameof(VagasPage)}");
            }
            else
            {
                await DisplayAlert("Erro", "N�o foi poss�vel cadastrar a vaga. Tente novamente.", "OK");
            }
        }
    }
}
