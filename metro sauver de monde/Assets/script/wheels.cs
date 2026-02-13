using UnityEngine;

public class wheels : MonoBehaviour
{
    private Transform tr;
    private float axis;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {tr = GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        tr.rotation = Quaternion.Euler(tr.rotation.eulerAngles.x + 10f * Time.deltaTime, tr.rotation.eulerAngles.y, tr.rotation.eulerAngles.z);
axis = -1*Input.GetAxis("Horizontal");
if (axis > 0.15f)
        {tr.rotation = Quaternion.Euler(tr.rotation.eulerAngles.x, tr.rotation.eulerAngles.y+12f * axis, tr.rotation.eulerAngles.z);}
    else
        {tr.rotation = Quaternion.Euler(tr.rotation.eulerAngles.x, 0f, tr.rotation.eulerAngles.z);}
        
        
    }

}
