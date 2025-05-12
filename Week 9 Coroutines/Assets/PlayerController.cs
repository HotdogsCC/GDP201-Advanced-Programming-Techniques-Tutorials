using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    public float speed = 5.0f;

    // *** Fire variables here

    // *** Bullet coroutine

    public IEnumerator ApplyBuff(PickupEffect pe)
    {
        pe.Apply(this);
        yield return new WaitForSeconds(pe.BuffTime);
        pe.UnApply(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // *** Step 6
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 move = new Vector3();
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move.x = -1.0f;
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move.x = 1.0f;
        }
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move.y = 1.0f;
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move.y = -1.0f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            // *** Step 6
        }

        transform.position = transform.position + move * speed * Time.deltaTime;

        (float x_dimension,float y_dimension)=GameManager.Manager.GetExtents();

        float sz = GameManager.Manager.GetSize()/2.0f;
        var p = transform.position;
        p.x = Mathf.Clamp(p.x, -x_dimension/2+sz, x_dimension/2- sz);
        p.y = Mathf.Clamp(p.y, -y_dimension/2 + sz, y_dimension/2-sz);
        transform.position = p;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PickUp"))
        {
            // *** Stage 12
            Explosion.ExplodeMe(collision.gameObject);
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("Enemy"))
        {
            Explosion.ExplodeMe(collision.gameObject);
            Explosion.ExplodeMe(gameObject);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            GameManager.GameOver();
        }
    }
}
