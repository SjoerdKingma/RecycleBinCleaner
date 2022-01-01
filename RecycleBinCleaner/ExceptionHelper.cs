using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleBinCleaner
{
    public class ExceptionHelper
    {
        public static void Log(Exception ex, Logger.Logger logger)
        {
            if(logger != null)
            {
                logger.CreateLog(ex.ToString(), 2);
                logger.UpdateLogFile();
            }
            else
            {
                Console.WriteLine("Could not log error because logger was NULL.");
            }
            
            Console.WriteLine("Error getting user settings.");
            Console.WriteLine(ex.ToString());
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
