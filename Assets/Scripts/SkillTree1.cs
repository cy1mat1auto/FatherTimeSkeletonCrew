using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// Error where the foreach loop stops when it reaches a null element but i need it to keep going, otherwise i need to remove all null elements from the <buttons> list

public class SkillTree1 : MonoBehaviour
{
    // Values to be modified ==== (these should be made static later I think) ==== (being saved)
    public int maxHealth;
    public int laserStrength;
    public int totalXP;

    // All buttons in skill tree (being saved)
    public Button healthIncr1Button;
    public Button laserStrIncr1Button;

    public Button healthIncr2Button;
    public Button laserStrIncr2Button;

    public Button healthIncr3Button;
    public Button laserStrIncr3Button;

    // List of all buttons in skill tree (doesn't get changed)
    private List<Button> buttons = new List<Button>();

    // Furthest unlocked level (Important !! STARTS AT 1 BY DEFAULT !!) 
    public int unlockedLevel = 1;

    // Number of skills purchased from each level
    private int NumUnlockedL1 = 0;
    private int NumUnlockedL2 = 0;
    private int NumUnlockedL3 = 0;

    // Number of skills from previous level needed to unlock this level
    public int NumL1ToUnlockL2;
    public int NumL2ToUnlockL3;

    // Different sprites overlayed over the buttons depending on their state
    // This is the better way of doing the thing i did the poor way
    public Sprite purchasedSprite;
    public Sprite cantAffordSprite;
    public Sprite levelTooLowSprite;

    // Start method called when script starts
    public void Start()
    {
        // Add all buttons to list (there should be a simpler way of doing it)
        AddButtons();
    }

    // Update method called once per frame
    public void Update()
    {
        // If player presses <S> save game
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        // If player presses <L> load game
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }


