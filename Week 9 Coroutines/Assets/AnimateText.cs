using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateText : MonoBehaviour
{
    // ** Step 3
    private Vector3 original;
    static int num = 0;
    int id = 0;

    void Start()
    {
        // *** Step 5
        original = transform.position;

        id = num;
        num++;

        StartCoroutine(SinWave(20, 5));
    }

    IEnumerator SinWave(float amp, float freq)
    {
        while (true)
        {
            Vector3 r = original;
            float t = ((float)id / num) * Mathf.PI * 2;
            r.y += Mathf.Sin(-Time.time * freq + t) * amp;
            r.x += Mathf.Cos(-Time.time * freq + t) * amp;

            transform.position = r;
            yield return null;
        }
    }
}
