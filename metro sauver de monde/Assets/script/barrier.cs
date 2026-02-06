using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class shrink : MonoBehaviour
{private Vector3 john;
 private GameObject enemey;   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {john = new Vector3 (1.3f,1.3f,1.3f);
        enemey = this.GameObject();
        enemey.transform.localScale = new Vector3 (0.01f,0.01f,0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemey.transform.localScale != new Vector3(1,1,1))
        {
            if (enemey.transform.localScale.x < 1)
            { 
                enemey.transform.localScale = Vector3.MoveTowards(   enemey.transform.lossyScale ,john,Time.deltaTime*15);  
                          }
            else
            {john = new Vector3(1,1,1);
                 enemey.transform.localScale = Vector3.MoveTowards(   enemey.transform.lossyScale ,john,Time.deltaTime*15);  
            }
        }
 
    }



}
