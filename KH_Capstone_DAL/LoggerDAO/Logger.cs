using System;
using System.Data.SqlClient;
using System.IO;

namespace KH_Capstone_DAL.LoggerDAO
{
    public static class Logger
    {
        public static string logPath;

        public static void LogSqlException(SqlException sqlEx)
        {
            LogException(sqlEx, sqlEx.Server);
        }

        public static void LogException(Exception ex, string Server = "")
        {
            StreamWriter sw = new StreamWriter(logPath, true);

            sw.WriteLine("******************");
            sw.WriteLine();
            sw.WriteLine("{0:MMMM dd, yyyy, hh:mm tt}", DateTime.Now);
            sw.WriteLine();
            sw.WriteLine(ex.Message);
            sw.WriteLine();
            
            if(Server != string.Empty)
            {
                sw.WriteLine(Server);
                sw.WriteLine();
            }

            sw.WriteLine(ex.StackTrace);
            sw.WriteLine();

            sw.Close();
            sw.Dispose();
        }
    }
}
