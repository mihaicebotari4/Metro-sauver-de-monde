using System;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{public float movespeed;
public float rotation;
public float rotationspeed;

   private Transform trplayer;
   private float mouvementx =0;
   private float mouvementz = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trplayer = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
         rotation = Input.GetAxis("Horizontal");
         transform.Rotate(0, rotation*rotationspeed*Time.deltaTime, 0);
         mouvementx = (float) Math.Sin(Math.PI/180*transform.rotation.eulerAngles.y);
         mouvementz = (float) Math.Cos(Math.PI/180*transform.rotation.eulerAngles.y);
        trplayer.position = new Vector3(trplayer.position.x+(mouvementx*movespeed*Time.deltaTime), trplayer.position.y, trplayer.position.z+(mouvementz*movespeed*Time.deltaTime));
    }
}
