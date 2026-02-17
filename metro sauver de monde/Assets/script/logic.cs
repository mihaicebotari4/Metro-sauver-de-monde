using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class logic : MonoBehaviour
{
    public GameObject[] obstacle =  new GameObject[5];
   

public static int level;
public static int mobspawed;
public static bool nextlevel;
private int timerset =15;
public static float lvltimer;
private bool waitingForUpgrade = false;
private bool firstLevelCompleted = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {lvltimer=timerset;
        level=1;
          }

    // Update is called once per frame
    void Update()
    {
        text.timeleft = (int)lvltimer;
        if (staticlogic.enemycount ==0)
        nextlevel=true;
       if (nextlevel && !waitingForUpgrade)
        {
            waitingForUpgrade = true;
            // Only show upgrades after first level
            if (firstLevelCompleted)
            {
                ShowUpgradeScreen();
            }
            else
            {
                firstLevelCompleted = true;
                ContinueAfterUpgrade();
            }
        }
        if (lvltimer<0)
        {
            lvltimer = 0;
            death();
        }
        lvltimer -= Time.deltaTime;
    }


    public void Nextlevel(int level, int mobspawed)
    {
         

            
           mobspawed = (int) (Math.Log10((level+1)*20)*3);
           staticlogic.enemycount = mobspawed;

           for (int i=0;i<mobspawed;i++)
            {// checks for a free space
                Vector3 spawnPos= new Vector3(0,0,0);
                bool spaceOccupied = true;
                int maxAttempts = 10;
                int attempts = 0;

                
                while (spaceOccupied && attempts < maxAttempts)
                {
                    spawnPos = new Vector3(UnityEngine.Random.Range(-10, 10), 4f, UnityEngine.Random.Range(-15, 15));
                    
                  
                    Collider[] colliders = Physics.OverlapSphere(spawnPos, 1f);
                    spaceOccupied = colliders.Length > 0;
                    
                    attempts++;
                }

              
                if (!spaceOccupied)
                {
                    Instantiate(obstacle[0], spawnPos, new quaternion(0, 0, 0, 0));
                }
            
            }
           
    }
    private void ShowUpgradeScreen()
    {
        if (UpgradeUI.instance != null)
        {
            UpgradeUI.instance.ShowUpgrades();
        }
        else
        {
            Debug.LogWarning("UpgradeUI not found! Continuing without upgrades.");
            ContinueAfterUpgrade();
        }
    }
    
    public void ContinueAfterUpgrade()
    {
        Nextlevel(level, logic.mobspawed);
        lvltimer = (int) (15f+(1+Math.Log10(level*35)));
        nextlevel = false;
        waitingForUpgrade = false;
        level++;
    }
    
    public void death() 
    {
        if (DeathScreenUI.instance != null)
        {
            DeathScreenUI.instance.ShowDeathScreen();
        }
        else
        {
            Debug.LogWarning("DeathScreenUI not found!");
            foreach(GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                obj.SetActive(false);
            }
        }
    }
}
