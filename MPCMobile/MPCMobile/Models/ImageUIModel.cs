using MPCMobile.Client.Models;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPCMobile.Models
{
    public class ImageUIModel : ImageModel, INotifyPropertyChanged
    {
        public string _imagePath = string.Empty;
        public string ImagePath
        {
            get => _imagePath;
            set => SetProperty(ref _imagePath, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void SetProperty(ref string field, string newValue, [CallerMemberName] string propertyName = null)
        {
            if(field != newValue)
                field = newValue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
