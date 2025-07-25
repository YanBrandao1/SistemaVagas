using System.Collections.ObjectModel;
using SistemaVagas.Models;

namespace SistemaVagas;

public partial class VagasPage : ContentPage
{
    // Propriedade que o XAML vai ler
    public ObservableCollection<Vaga> Vagas { get; } = new();

    public VagasPage()
    {
        InitializeComponent();

        // Exemplo de dados iniciais
        Vagas.Add(new Vaga
        {
            Nome = "Desenvolvedor .NET",
            Nivel = "Pleno",
            Modalidade = "Remoto",
            LocalidadeSigla = "SSA-BA",
            Url = "https://cnaidiomas.gupy.io/job/eyJqb2JJZCI6OTM3ODcwOCwic291cmNlIjoicmVtb3RhciJ9?jobBoardSource=share_link",
            Descricao = "Desenvolvedor .NET com experiência em ASP.NET Core e Entity Framework."
        });
        Vagas.Add(new Vaga
        {
            Nome = "Estagiário Java",
            Nivel = "Estágio",
            Modalidade = "Presencial",
            LocalidadeSigla = "FEIRA-BA",
            Url = "https://cnaidiomas.gupy.io/job/eyJqb2JJZCI6OTM3ODcxMCwic291cmNlIjoicmVtb3RhciJ9?jobBoardSource=share_link",
            Descricao = "Estagiário Java para atuar em projetos de desenvolvimento de software."
        });
        Vagas.Add(new Vaga
        {
            Nome = "Analista de Dados",
            Nivel = "Júnior",
            Modalidade = "Híbrido",
            LocalidadeSigla = "RMS-BA",
            Url = "https://cnaidiomas.gupy.io/job/eyJqb2JJZCI6OTM3ODcxMiwic291cmNlIjoicmVtb3RhciJ9?jobBoardSource=share_link",
            Descricao = "Analista de Dados com foco em análise de grandes volumes de dados e geração de relatórios."
        });

        // Liga o BindingContext
        BindingContext = this;
    }

    async void OnVagaSelected(object sender, SelectionChangedEventArgs e)
    {
        var vaga = e.CurrentSelection.FirstOrDefault() as Vaga;
        if (vaga == null) return;

        // monta a rota com todos os campos que a detail page vai exibir
        var rota = $"{nameof(VagaDetailPage)}" +
                   $"?nome={Uri.EscapeDataString(vaga.Nome)}" +
                   $"&nivel={Uri.EscapeDataString(vaga.Nivel)}" +
                   $"&modalidade={Uri.EscapeDataString(vaga.Modalidade)}" +
                   $"&localidade={Uri.EscapeDataString(vaga.LocalidadeSigla)}" +
                   $"&descricao={Uri.EscapeDataString(vaga.Descricao)}" +
                   $"&url={Uri.EscapeDataString(vaga.Url)}";

        await Shell.Current.GoToAsync(rota);
        ((CollectionView)sender).SelectedItem = null;
    }

}
