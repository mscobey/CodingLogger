using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using static System.Collections.Specialized.BitVector32;

namespace CodingTracker
{
    public class SqliteDataAccess
    {

        public static List<CodingSession> LoadSessions()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CodingSession>("select * from CodingSessions", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveSession(CodingSession session)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into CodingSessions(StartTime,EndTime,Duration) values (@StartTime,@EndTime,@Duration)", session);
            }
        }
        public static void UpdateSessions(CodingSession session)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update CodingSessions set StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration where Id = @id", session);
            }
        }
        public static void DeleteSessions(CodingSession session)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from CodingSessions where Id = @id", session);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
