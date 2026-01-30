using UnityEngine;

public class keepstraite : MonoBehaviour
{private Transform tr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.rotation=Quaternion.Euler(0, 0, 0);
    }
}
