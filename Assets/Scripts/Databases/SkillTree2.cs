using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillTree2 : MonoBehaviour
{
    // All upgrade Toggles
    // ==========================
    // Tier 1
    // Weapons
    public Toggle TopGradeCrystal;
    public Toggle AIMissiles;
    public Toggle HeatSink;

    // Defensive
    public Toggle LaminatedCladding;
    public Toggle Security;
    public Toggle Reliability;

    // Crew
    public Toggle CrewSlot1;

    // Tier 2
    // Weapons
    public Toggle GravityWell;
    public Toggle Reflectors;

    // Defensive
    public Toggle CompositeCladding;
    public Toggle CausticShield;
    public Toggle Flares;

    // Crew
    public Toggle CrewSlot2;
    public Toggle MedicalBay;

    // Tier 3
    // Weapons
    public Toggle BoreHoles;
    public Toggle EMP;

    // Defensive
    public Toggle BatteringRam;
    public Toggle AllClear;

    // Crew
    public Toggle CrewSlot3;

    // Tier 4
    // Weapons
    public Toggle Buddy;

    // Defensive
    public Toggle DragonScales;

    // ==========================

    // Player Database Variables
    public int totalXP = PlayerDatabase.playerXP;

    // List of all toggles
    private List<Toggle> toggles = new List<Toggle>(); 

    // Furthest unlocked level (Important !! STARTS AT 1 BY DEFAULT !!) 
    public int unlockedTier = 1;

    // Number of skills purchased from each level
    private int NumUnlockedT1 = 0;
    private int NumUnlockedT2 = 0;
    private int NumUnlockedT3 = 0;
    private int NumUNlockedT4 = 0;

    // Number of skills from previous level needed to unlock this level
    public int NumT1ToUnlockT2;
    public int NumT2ToUnlockT3;
    public int NumT3ToUnlockT4;

    // Start method called when script starts
    public void Start()
    {
        AddToggles();
    }

    public void Update()
    {
        EnableToggles();
    }

    // Enable/Disable buttons based on their level and current level and update sprites
    private void EnableToggles()
    {
        foreach (Toggle toggle in toggles)
        {
            Debug.Log("test: " + toggle);
            Debug.Log("length of buttons: " + toggles.Count);

            // Hopefully this is now redundant but who knows lol
            if (toggle is null) { Debug.Log("Null object"); }

            // Disables if button level is too high
            else if (toggle.GetComponent<SkillTreeButton>().tier > unlockedTier)
            {
                toggle.GetComponent<Toggle>().enabled = false;
                Debug.Log("Disabled, level too high: " + toggle);

                //// This is probably bad will need to fix later
                //toggle.transform.Find("purchased").GetComponent<Image>().enabled = false;
                //toggle.transform.Find("levelLock").GetComponent<Image>().enabled = true;
                //toggle.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }

            // Disables if its already been purchased
            else if (toggle.GetComponent<SkillTreeButton>().purchased)
            {
                toggle.GetComponent<Toggle>().enabled = false;
                Debug.Log("Disabled, already purchased: " + toggle);

                // This is probably bad will need to fix later
                //button.transform.Find("purchased").GetComponent<Image>().enabled = true;
                //button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                //button.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }

            // Disables if cant afford it 
            else if (totalXP < toggle.GetComponent<SkillTreeButton>().cost)
            {
                toggle.GetComponent<Toggle>().enabled = false;
                Debug.Log("Disabled, can't afford: " + toggle);

                // This is probably bad will need to fix later
                //button.transform.Find("purchased").GetComponent<Image>().enabled = false;
                //button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                //button.transform.Find("cantAfford").GetComponent<Image>().enabled = true;
            }

            // Otherwise enables button
            else
            {
                toggle.GetComponent<Toggle>().enabled = true; 
                Debug.Log("Enabled: " + toggle);

                // This is probably bad will need to fix later
                //button.transform.Find("purchased").GetComponent<Image>().enabled = false;
                //button.transform.Find("levelLock").GetComponent<Image>().enabled = false;
                //button.transform.Find("cantAfford").GetComponent<Image>().enabled = false;
            }
        }
    }

    // Adds Toggles to list <Toggles>
    private void AddToggles()
    {
        // Tier 1
        // Weapons
        if (TopGradeCrystal != null) { toggles.Add(TopGradeCrystal); }
        if (AIMissiles != null) { toggles.Add(AIMissiles); }
        if (HeatSink != null) { toggles.Add(HeatSink); }

        // Defense
        if (LaminatedCladding != null) { toggles.Add(LaminatedCladding); }
        if (Security != null) { toggles.Add(Security); }
        if (Reliability != null) { toggles.Add(Reliability); }

        // Crew
        if (CrewSlot1 != null) { toggles.Add(CrewSlot1); }

        // Tier 2
        // Weapons
        if (GravityWell != null) { toggles.Add(GravityWell); }
        if (Reflectors != null) { toggles.Add(Reflectors); }

        // Defense
        if (CompositeCladding != null) { toggles.Add(CompositeCladding); }
        if (CausticShield != null) { toggles.Add(CausticShield); }

        // Crew
        if (CrewSlot2 != null) { toggles.Add(CrewSlot2); }
        if (MedicalBay != null) { toggles.Add(MedicalBay); }

        // Tier 3
        // Weapons
        if (BoreHoles != null) { toggles.Add(BoreHoles); }
        if (EMP != null) { toggles.Add(EMP); }

        // Defense
        if (BatteringRam != null) { toggles.Add(BatteringRam); }
        if (AllClear != null) { toggles.Add(AllClear); }

        // Crew
        if (CrewSlot3 != null) { toggles.Add(CrewSlot3); }

        // Tier 4
        // Weapons
        if (Buddy != null) { toggles.Add(Buddy); }

        // Defense
        if (DragonScales != null) { toggles.Add(DragonScales); }
    }

    // Unlocks next level if possible and updates enability of buttons as necessary
    public void UnlockNextTier(int purchasedThisLevel, int neededToUnlockNextLevel, int levelToUnlock)
    {
        if (purchasedThisLevel >= neededToUnlockNextLevel && levelToUnlock > unlockedTier)
        {
            unlockedTier = levelToUnlock;
            Debug.Log("Unlocked next Tier. Current max unlocked level is: " + unlockedTier);
        }
    }

    // The acutal upgrade functions
    // =============================================================================================
    // Tier 1
    public void TopGradeCrystalUpgrade()
    {
        PlayerDatabase.laserDamage += (int)(PlayerDatabase.laserDamage * 0.2);

        totalXP -= TopGradeCrystal.GetComponent<SkillTreeButton>().cost;
        TopGradeCrystal.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Top Grade Crystal Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void AIMissilesUpgrade()
    {
        PlayerDatabase.missileSpeed += 10; // Temp missile man should change
        PlayerDatabase.missileRange += 10; // Temp missile man should change

        totalXP -= AIMissiles.GetComponent<SkillTreeButton>().cost;
        AIMissiles.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased AI Missiles Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void HeatSinkUpgrade()
    {
        PlayerDatabase.laserOverheatRate -= (int)(PlayerDatabase.laserOverheatRate * .5);
        PlayerDatabase.laserCoolingRate += (int)(PlayerDatabase.laserOverheatRate * .5);

        totalXP -= HeatSink.GetComponent<SkillTreeButton>().cost;
        HeatSink.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Heat Sink Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void LaminatedCladdingUpgrade()
    {
        PlayerDatabase.maxHealth += (int)(PlayerDatabase.maxHealth * .2);

        totalXP -= LaminatedCladding.GetComponent<SkillTreeButton>().cost;
        LaminatedCladding.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Laminated Cladding Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void SecurityUpgrade()
    {
        PlayerDatabase.shieldCapacity += (int)(PlayerDatabase.shieldCapacity * .25);

        totalXP -= Security.GetComponent<SkillTreeButton>().cost;
        Security.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Security Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void ReliabilityUpgrade()
    {
        PlayerDatabase.shieldRechargeRate += (int)(PlayerDatabase.shieldRechargeRate * .5);

        totalXP -= Reliability.GetComponent<SkillTreeButton>().cost;
        Reliability.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Reliability Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    public void CrewSlot1Upgrade()
    {
        PlayerDatabase.crewSlots++; // Temp crew man might want to change to = 2

        totalXP -= CrewSlot1.GetComponent<SkillTreeButton>().cost;
        CrewSlot1.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT1++;
        Debug.Log("Purchased Crew Slot 1 Upgrade");
        UnlockNextTier(NumUnlockedT1, NumT1ToUnlockT2, 2);
    }

    // Tier 2
    public void GravityWellUpgrade()
    {
        // temp missile man should change
        PlayerDatabase.missileExplosionRadius += (int)(PlayerDatabase.missileExplosionRadius * .25);
        PlayerDatabase.missileExplosionSlowRate += (int)(PlayerDatabase.missileExplosionSlowRate * .25);

        totalXP -= GravityWell.GetComponent<SkillTreeButton>().cost;
        GravityWell.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Gravity Well Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void ReflectorsUpgrade()
    {
        // temp laser man should change
        PlayerDatabase.laserReflectorLevel++;
        PlayerDatabase.laserReflectorDamge = (int)(PlayerDatabase.laserDamage / 2);

        totalXP -= Reflectors.GetComponent<SkillTreeButton>().cost;
        Reflectors.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Reflectors Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void CompositeCladdingUpgrade()
    {
        PlayerDatabase.maxHealth += (int)(PlayerDatabase.maxHealth * .5);

        totalXP -= CompositeCladding.GetComponent<SkillTreeButton>().cost;
        CompositeCladding.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Composite Cladding Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void CausticShieldUpgrade()
    {
        PlayerDatabase.canShildDamage = true;
        PlayerDatabase.shieldDamage += 10; // temp shield man should change

        totalXP -= CausticShield.GetComponent<SkillTreeButton>().cost;
        CausticShield.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Caustic Shield Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void FlaresUpgrade()
    {
        PlayerDatabase.flareCapacity = 2; // temp flares man should change

        totalXP -= Flares.GetComponent<SkillTreeButton>().cost;
        Flares.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Flares Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void CrewSlot2Upgrade()
    {
        PlayerDatabase.crewSlots++; // temp crew man should change

        totalXP -= CrewSlot2.GetComponent<SkillTreeButton>().cost;
        CrewSlot2.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Crew Slot 2 Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    public void MedicalBayUpgrade()
    {
        // No idea what should happen here

        totalXP -= MedicalBay.GetComponent<SkillTreeButton>().cost;
        MedicalBay.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT2++;
        Debug.Log("Purchased Medical Bay Upgrade");
        UnlockNextTier(NumUnlockedT2, NumT2ToUnlockT3, 3);
    }

    // Tier 3
    public void BoreHolesUpgrade()
    {
        PlayerDatabase.canLaserVitalComponentsDamge = true;

        totalXP -= BoreHoles.GetComponent<SkillTreeButton>().cost;
        BoreHoles.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT3++;
        Debug.Log("Purchased Bore Holes Upgrade");
        UnlockNextTier(NumUnlockedT3, NumT3ToUnlockT4, 4);
    }

    public void EMPUpgrade()
    {
        PlayerDatabase.canMissileDisableDrone = true;

        totalXP -= EMP.GetComponent<SkillTreeButton>().cost;
        EMP.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT3++;
        Debug.Log("Purchased EMP Upgrade");
        UnlockNextTier(NumUnlockedT3, NumT3ToUnlockT4, 4);
    }

    public void BatteringRamUpgrade()
    {
        PlayerDatabase.canBatteringRam = true;

        totalXP -= BatteringRam.GetComponent<SkillTreeButton>().cost;
        BatteringRam.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT3++;
        Debug.Log("Purchased Battering Ram Upgrade");
        UnlockNextTier(NumUnlockedT3, NumT3ToUnlockT4, 4);
    }

    public void AllClearUpgrade()
    {
        PlayerDatabase.canAllClear = true;

        totalXP -= BatteringRam.GetComponent<SkillTreeButton>().cost;
        BatteringRam.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT3++;
        Debug.Log("Purchased All Clear Upgrade");
        UnlockNextTier(NumUnlockedT3, NumT3ToUnlockT4, 4);
    }

    public void CrewSlot3Upgrade()
    {
        PlayerDatabase.crewSlots++; // temp crew man should change

        totalXP -= CrewSlot3.GetComponent<SkillTreeButton>().cost;
        CrewSlot3.GetComponent<SkillTreeButton>().purchased = true;
        NumUnlockedT3++;
        Debug.Log("Purchased Crew Slot 3 Upgrade");
        UnlockNextTier(NumUnlockedT3, NumT3ToUnlockT4, 4);
    }

    // Tier 4
    public void BuddyUpgrade()
    {
        PlayerDatabase.canReleaseDrones = true;

        totalXP -= Buddy.GetComponent<SkillTreeButton>().cost;
        Buddy.GetComponent<SkillTreeButton>().purchased = true;
        NumUNlockedT4++;
        Debug.Log("Purchased Buddy Upgrade");
        // UnlockNextTier(NumUnlockedT4, NumT4ToUnlockT5, 5)
    }

    public void DragonScalesUpgrade()
    {
        PlayerDatabase.maxHealth += (int)(PlayerDatabase.maxHealth * .5);
        PlayerDatabase.canHealthScales = true;

        totalXP -= DragonScales.GetComponent<SkillTreeButton>().cost;
        DragonScales.GetComponent<SkillTreeButton>().purchased = true;
        NumUNlockedT4++;
        Debug.Log("Purchased Dragon Scales Upgrade");
        // UnlockNextTier(NumUnlockedT4, NumT4ToUnlockT5, 5)
    }
    // =============================================================================================

    // Save
    public void Save()
    {
        List<int> costs = new List<int>();
        List<bool> purchased = new List<bool>();
        List<int> tiers = new List<int>();

        foreach (Toggle toggle in toggles)
        {
            costs.Add(toggle.GetComponent<SkillTreeButton>().cost);
            purchased.Add(toggle.GetComponent<SkillTreeButton>().purchased);
            tiers.Add(toggle.GetComponent<SkillTreeButton>().tier);
        }

        SaveObject saveObject = new SaveObject
        {
            unlockedTier = unlockedTier,

            NumUnlockedT1 = NumUnlockedT1,
            NumUnlockedT2 = NumUnlockedT2,
            NumUnlockedT3 = NumUnlockedT3,
            NumUllockedT4 = NumUNlockedT4,

            NumT1ToUnlockT2 = NumT1ToUnlockT2,
            NumT2ToUnlockT3 = NumT2ToUnlockT3,
            NumT3ToUnlockT4 = NumT3ToUnlockT4,

            costs = costs,
            purchased = purchased,
            tiers = tiers,
        };

        string json = JsonUtility.ToJson(saveObject);

        SaveSkillTree.Save(json);

    }

    // Load
    public void Load()
    {
        string saveString = SaveSkillTree.Load();

        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        unlockedTier = saveObject.unlockedTier;

        NumUnlockedT1 = saveObject.NumUnlockedT1;
        NumUnlockedT2 = saveObject.NumUnlockedT2;
        NumUnlockedT3 = saveObject.NumUnlockedT3;
        NumUNlockedT4 = saveObject.NumUllockedT4;

        NumT1ToUnlockT2 = saveObject.NumT1ToUnlockT2;
        NumT2ToUnlockT3 = saveObject.NumT2ToUnlockT3;
        NumT3ToUnlockT4 = saveObject.NumT3ToUnlockT4;

        for (int i = 0; i < toggles.Count; i++)
        {
            toggles[i].GetComponent<SkillTreeButton>().cost = saveObject.costs[i];
            toggles[i].GetComponent<SkillTreeButton>().purchased = saveObject.purchased[i];
            toggles[i].GetComponent<SkillTreeButton>().tier = saveObject.tiers[i];
        }
    }

    // Save Object
    private class SaveObject
    {
        // Furthest unlocked level (Important !! STARTS AT 1 BY DEFAULT !!) 
        public int unlockedTier;

        // Number of skills purchased from each level
        public int NumUnlockedT1;
        public int NumUnlockedT2;
        public int NumUnlockedT3;
        public int NumUllockedT4;

        // Number of skills from previous level needed to unlock this level
        public int NumT1ToUnlockT2;
        public int NumT2ToUnlockT3;
        public int NumT3ToUnlockT4;

        // Related lists for all the stuff data in buttons
        public List<int> costs;
        public List<bool> purchased;
        public List<int> tiers;
    }
}
