using System.Collections;
using UnityEngine;

public class ConstantMotion : MonoBehaviour
{
    // *** Step 7

    public Vector3 velocity { get; set; }
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
