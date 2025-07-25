using Microsoft.Maui.ApplicationModel;  // para Launcher
using Microsoft.Maui.Controls;

namespace SistemaVagas
{
    // Declara um QueryProperty para cada par�metro que vem na rota
    [QueryProperty(nameof(Nome), "nome")]
    [QueryProperty(nameof(Nivel), "nivel")]
    [QueryProperty(nameof(Modalidade), "modalidade")]
    [QueryProperty(nameof(LocalidadeSigla), "localidade")]
    [QueryProperty(nameof(Descricao), "descricao")]
    [QueryProperty(nameof(Url), "url")]
    public partial class VagaDetailPage : ContentPage
    {
        public VagaDetailPage()
        {
            InitializeComponent();
            BindingContext = this;  // faz com que {Binding X} olhe nesta inst�ncia
        }

        // Para cada propriedade, use backing field e OnPropertyChanged
        string nome;
        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
                Title = nome;     // atualiza o t�tulo da p�gina tamb�m
            }
        }

        string nivel;
        public string Nivel
        {
            get => nivel;
            set { nivel = value; OnPropertyChanged(); }
        }

        string modalidade;
        public string Modalidade
        {
            get => modalidade;
            set { modalidade = value; OnPropertyChanged(); }
        }

        string localidadeSigla;
        public string LocalidadeSigla
        {
            get => localidadeSigla;
            set { localidadeSigla = value; OnPropertyChanged(); }
        }

        string descricao;
        public string Descricao
        {
            get => descricao;
            set { descricao = value; OnPropertyChanged(); }
        }

        string url;
        public string Url
        {
            get => url;
            set { url = value; OnPropertyChanged(); }
        }

        // M�todo do bot�o �Inscrever-se�
        private async void OnApplyClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Url))
                await Launcher.OpenAsync(Url);
        }
    }
}
