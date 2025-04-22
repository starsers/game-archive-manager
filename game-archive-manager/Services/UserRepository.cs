using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using game_archive_manager.DataItems;
using game_archive_manager.DataItems.DB;

namespace game_archive_manager.Services
{
    public class UserRepository
    {
        //private string fileName = "users.db";
        private DataBase DataBase = new DataBase("users.db");


        //private async Task<StorageFile> GetUserFileAsync()
        //{
        //    StorageFolder localFolder = ApplicationData.Current.LocalFolder;
        //    StorageFile file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        //    return file;
        //}

        //public async Task SaveUserAsync(User user)
        //{
        //    List<User> users = await GetAllUsersAsync();
        //    users.Add(user);

        //    StorageFile file = await GetUserFileAsync();

        //    using (Stream stream = await file.OpenStreamForWriteAsync())
        //    {
        //        using (BinaryWriter writer = new BinaryWriter(stream))
        //        {
        //            writer.Write(users.Count);
        //            foreach (var u in users)
        //            {
        //                writer.Write(u.Username);
        //                writer.Write(u.PasswordHash);
        //            }
        //        }
        //    }

        //}
        public async Task<int> AddUserAsync(User user)
        {
            await InitAsync();
            return await DataBase.InsertDatsAsync<User>(user);
        }
        public async Task InitAsync()
        {
            await DataBase.InitAsync<User>(); 
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            //List<User> users = new List<User>();

            //StorageFile file = await GetUserFileAsync();

            //using (Stream stream = await file.OpenStreamForReadAsync())
            //{
            //    if (stream.Length == 0)
            //        return users;

            //    using (BinaryReader reader = new BinaryReader(stream))
            //    {
            //        try
            //        {
            //            int count = reader.ReadInt32();
            //            for (int i = 0; i < count; i++)
            //            {
            //                User user = new User
            //                {
            //                    Username = reader.ReadString(),
            //                    PasswordHash = reader.ReadString()
            //                };
            //                users.Add(user);
            //            }
            //        }
            //        catch (EndOfStreamException)
            //        {
            //            // 文件可能为空或格式不正确
            //        }
            //    }
            //}
            await InitAsync();
            List<User> users = await DataBase.GetDataAsync<User>();
            return users;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            //List<User> users = await GetAllUsersAsync();
            //return users.FirstOrDefault(u => u.Username == username);

            await InitAsync();
            List<User> users = await DataBase.EncryptedDb.QueryAsync<User>("select * from User where UserName = ?", username);

            return users.FirstOrDefault(u => u.UserName == username);
        }
    }
}
