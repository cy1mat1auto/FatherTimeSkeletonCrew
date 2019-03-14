using UnityEngine;
using System.Collections;
using System.IO;

public static class SaveSkillTree
{
    // Name of subfolders of skill tree saves 
    public static string skill_tree_save_folder = "skill_tree_saves";
    public static string skill_tree_file_name = "skill_tree";

    // Folder of skill tree saves
    public static string current_folder_path = SystemData.MAIN_SAVE_FOLDER + SystemData.cur_save_folder + "/" + skill_tree_save_folder + "/";

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

        while (File.Exists(current_folder_path + skill_tree_file_name + saveNumber + ".txt"))
        {
            saveNumber++;
        }

        File.WriteAllText(current_folder_path + skill_tree_file_name + saveNumber + ".txt", saveString);

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

            return File.ReadAllText(mostRecentFile.FullName);
        }
        else
        {
            Debug.Log("File Doesn't exist!");
            return null;
        }
    }
}
