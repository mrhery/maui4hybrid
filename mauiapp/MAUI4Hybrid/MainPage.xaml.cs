namespace MAUI4Hybrid
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();

            string host = "http://127.0.0.1:" + M4HttpServer.Port;

            M4HttpServer.Data.Add("TEXT", "hello from c#");
            M4HttpServer.Data.Add("SAMPLE", "Sample widget included from widgets folder.");
            M4HttpServer.Data.Add("username", Convert.ToString(M4HttpServer.configure.sample.username));

            wb.Source = host + "/index.html";
        }
    }

}
