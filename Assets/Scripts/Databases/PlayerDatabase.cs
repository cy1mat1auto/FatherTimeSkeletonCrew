using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDatabase
{
    // ================= BASE STATS =========================
    // Other stats that will need saving
    public static int movementSpeedBase;

    // Offensive Stats
    public static int laserDamageBase = 0; // How much damage lasers deal
    public static int missileRangeBase = 0; // Range of the missiles
    public static int missileSpeedBase = 0; // Speed of the missiles
    public static int laserOverheatRateBase = 0; // How fast laser overheats
    public static int laserCoolingRateBase = 0; // How fast laser cools

    public static int missileExplosionRadiusBase = 0; // Explosion radius of missiles
    public static int missileExplosionSlowRateBase = 0; // How much enemies in explosion radius slow down by
    public static int laserReflectorLevelBase = 0; // How many enemies a laser reflects on
    public static int laserReflectorDamgeBase = 0; // How much damage a reflected laser deals

    public static int laserVitalComponentsDamgageChanceBase = 0; // How likely a laser is too damage a vital component of enememy
    public static int MissileDisableDroneBase = 0; // Chance of missile explosion disabling drone ship (0 = impossible)

    public static int numDronesReleasedBase = 0; // How many drones get released to fight alongside you
    public static int numDronesBase = 0; // Number of drones your ship can release (not in skill tree but I thought it was useful)

    // Defensive Stats
    public static int maxHealthBase = 500; // Max health of the player
    public static int shieldCapacityBase = 100; // Shield Capacity of the player
    public static int shieldRechargeRateBase = 5; // Rate at which shield recharges

    public static int shieldDamageBase = 0; // How much damage your shield deals (0 = impossible)
    public static int flareCapacityBase = 0; // How much flares your ship carries or 
                                             // maybe how much flares it shoots out at once or 
                                             // turn into bool if it should always stay the same

    public static int batteringRamDamageDealtBase = 0; // How much damage you deal for battering enemy (0 = impossible)
    public static int batteringRamDamgeRecievedBase = 0; // How much pain you feel for battering enemy (0 = impossible)
    // =========================================================================

    // ============== MODIFIERS (Multipliers) =====================
    // Other stats that will need saving
    public static double movementSpeed = 1;
    public static int currentHealth = 1;
    public static Vector3 playerPosition = new Vector3(0,0,0);
    public static int playerXP = 100;

    // Offensive Stats
    public static double laserDamage = 1; // How much damage lasers deal
    public static double missileRange = 1; // Range of the missiles
    public static double missileSpeed = 1; // Speed of the missiles
    public static double laserOverheatRate = 1; // How fast laser overheats
    public static double laserCoolingRate = 1; // How fast laser cools

    public static double missileExplosionRadius = 1; // Explosion radius of missiles
    public static bool canMissileExplosionSlow; // Whether or not missile explosions can slow down enemies
    public static double missileExplosionSlowRate = 1; // How much enemies in explosion radius slow down by
    public static double laserReflectorLevel = 1; // How many enemies a laser reflects on
    public static double laserReflectorDamge = 1; // How much damage a reflected laser deals

    public static bool canLaserVitalComponentsDamge; // Whether or not laser can damage vital components
    public static double laserVitalComponentsDamgageChance = 1; // How likely a laser is too damage a vital component of enememy
    public static bool canMissileDisableDrone; // Whether or not missile can disable drones
    public static double MissileDisableDrone = 1; // Chance of missile explosion disabling drone ship (0 = impossible)

    public static bool canReleaseDrones; // Whether or not you can realease drones
    public static double numDronesReleased = 1; // How many drones get released to fight alongside you
    public static double numDrones = 1; // Number of drones your ship can release (not in skill tree but I thought it was useful)

    // Defensive Stats
    public static double maxHealth = 1; // Max health of the player
    public static double shieldCapacity = 1; // Shield Capacity of the player
    public static double shieldRechargeRate = 1; // Rate at which shield recharges

    public static bool canShildDamage; // Whether or not your shield can deal damage
    public static double shieldDamage = 1; // How much damage your shield deals (0 = impossible)
    public static double flareCapacity = 1; // How much flares your ship carries or 
                                     // maybe how much flares it shoots out at once or 
                                     // turn into bool if it should always stay the same

    public static bool canBatteringRam; // Whether or not you can use battering ram
    public static double batteringRamDamageDealt = 1; // How much damage you deal for battering enemy (0 = impossible)
    public static double batteringRamDamgeRecieved = 1; // How much pain you feel for battering enemy (0 = impossible)
    public static bool canAllClear; // Whether you are able to allClear

    public static bool canHealthScales; // Whether or not your ship can use Health Scales ability

    // Crew Stats
    public static int crewSlots = 0; // Number of crew slots you have
    // =========================================================================

    // Save Database ===========================================================
    public static void Save()
    {
        SaveObject saveObject = new SaveObject
        {
            movementSpeedSO = movementSpeed,
            currentHealthSO = currentHealth,
            playerPositionSO = playerPosition,
            playerXPSO = playerXP,

            // Offensive Stats
            laserDamageSO = laserDamage,
            missileRangeSO = missileRange,
            missileSpeedSO = missileSpeed,
            laserOverheatRateSO = laserOverheatRate,
            laserCoolingRateSO = laserCoolingRate,

            missileExplosionRadiusSO = missileExplosionRadius,
            canMissileExplosionSlowSO = canMissileExplosionSlow,
            missileExplosionSlowRateSO = missileExplosionSlowRate,
            laserReflectorLevelSO = laserReflectorDamge,
            laserReflectorDamgeSO = laserReflectorDamge,

            canLaserVitalComponentsDamgeSO = canLaserVitalComponentsDamge,
            laserVitalComponentsDamgageChanceSO = laserVitalComponentsDamgageChance,
            canMissileDisableDroneSO = canMissileDisableDrone,
            MissileDisableDroneSO = MissileDisableDrone,

            canReleaseDronesSO = canReleaseDrones,
            numDronesReleasedSO = numDronesReleased,
            numDronesSO = numDrones,

            // Defensive Stats
            maxHealthSO = maxHealth,
            shieldCapacitySO = shieldCapacity,
            shieldRechargeRageSO = shieldRechargeRate,

            canShildDamageSO = canShildDamage,
            shieldDamageSO = shieldDamage,
            flareCapacitySO = flareCapacity,

            canBatteringRamSO = canBatteringRam,
            batteringRamDamageDealtSO = batteringRamDamageDealt,
            batteringRamDamgeRecievedSO = batteringRamDamgeRecieved,
            canAllClearSO = canAllClear,

            canHealthScalesSO = canHealthScales,

            // Crew Stats
            crewSlotsSO = crewSlots,
        };

        string json = JsonUtility.ToJson(saveObject);
        SavePlayerDatabase.Save(json);
    }

    // Load Database ===========================================================
    public static void Load()
    {
        string saveString = SavePlayerDatabase.Load();

        SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

        movementSpeed = saveObject.movementSpeedSO;
        currentHealth = saveObject.currentHealthSO;
        playerPosition = saveObject.playerPositionSO;
        playerXP = saveObject.playerXPSO;

        // Offensive Stats
        laserDamage = saveObject.laserDamageSO;
        missileRange = saveObject.missileRangeSO;
        missileSpeed = saveObject.missileSpeedSO;
        laserOverheatRate = saveObject.laserOverheatRateSO;
        laserCoolingRate = saveObject.laserCoolingRateSO;

        missileExplosionRadius = saveObject.missileExplosionRadiusSO;
        canMissileExplosionSlow = saveObject.canMissileExplosionSlowSO;
        missileExplosionSlowRate = saveObject.missileExplosionSlowRateSO;
        laserReflectorLevel = saveObject.laserReflectorDamgeSO;
        laserReflectorDamge = saveObject.laserReflectorDamgeSO;

        canLaserVitalComponentsDamge = saveObject.canLaserVitalComponentsDamgeSO;
        laserVitalComponentsDamgageChance = saveObject.laserVitalComponentsDamgageChanceSO;
        canMissileDisableDrone = saveObject.canMissileDisableDroneSO;
        MissileDisableDrone = saveObject.MissileDisableDroneSO;

        canReleaseDrones = saveObject.canReleaseDronesSO;
        numDronesReleased = saveObject.numDronesReleasedSO;
        numDrones = saveObject.numDronesSO;

        // Defensive Stats
        maxHealth = saveObject.maxHealthSO;
        shieldCapacity = saveObject.shieldCapacitySO;
        shieldRechargeRate = saveObject.shieldRechargeRageSO;

        canShildDamage = saveObject.canShildDamageSO;
        shieldDamage = saveObject.shieldDamageSO;
        flareCapacity = saveObject.flareCapacitySO;

        canBatteringRam = saveObject.canBatteringRamSO;
        batteringRamDamageDealt = saveObject.batteringRamDamageDealtSO;
        batteringRamDamgeRecieved = saveObject.batteringRamDamgeRecievedSO;
        canAllClear = saveObject.canAllClearSO;

        canHealthScales = saveObject.canHealthScalesSO;

        // Crew Stats
        crewSlots = saveObject.crewSlotsSO;

    }

    // SaveObject used for saving database data with json
    private class SaveObject
    {
        // I guess this is pretty bad code desing to repeat data values but whatever it works well enough for now :P
        // Other stats that will need saving
        public double movementSpeedSO;
        public  int currentHealthSO;
        public  Vector3 playerPositionSO;
        public int playerXPSO;

        // Offensive Stats
        public  double laserDamageSO; // How much damage lasers deal
        public  double missileRangeSO; // Range of the missiles
        public  double missileSpeedSO; // Speed of the missiles
        public  double laserOverheatRateSO; // How fast laser overheats
        public  double laserCoolingRateSO; // How fast laser cools

        public  double missileExplosionRadiusSO; // Explosion radius of missiles
        public  bool canMissileExplosionSlowSO; // Whether or not missile explosions can slow down enemies
        public  double missileExplosionSlowRateSO; // How much enemies in explosion radius slow down by
        public  double laserReflectorLevelSO; // How many enemies a laser reflects on
        public  double laserReflectorDamgeSO; // How much damage a reflected laser deals

        public  bool canLaserVitalComponentsDamgeSO; // Whether or not laser can damage vital components
        public  double laserVitalComponentsDamgageChanceSO; // How likely a laser is too damage a vital component of enememy
        public  bool canMissileDisableDroneSO; // Whether or not missile can disable drones
        public  double MissileDisableDroneSO; // Chance of missile explosion disabling drone ship (0 = impossible)

        public  bool canReleaseDronesSO; // Whether or not you can realease drones
        public  double numDronesReleasedSO; // How many drones get released to fight alongside you
        public  double numDronesSO; // Number of drones your ship can release (not in skill tree but I thought it was useful)

        // Defensive Stats
        public  double maxHealthSO; // Max health of the player
        public  double shieldCapacitySO; // Shield Capacity of the player
        public  double shieldRechargeRageSO; // Rate at which shield recharges

        public  bool canShildDamageSO; // Whether or not your shield can deal damage
        public  double shieldDamageSO; // How much damage your shield deals (0 = impossible)
        public  double flareCapacitySO; // How much flares your ship carries or 
                                         // maybe how much flares it shoots out at once or 
                                         // turn doubleo bool if it should always stay the same

        public  bool canBatteringRamSO; // Whether or not you can use battering ram
        public  double batteringRamDamageDealtSO; // How much damage you deal for battering enemy (0 = impossible)
        public  double batteringRamDamgeRecievedSO; // How much pain you feel for battering enemy (0 = impossible)
        public  bool canAllClearSO; // Whether you are able to allClear

        public  bool canHealthScalesSO; // Whether or not your ship can use Health Scales ability

        // Crew Stats
        public  int crewSlotsSO; // Number of crew slots you have
    }
}
