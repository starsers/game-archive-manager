using SQLite;
using System;

namespace game_archive_manager.DataItems
{
    
    /// 表示游戏存档/保存文件的实体类
   
    public class Archive
    {
        // 用于属性封装的私有字段，初始化默认值为空字符串
        private string _archiveName = string.Empty;
        private string _archivePath = string.Empty;


        /// 存档ID（主键，自增长）
        [PrimaryKey, AutoIncrement]
        public int ArchiveId { get; set; }

        
        /// 用户唯一标识（外键，关联用户表）
   
        public Guid UUID { get; set; }

     
        /// 存档名称（自动处理null值）
       
        public string ArchiveName
        {
            get => _archiveName;
            set => _archiveName = value ?? string.Empty; // 确保不为null
        }

        /// 存档文件路径（自动处理null值）
       
        public string ArchivePath
        {
            get => _archivePath;
            set => _archivePath = value ?? string.Empty; // 确保不为null
        }

        /// 存档创建时间（默认为当前UTC时间）
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

      
        /// 标记存档是否被修改过
        public bool IsModified { get; set; } = false;

        /// 导航属性 - 关联的用户对象
        public User User { get; set; } = null!; // 使用null宽容运算符
       
        /// 默认构造函数（初始化默认值）
       
        public Archive()
        {
            _archiveName = string.Empty;
            _archivePath = string.Empty;
            CreatedAt = DateTime.UtcNow;
            IsModified = false;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="archiveName">存档名称</param>
        /// <param name="archivePath">存档路径</param>
        /// <param name="uuid">关联用户ID</param>
        public Archive(string archiveName, string archivePath, Guid uuid)
        {
            ArchiveName = archiveName;
            ArchivePath = archivePath;
            UUID = uuid;
            CreatedAt = DateTime.UtcNow;
            IsModified = false;
        }
    }
}