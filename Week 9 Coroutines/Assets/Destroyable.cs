using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDestroyable
{
    void DestroyMe();
}

public class Destroyable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)GameManager.LayerNames.PlayerBullet)
        {
            GetComponent<IDestroyable>()?.DestroyMe();
        }
    }
}