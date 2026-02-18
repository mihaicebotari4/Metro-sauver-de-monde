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
        IncreaseColliderSize
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
        new Upgrade(UpgradeType.IncreaseColliderSize, "Bigger Hitbox", "Increase collision box size by 20%", 0.2f),
        new Upgrade(UpgradeType.IncreaseColliderSize, "Massive Hitbox", "Increase collision box size by 35%", 0.35f)
    };
    
    public static bool boostUnlocked = false;
    private List<Upgrade> appliedUpgrades = new List<Upgrade>();
    
    public Upgrade[] currentUpgrades = new Upgrade[3];
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void ResetRunState()
    {
        boostUnlocked = false;
        
        // Reset player upgrades
        player playerScript = FindFirstObjectByType<player>();
        if (playerScript != null)
        {
            playerScript.boostMultiplier = 0f;
            playerScript.driftEnabled = false;
            playerScript.movespeed = 7.5f; // Reset to default
            playerScript.rotationspeed = 125f; // Reset to default
            
            // Reset collider size to default
            if (playerScript.playerCollider != null)
            {
                playerScript.playerCollider.size = new Vector3(1f, 1f, 1f); // Reset to default size
            }
        }
        
        if (instance != null)
        {
            instance.ResetUpgradePool();
        }
    }

    private void ResetUpgradePool()
    {
        appliedUpgrades.Clear();
        foreach (Upgrade upgrade in upgradePool)
        {
            if (upgrade.type == UpgradeType.IncreaseBoost)
            {
                upgrade.isLocked = true;
                if (string.IsNullOrEmpty(upgrade.lockedReason))
                    upgrade.lockedReason = "Unlock the Boost first!";
            }
        }

        currentUpgrades = new Upgrade[3];
    }
    
    public Upgrade[] GetRandomUpgrades()
    {
        List<Upgrade> available = new List<Upgrade>();
        
        // Only include upgrades not yet applied (one-time upgrades: UnlockBoost and Drift)
        foreach (Upgrade upgrade in upgradePool)
        {
            bool isOneTimeUpgrade = (upgrade.type == UpgradeType.UnlockBoost || upgrade.type == UpgradeType.Drift);
            
            if (isOneTimeUpgrade)
            {
                // One-time upgrades can only be picked once
                if (!appliedUpgrades.Contains(upgrade))
                    available.Add(upgrade);
            }
            else
            {
                // All other upgrades can be picked repeatedly
                available.Add(upgrade);
            }
        }
        
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
                
            case UpgradeType.IncreaseColliderSize:
                if (playerScript.playerCollider != null)
                {
                    playerScript.playerCollider.size *= (1f + upgrade.value);
                    Debug.Log($"Collider size increased to {playerScript.playerCollider.size}");
                }
                else
                {
                    Debug.LogWarning("No BoxCollider assigned to player!");
                }
                break;
        }
        
        // Mark upgrade as applied so it won't appear again
        appliedUpgrades.Add(upgrade);
    }
}
