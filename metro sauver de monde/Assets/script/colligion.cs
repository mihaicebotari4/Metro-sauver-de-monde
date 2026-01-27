using UnityEngine;

public class colligion : MonoBehaviour
{
    private Rigidbody rbo;
    private Transform trs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbo = GetComponent<Rigidbody>();
        trs = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
     if (other.CompareTag("colligion box"))
    {Rigidbody rb = other.attachedRigidbody;

       Vector3 dir = (other.transform.position - transform.position).normalized;
    dir.y = 1f;

    rb.AddForce(dir * 1000f, ForceMode.Impulse);
    }

   
}
}
