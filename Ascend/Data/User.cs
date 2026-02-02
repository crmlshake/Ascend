using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ascend.Data
{
    public partial class User : INotifyPropertyChanged
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private string _profileImagePath;
        public string ProfileImagePath
        {
            get => _profileImagePath;
            set
            {
                _profileImagePath = value;
                OnPropertyChanged(nameof(ProfileImagePath)); //UI über die Änderung informieren!
            }
        }

        private string _framePath;
        public string FramePath
        {
            get => _framePath;
            set
            {
                _framePath = value;
                OnPropertyChanged(nameof(FramePath));
            }
        }

        private string _backgroundPath;
        public string BackgroundPath
        {
            get => _backgroundPath;
            set
            {
                _backgroundPath = value;
                OnPropertyChanged(nameof(BackgroundPath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}