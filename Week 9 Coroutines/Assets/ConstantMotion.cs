using System.Collections;
using UnityEngine;

public class ConstantMotion : MonoBehaviour
{
    // *** Step 7
    public float timeout = 3.0f;

    public Vector3 velocity { get; set; }
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    IEnumerator TimeOut(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
    private void Start()
    {
        StartCoroutine(TimeOut(timeout));
    }
}
