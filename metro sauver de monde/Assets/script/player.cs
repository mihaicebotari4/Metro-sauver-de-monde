using System;
using Unity.VisualScripting;
using UnityEngine;

public class player : MonoBehaviour
{
  public float movespeed = 7.5f;
  public float rotation;
  public float rotationspeed = 125f;
  public float boostMultiplier = 0f;
  public bool driftEnabled = false;
  [SerializeField] public BoxCollider playerCollider;
  private float timer;
  [SerializeField] private Rigidbody rb;

  private Transform trplayer;
  private float mouvementx =0;
  private float mouvementz = 0;
  float currentSpeed;
  private ConstantForce constantForce2;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  { 
    Application.targetFrameRate = 180;
    constantForce2 = GetComponent<ConstantForce>();
    trplayer = GetComponent<Transform>();
    rb = GetComponent<Rigidbody>();
    
    if (playerCollider == null)
        playerCollider = GetComponent<BoxCollider>();
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
       // Quaternion turn = Quaternion.Euler(0f, rotation * rotationspeed  * Time.fixedDeltaTime, 0f);
       // rb.MoveRotation(rb.rotation * turn);

       //Vector3 move = transform.forward  * movespeed * Time.fixedDeltaTime;
      //  rb.MovePosition(rb.position + move);
        rb.AddTorque(0, rotation*rotationspeed*Time.deltaTime, 0);
      // transform.Rotate(0, rotation*rotationspeed*Time.deltaTime, 0);
        mouvementx = (float) Math.Sin(Math.PI/180*transform.rotation.eulerAngles.y);
        mouvementz = (float) Math.Cos(Math.PI/180*transform.rotation.eulerAngles.y);
    
    // Apply boost when W key is held (only if unlocked)
     currentSpeed = movespeed;
    if (Input.GetKey(KeyCode.W) && boostMultiplier > 0)
    {
        currentSpeed *= boostMultiplier;
    }
    
    // Drift when Left Shift is held (only if unlocked)
    if (Input.GetKey(KeyCode.LeftShift) && driftEnabled)
    {
        // Reduce forward speed, increase rotation for drifting
        currentSpeed *= 0.7f;
        rb.AddTorque(0, rotation * rotationspeed * Time.deltaTime * 1.5f, 0);
    }
    
    rb.AddRelativeForce(new Vector3(0, 0,currentSpeed*Time.deltaTime),ForceMode.Force);
  
     //trplayer.position = new Vector3(trplayer.position.x+(mouvementx*2.5f*Time.deltaTime), trplayer.position.y, trplayer.position.z+(mouvementz*2.5f*Time.deltaTime));
  //Debug.Log(constantForce2.relativeForce.z);
  }
  
}
