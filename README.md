# RecycelBinCleaner
Removes certain file names from Windows recycle bin, based on user input.

## How to use:

### 1. Download this and extract the contents to a new folder.
### 2. Set your user settings.
1. Navigate to the folder `UserSettings` and open the file inside called `UserSettings.json`.
2. Replace the sample file names with the filenames you wish to be deleted from the Recycle bin. 
    >Important: File names must be in double-quotes and seperated by a comma.
3. Save your changes to the document.

### 3. Run `RecycleBinCleaner.exe`.

### 4. Open `Logs.json` to view the results.
---
## 
## (Optional) Automatically launching this program with Windows Task Scheduler:
1. In the start menu, search for: `Task Scheduler` and open it.
2. On the left hand side, select the folder `Task Scheduler Library` by **left clicking** it. This step is important.
3. Right click the folder `Task Scheduler` and select `New Folder`. Give your folder a meaningful name such as 'MyTasks'.
4. Now create a new task by right clicking your custom folder and select `Create basic task`.
5. Follow the steps untill you reach `Action`. Here you want to select `RecycleBinCleaner.exe` as the action to be performed. Leave the optional fields blank.
6. When you reach the `Finish` tab, you can check `Open the Properties dialog for this task when I click Finish` checkbox to further customize the task once it's created.
