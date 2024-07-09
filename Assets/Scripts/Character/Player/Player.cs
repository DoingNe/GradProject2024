using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    [SerializeField]
    float speed;
    [SerializeField]
    float jump;
    [SerializeField]
    bool isJumping;

    float horizontal;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Jump
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            rigidBody.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        }

        // stop
        if (Input.GetButtonUp("Horizontal"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.normalized.x * 0.5f, rigidBody.velocity.y);
        }

        if (rigidBody.velocity.y > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DamagedPlayer"), LayerMask.NameToLayer("Platform"), true);
        }

        if (rigidBody.velocity.y < 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("DamagedPlayer"), LayerMask.NameToLayer("Platform"), false);

            RaycastHit2D rayCast = Physics2D.Raycast(rigidBody.position, Vector2.down, 1f, LayerMask.GetMask("Platform"));

            if (rayCast.collider != null)
            {
                if (rayCast.distance < 0.6f)
                {
                    isJumping = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        rigidBody.AddForce(Vector2.right * horizontal, ForceMode2D.Impulse);

        if (rigidBody.velocity.x > speed)
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
        else if (rigidBody.velocity.x < speed * (-1))
            rigidBody.velocity = new Vector2(speed * (-1), rigidBody.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Attacked by enemy!");
            Damage(10, collision.transform.position);
        }
    }

    protected void Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected override IEnumerator OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");

        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0, 1, 0, 0.4f);

        int d = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigidBody.AddForce(new Vector2(d, 1) * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.color = new Color(0, 1, 0, 1);
    }
}
