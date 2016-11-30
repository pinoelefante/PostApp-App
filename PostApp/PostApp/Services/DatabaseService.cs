using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Services
{
    public class DatabaseService
    {
        private SQLiteConnection conn;
        public DatabaseService()
        {
            conn = Xamarin.Forms.DependencyService.Get<ISQLite>().GetConnection();
        }
    }
}
