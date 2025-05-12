using System.Collections.Generic;
using UnityEngine;

public class GridGameObject : MonoBehaviour
{
    protected Vector3 velocity = Vector3.zero;
    static GameObject border = null;

    void Start()
    {
        if (border is null)
        {
            border = new GameObject();
            border.tag = "Border";
        }
    }

    public GameObject WhatsAhead(Vector3 offset)
    {
        var sz = GameManager.Manager.GetSize();
        List<Collider2D> collisions = new List<Collider2D>();
        ContactFilter2D cf = new ContactFilter2D();
        cf.layerMask = ~0;

        var coll = Physics2D.OverlapBox(transform.position + offset, new Vector2(sz, sz), 0f,cf,collisions);

        if (coll != 0)
        {
            //Make sure we don't hit ourselves
            foreach (var c in collisions)
            {
                if (c.gameObject != gameObject)
                {
                    return c.gameObject;
                }
            }
        }

        var (ex, ey) = GameManager.Manager.GetExtents();
        ex /= 2;
        ey /= 2;

        if (
            transform.position.x + offset.x < -ex ||
            transform.position.y + offset.y < -ey ||
            transform.position.x + offset.x > ex ||
            transform.position.y + offset.y > ey
           )
        {
            return border;
        }

        return null;
    }
}

