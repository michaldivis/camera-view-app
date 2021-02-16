using CameraViewApp.ViewModels;
using CameraViewApp.Views;
using Xamarin.Forms;

namespace CameraViewApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new CameraPage(new CameraViewModel());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
