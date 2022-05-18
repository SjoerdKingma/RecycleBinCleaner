using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using RecycleBinCleaner;
using Newtonsoft.Json;
using RecycleBinCleaner.Logger;
using RecycleBinCleanerLibrary;
using RecycleBinCleaner.UserSettings;

namespace RecycleBinCleaner
{
    class Program
    {
        public static Logger.Logger logger { get; set; }
        public static string logPath = "Log.json";
        public static string userSettingsPath = "UserSettings/UserSettings.json";

        [STAThread]
        static void Main(string[] args)
        {
            //Initialize Logger
            try
            {
                logger = new Logger.Logger(logPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while initializing the logger.");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                Environment.Exit(0);
            }

            //Get user settings
            UserSettingsHelper userSettingsHelper = new UserSettingsHelper(logger);
            UserSettings.UserSettings userSettings = userSettingsHelper.GetUserSettings(userSettingsPath);

            //Convert properties in UserSettings.json
            List<string> input = userSettings.FilesToDelete;
            bool includeFileExtension = bool.Parse(userSettings.IncludesFileExtension);
            bool logDeletedFilenames = bool.Parse(userSettings.LogDeletedFilenames);
            bool caseSensitive = bool.Parse(userSettings.CaseSensitiveFilenames);

            //Clean recycle bin
            try
            {
                CleanBinResult result = RecycleBinHelper.CleanBin(input, includeFileExtension, caseSensitive);
                Console.WriteLine("{0} Succesfully deleted.", result.FilesSuccessfullyRemoved.Count);
                Console.WriteLine("{0} Failed to delete.", result.FilesFailedToRemove.Count);

                if (logDeletedFilenames)
                {
                    logger.CreateLog(result);
                }
            }
            catch (Exception ex)
            {
                logger.CreateLog(ex.ToString(), 2);
            }
            finally
            {
                logger.UpdateLogFile();
            }
        }
    }
}
