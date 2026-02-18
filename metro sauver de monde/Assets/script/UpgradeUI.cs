using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI instance;
    
    public GameObject upgradePanel;
    public Button[] upgradeButtons = new Button[3];
    public TextMeshProUGUI[] upgradeNames = new TextMeshProUGUI[3];
    public TextMeshProUGUI[] upgradeDescriptions = new TextMeshProUGUI[3];
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
            
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }
    
    public void ShowUpgrades()
    {
        if (UpgradeManager.instance == null)
        {
            Debug.LogError("UpgradeManager not found!");
            return;
        }
        
        UpgradeManager.Upgrade[] upgrades = UpgradeManager.instance.GetRandomUpgrades();
        
        for (int i = 0; i < 3; i++)
        {
            int index = i; // Capture for lambda
            
            // Check if upgrade is null
            if (upgrades[i] == null)
            {
                if (upgradeNames[i] != null)
                    upgradeNames[i].text = "No Upgrade";
                if (upgradeDescriptions[i] != null)
                    upgradeDescriptions[i].text = "No more upgrades available";
                if (upgradeButtons[i] != null)
                    upgradeButtons[i].interactable = false;
                continue;
            }
            
            if (upgradeNames[i] != null)
                upgradeNames[i].text = upgrades[i].name;
                
            if (upgradeDescriptions[i] != null)
                upgradeDescriptions[i].text = upgrades[i].description;
            
            if (upgradeButtons[i] != null)
            {
                upgradeButtons[i].interactable = true;
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(index));
            }
        }
        
        if (upgradePanel != null)
            upgradePanel.SetActive(true);
            
        Time.timeScale = 0f;
    }
    
    public void SelectUpgrade(int index)
    {
        if (UpgradeManager.instance == null)
        {
            Debug.LogError("UpgradeManager not found!");
            return;
        }
        
        if (index < 0 || index >= UpgradeManager.instance.currentUpgrades.Length)
        {
            Debug.LogError("Invalid upgrade index!");
            return;
        }
        
        if (UpgradeManager.instance.currentUpgrades[index] == null)
        {
            Debug.LogWarning("Selected upgrade is null - continuing without upgrade");
            if (upgradePanel != null)
                upgradePanel.SetActive(false);
            Time.timeScale = 1f;
            logic.nextlevel = false;
            logic logicScript = FindFirstObjectByType<logic>();
            if (logicScript != null)
                logicScript.ContinueAfterUpgrade();
            return;
        }
        
        // Apply upgrade before hiding panel
        UpgradeManager.instance.ApplyUpgrade(UpgradeManager.instance.currentUpgrades[index]);
        Debug.Log("Upgrade applied!");
        
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
            
        Time.timeScale = 1f;
        
        // Reset flags and continue
        logic.nextlevel = false;
        logic logicScript2 = FindFirstObjectByType<logic>();
        if (logicScript2 != null)
        {
            logicScript2.ContinueAfterUpgrade();
        }
    }
}
