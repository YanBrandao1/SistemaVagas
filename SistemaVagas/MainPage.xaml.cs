namespace SistemaVagas
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, EventArgs e)
        {
            SecureStorage.Remove("user_token");

            // Redireciona para tela de login
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }

}
