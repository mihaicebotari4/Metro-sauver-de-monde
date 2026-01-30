using Unity.VisualScripting;
using UnityEngine;

public class colligion : MonoBehaviour
{
    private Rigidbody rbo;
    private Transform trs;
    
    private float timer;
    private int used=0;
    public float minVelocity = 5f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbo = GetComponent<Rigidbody>();
        trs = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {if (used ==1)
        timer += Time.deltaTime;
        if (timer >=2f)
        Destroy(gameObject);
            }

    void OnTriggerEnter(Collider other){

        if(!other.CompareTag("colligion box")) return;
        Rigidbody rb = other.attachedRigidbody;

        if(rb != null && rb.linearVelocity.magnitude <= minVelocity) return;

        Vector3 dir = (transform.position-other.transform.position ).normalized;
        dir.y = 1f;

        rbo.AddForce(dir * 15f, ForceMode.Impulse);
        used =1;
    }
}
