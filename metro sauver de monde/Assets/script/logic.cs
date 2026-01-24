using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class logic : MonoBehaviour
{
    public GameObject[] obstacle =  new GameObject[5];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(obstacle[0],new Vector3(0,0,0), new quaternion(0,0,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
