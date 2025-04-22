using SQLite;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace game_archive_manager.DataItems.DB
{
    class DataBase
    {
        public string DatabasePath { get; private set; }
        public SQLiteAsyncConnection EncryptedDb { get; private set; }

        public DataBase(string customPath = "null",string password="password")
        {
            // 如果传了路径就用，否则使用默认路径
            DatabasePath = "null" == customPath
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyData.db")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"GameArchiveManger", customPath);

            Debug.WriteLine($"当前数据库路径: {DatabasePath}");

            var options = new SQLiteConnectionString(DatabasePath, storeDateTimeAsTicks: true,
                key: password,
                preKeyAction: db =>
                {
                    db.Execute("PRAGMA cipher_default_use_hmac = ON;");
                },
                postKeyAction: db =>
                {
                    db.Execute("PRAGMA cipher_page_size = 4096;");
                    db.Execute("PRAGMA kdf_iter = 256000;");
                    db.Execute("PRAGMA cipher_hmac_algorithm = HMAC_SHA512;");
                    db.Execute("PRAGMA cipher_kdf_algorithm = PBKDF2_HMAC_SHA512;"); 
                    db.Execute("PRAGMA cipher_plaintext_header_size = 0;");
                });

            EncryptedDb = new SQLiteAsyncConnection(options);
            CreateDierctory();
        }

        ~DataBase()
        {
            EncryptedDb.CloseAsync();
        }
        /// <summary>
        /// 初始化数据库，例如创建表结构。
        /// 调用时机：构造后立即 await InitAsync()
        /// </summary>
        public async Task InitAsync<T>() where T : new()
        {
            try
            {
                await EncryptedDb.CreateTableAsync<T>();
                Debug.WriteLine($"{typeof(T).Name} 表已创建或已存在。");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"初始化数据库失败: {ex.Message}");
                throw;
            }
        }


        public async Task<int> InsertDatsAsync<T>(T data) where T : new()
        {
            try
            {
                return await EncryptedDb.InsertAsync(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"数据插入异常: {ex.Message}");
                throw;
            }
        }

        public void ShowData<T>() where T : new()
        {
            // 查询所有数据
            var allData = EncryptedDb.Table<T>().ToListAsync().Result;

            // 打印数据
            foreach (var item in allData)
            {
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    Debug.WriteLine($"{property.Name}: {value}");
                }
                Debug.WriteLine("-----------------------"); // 分隔每个对象的输出
            }
        }
        // ... 其他代码 ...

        public async Task<System.Collections.Generic.List<T>> GetDataAsync<T>() where T : new()
        {
            try
            {
                return await EncryptedDb.Table<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取数据失败: {ex.Message}");
                throw;
            }
        }

        // 如果需要条件查询，可以添加这个方法
        public async Task<System.Collections.Generic.List<T>> GetDataByConditionAsync<T>(System.Linq.Expressions.Expression<Func<T, bool>> predicate) where T : new()
        {
            try
            {
                return await EncryptedDb.Table<T>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"条件查询失败: {ex.Message}");
                throw;
            }
        }

        // ... 其他代码 ...
        void CreateDierctory()
        {
            var path = Path.GetDirectoryName(DatabasePath);
            if (path == null)
            {
                Debug.WriteLine("路径无效");
                return;
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
