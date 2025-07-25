namespace SistemaVagas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VagaDetailPage), typeof(VagaDetailPage));
        }
    }
}
