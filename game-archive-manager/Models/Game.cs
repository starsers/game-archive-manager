using System.Collections.ObjectModel;

namespace game_archive_manager.Models
{
    public class Game
    {
        public string Name { get; set; }
        public string IconPath { get; set; }
        public ObservableCollection<SaveFile> SaveFiles { get; set; } = new ObservableCollection<SaveFile>();
    }
}