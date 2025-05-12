using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Particle : ConstantMotion
{
    // *** Step 10

    public void Move(Vector3 vel, float time)
    {
        velocity = vel;
        // *** Step 10
    }
}
