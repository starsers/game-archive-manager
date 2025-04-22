using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class Game
    {
        private string _imagePath = string.Empty;
        private string _gameName = string.Empty;
        private string _saveLocation = string.Empty;

        [PrimaryKey, AutoIncrement]
        public int GameId { get; set; }  // Ö÷¼ü£¬×ÔÔö

        public string ImagePath
        {
            get => _imagePath;
            set => _imagePath = value ?? string.Empty;
        }

        public string GameName
        {
            get => _gameName;
            set => _gameName = value ?? string.Empty;
        }

        public string SaveLocation
        {
            get => _saveLocation;
            set => _saveLocation = value ?? string.Empty;
        }

        public Game()
        {
            _imagePath = string.Empty;
            _gameName = string.Empty;
            _saveLocation = string.Empty;
        }

        public Game(string imagePath, string gameName, string saveLocation)
        {
            ImagePath = imagePath;
            GameName = gameName;
            SaveLocation = saveLocation;
        }
    }
}
