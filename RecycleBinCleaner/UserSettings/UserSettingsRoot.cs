using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleBinCleaner.UserSettings
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<UserSettingsRoot>(myJsonResponse); 
    public class UserSettingsRoot
    {
        public UserSettings UserSettings { get; set; }
    }
    public class UserSettings
    {
        public List<string> FilesToDelete { get; set; }
        public string CaseSensitiveFilenames { get; set; }
        public string IncludesFileExtension { get; set; }
        public string LogDeletedFilenames { get; set; }
    }
}
