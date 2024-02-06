using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code_Challenge_Aymen_Baccar.Classes
{
    public class Logging
    {
        public static void WriteLog(Exception ex)
        {
            try
            {
                string logDirectory = "LOGS";
                string logFileName = DateTime.Now.ToString("ddMMyyyy") + ".log";
                string logFilePath = Path.Combine(logDirectory, logFileName);

                // Assurez-vous que le répertoire LOGS existe
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                using (StreamWriter myFile = new StreamWriter(logFilePath, true))
                {
                    myFile.WriteLine(DateTime.Now + ";" + ex.Message + ";" + ex.StackTrace + "\r\n");
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
