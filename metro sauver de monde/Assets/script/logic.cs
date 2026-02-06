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
       if (nextlevel)
        {
            Nextlevel(level,logic.mobspawed);
            lvltimer = (int) (15f+(1+Math.Log10(level*35)));
            nextlevel = false;
            level++;
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
            {
                Instantiate(obstacle[0],new Vector3(UnityEngine.Random.Range(-10,10),1f,UnityEngine.Random.Range(-15,15)), new quaternion(0,0,0,0));
               
                
                
            
            }
           
    }
    public void death() 
    {
        foreach(GameObject obj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            obj.SetActive(false);
        }
    }
}
