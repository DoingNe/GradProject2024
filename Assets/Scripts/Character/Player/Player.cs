using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    float moveDir;
    float refVelocity;

    public Animator animator;
    public GameObject hitBoxCollider;
    public GameObject weaponCollider;

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
    [SerializeField]
    float hitRecovery = 0.2f;
    [SerializeField]
    bool isGround;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        PlayerInput();
        isGrounded();
        PlayerAnimate();
        GroundFriction();
    }

    void FixedUpdate()
    {
        if (!IsPlayingAnimation("NormalAttack"))
        {
            if (PlayerFlip() || Mathf.Abs(moveDir * rigidBody.velocity.x) < maxSpeed)
                rigidBody.AddForce(new Vector2(moveDir * Time.fixedDeltaTime * speed, 0f));
            else
            {
                rigidBody.velocity = new Vector2(moveDir * maxSpeed, rigidBody.velocity.y);
            }
        }
        
    }

    // Init Component
    protected void Init()
    {
        GameManager.instance.Player = this;
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.SetBool("isMelee", true);

        AnimationSetTrigger("Idle");
        Debug.Log("Idle");

        StartCoroutine(ResetCollider());
    }

    void PlayerInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGround && !IsPlayingAnimation("NormalAttack"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
            AnimationSetTrigger("Jump");
            Debug.Log("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            AnimationSetTrigger("NormalAttack");
            Debug.Log("NormalAttack");
        }
    }

    void PlayerAnimate()
    {
        if (isGround)
        {
            if (Mathf.Abs(moveDir) <= 0.01f || Mathf.Abs(rigidBody.velocity.x) <= 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Idle");
                Debug.Log("Idle");
            }
            else if (Mathf.Abs(rigidBody.velocity.x) > 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Run");
                Debug.Log("Run");
            }
        }
        else if (rigidBody.velocity.y < 0 && !IsPlayingAnimation("Jump"))
        {
            AnimationSetTrigger("Fall");
            Debug.Log("Fall");
        }
    }

    void GroundFriction()
    {
        if (isGround)
            if (Mathf.Abs(moveDir) <= 0.01f)
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0f, ref refVelocity, slideRate), rigidBody.velocity.y);
    }

    bool PlayerFlip()
    {
        bool flipSprite = ((transform.localScale.x < 0) ? (moveDir > 0f) : (moveDir < 0f)); // right : left

        if (flipSprite)
        {
            transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
            //spriteRenderer.flipX = !spriteRenderer.flipX;
            GroundFriction();
        }

        return flipSprite;
    }

    // Ground Check
    void isGrounded()
    {
        if (Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.3f, 0.15f), CapsuleDirection2D.Horizontal, 0, LayerMask.GetMask("Platform")))
        {
            animator.ResetTrigger("Idle");
            isGround = true;
        }
        else isGround = false;
    }

    public bool IsPlayingAnimation(string animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;

        return false;
    }

    public void AnimationSetTrigger(string animName)
    {
        if (!IsPlayingAnimation(animName))
            animator.SetTrigger(animName);
    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;
            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(hitRecovery);
                hitBoxCollider.SetActive(true);
            }
        }
    }

    public void WeaponColliderOnOff()
    {
        weaponCollider.SetActive(!weaponCollider.activeInHierarchy);
    }
}
