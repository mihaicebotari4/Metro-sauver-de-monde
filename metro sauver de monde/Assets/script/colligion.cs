using Unity.VisualScripting;
using UnityEngine;

public class colligion : MonoBehaviour
{
    private Rigidbody rbo;
    private Transform trs;
    
    private float timer;
    private int used=0;
   static public float minVelocity = 5f; 
   
    void Start()
    {
        rbo = GetComponent<Rigidbody>();
        trs = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {minVelocity = 4.8f + (logic.level/6f);
        text.difficulty = minVelocity;

        if (used ==1)
        timer += Time.deltaTime;
        if (timer >=2f)
        Destroy(gameObject);
            }

    void OnTriggerEnter(Collider other){

        if(!other.CompareTag("colligion box")) return;
        Rigidbody rb = other.attachedRigidbody;

        if(rb != null && rb.linearVelocity.magnitude <= minVelocity)
        { text.damage();
            return;
        }
        
        // Enemy killed by impact - add score only once and only if game is not over
        if (rb != null && used == 0 && !logic.isGameOver)
        {
            float impactVelocity = rb.linearVelocity.magnitude;
            ScoreManager.AddScore(impactVelocity, logic.level);
            logic.lvltimer += 1f; // Add 1 second to timer
        }
        
        Vector3 dir = (transform.position-other.transform.position ).normalized;
        dir.y = 1f;

        rbo.AddForce(dir * 15f, ForceMode.Impulse);        
        // Add random axis rotation
        Vector3 randomAxis = UnityEngine.Random.onUnitSphere;
        rbo.AddTorque(randomAxis * 15f, ForceMode.Impulse);
                used =1;
        staticlogic.enemycount --;
        
        // Shrink collider so it doesn't kill other enemies
        Collider myCollider = GetComponent<Collider>();
        if (myCollider != null)
        {
            myCollider.enabled = false;
        }
    }
}
