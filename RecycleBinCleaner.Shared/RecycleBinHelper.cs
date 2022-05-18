using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Shell32;

namespace RecycleBinCleanerLibrary
{
    public class RecycleBinHelper
    {
        [STAThread]
        public static CleanBinResult CleanBin(List<string> input, bool includesFileExtension, bool caseSensitive)
        {
            //Initialize user filesToDelete
            List<string> filesToDelete = modifyUserInput(input, caseSensitive);

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
                //Initialize properties for the CleanBinResultItem
                string originalFileName = "";
                string originalFilePath = "";
                string originalFileDeletedDate = "";

                try
                {
                    FolderItem fi = items.Item(i);
                    originalFileName = recycler.GetDetailsOf(fi, 0);
                    string fileName = originalFileName;
                    originalFilePath = recycler.GetDetailsOf(fi, 1);
                    originalFileDeletedDate = recycler.GetDetailsOf(fi, 2);

                    foreach (string item in filesToDelete)
                    {
                        string fileNameInput = "";

                        //Check if the user added file extensions to the input
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
                        OriginalFilePath = originalFilePath,
                        OriginalFileDeletedDate = originalFileDeletedDate,
                        Errormessage = exc.ToString()
                    });
                }
            }

            return result;
        }

        private static List<string> modifyUserInput(List<string> input, bool caseSensitive)
        {
            List<string> result = input;
            if (!caseSensitive)
            {
                //Convert all to lowercase
                result = result.ConvertAll(x => x.ToLower());
            }

            return result;
        }
    }
}
