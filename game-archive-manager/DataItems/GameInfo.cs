using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_archive_manager.DataItems
{
    public class GameInfo : INotifyPropertyChanged
    {
        private string _gameID = string.Empty;
        public string GameID
        {
            get { return _gameID; }
            set
            {
                _gameID = value;
                OnPropertyChanged(nameof(GameID));
            }
        }
        private string _gameName = string.Empty;
        public string GameName
        {
            get { return _gameName; }
            set
            {
                _gameName = value;
                OnPropertyChanged(nameof(GameName));
            }
        }

        private string _gamePath = string.Empty;
        public string GamePath
        {
            get { return _gamePath; }
            set
            {
                _gamePath = value;
                OnPropertyChanged(nameof(GamePath));
            }
        }

        private ImageData _imageData = new ImageData("/Assets/pic/blank_game.png", "new gamne");
        public ImageData ImageData
        {
            get { return _imageData; }
            set
            {
                _imageData = value;
                OnPropertyChanged(nameof(ImageData));
            }
        }

        private string _saveLocation = string.Empty;
        public string SaveLocation
        {
            get { return _saveLocation; }
            set
            {
                _saveLocation = value;
                OnPropertyChanged(nameof(SaveLocation));
            }
        }

        private string _gameDescription = string.Empty;
        public string GameDescription
        {
            get { return _gameDescription; }
            set
            {
                _gameDescription = value;
                OnPropertyChanged(nameof(GameDescription));
            }
        }

        public ObservableCollection<Rule> ActiveRules { get; set; } = new ObservableCollection<Rule>();
        public ObservableCollection<Archive> Archives { get; set; } = new ObservableCollection<Archive>();

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameInfo()
        {
            // Initialization is already handled by default values.  
        }
    }
       
}
