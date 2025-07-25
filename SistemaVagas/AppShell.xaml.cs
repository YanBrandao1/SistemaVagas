using SistemaVagas.Services;

namespace SistemaVagas
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(VagaDetailPage), typeof(VagaDetailPage));
            Routing.RegisterRoute(nameof(NovaVagaPage), typeof(NovaVagaPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(VagasPendentesPage), typeof(VagasPendentesPage));
            Routing.RegisterRoute(nameof(VagasPage), typeof(VagasPage));
            this.Navigating += OnShellNavigating;
        }

        private async void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[ShellNav] target={e.Target.Location}  " +
                $"IsUserLoggedIn={FirebaseAuthService.IsUserLoggedIn}  " +
                $"IdToken={FirebaseAuthService.IdToken}"
            );

            var target = e.Target.Location.OriginalString;
            bool indoParaProtectedRoute = target.Contains("//home")
                                        ||target.Contains("//VagasPendentesPage");
            if (indoParaProtectedRoute && !FirebaseAuthService.IsUserLoggedIn)
            {
                e.Cancel();
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
        }
    }
}
