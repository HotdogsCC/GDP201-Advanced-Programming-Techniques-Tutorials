using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PickupEffect
{
    float BuffTime { get; set; }
    void Apply(PlayerController pc);
    void UnApply(PlayerController pc);
}

namespace Pickups {
    public class SpeedUp : PickupEffect
    {
         
        private float spd;

        public SpeedUp(float buff_time , float m) { 
            spd = m; 
            BuffTime = buff_time;
        }

        public float BuffTime { get ; set; }

        public void Apply(PlayerController pc)
        {
            pc.speed *= spd;
        }

        public void UnApply(PlayerController pc)
        {
            pc.speed /= spd;
        }
    }
    
    public class BulletSpeedUp : PickupEffect
    {
        public float BuffTime { get; set; }

        private float spd;
        private float old_rate;

        public BulletSpeedUp(float bufftime, float newspeed) { BuffTime = bufftime; spd = newspeed; }
        public void Apply(PlayerController pc)
        {
            // *** Stage 11
        }

        public void UnApply(PlayerController pc)
        {
            // *** Stage 11
        }
    }
}


public class Pickup : MonoBehaviour
{
    public Color speedupclr;
    public float speed_up = 2.0f;


    public Color bulletupclr;
    public float bullet_fire_rate = 0.010f; //per second


    public float time_out = 4.0f;
    public float buff_time = 18.0f;

    public PickupEffect Effect { get; private set; }

    // *** Step 11

    // Start is called before the first frame update
    void Start()
    {
        //Pick a random effect
        if (Random.Range(0, 2) == 0)
        {
            Effect = new Pickups.BulletSpeedUp(buff_time, bullet_fire_rate);
            GetComponent<SpriteRenderer>().color = bulletupclr;
        }
        else
        {
            Effect = new Pickups.SpeedUp(buff_time, speed_up);
            GetComponent<SpriteRenderer>().color = speedupclr;
        }
        
        // *** Step 11
    }


}
