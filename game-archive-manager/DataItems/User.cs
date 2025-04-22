using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    public class User
    {
        private string _userName = string.Empty;
        private string _passwordHash = string.Empty;
        private string _encryptedArchiveDirectory = string.Empty;

        [PrimaryKey]
        public Guid UUID { get; set; } = Guid.NewGuid();  // Ö÷¼ü

        public string UserName
        {
            get => _userName;
            set => _userName = value ?? string.Empty;
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = value ?? string.Empty;
        }

        public string? OnlineAccount { get; set; }  // ¿É¿Õ£¬±£Áô

        public string EncryptedArchiveDirectory
        {
            get => _encryptedArchiveDirectory;
            set => _encryptedArchiveDirectory = value ?? string.Empty;
        }

        public User()
        {
            _userName = string.Empty;
            _passwordHash = string.Empty;
            _encryptedArchiveDirectory = string.Empty;
        }
        public User(Models.User user)
        {
            _userName = user.Username;
            _passwordHash = user.PasswordHash;
            _encryptedArchiveDirectory = string.Empty;
        }
    }
}
