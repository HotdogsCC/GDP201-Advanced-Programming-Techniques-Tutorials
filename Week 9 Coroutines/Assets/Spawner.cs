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
    IEnumerator MushroomSpawner()
    {
        while(Application.isPlaying)
        {
            var (x, y) = GameManager.Manager.PickRandomCell();
            Vector3 point = new Vector3();
            (point.x, point.y) = GameManager.Manager.GetPosition(x, y);
            SafeSpawn(mushroom, point);
            yield return new WaitForSeconds(GameManager.LevelScale(0.2f, 5.0f));
        }
    }

    IEnumerator BugSpawner()
    {
        while(Application.isPlaying)
        {
            var (x, y) = GameManager.Manager.PickRandomCell();
            var (xCount, yCount) = GameManager.Manager.GetCount();
            Vector3 point = new Vector3();
            (point.x, point.y) = GameManager.Manager.GetPosition(x, yCount - 1); // top row
            var bugInstance = SafeSpawn(bug, point);
            if(bugInstance != null)
            {
                bugInstance.GetComponent<BugScript>().spd =
                    ((float)GameManager.Manager.Level()) * 0.35f + 5f;
            }

            yield return new WaitForSeconds(GameManager.LevelScale(0.2f, 5.0f));
        }
    }

    void Start()
    {
        // *** Step 8
        StartCoroutine(MushroomSpawner());
        StartCoroutine(BugSpawner());
    }

}

