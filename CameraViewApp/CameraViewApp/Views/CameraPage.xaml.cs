using CameraViewApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CameraViewApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        private readonly CameraViewModel _vm;

        public CameraPage(CameraViewModel vm)
        {
            InitializeComponent();
            BindingContext = _vm = vm;
        }

        private void cameraView_OnAvailable(object sender, bool e)
        {
            _vm.OnAvailableChanged(e);
        }

        private async void cameraView_MediaCaptured(object sender, Xamarin.CommunityToolkit.UI.Views.MediaCapturedEventArgs e)
        {
            await _vm.OnPhotoCapturedAsync(e.ImageData, e.Image);
        }

        private void cameraView_MediaCaptureFailed(object sender, string e)
        {
            _vm.OnPhotoCaptureFailed(e);
        }
    }
}