using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using RecycleBinCleaner.Logger;

namespace RecycleBinCleaner.UserSettings
{
    public class UserSettingsHelper
    {
        private Logger.Logger logger = null;

        public UserSettingsHelper(Logger.Logger _logger)
        {
            logger = _logger;
        }

        public UserSettings GetUserSettings(string relativePath)
        {
            string jsonString = "";

            try
            {
                StreamReader r = new StreamReader(relativePath);
                jsonString = r.ReadToEnd();
                r.Close();
            }
            catch (Exception ex)
            {
                ExceptionHelper.Log(ex, logger);
            }

            UserSettingsRoot userSettingsRoot = JsonConvert.DeserializeObject<UserSettingsRoot>(jsonString);

            return userSettingsRoot.UserSettings;
        }
    }
}
