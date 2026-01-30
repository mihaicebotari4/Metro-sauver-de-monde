using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class logic : MonoBehaviour
{
    public GameObject[] obstacle =  new GameObject[5];
   
private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          }

    // Update is called once per frame
    void Update()
    {timer+=Time.deltaTime;
    if (timer >=2f){
        Instantiate(obstacle[0],new Vector3(UnityEngine.Random.Range(-10,10),1.6f,UnityEngine.Random.Range(-10,10)), new quaternion(0,0,0,0));
  timer=0f;
    }   
    }
}
