using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    float moveDir;
    float refVelocity;

    public Animator animator;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    float speed = 4500f;
    [SerializeField]
    float maxSpeed = 10f;
    [SerializeField]
    float jump = 30f;
    [SerializeField]
    float slideRate = 0.35f;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        PlayerInput();
        PlayerAnimate();
        GroundFriction();
    }

    void FixedUpdate()
    {
        if (PlayerFlip() || Mathf.Abs(moveDir * rigidBody.velocity.x) < maxSpeed)
            rigidBody.AddForce(new Vector2(moveDir * Time.fixedDeltaTime * speed, 0f));
    }

    // Init Component
    protected void Init()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void PlayerInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
        }
    }

    void PlayerAnimate()
    {
        if (isGrounded())
        {
            if (Mathf.Abs(moveDir) <= 0.01f || Mathf.Abs(rigidBody.velocity.x) <= 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                //AnimationSetTrigger("Idle");
            }
            else if(Mathf.Abs(rigidBody.velocity.x) > 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                //AnimationSetTrigger("Run");
            }
        }
    }

    void GroundFriction()
    {
        if (isGrounded())
            if (Mathf.Abs(moveDir) <= 0.01f)
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0f, ref refVelocity, slideRate), rigidBody.velocity.y);
    }

    bool PlayerFlip()
    {
        bool flipSprite = (spriteRenderer.flipX ? (moveDir > 0f) : (moveDir < 0f));

        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            GroundFriction();
        }

        return flipSprite;
    }

    // Ground Check
    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.3f, 0.15f), CapsuleDirection2D.Horizontal, 0, LayerMask.GetMask("Platform"));
    }

    bool IsPlayingAnimation(string animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;

        return false;
    }

    void AnimationSetTrigger(string animName)
    {
        if (!IsPlayingAnimation(animName))
            animator.SetTrigger(animName);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Attacked by enemy!");
            Damage(10, collision.transform.position);
        }
    }

    protected override IEnumerator OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");

        spriteRenderer.color = new Color(0, 1, 0, 0.4f);

        int d = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigidBody.AddForce(new Vector2(d, 1) * 10, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.color = new Color(0, 1, 0, 1);
    }
}
