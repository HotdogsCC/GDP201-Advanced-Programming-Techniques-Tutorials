using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour , IDestroyable
{
    // From Interface
    public void DestroyMe()
    {
        Explosion.ExplodeMe(gameObject);
    }
}
