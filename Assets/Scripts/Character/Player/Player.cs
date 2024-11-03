using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    float moveDir;
    float refVelocity;

    public Animator animator;
    public GameObject hitBoxCollider;
    public GameObject weaponCollider;

    public GameObject Boss;
    public GameObject Door;

    public RectTransform heartPanel;
    public GameObject heartImage;

    public List<GameObject> heartImages = new List<GameObject>();

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    int hp;
    [SerializeField]
    int atk;
    [SerializeField]
    int gold = 0;
    float speed = 4500f;
    [SerializeField]
    float maxSpeed = 10f;
    public float jump;
    float slideRate = 0.35f;
    [SerializeField]
    float hitRecovery = 1f;
    bool isGround;
    public bool isInvulnerable = false;
    public bool isDead = false;
    public bool isPlaying;
    public bool canControl = true;
    public bool canInteract = false;

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

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
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
            atk = value;
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
            if (PlayerFlip() || Mathf.Abs(moveDir * rigidBody.velocity.x) < maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]))
                rigidBody.AddForce(new Vector2(moveDir * Time.fixedDeltaTime * speed, 0f));
            else
            {
                rigidBody.velocity = new Vector2(moveDir * maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]), rigidBody.velocity.y);
            }
        }
        
    }

    // Init Component
    protected void Init()
    {
        GameManager.Instance.Player = this;
        GameManager.Instance.camera = Camera.main;

        GameManager.Instance.InitGame();

        isPlaying = true;

        canControl = true;
        canInteract = false;
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(CameraMovement.Instance.FadeIn());

        animator.SetBool("isMelee", true);

        AnimationSetTrigger("Idle");

        StartCoroutine(ResetCollider());

        for(int i = 0; i < Hp; i++)
        {
            GameObject newHeart = Instantiate(heartImage, heartPanel);
            heartImages.Add(newHeart);
        }
    }

    void PlayerInput()
    {
        if (canControl)
        {
            moveDir = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space) && isGround && !IsPlayingAnimation("NormalAttack"))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
                AnimationSetTrigger("Jump");
            }

            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("공격");
                AnimationSetTrigger("NormalAttack");
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canInteract)
            {
                StartCoroutine(CameraMovement.Instance.FadeOut());
                ++GameManager.Instance.currentStage;
                if(!Boss.gameObject.activeInHierarchy && GameManager.Instance.currentStage == 2)
                {
                    Boss.gameObject.SetActive(true);
                }
                transform.position = Door.transform.GetChild(0).position;
                StartCoroutine(CameraMovement.Instance.FadeIn());
            }
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
        isInvulnerable = true;

        yield return new WaitForSeconds(hitRecovery);

        isInvulnerable = false;
    }

    public void WeaponColliderOnOff()
    {
        weaponCollider.SetActive(!weaponCollider.activeInHierarchy);
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvulnerable) return;

        Hp -= damage;
        Hp = Mathf.Max(Hp, 0);

        RemoveHeart(heartImages.Count - Hp);

        if (Hp <= 0)
        {
            Die();
        }

        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce(-(knockbackDirection+new Vector2(0f, 0.5f)) * 15f, ForceMode2D.Impulse);
        
        StartCoroutine(ResetCollider());
    }

    public void GainHeart(int count)
    {
        Hp += count;

        for(int i = 0; i < count; i++)
        {
            GameObject newHeart = Instantiate(heartImage, heartPanel);
            heartImages.Add(newHeart);
        }
    }

    public void RemoveHeart(int heartsToRemove)
    {
        for (int i = 0; i < heartsToRemove; i++)
        {
            if (heartImages.Count > 0)
            {
                GameObject heart = heartImages[heartImages.Count - 1];
                heartImages.RemoveAt(heartImages.Count - 1);
                Destroy(heart);
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            canControl = false;
            rigidBody.velocity = Vector2.zero;
            AnimationSetTrigger("Die");
        }
    }

    public void GameOver()
    {
        isPlaying = false;
        Debug.Log("플레이어 사망");
        SceneManager.LoadScene("Result");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Die();
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = true;
            Door = collision.gameObject;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Mob collisionMob = collision.gameObject.GetComponent<Mob>();
            if (collisionMob != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                TakeDamage(collisionMob.atk, -direction);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = false;
            Door = null;
        }
    }
}
