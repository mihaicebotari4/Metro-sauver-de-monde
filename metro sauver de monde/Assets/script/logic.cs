using System;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;

public class logic : MonoBehaviour
{
    public GameObject[] obstacle =  new GameObject[5];
   

public static int level;
public static int mobspawed;
public static bool nextlevel;
public static bool retryRequested;
private int timerset =14;
public static float lvltimer;
public static bool isGameOver = false;
private bool waitingForUpgrade = false;
private bool levelStarted = false;
    
    void Awake()
    {
        // Reset static run state on scene load.
        Time.timeScale = 1f;
        level = 1;
        mobspawed = 0;
        nextlevel = false;
        isGameOver = false;
        staticlogic.enemycount = 0;
        staticlogic.score = 0;
        ScoreManager.ResetScore();
        UpgradeManager.ResetRunState();
        text.ResetRunState();
        lvltimer = timerset;
        waitingForUpgrade = false;
        retryRequested = false;
        levelStarted = false;
    }

    void Start()
    {
        // Start the first level immediately after scene load.
        ContinueAfterUpgrade();
    }

    // Update is called once per frame
    void Update()
    {
        text.timeleft = (int)lvltimer;
        if (levelStarted && staticlogic.enemycount == 0)
            nextlevel = true;
       if (nextlevel && !waitingForUpgrade)
        {
            waitingForUpgrade = true;
            // Show upgrades after level 2+, skip for level 1
            if (level <= 2)
            {
                ContinueAfterUpgrade();
            }
            else
            {
                ShowUpgradeScreen();
            }
        }
        if (lvltimer < 0 && !isGameOver)
        {
            lvltimer = 0;
            isGameOver = true;
            death();
        }
        lvltimer -= Time.deltaTime;
    }


    public void Nextlevel(int level, int mobspawed)
    {
         

            
           mobspawed = (int) (Math.Log10((level+1)*20)*4)-3;
           staticlogic.enemycount = mobspawed;

           for (int i=0;i<mobspawed;i++)
            {// checks for a free space
                Vector3 spawnPos= new Vector3(0,0,0);
                bool spaceOccupied = true;
                int maxAttempts = 10;
                int attempts = 0;

                
                while (spaceOccupied && attempts < maxAttempts)
                {
                    float spawnScale = (float)(Math.Log10(level + 1f) * 20f);
                    float spawnRangeX = 10f + spawnScale;
                    float spawnRangeZ = 15f + spawnScale;
                    spawnPos = new Vector3(UnityEngine.Random.Range(-spawnRangeX, spawnRangeX), 4f, UnityEngine.Random.Range(-spawnRangeZ, spawnRangeZ));
                    
                  
                    Collider[] colliders = Physics.OverlapSphere(spawnPos, 1f);
                    spaceOccupied = colliders.Length > 0;
                    
                    attempts++;
                }

              
                if (!spaceOccupied)
                {
                    int randomIndex = UnityEngine.Random.Range(0, 2); // Random between 0 and 1
                    Instantiate(obstacle[randomIndex], spawnPos, new quaternion(0, 0, 0, 0));
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
        levelStarted = true;
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
