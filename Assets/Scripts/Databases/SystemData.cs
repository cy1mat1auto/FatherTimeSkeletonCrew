using UnityEngine;
using System.Collections;

public static class SystemData
{
    // This is the name of the subfolder that you will be saving to 
    // 
    // *** IF YOU WANT TO CHANGE FOLDER YOU ARE SAVING TO AND LOADING FROM, YOU MUST CHANGE THESE VARIABLES HERE ***

    public static string cur_save_folder = "start"; // This one changes if you want to access a different profile or different game state
    public static readonly string MAIN_SAVE_FOLDER = Application.dataPath + "/Saves/"; // This one should never really be changed at all
}
