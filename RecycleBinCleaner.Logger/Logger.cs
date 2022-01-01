using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecycleBinCleaner.Shared;

namespace RecycleBinCleaner.Logger
{   
    public class Logger
    {
        public List<Log> Logs { get; set; }
        public static string FilePath { get; set; }
        public Logger(string filePath)
        {
            //var x = Path.GetFullPath(filePath);
            FilePath = filePath;

            //Get all logs
            StreamReader r = new StreamReader(FilePath);
            string jsonString = r.ReadToEnd();
            r.Close();
            Root jsonRoot = JsonConvert.DeserializeObject<Root>(jsonString);
            Logs = jsonRoot.Logs;
            var sdf = 0;
        }

        public void CreateLog(string message, int logCode)
        {
            //Create new log
            Log newLog = new Log();
            newLog.Id = Guid.NewGuid().ToString();
            newLog.Date = DateTime.Now.ToString();
            newLog.Code = logCode.ToString();
            newLog.Message = message;

            Logs.Add(newLog);
        }

        public void CreateLog(CleanBinResult cleanBinResult)
        {
            //Create new log
            Log newLog = new Log();
            newLog.Id = Guid.NewGuid().ToString();
            newLog.Date = DateTime.Now.ToString();
            newLog.Code = "1";
            newLog.Message = "Scheduled Task";
            newLog.ApplicationResult = new ApplicationResult()
            {
                FilesSuccesfullyRemoved = new List<FilesSuccesfullyRemoved>(),
                FilesFailedToRemove = new List<FilesFailedToRemove>()
            };

            foreach(var succesfulItem in cleanBinResult.FilesSuccessfullyRemoved)
            {
                newLog.ApplicationResult.FilesSuccesfullyRemoved.Add(new FilesSuccesfullyRemoved()
                {
                    Filename = succesfulItem.Filename,
                    DeletedDate = succesfulItem.OriginalFileDeletedDate,
                    OriginalPath = succesfulItem.OriginalFilePath
                });
            }

            foreach(var failedItem in cleanBinResult.FilesFailedToRemove)
            {
                newLog.ApplicationResult.FilesFailedToRemove.Add(new FilesFailedToRemove() 
                {
                    Filename = failedItem.Filename,
                    OriginalPath = failedItem.OriginalFilePath,
                    ErrorMessage  = failedItem.Errormessage
                });
            }

            Logs.Add(newLog);
        }

        public void UpdateLogFile()
        {
            string jsonString = JsonConvert.SerializeObject(new Root() {Logs = Logs });
            File.WriteAllText(FilePath, jsonString, Encoding.Default);
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class FilesSuccesfullyRemoved
    {
        public string Filename { get; set; }

        [JsonProperty("Deleted Date")]
        public string DeletedDate { get; set; }

        [JsonProperty("Original Path")]
        public string OriginalPath { get; set; }
    }

    public class FilesFailedToRemove
    {
        public string Filename { get; set; }

        [JsonProperty("Deleted Date")]
        public string DeletedDate { get; set; }

        [JsonProperty("Original Path")]
        public string OriginalPath { get; set; }

        [JsonProperty("Error message")]
        public string ErrorMessage { get; set; }
    }

    public class ApplicationResult
    {
        [JsonProperty("Files succesfully removed")]
        public List<FilesSuccesfullyRemoved> FilesSuccesfullyRemoved { get; set; }

        [JsonProperty("Files failed to remove")]
        public List<FilesFailedToRemove> FilesFailedToRemove { get; set; }
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

    public class Root
    {
        public List<Log> Logs { get; set; }
    }
}
