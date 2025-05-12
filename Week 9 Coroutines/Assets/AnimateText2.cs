using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateText2 : MonoBehaviour
{
    static int num = 0;
    int id = 0;
    Vector3 original;
    // Start is called before the first frame update
    void Start()
    {
        original = transform.position;
        id = num;
        num++;
        StartCoroutine(SinWave(20,5));
    }

    IEnumerator SinWave(float amp, float freq)
    {
        while (true)
        {
            var r = original;
            float t = ((float)id / num) * Mathf.PI * 2;
            r.y += Mathf.Sin(-Time.time * freq + t) * amp;
            //r.x += Mathf.Cos(-Time.time * freq + t) * amp;
            transform.position = r;
            yield return null;
        }
    }
}
