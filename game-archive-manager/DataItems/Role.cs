using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class Role
    {
        private string _roleName = string.Empty;
        private string _description = string.Empty;


        [PrimaryKey, AutoIncrement]
        public int RoleId { get; set; }  // Ö÷¼ü£¬×ÔÔö

        public string RoleName
        {
            get => _roleName;
            set => _roleName = value ?? string.Empty;
        }

        public string Description
        {
            get => _description;
            set => _description = value ?? string.Empty;
        }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Role()
        {
            _roleName = string.Empty;
            _description = string.Empty;
        }
    }
}
