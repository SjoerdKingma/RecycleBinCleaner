using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleBinCleaner
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class UserSettings
    {
        public List<string> FilesToDelete { get; set; }
        public string CaseSensitiveFilenames { get; set; }
        public string IncludesFileExtension { get; set; }
        public string LogDeletedFilenames { get; set; }
    }

    public class UserSettingsRoot
    {
        public UserSettings UserSettings { get; set; }
    }


}
