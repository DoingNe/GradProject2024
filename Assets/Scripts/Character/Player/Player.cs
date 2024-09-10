using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    float moveDir;
    float refVelocity;

    public Animator animator;
    public GameObject hitBoxCollider;
    public GameObject weaponCollider;

    public GameObject Door;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    int hp = 30;
    [SerializeField]
    int atk = 1;
    [SerializeField]
    int coin = 0;
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
    [SerializeField]
    bool canInteract = false;

    public int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }
    }

    public int Coin
    {
        get
        {
            return coin;
        }
        set
        {
            coin += value;
        }
    }

    public int Atk
    {
        get
        {
            return atk;
        }
        set
        {
            atk += value;
        }
    }

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
        GameManager.Instance.Player = this;
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(CameraMovement.Instance.FadeIn());

        animator.SetBool("isMelee", true);

        AnimationSetTrigger("Idle");

        StartCoroutine(ResetCollider());
    }

    void PlayerInput()
    {
        moveDir = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGround && !IsPlayingAnimation("NormalAttack"))
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
            AnimationSetTrigger("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            AnimationSetTrigger("NormalAttack");
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canInteract)
        {
            StartCoroutine(CameraMovement.Instance.FadeOut());
            GameManager.Instance.currentStage++;
            transform.position = Door.transform.GetChild(0).position;
            StartCoroutine(CameraMovement.Instance.FadeIn());
        }
    }

    void PlayerAnimate()
    {
        if (isGround)
        {
            if (Mathf.Abs(moveDir) <= 0.01f || Mathf.Abs(rigidBody.velocity.x) <= 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Idle");
            }
            else if (Mathf.Abs(rigidBody.velocity.x) > 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Run");
            }
        }
        else if (rigidBody.velocity.y < 0 && !IsPlayingAnimation("Jump"))
        {
            AnimationSetTrigger("Fall");
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

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        hitBoxCollider.SetActive(false);
        StartCoroutine(ResetCollider());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
            canInteract = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
            canInteract = false;
    }
}
