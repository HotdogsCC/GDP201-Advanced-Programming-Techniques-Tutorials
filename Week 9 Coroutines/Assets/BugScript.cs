using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugScript : MonoBehaviour , IDestroyable
{
    
    public float spd = 5.0f;

    // *** Step 9
    IEnumerator MoveSideways()
    {
        yield return null;

        float d2 = ((Random.Range(0, 2) == 1) ? -1f : 1f) * spd;
    }

    // *** Step 9

    // Start is called before the first frame update
    void Start()
    {
        // *** Step 9
    }

    // From Interface
    public void DestroyMe()
    {
        Explosion.ExplodeMe(gameObject);
        if (Random.Range(0.0f, 1.0f) < 0.25f)
        {
            // *** Step 11
        }
    }
}