        // Check each button to see if its available to purchase and enable if yes
        EnableButtons();
    }

    // Increase maxHealth by 10 points
    public void IncreaseHealth1()
    {
        maxHealth += 10;
        totalXP -= healthIncr1Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        healthIncr1Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL1++;
        Debug.Log("IncreaseHealth1: Increased max health by 10. Health is now: " + maxHealth);

        // Checks if we can unlock next level and unlocks if we can
        UnlockNextLevel(NumUnlockedL1, NumL1ToUnlockL2, 2);
    }

    // Increase maxHealth by 20 points
    public void IncreaseHealth2()
    {
        maxHealth += 20;
        totalXP -= healthIncr2Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        healthIncr2Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL2++;
        Debug.Log("IncreaseHealth2: Increased max health by 20. Health is now: " + maxHealth);

        // Checks if we can unlock next level and unlocks if we can
        UnlockNextLevel(NumUnlockedL2, NumL2ToUnlockL3, 3);
    }

    // Increase maxHealth by 30 points
    public void IncreaseHealth3()
    {
        maxHealth += 30;
        totalXP -= healthIncr3Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        healthIncr3Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL3++;
        Debug.Log("IncreaseHealth3: Increased max health by 30. Health is now: " + maxHealth);
    }

    // Increase laserStrength by 10 points
    public void IncreaseLaserStr1()
    {
        laserStrength += 10;
        totalXP -= laserStrIncr1Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        laserStrIncr1Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL1++;
        Debug.Log("IncreaseLaserStr: Increased laser strength by 10. Laser strength is now: " + laserStrength);

        // Checks if we can unlock next level and unlocks if we can
        UnlockNextLevel(NumUnlockedL1, NumL1ToUnlockL2, 2);
    }

    // Increase laserStrength by 20 points
    public void IncreaseLaserStr2()
    {
        laserStrength += 20;
        totalXP -= laserStrIncr2Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        laserStrIncr2Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL2++;
        Debug.Log("IncreaseLaserStr: Increased laser strength by 20. Laser strength is now: " + laserStrength);

        // Checks if we can unlock next level and unlocks if we can
        UnlockNextLevel(NumUnlockedL2, NumL2ToUnlockL3, 3);
    }

    // Increase laserStrength by 30 points
    public void IncreaseLaserStr3()
    {
        laserStrength += 30;
        totalXP -= laserStrIncr3Button.GetComponent<SkillTreeButton>().cost;  // Subtracts cost from total XP
        laserStrIncr3Button.GetComponent<SkillTreeButton>().purchased = true; // Sets purchased to true
        NumUnlockedL3++;
        Debug.Log("IncreaseLaserStr: Increased laser strength by 30. Laser strength is now: " + laserStrength);
    }

    // Unlocks next level if possible and updates enability of buttons as necessary
    public void UnlockNextLevel(int purchasedThisLevel, int neededToUnlockNextLevel, int levelToUnlock)
    {
        if (purchasedThisLevel >= neededToUnlockNextLevel && levelToUnlock > unlockedLevel) 
        { 
            unlockedLevel = levelToUnlock;
            Debug.Log("Unlocked next level. Current max unlocked level is: " + unlockedLevel);
        }
    }

    // Adds buttons to list <buttons>
    private void AddButtons()
    {
        // Should be a way of simplifying this but idk how
        if (healthIncr1Button != null) { buttons.Add(healthIncr1Button); }
        if (laserStrIncr1Button != null) { buttons.Add(laserStrIncr1Button); }

        if (healthIncr2Button != null) { buttons.Add(healthIncr2Button); }
        if (laserStrIncr2Button != null) { buttons.Add(laserStrIncr2Button); }

        if (healthIncr3Button != null) { buttons.Add(healthIncr3Button); }
        if (laserStrIncr3Button != null) { buttons.Add(laserStrIncr3Button); }

        Debug.Log("Add button test length: " + buttons.Count);

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            Debug.Log("Add button test: " + button);
        }
    }

    // Enable/Disable buttons based on their level and current level and update sprites
    private void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            Debug.Log("test: " + button);
            Debug.Log("length of buttons: " + buttons.Count);

            // Hopefully this is now redundant but who knows lol
            if (button is null) { Debug.Log("Null object"); }

            // Disables if button level is too high
            else if (button.GetComponent<SkillTreeButton>().tier > unlockedLevel)
            {
                button.GetComponent<Button>().enabled = false;
                Debug.Log("Disabled, level too high: " + button);

                // This is probably bad will need to fix later
                button.transform.Find("purchased").GetComponent<Image>().enabled = false;
                button.transform.Find("levelLock").GetComponent<Image>().enabled = true;
                button.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }

            // Disables if its already been purchased
            else if (button.GetComponent<SkillTreeButton>().purchased)
            {
                button.GetComponent<Button>().enabled = false;
                // button.GetComponent<Image>().color = new Color(0, 200, 0);
                Debug.Log("Disabled, already purchased: " + button);

                // This is probably bad will need to fix later
                button.transform.Find("purchased").GetComponent<Image>().enabled = true;
                button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                button.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }

            // Disables if cant afford it 
            else if (totalXP < button.GetComponent<SkillTreeButton>().cost)
            {
                button.GetComponent<Button>().enabled = false;
                // button.GetComponent<Image>().color = new Color(211, 211, 211);
                Debug.Log("Disabled, can't afford: " + button);

                // This is probably bad will need to fix later
                button.transform.Find("purchased").GetComponent<Image>().enabled = false;
                button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                button.transform.Find("cantAfford").GetComponent<Image>().enabled = true;
            }

            // Otherwise enables button
            else 
            { 
                button.GetComponent<Button>().enabled = true; Debug.Log("Enabled: " + button);

                // This is probably bad will need to fix later
                button.transform.Find("purchased").GetComponent<Image>().enabled = false;
                button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                button.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }
        }
    }

    public void Save()
    {
        List<int> costs = new List<int>();
        List<bool> purchased = new List<bool>();
        List<int> levels = new List<int>();

        foreach (Button button in buttons)
        {
            costs.Add(button.GetComponent<SkillTreeButton>().cost);
            purchased.Add(button.GetComponent<SkillTreeButton>().purchased);
            levels.Add(button.GetComponent<SkillTreeButton>().tier);
        }

        SaveObject saveObject = new SaveObject
        {
            maxHealth = maxHealth,
            laserStrength = laserStrength,
            totalXP = totalXP,

            unlockedLevel = unlockedLevel,

            NumUnlockedL1 = NumUnlockedL1,
            NumUnlockedL2 = NumUnlockedL2,
            NumUnlockedL3 = NumUnlockedL3,

            NumL1ToUnlockL2 = NumL1ToUnlockL2,
            NumL2ToUnlockL3 = NumL2ToUnlockL3,

            costs = costs,
            purchased = purchased,
            levels = levels,
        };

        string json = JsonUtility.ToJson(saveObject);

        SaveSkillTreeSystem.Save(json);
    }

    public void Load()
    {
        string saveString = SaveSkillTreeSystem.Load();

        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        maxHealth = saveObject.maxHealth;
        laserStrength = saveObject.laserStrength;
        totalXP = saveObject.totalXP;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<SkillTreeButton>().cost = saveObject.costs[i];
            buttons[i].GetComponent<SkillTreeButton>().purchased = saveObject.purchased[i];
            buttons[i].GetComponent<SkillTreeButton>().tier = saveObject.levels[i];
        }

        unlockedLevel = saveObject.unlockedLevel;

        NumUnlockedL1 = saveObject.NumUnlockedL1;
        NumUnlockedL2 = saveObject.NumUnlockedL2;
        NumUnlockedL3 = saveObject.NumUnlockedL3;

        NumL1ToUnlockL2 = saveObject.NumL1ToUnlockL2;
        NumL2ToUnlockL3 = saveObject.NumL2ToUnlockL3;
    }

    private class SaveObject
    {
        // These should probably be database stuff
        public int maxHealth;
        public int laserStrength;
        public int totalXP;

        // Furthest unlocked level (Important !! STARTS AT 1 BY DEFAULT !!) 
        public int unlockedLevel;

        // Number of skills purchased from each level
        public int NumUnlockedL1;
        public int NumUnlockedL2;
        public int NumUnlockedL3;

        // Number of skills from previous level needed to unlock this level
        public int NumL1ToUnlockL2;
        public int NumL2ToUnlockL3;

        // Related lists for all the stuff data in buttons
        public List<int> costs;
        public List<bool> purchased;
        public List<int> levels;
    }
}
