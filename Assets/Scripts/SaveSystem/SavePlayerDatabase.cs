using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SavePlayerDatabase
{
    //*** IF YOU WANT TO CHANGE SAVE/LOAD LOCATION, YOU MUST DO IT IN: SystemData.cs ***
    public static string MAIN_SAVE_FOLDER = SystemData.MAIN_SAVE_FOLDER;
    public static string cur_save_folder = SystemData.cur_save_folder;

    // File path of saving and loading to
    public static string current_folder_path = MAIN_SAVE_FOLDER + cur_save_folder + "/";

    public static void CheckFolderPath()
    {
        // Test if Save Folder exists
        if (!Directory.Exists(current_folder_path))
        {
            Directory.CreateDirectory(current_folder_path);
            Debug.Log("CREATED NEW save subfolder: " + current_folder_path);
        }
    }

    // Save
    public static void Save(string saveString)
    {
        CheckFolderPath();

        int saveNumber = 1;

        while (File.Exists(current_folder_path + cur_save_folder + saveNumber + ".txt"))
        {
            saveNumber++;
        }

        File.WriteAllText(current_folder_path + cur_save_folder + saveNumber + ".txt", saveString);

        // TODO: Save skill tree here
        // SkillTree2.Save();

        Debug.Log("SAVED to: " + current_folder_path);
    }

    // Load
    public static string Load()
    {
        CheckFolderPath();

        DirectoryInfo directoryInfo = new DirectoryInfo(current_folder_path);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.txt");
        FileInfo mostRecentFile = null;

        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentFile == null)
            {
                mostRecentFile = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                {
                    mostRecentFile = fileInfo;
                }
            }
        }


        if (mostRecentFile != null)
        {
            Debug.Log("LOADED from: " + current_folder_path);

            // TODO: Load skill tree here
            // SkillTree2.Load();

            return File.ReadAllText(mostRecentFile.FullName);
        }
        else
        {
            Debug.Log("File Doesn't exist!");
            return null;
        }
    }
}
