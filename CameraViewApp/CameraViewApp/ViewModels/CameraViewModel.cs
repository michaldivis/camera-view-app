using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace CameraViewApp.ViewModels
{
    public class CameraViewModel : ObservableObject
    {
        #region Properties

        private ImageSource _lastPhoto;
        public ImageSource LastPhoto
        {
            get => _lastPhoto;
            set => SetProperty(ref _lastPhoto, value);
        }

        private bool _canTakePhoto;
        public bool CanTakePhoto
        {
            get => _canTakePhoto;
            set => SetProperty(ref _canTakePhoto, value);
        }

        #endregion

        #region Public methods

        public void OnAvailableChanged(bool cameraAvailable)
        {
            CanTakePhoto = cameraAvailable;
        }

        public async Task OnPhotoCapturedAsync(byte[] photoData, ImageSource photoPreview)
        {
            LastPhoto = photoPreview;
            await TrySavePhotoAsync(photoData);
        }

        public void OnPhotoCaptureFailed(string message)
        {
            Debug.WriteLine("Photo capture failed: " + message);
        }

        #endregion

        #region Private methods

        private async Task TrySavePhotoAsync(byte[] photoData)
        {
            var readStatus = await CheckAndRequestPermissionAsync(new StorageRead());
            if (readStatus != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            var writeStatus = await CheckAndRequestPermissionAsync(new StorageWrite());
            if (writeStatus != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            var savedPhotoPath = await SavePhotoFileAsync(photoData, Guid.NewGuid().ToString() + ".jpg");
        }

        private async Task<string> SavePhotoFileAsync(byte[] photoData, string fileName)
        {
            var dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(dirPath);

            var savedPhotoPath = Path.Combine(dirPath, fileName);

            using (var fs = new FileStream(savedPhotoPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                await fs.WriteAsync(photoData, 0, photoData.Length);
                return savedPhotoPath;
            }
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission) where T : BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        #endregion
    }
}
