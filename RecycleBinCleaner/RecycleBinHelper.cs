using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shell32;
using RecycleBinCleaner.Shared;

namespace RecycleBinCleaner
{
    public class RecycleBinHelper
    {
        [STAThread]
        public static CleanBinResult CleanBin(List<string> filesToDelete, bool includesFileExtension, bool caseSensitive)
        {
            //Initialize user input
            List<string> input = filesToDelete;

            if (!caseSensitive)
            {
                //Convert all filesToDelete to lowercase
                input = filesToDelete.ConvertAll(x => x.ToLower());
            }

            // Initialize this method return value
            CleanBinResult result = new CleanBinResult()
            {
                FilesSuccessfullyRemoved = new List<BinResultItemSuccess>(),
                FilesFailedToRemove = new List<BinResultItemFailed>()
            };

            Shell32.Shell shl = new Shell32.Shell();

            // Get recycle folder
            Folder recycler = shl.NameSpace(10);

            //Get all items in recycle bin
            FolderItems items = recycler.Items();
            int binItemCount = items.Count;
            Console.WriteLine("Total items in recycle bin: " + binItemCount);

            for (int i = 0; i < binItemCount; i++)
            {
                string originalFileName = "";
                try
                {
                    FolderItem fi = items.Item(i);
                    originalFileName = recycler.GetDetailsOf(fi, 0);
                    string fileName = originalFileName;
                    string originalFilePath = recycler.GetDetailsOf(fi, 1);
                    string originalFileDeletedDate = recycler.GetDetailsOf(fi, 2);

                    foreach (string item in input)
                    {
                        string fileNameInput = "";

                        //Check if the user added file extensions to the filesToDelete
                        if (includesFileExtension)
                        {
                            fileNameInput = item;
                        }
                        else
                        {
                            //Remove file extensions before comparison
                            fileNameInput = item.Split('.')[0];
                            fileName = fileName.Split('.')[0];
                        }

                        if (!caseSensitive)
                        {
                            //Convert to lowercase before comparison
                            fileName = fileName.ToLower();
                        }

                        //The comparison
                        if (fileNameInput == fileName)
                        {
                            // Decide whether it's a folder or file that needs to be deleted
                            if (fi.IsFolder)
                            {
                                Directory.Delete(fi.Path, true);
                            }
                            else
                            {
                                File.Delete(fi.Path);
                            }

                            result.FilesSuccessfullyRemoved.Add(new BinResultItemSuccess() 
                            {
                                Filename = originalFileName,
                                OriginalFilePath = originalFilePath,
                                OriginalFileDeletedDate = originalFileDeletedDate
                            });
                        }
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    result.FilesFailedToRemove.Add(new BinResultItemFailed()
                    {
                        Filename = originalFileName,
                        Errormessage = exc.ToString()
                    });
                }
            }

            return result;
        }
    }
}
