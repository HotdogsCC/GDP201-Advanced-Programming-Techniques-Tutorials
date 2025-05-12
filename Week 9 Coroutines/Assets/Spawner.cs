using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject mushroom;

    [SerializeField]
    GameObject bug;

    static public GameObject SafeSpawn(GameObject proto, Vector3 p)
    {
        
        var sz = GameManager.Manager.GetSize() / 2.0f;
        Vector3 extent = new Vector3(sz, sz, 0);
        var collider = Physics2D.OverlapBox((Vector2)p, (Vector2)extent, 0f);
        if (collider == null)
        {
            return Instantiate(proto, p, Quaternion.identity);
        }
        else
        {
            Debug.Log("Overlap");
        }
        return null;
    }

    // *** Step 8

    void Start()
    {
        // *** Step 8
    }

}

