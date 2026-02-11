using System;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
  public float movespeed;
  public float rotation;
  public float rotationspeed;
  private float timer;
  [SerializeField] private Rigidbody rb;

  private Transform trplayer;
  private float mouvementx =0;
  private float mouvementz = 0;
  private ConstantForce constantForce2;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  { constantForce2 = GetComponent<ConstantForce>();
    trplayer = GetComponent<Transform>();
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
    if (Input.GetKeyDown(KeyCode.Space))
  {
    
    constantForce2.relativeForce = new Vector3(0, 5,0);
  }
        rotation = Input.GetAxis("Horizontal");
        Quaternion turn = Quaternion.Euler(0f, rotation * rotationspeed  * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turn);

       //Vector3 move = transform.forward  * movespeed * Time.fixedDeltaTime;
      //  rb.MovePosition(rb.position + move);
       // rb.AddTorque(0, rotation*rotationspeed*Time.deltaTime, 0);
      // transform.Rotate(0, rotation*rotationspeed*Time.deltaTime, 0);
        mouvementx = (float) Math.Sin(Math.PI/180*transform.rotation.eulerAngles.y);
        mouvementz = (float) Math.Cos(Math.PI/180*transform.rotation.eulerAngles.y);
     rb.AddRelativeForce(new Vector3(0, 0,movespeed*Time.deltaTime),ForceMode.Impulse);
  
     //trplayer.position = new Vector3(trplayer.position.x+(mouvementx*2.5f*Time.deltaTime), trplayer.position.y, trplayer.position.z+(mouvementz*2.5f*Time.deltaTime));
  //Debug.Log(constantForce2.relativeForce.z);
  }
  
}
