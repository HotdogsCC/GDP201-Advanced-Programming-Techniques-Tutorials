using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Explosion
{
    static public Particle particle;

    static Vector3 RandomVector()
    {
        return (Vector3)Random.insideUnitCircle;
    }

    // Start is called before the first frame update
    static public void Explode(int count, Vector3 location, float speed, float time)
    {
        for(int i=0; i < count; ++i)
        {
            //var spd = RandomVector();
            Particle np = GameObject.Instantiate(particle, location, Quaternion.identity);
            np.Move(RandomVector(),time * i / (float)count);
        }
    }

    static public void ExplodeMe(GameObject g)
    {
        Explode(15, g.transform.position, 3, 1);
        GameObject.Destroy(g);
    }
}
