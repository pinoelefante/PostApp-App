using PostApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using PostApp.WinPhone.Services;
using Xamarin.Forms;
using Windows.Storage;
using System.IO;

[assembly : Dependency(typeof(SQLiteService_WinRT))]
namespace PostApp.WinPhone.Services
{
    public class SQLiteService_WinRT : ISQLite
    {
        public SQLiteService_WinRT() { }
        private readonly static string DATABASE_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
        public SQLiteConnection GetConnection()
        {
            SQLiteConnection conn = new SQLiteConnection(DATABASE_PATH);
            return conn;
        }
    }
}
