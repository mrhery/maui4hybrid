namespace MAUI4Hybrid
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                M4HttpServer.Init();
                M4HttpServer.Start();
            });

            MainPage = new MainPage();
        }
    }
}
