using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RecycleBinCleaner.Logger
{
    // JsonRoot myDeserializedClass = JsonConvert.DeserializeObject<JsonRoot>(myJsonResponse);
    public class JsonRoot
    {
        public List<Log> Logs { get; set; }
    }
    public class Log
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        [JsonProperty("Application Result")]
        public ApplicationResult ApplicationResult { get; set; }
    }
    public class ApplicationResult
    {
        [JsonProperty("Files succesfully removed")]
        public List<FilesSuccesfullyRemoved> FilesSuccesfullyRemoved { get; set; }

        [JsonProperty("Files failed to remove")]
        public List<FilesFailedToRemove> FilesFailedToRemove { get; set; }
    }

    public abstract class FileResult
    {
        public string Filename { get; set; }

        [JsonProperty("Deleted Date")]
        public string DeletedDate { get; set; }

        [JsonProperty("Original Path")]
        public string OriginalPath { get; set; }
    }
    public class FilesSuccesfullyRemoved : FileResult
    {

    }

    public class FilesFailedToRemove : FileResult
    {
        [JsonProperty("Error message")]
        public string ErrorMessage { get; set; }
    }
}
