using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.IO;
namespace KH_Capstone.LoggerPL
{
    public class Logger
    {
        public static string logPath;

        public static void LogSqlException(SqlException sqlEx)
        {
            LogException(sqlEx, sqlEx.Server);
        }

        public static void LogException(Exception ex, string Server = "")
        {
            StreamWriter sw = new StreamWriter(logPath);

            sw.WriteLine("***********");
            sw.WriteLine();
            sw.WriteLine("{0:MMMM dd, hh:mm tt}", DateTime.Now);
            sw.WriteLine();
            if(Server != string.Empty)
            {
                sw.WriteLine(Server);
                sw.WriteLine();
            }
            sw.WriteLine(ex.Message);
            sw.WriteLine();
            sw.WriteLine(ex.StackTrace);
            sw.WriteLine();

            sw.Close();
            sw.Dispose();
        }

    }
}