using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explodable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)GameManager.LayerNames.PlayerBullet)
        {
            Explosion.ExplodeMe(gameObject);
        }
    }
}
