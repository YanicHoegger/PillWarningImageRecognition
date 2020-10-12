namespace MobileInterface
{
    public partial class App
    {
        public App()
        {
            Startup.Init();
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Startup.Start();
        }

        protected override void OnSleep()
        {
            Startup.Stop();
        }

        protected override void OnResume()
        {
            Startup.Resume();
        }
    }
}
