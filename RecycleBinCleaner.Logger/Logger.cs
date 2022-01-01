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
            JsonRoot jsonRoot = JsonConvert.DeserializeObject<JsonRoot>(jsonString);
            Logs = jsonRoot.Logs;
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
            //Initialize new log
            Log newLog = new Log();
            newLog.Id = Guid.NewGuid().ToString();
            newLog.Date = DateTime.Now.ToString();
            newLog.Code = "1";
            newLog.Message = "Clean Bin Result";
            newLog.ApplicationResult = new ApplicationResult()
            {
                FilesSuccesfullyRemoved = new List<FilesSuccesfullyRemoved>(),
                FilesFailedToRemove = new List<FilesFailedToRemove>()
            };

            //Add succesful files to new log
            foreach(var succesfulItem in cleanBinResult.FilesSuccessfullyRemoved)
            {
                newLog.ApplicationResult.FilesSuccesfullyRemoved.Add(new FilesSuccesfullyRemoved()
                {
                    Filename = succesfulItem.Filename,
                    DeletedDate = succesfulItem.OriginalFileDeletedDate,
                    OriginalPath = succesfulItem.OriginalFilePath
                });
            }

            //Add failed files to new log
            foreach (var failedItem in cleanBinResult.FilesFailedToRemove)
            {
                newLog.ApplicationResult.FilesFailedToRemove.Add(new FilesFailedToRemove() 
                {
                    Filename = failedItem.Filename,
                    OriginalPath = failedItem.OriginalFilePath,
                    ErrorMessage  = failedItem.Errormessage
                });
            }

            Logs.Add(newLog); //Add the log
        }

        public void UpdateLogFile()
        {
            string jsonString = JsonConvert.SerializeObject(new JsonRoot() {Logs = Logs });
            File.WriteAllText(FilePath, jsonString, Encoding.Default);
        }
    }
}
