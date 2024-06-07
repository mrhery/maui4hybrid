namespace MAUI4Hybrid
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            string host = "http://127.0.0.1:" + M4HttpServer.Port;

            wb.Source = host + "/index.html";
        }
    }

}
