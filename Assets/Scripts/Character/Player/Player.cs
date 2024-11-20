using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * �÷��̾� ��ũ��Ʈ
 * �÷��̾� ����, �̵�, �ִϸ��̼�, ����, �ǰ�
 */
public class Player : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    readonly int maxHp = 10;
    float moveDir;                              // �̵�����
    float refVelocity;

    public Animator animator;
    public GameObject hitBoxCollider;           // �ǰ� ����
    public GameObject weaponCollider;           // ���� ����

    public GameObject Boss;                     // ����
    public GameObject BossUI;
    public GameObject Door;                     // ��

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
    public bool isGround;
    public bool isInvulnerable = false;         // ���� ����
    public bool isDead = false;
    public bool isPlaying;
    public bool canControl = true;              // ��Ʈ�� ����
    public bool canInteract = false;            // ��ȣ�ۿ� ����

    public int MaxHp
    {
        get
        {
            return maxHp;
        }
    }

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
        // �����ϴ� ���� �ƴҶ��� �̵�
        if (!IsPlayingAnimation("NormalAttack"))
        {
            // �̵��ӵ� ���� ��ȭ ����
            if (PlayerFlip() || Mathf.Abs(moveDir * rigidBody.velocity.x) < maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]))
                rigidBody.AddForce(new Vector2(moveDir * Time.fixedDeltaTime * speed, 0f));
            else
            {
                rigidBody.velocity = new Vector2(moveDir * maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]), rigidBody.velocity.y);
            }
        }
    }

    // �÷��̾� �ʱ�ȭ
    protected void Init()
    {
        GameManager.Instance.Player = this;
        GameManager.Instance.camera = Camera.main;

        GameManager.Instance.InitGame();

        isPlaying = true;
        canControl = true;
        canInteract = false;

        Hp = MaxHp;
        
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(CameraMovement.Instance.FadeIn());

        animator.SetBool("isMelee", true);

        AnimationSetTrigger("Idle");

        StartCoroutine(ResetCollider());
    }

    // �Է�
    void PlayerInput()
    {
        if (canControl)
        {
            // �̵� ���� (�� - A, �� - D)
            moveDir = Input.GetAxisRaw("Horizontal");

            // ���� (Space)
            if (Input.GetKeyDown(KeyCode.Space) && isGround && !IsPlayingAnimation("NormalAttack"))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
                AnimationSetTrigger("Jump");
            }

            // ���� (���콺 ��Ŭ��)
            if (Input.GetMouseButtonDown(1))
            {
                AnimationSetTrigger("NormalAttack");
            }

            // ��ȣ�ۿ� (���� Shift)
            if (Input.GetKeyDown(KeyCode.LeftShift) && canInteract)
            {
                StartCoroutine(CameraMovement.Instance.FadeOut());
                ++GameManager.Instance.currentStage;

                // ������ ���� �� ���� ��ȯ
                if(!Boss.gameObject.activeInHierarchy && GameManager.Instance.currentStage == 2)
                {
                    Boss.gameObject.SetActive(true);
                    if (!BossUI.activeSelf)
                    {
                        BossUI.SetActive(true);
                    }
                }

                // �������� �̵�
                transform.position = Door.transform.GetChild(0).position;
                StartCoroutine(CameraMovement.Instance.FadeIn());
            }
        }
    }

    // �÷��̾� �ִϸ��̼�
    void PlayerAnimate()
    {
        if (isGround)
        {
            // ����
            if (Mathf.Abs(moveDir) <= 0.01f || Mathf.Abs(rigidBody.velocity.x) <= 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Idle");
            }
            // �¿� �̵�
            else if (Mathf.Abs(rigidBody.velocity.x) > 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Run");
            }
        }
        // �ϰ�
        else if (rigidBody.velocity.y < 0 && !IsPlayingAnimation("Jump") && !isInvulnerable)
        {
            AnimationSetTrigger("Fall");
        }
    }

    // ������
    void GroundFriction()
    {
        if (isGround)
            if (Mathf.Abs(moveDir) <= 0.01f)
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0f, ref refVelocity, slideRate), rigidBody.velocity.y);
    }

    // ���� ��ȯ
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
        else if (Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.3f, 0.15f), CapsuleDirection2D.Horizontal, 0, LayerMask.GetMask("Obstacle")))
        {
            animator.ResetTrigger("Idle");
            isGround = true;
        }
        else isGround = false;
    }

    // �ش� �ִϸ��̼� ���� ����
    public bool IsPlayingAnimation(string animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;

        return false;
    }

    // �ִϸ��̼� ����
    public void AnimationSetTrigger(string animName)
    {
        if (!IsPlayingAnimation(animName))
            animator.SetTrigger(animName);
    }

    // ���� ����
    IEnumerator ResetCollider()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(hitRecovery);

        isInvulnerable = false;
    }

    // ���� ����
    public void WeaponColliderOnOff()
    {
        weaponCollider.SetActive(!weaponCollider.activeInHierarchy);
    }

    // ������ ����
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvulnerable) return;

        // ü�� ����
        LoseHp(damage);

        // ���
        if (Hp <= 0)
        {
            Die();
        }

        // �˹�
        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce((knockbackDirection+new Vector2(0f, 1f)) * 15f, ForceMode2D.Impulse);
        
        StartCoroutine(ResetCollider());
    }

    // ü�� ȸ��
    public void GainHp()
    {
        Hp += (int)(MaxHp * 0.8);
        Hp = Mathf.Clamp(Hp, 0, 10);
    }

    // ü�� ����
    public void LoseHp(int damage)
    {
        Hp -= damage;
        Hp = Mathf.Clamp(Hp, 0, 10);
    }


    // ���
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

    // ���� ����
    public void GameOver()
    {
        isPlaying = false;
        SceneManager.LoadScene("Result");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ �浹 �� 2�� ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            //TakeDamage();
            if (!isInvulnerable)
            {
                TakeDamage(2, (moveDir * Vector2.right).normalized);
            }
        }
        // ���̶� ��ȣ�ۿ� ����
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = true;
            Door = collision.gameObject;
        }
        // ���� �ǰ� ����
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Mob collisionMob = collision.gameObject.GetComponent<Mob>();
            if (collisionMob != null)
            {
                // �˹� ����
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                TakeDamage(collisionMob.atk, direction);
            }
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Boss") || collision.gameObject.layer == LayerMask.NameToLayer("BossAttack"))
        {
            Boss collisionBoss = collision.gameObject.GetComponent<Boss>();

            if (collisionBoss==null)
            {
                collisionBoss = collision.gameObject.transform.parent.gameObject.GetComponent<Boss>();
            }
            
            if(collisionBoss != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;

                switch (collisionBoss.state)
                {
                    case Define.BossState.Idle:
                    case Define.BossState.Run:
                    case Define.BossState.Chase:
                    case Define.BossState.Stun:
                        Debug.Log("���� ���� ����");
                        TakeDamage(collisionBoss.damage, direction);
                        break;
                    case Define.BossState.MeleeAttack:
                        Debug.Log("���� ��Ÿ ����");
                        TakeDamage(collisionBoss.atkDamage, direction);
                        break;
                    case Define.BossState.Dash:
                        Debug.Log("���� ��� ����");
                        TakeDamage(collisionBoss.dashDamage, direction);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("���� ��ũ��Ʈ�� ã�� �� ����");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ���̶� ��ȣ�ۿ� �Ұ�
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = false;
            Door = null;
        }
    }
}
