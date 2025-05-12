using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobScript : MonoBehaviour , IDestroyable // GridGameObject
{
    // blob moves until it hits mushroom then drops and reverses direction
    public float spd = 5.0f;


    IEnumerator MoveSideways()
    {
        yield return null;

        float d2 = ((Random.Range(0, 2) == 1) ? -1f : 1f) * spd;

        var ahead = GameManager.WhatsAhead(gameObject,new Vector3(d2 * spd, 0));
        if (ahead) {
            d2 = -d2;
        }

        Vector3 velocity = new Vector3(d2, 0, 0);

        while (true)
        {
            var p = transform.position;
            ahead = GameManager.WhatsAhead(gameObject, velocity * Time.deltaTime);

            if (ahead && (ahead.CompareTag("Mushroom") || ahead.CompareTag("Border")))
            {
                StartCoroutine(MoveObjectDown(1.0f));
                break;
            }

            transform.position += velocity * Time.deltaTime;
            yield return null;
        }
    }


    //
    IEnumerator MoveObjectDown(float distance)
    {
        yield return null;

        float mv = transform.position.y - distance;
        Vector3 velocity = new Vector3(0, -spd , 0);
        while (true)
        {
            //check the square ahead
            var nv = velocity.normalized;
            var ahead = GameManager.WhatsAhead(gameObject,velocity * Time.deltaTime);

            if ( ahead && (ahead.CompareTag("Mushroom") || ahead.CompareTag("Border")) 
              || transform.position.y < mv)
            {
                break; //stop going down
            }

            transform.position += velocity * Time.deltaTime;
            yield return null;
        }

        if (transform.position.y < -GameManager.Manager.GetExtents().y)
        {
            StartCoroutine(MoveObjectDown(3));
            Destroy(gameObject);
        }
        else
            StartCoroutine(MoveSideways());
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSideways());
    }

    // From Interface
    public void DestroyMe()
    {
        Explosion.ExplodeMe(gameObject);
        if (Random.Range(0.0f, 1.0f) < 0.25f)
        {
            Instantiate(GameManager.Manager.pickup, transform.position, Quaternion.identity);
        }
    }
}
