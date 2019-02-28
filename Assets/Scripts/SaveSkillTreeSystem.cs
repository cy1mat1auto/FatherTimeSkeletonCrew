using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSkillTreeSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init()
    {
        // Test if Save Folder exists
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveString) 
    {
        int saveNumber = 1;

        while (File.Exists(SAVE_FOLDER + "STsave_" + saveNumber + ".txt"))
        {
            saveNumber++;
        }

        File.WriteAllText(SAVE_FOLDER + "STsave_" + saveNumber + ".txt", saveString);
        Debug.Log("Saved!");
    }


    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
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
            Debug.Log("Loaded!");
            return File.ReadAllText(mostRecentFile.FullName);
        }
        else
        {
            Debug.Log("File Doesn't exist!");
            return null;
        }
    }
}
