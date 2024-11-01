using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobInfo : Mob
{
    public Transform[] wallCheck;

    void Awake()
    {
        base.Awake();
        currentHp = 3;
        speed = 2.5f;
        jump = 20f;
        atk = 1;
    }


    void Update() 
    {
        if (!isHit)
        {
            rigidbody.velocity = new Vector2(transform.localScale.x * speed, rigidbody.velocity.y);

            if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask)|| !Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                MobFlip();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.CompareTag("PlayerHitBox"))
            MobFlip();
    }
}
