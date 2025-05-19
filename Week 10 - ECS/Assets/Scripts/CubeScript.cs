using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public float yMultiplier = 1.0f;
    public float resolution = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin(transform.position.x * resolution + Time.fixedTime) + Mathf.Sin(transform.position.z * resolution + Time.fixedTime);
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
}
