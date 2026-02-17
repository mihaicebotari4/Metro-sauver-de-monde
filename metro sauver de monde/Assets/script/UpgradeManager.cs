using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    
    public enum UpgradeType
    {
        IncreaseSpeed,
        IncreaseRotation,
        UnlockBoost,
        IncreaseBoost,
        Drift,
        IncreaseLevelTime
    }
    
    [System.Serializable]
    public class Upgrade
    {
        public UpgradeType type;
        public string name;
        public string description;
        public float value;
        public bool isLocked;
        public string lockedReason;
        
        public Upgrade(UpgradeType type, string name, string description, float value, bool isLocked = false, string lockedReason = "")
        {
            this.type = type;
            this.name = name;
            this.description = description;
            this.value = value;
            this.isLocked = isLocked;
            this.lockedReason = lockedReason;
        }
    }
    
    private List<Upgrade> upgradePool = new List<Upgrade>
    {
        new Upgrade(UpgradeType.IncreaseSpeed, "Speed Boost", "Increase movement speed by 10%", 0.1f),
        new Upgrade(UpgradeType.IncreaseSpeed, "Turbo Engine", "Increase movement speed by 15%", 0.15f),
        new Upgrade(UpgradeType.IncreaseRotation, "Better Handling", "Increase rotation speed by 7.5%", 0.075f),
        new Upgrade(UpgradeType.IncreaseRotation, "Quick Turn", "Increase rotation speed by 12.5%", 0.125f),
        new Upgrade(UpgradeType.UnlockBoost, "Unlock Boost", "Unlock the boost ability (W key)", 0f),
        new Upgrade(UpgradeType.IncreaseBoost, "Enhanced Boost", "Boost multiplier +0.5x", 0.5f, true, "Unlock the Boost first!"),
        new Upgrade(UpgradeType.IncreaseBoost, "Nitro Boost", "Boost multiplier +1.0x", 1f, true, "Unlock the Boost first!"),
        new Upgrade(UpgradeType.Drift, "Drift Control", "Enable drifting with Left Shift key", 1f),
        new Upgrade(UpgradeType.IncreaseLevelTime, "Extra Time", "Increase level timer by 5 seconds", 5f),
        new Upgrade(UpgradeType.IncreaseLevelTime, "Bonus Time", "Increase level timer by 10 seconds", 10f)
    };
    
    public static bool boostUnlocked = false;
    
    public Upgrade[] currentUpgrades = new Upgrade[3];
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public Upgrade[] GetRandomUpgrades()
    {
        List<Upgrade> available = new List<Upgrade>(upgradePool);
        List<Upgrade> validUpgrades = new List<Upgrade>();
        
        // Filter out locked upgrades
        foreach (Upgrade upgrade in available)
        {
            if (!upgrade.isLocked)
                validUpgrades.Add(upgrade);
        }
        
        // If not enough valid upgrades, add locked ones
        if (validUpgrades.Count < 3)
        {
            foreach (Upgrade upgrade in available)
            {
                if (upgrade.isLocked && !validUpgrades.Contains(upgrade))
                    validUpgrades.Add(upgrade);
            }
        }
        
        Upgrade[] selected = new Upgrade[3];
        for (int i = 0; i < 3 && validUpgrades.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, validUpgrades.Count);
            selected[i] = validUpgrades[randomIndex];
            validUpgrades.RemoveAt(randomIndex);
        }
        
        currentUpgrades = selected;
        return selected;
    }
    
    public void ApplyUpgrade(Upgrade upgrade)
    {
        player playerScript = FindFirstObjectByType<player>();
        
        if (playerScript == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        
        switch (upgrade.type)
        {
            case UpgradeType.IncreaseSpeed:
                playerScript.movespeed += playerScript.movespeed * upgrade.value;
                Debug.Log($"Speed increased to {playerScript.movespeed}");
                break;
                
            case UpgradeType.IncreaseRotation:
                playerScript.rotationspeed += playerScript.rotationspeed * upgrade.value;
                Debug.Log($"Rotation speed increased to {playerScript.rotationspeed}");
                break;
                
            case UpgradeType.UnlockBoost:
                boostUnlocked = true;
                playerScript.boostMultiplier = 2f;
                // Update unlock status of boost upgrades
                foreach (Upgrade u in upgradePool)
                {
                    if (u.type == UpgradeType.IncreaseBoost)
                        u.isLocked = false;
                }
                Debug.Log("Boost unlocked!");
                break;
                
            case UpgradeType.IncreaseBoost:
                playerScript.boostMultiplier += upgrade.value;
                Debug.Log($"Boost multiplier increased to {playerScript.boostMultiplier}");
                break;
                
            case UpgradeType.Drift:
                if (playerScript != null)
                {
                    playerScript.driftEnabled = true;
                    Debug.Log("Drift enabled!");
                }
                break;
                
            case UpgradeType.IncreaseLevelTime:
                logic.lvltimer += upgrade.value;
                Debug.Log($"Level time increased by {upgrade.value} seconds");
                break;
        }
    }
}
