namespace game_archive_manager
{
    public class ControlInfoDataItem
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public ControlInfoDataItem()
        {
            Title = "";
            ImagePath = "";
        }
        public ControlInfoDataItem(string title, string imagePath)
        {
            Title = title;
            ImagePath = imagePath;
        }
    }
}
