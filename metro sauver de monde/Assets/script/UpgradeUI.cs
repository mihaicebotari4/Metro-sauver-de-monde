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
            
            if (upgradeNames[i] != null)
                upgradeNames[i].text = upgrades[i].name;
                
            if (upgradeDescriptions[i] != null)
                upgradeDescriptions[i].text = upgrades[i].description;
            
            if (upgradeButtons[i] != null)
            {
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
        if (UpgradeManager.instance == null || UpgradeManager.instance.currentUpgrades[index] == null)
        {
            Debug.LogError("Invalid upgrade selection!");
            return;
        }
        
        UpgradeManager.instance.ApplyUpgrade(UpgradeManager.instance.currentUpgrades[index]);
        
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
            
        Time.timeScale = 1f;
        
        // Signal to logic that we can continue
        logic.nextlevel = false;
        logic logicScript = FindFirstObjectByType<logic>();
        if (logicScript != null)
            logicScript.ContinueAfterUpgrade();
    }
}
