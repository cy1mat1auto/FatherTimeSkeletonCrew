using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Error where the foreach loop stops when it reaches a null element but i need it to keep going, otherwise i need to remove all null elements from the <buttons> list

public class SkillTree1 : MonoBehaviour
{
    // Values to be modified ==== (these should be made static later I think) ====
    public int maxHealth;
    public int laserStrength;
    public int totalXP;

    // All buttons in skill tree
    public Button healthIncr1Button;
    public Button laserStrIncr1Button;

    public Button healthIncr2Button;
    public Button laserStrIncr2Button;

    public Button healthIncr3Button;
    public Button laserStrIncr3Button;

    // List of all buttons in skill tree
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

        //buttons.Add(healthIncr2Button);
        //Debug.Log("health 2: " + healthIncr2Button);
        //buttons.Add(laserStrIncr2Button);

        //buttons.Add(healthIncr3Button);
        //buttons.Add(laserStrIncr3Button);

        //foreach (Button button in buttons)
        //{
        //    Debug.Log("Add button test: " + button);
        //}
        Debug.Log("Add button test length: " + buttons.Count);

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            Debug.Log("Add button test: " + button);
        }
    }

    /*
    // Enable/Disable buttons based on their level and current level
    private void EnableButtonsOld()
    {   
        /* This loop doesnt work because there can be null elements in the list which the loop doesnt loop over
        foreach (Button button in buttons)
        {
            // Disables button if its level is greater than the unlockedLevel or if it's already been purchased 
            // or if you don't have enough XP to buy it 
            //                                                                                                                     ==== (should seperate this later to be better) ====
            if (button.GetComponent<SkillTreeButton>().level > unlockedLevel || button.GetComponent<SkillTreeButton>().purchased || totalXP < button.GetComponent<SkillTreeButton>().cost)
            {
                button.GetComponent<Button>().enabled = false;
                Debug.Log("Disabled: " + button);
            }

            else { button.GetComponent<Button>().enabled = true; Debug.Log("Enabled: " + button); }
        }

        /* this doesnt work
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null)
            {
                Button button = buttons[i];

                if (button.GetComponent<SkillTreeButton>().level > unlockedLevel || button.GetComponent<SkillTreeButton>().purchased || totalXP < button.GetComponent<SkillTreeButton>().cost)
                {
                    button.GetComponent<Button>().enabled = false;
                    Debug.Log("Disabled: " + button);
                }

                else { button.GetComponent<Button>().enabled = true; Debug.Log("Enabled: " + button); }
            }
        }
    } */

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
            else if (button.GetComponent<SkillTreeButton>().level > unlockedLevel)
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
}
