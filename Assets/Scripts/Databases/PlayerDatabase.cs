using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDatabase
{
    // Other stats that will need saving
    public static int movementSpeed;
    public static int currentHealth;
    public static Vector3 playerPosition;
    public static int playerXP;

    // Offensive Stats
    public static int laserDamage; // How much damage lasers deal
    public static int missileRange; // Range of the missiles
    public static int missileSpeed; // Speed of the missiles
    public static int laserOverheatRate; // How fast laser overheats
    public static int laserCoolingRate; // How fast laser cools

    public static int missileExplosionRadius; // Explosion radius of missiles
    public static bool canMissileExplosionSlow; // Whether or not missile explosions can slow down enemies
    public static int missileExplosionSlowRate; // How much enemies in explosion radius slow down by
    public static int laserReflectorLevel; // How many enemies a laser reflects on
    public static int laserReflectorDamge; // How much damage a reflected laser deals

    public static bool canLaserVitalComponentsDamge; // Whether or not laser can damage vital components
    public static int laserVitalComponentsDamgageChance; // How likely a laser is too damage a vital component of enememy
    public static bool canMissileDisableDrone; // Whether or not missile can disable drones
    public static int MissileDisableDrone; // Chance of missile explosion disabling drone ship (0 = impossible)

    public static bool canReleaseDrones; // Whether or not you can realease drones
    public static int numDronesReleased; // How many drones get released to fight alongside you
    public static int numDrones; // Number of drones your ship can release (not in skill tree but I thought it was useful)

    // Defensive Stats
    public static int maxHealth = 500; // Max health of the player
    public static int shieldCapacity; // Shield Capacity of the player
    public static int shieldRechargeRate; // Rate at which shield recharges

    public static bool canShildDamage; // Whether or not your shield can deal damage
    public static int shieldDamage; // How much damage your shield deals (0 = impossible)
    public static int flareCapacity; // How much flares your ship carries or 
                                     // maybe how much flares it shoots out at once or 
                                     // turn into bool if it should always stay the same

    public static bool canBatteringRam; // Whether or not you can use battering ram
    public static int batteringRamDamageDealt; // How much damage you deal for battering enemy (0 = impossible)
    public static int batteringRamDamgeRecieved; // How much pain you feel for battering enemy (0 = impossible)
    public static bool canAllClear; // Whether you are able to allClear

    public static bool canHealthScales; // Whether or not your ship can use Health Scales ability

    // Crew Stats
    public static int crewSlots; // Number of crew slots you have


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
        public int movementSpeedSO;
        public  int currentHealthSO;
        public  Vector3 playerPositionSO;
        public int playerXPSO;

        // Offensive Stats
        public  int laserDamageSO; // How much damage lasers deal
        public  int missileRangeSO; // Range of the missiles
        public  int missileSpeedSO; // Speed of the missiles
        public  int laserOverheatRateSO; // How fast laser overheats
        public  int laserCoolingRateSO; // How fast laser cools

        public  int missileExplosionRadiusSO; // Explosion radius of missiles
        public  bool canMissileExplosionSlowSO; // Whether or not missile explosions can slow down enemies
        public  int missileExplosionSlowRateSO; // How much enemies in explosion radius slow down by
        public  int laserReflectorLevelSO; // How many enemies a laser reflects on
        public  int laserReflectorDamgeSO; // How much damage a reflected laser deals

        public  bool canLaserVitalComponentsDamgeSO; // Whether or not laser can damage vital components
        public  int laserVitalComponentsDamgageChanceSO; // How likely a laser is too damage a vital component of enememy
        public  bool canMissileDisableDroneSO; // Whether or not missile can disable drones
        public  int MissileDisableDroneSO; // Chance of missile explosion disabling drone ship (0 = impossible)

        public  bool canReleaseDronesSO; // Whether or not you can realease drones
        public  int numDronesReleasedSO; // How many drones get released to fight alongside you
        public  int numDronesSO; // Number of drones your ship can release (not in skill tree but I thought it was useful)

        // Defensive Stats
        public  int maxHealthSO; // Max health of the player
        public  int shieldCapacitySO; // Shield Capacity of the player
        public  int shieldRechargeRageSO; // Rate at which shield recharges

        public  bool canShildDamageSO; // Whether or not your shield can deal damage
        public  int shieldDamageSO; // How much damage your shield deals (0 = impossible)
        public  int flareCapacitySO; // How much flares your ship carries or 
                                         // maybe how much flares it shoots out at once or 
                                         // turn into bool if it should always stay the same

        public  bool canBatteringRamSO; // Whether or not you can use battering ram
        public  int batteringRamDamageDealtSO; // How much damage you deal for battering enemy (0 = impossible)
        public  int batteringRamDamgeRecievedSO; // How much pain you feel for battering enemy (0 = impossible)
        public  bool canAllClearSO; // Whether you are able to allClear

        public  bool canHealthScalesSO; // Whether or not your ship can use Health Scales ability

        // Crew Stats
        public  int crewSlotsSO; // Number of crew slots you have
    }
}
