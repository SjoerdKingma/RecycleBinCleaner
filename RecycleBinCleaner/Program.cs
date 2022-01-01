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
using RecycleBinCleaner.Shared;

namespace RecycleBinCleaner
{
    class Program
    {
        public static Logger.Logger logger{ get; set; }

        [STAThread]
        static void Main(string[] args)
        {
            //Initialize Logger
            try
            {
               logger = new Logger.Logger("Log.json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while initializing the logger.");
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                Environment.Exit(0);
            }

            //Get user settings
            UserSettings userSettings = GetUserSettings();

            //Convert properties in UserSettings.json
            List<string> filesToDelete = userSettings.FilesToDelete;
            bool includeFileExtension = bool.Parse(userSettings.IncludesFileExtension);
            bool logDeletedFilenames = bool.Parse(userSettings.LogDeletedFilenames);
            bool caseSensitive = bool.Parse(userSettings.CaseSensitiveFilenames);

            //Clean recycle bin
            try
            {
                CleanBinResult result = RecycleBinHelper.CleanBin(filesToDelete, includeFileExtension, caseSensitive);
                Console.WriteLine("{0} Succesfully deleted.", result.FilesSuccessfullyRemoved.Count);
                Console.WriteLine("{0} Failed to delete.", result.FilesFailedToRemove.Count);

                if (logDeletedFilenames)
                {
                    logger.CreateLog(result);
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

        private static UserSettings GetUserSettings()
        {
            string userSettingsPath = "UserSettings.json";
            string jsonString = "";

            try
            {
                StreamReader r = new StreamReader(userSettingsPath);
                jsonString = r.ReadToEnd();
                r.Close();
            }
            catch(Exception ex)
            {
                logger.CreateLog(ex.ToString(), 2);
                Console.WriteLine("Error getting user settings.");
                Console.WriteLine(ex.ToString());
                Environment.Exit(0);
            }
            
            UserSettingsRoot userSettingsRoot = JsonConvert.DeserializeObject<UserSettingsRoot>(jsonString);

            return userSettingsRoot.UserSettings;
        }
    }
}
