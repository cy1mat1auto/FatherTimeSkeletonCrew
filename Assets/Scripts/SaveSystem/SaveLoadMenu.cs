using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadMenu : MonoBehaviour
{
    // the skill tree object of the scene
    public SkillTree2 skillTree2;

    public List<Text> saveSlotNames;

    public void Start()
    {
        Debug.Log("Current save slot: " + SystemData.cur_save_folder);
    }

    // Save slot names
    public enum SaveSlots
    {
        SaveSlot1_,
        SaveSlot2_,
        SaveSlot3_,
        SaveSlot4_,
        SaveSlot5_,
    }

    // When save buttons are pressed
    public void OnSave1()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot1_.ToString();
        Save();
        SetCurrentSSNameGreen(0);
        Debug.Log("Saved (ss1)");
    }

    public void OnSave2()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot2_.ToString();
        Save();
        SetCurrentSSNameGreen(1);
        Debug.Log("Saved (ss2)");
    }

    public void OnSave3()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot3_.ToString();
        Save();
        SetCurrentSSNameGreen(2);
        Debug.Log("Saved (ss3)");
    }

    public void OnSave4()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot4_.ToString();
        Save();
        SetCurrentSSNameGreen(3);
        Debug.Log("Saved (ss4)");
    }

    public void OnSave5()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot5_.ToString();
        Save();
        SetCurrentSSNameGreen(4);
        Debug.Log("Saved (ss5)");
    }


    // When load buttons are pressed
    public void OnLoad1()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot1_.ToString();
        Load();
        SetCurrentSSNameGreen(0);
        Debug.Log("Loaded (ss1)");
    }

    public void OnLoad2()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot2_.ToString();
        Load();
        SetCurrentSSNameGreen(1);
        Debug.Log("Loaded (ss2)");
    }

    public void OnLoad3()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot3_.ToString();
        Load();
        SetCurrentSSNameGreen(2);
        Debug.Log("Loaded (ss3)");
    }

    public void OnLoad4()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot4_.ToString();
        Load();
        SetCurrentSSNameGreen(3);
        Debug.Log("Loaded (ss4)");
    }

    public void OnLoad5()
    {
        ClearAllNameColors();
        SystemData.cur_save_folder = SaveSlots.SaveSlot5_.ToString();
        Load();
        SetCurrentSSNameGreen(4);
        Debug.Log("Loaded (ss5)");
    }


    // Saves the data to the selected folder
    private void Save()
    {
        PlayerDatabase.Save();
        skillTree2.Save();
        Debug.Log("Current save slot: " + SystemData.cur_save_folder);
    }

    // Loads the data to the selected folder
    private void Load()
    {
        PlayerDatabase.Load();
        skillTree2.Load();
        Debug.Log("Current save slot: " + SystemData.cur_save_folder);
    }

    // Clears the colors of each name
    private void ClearAllNameColors()
    {
        foreach (Text ssName in saveSlotNames)
        {
            ssName.color = Color.white;
        }
    }

    // Sets color of selected (green)
    private void SetCurrentSSNameGreen(int ssIndex)
    {
        saveSlotNames[ssIndex].color = Color.green;
    }
} 
