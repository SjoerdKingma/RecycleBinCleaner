using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecycleBinCleanerLibrary
{
    public class CleanBinResult
    {
        public List<BinResultItemSuccess> FilesSuccessfullyRemoved { get; set; }
        public List<BinResultItemFailed> FilesFailedToRemove { get; set; }
    }

    public class BinResultItemSuccess : BinResultItem
    {

    }

    public class BinResultItemFailed : BinResultItem
    {
        public string Errormessage { get; set; }
    }

    public abstract class BinResultItem
    {
        public string Filename { get; set; }
        public string OriginalFilePath { get; set; }
        public string OriginalFileDeletedDate { get; set; }
    }
}
