using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PromptFavorites.Models
{
    public class PromptModule : INotifyPropertyChanged
    {
        private string _name;
        private int _entryCount;

        public string Name
        {
            get { return _name; }
            set { if (_name != value) { _name = value; OnPropertyChanged(); } }
        }

        public string FullPath { get; set; }

        public int EntryCount
        {
            get { return _entryCount; }
            set { if (_entryCount != value) { _entryCount = value; OnPropertyChanged(); } }
        }

        public System.DateTime CreatedAt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
