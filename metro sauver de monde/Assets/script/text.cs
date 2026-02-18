using UnityEngine;
using TMPro;
using System;

public class text : MonoBehaviour
{
    private TMP_Text myText;
    static int helth =3;
    static int score;
   public static float timeleft;
    static int enemeysleft;
    public static float difficulty;
    public static float timesincelastdmg;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         myText = GetComponent<TMP_Text>();
         
        
    }

    // Update is called once per frame
    void Update()
    {
        timesincelastdmg += Time.deltaTime;
        enemeysleft = staticlogic.enemycount;
        score = ScoreManager.currentScore;
        myText.text = "Score: " + score + "\nTime Left: " + timeleft + "\nEnemies: " + enemeysleft;
        
        // Hide when death screen is active
        if (DeathScreenUI.instance != null && DeathScreenUI.instance.deathPanel != null)
        {
            bool deathScreenActive = DeathScreenUI.instance.deathPanel.activeSelf;
            if (myText.gameObject.activeSelf == deathScreenActive)
            {
                myText.gameObject.SetActive(!deathScreenActive);
            }
        }
    }

    public static void ResetRunState()
    {
        helth = 3;
        score = 0;
        timeleft = 0f;
        enemeysleft = 0;
        difficulty = 0f;
        timesincelastdmg = 0f;
    }
    public static void damage()
    {if (timesincelastdmg <1.5f) return;
      //  helth--;
        timesincelastdmg = 0f;
    }
}
