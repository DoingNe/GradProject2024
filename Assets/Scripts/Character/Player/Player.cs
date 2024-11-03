using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 플레이어 스크립트
 * 플레이어 정보, 이동, 애니메이션, 공격, 피격
 */
public class Player : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;

    float moveDir;                              // 이동방향
    float refVelocity;

    public Animator animator;
    public GameObject hitBoxCollider;           // 피격 판정
    public GameObject weaponCollider;           // 공격 판정

    public GameObject Boss;                     // 보스
    public GameObject Door;                     // 문

    public RectTransform heartPanel;            // 체력을 출력할 패널
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
    public bool isInvulnerable = false;         // 무적 여부
    public bool isDead = false;
    public bool isPlaying;
    public bool canControl = true;              // 컨트롤 제어
    public bool canInteract = false;            // 상호작용 제어

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
        // 공격하는 중이 아닐때만 이동
        if (!IsPlayingAnimation("NormalAttack"))
        {
            // 이동속도 스탯 강화 적용
            if (PlayerFlip() || Mathf.Abs(moveDir * rigidBody.velocity.x) < maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]))
                rigidBody.AddForce(new Vector2(moveDir * Time.fixedDeltaTime * speed, 0f));
            else
            {
                rigidBody.velocity = new Vector2(moveDir * maxSpeed * (1 + 0.05f * GameManager.Instance.playerStat[1]), rigidBody.velocity.y);
            }
        }
        
    }

    // 플레이어 초기화
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

    // 입력
    void PlayerInput()
    {
        if (canControl)
        {
            // 이동 방향 (좌 - A, 우 - B)
            moveDir = Input.GetAxisRaw("Horizontal");

            // 점프 (Space)
            if (Input.GetKeyDown(KeyCode.Space) && isGround && !IsPlayingAnimation("NormalAttack"))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
                AnimationSetTrigger("Jump");
            }

            // 공격 (마우스 우클릭)
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("공격");
                AnimationSetTrigger("NormalAttack");
            }

            // 상호작용 (왼쪽 Shift)
            if (Input.GetKeyDown(KeyCode.LeftShift) && canInteract)
            {
                StartCoroutine(CameraMovement.Instance.FadeOut());
                ++GameManager.Instance.currentStage;

                // 보스룸 입장 시 보스 소환
                if(!Boss.gameObject.activeInHierarchy && GameManager.Instance.currentStage == 2)
                {
                    Boss.gameObject.SetActive(true);
                }

                // 스테이지 이동
                transform.position = Door.transform.GetChild(0).position;
                StartCoroutine(CameraMovement.Instance.FadeIn());
            }
        }
    }

    // 플레이어 애니메이션
    void PlayerAnimate()
    {
        if (isGround)
        {
            // 정지
            if (Mathf.Abs(moveDir) <= 0.01f || Mathf.Abs(rigidBody.velocity.x) <= 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Idle");
            }
            // 좌우 이동
            else if (Mathf.Abs(rigidBody.velocity.x) > 0.01f && Mathf.Abs(rigidBody.velocity.y) <= 0.01f)
            {
                AnimationSetTrigger("Run");
            }
        }
        // 하강
        else if (rigidBody.velocity.y < 0 && !IsPlayingAnimation("Jump"))
        {
            AnimationSetTrigger("Fall");
        }
    }

    // 마찰력
    void GroundFriction()
    {
        if (isGround)
            if (Mathf.Abs(moveDir) <= 0.01f)
                rigidBody.velocity = new Vector2(Mathf.SmoothDamp(rigidBody.velocity.x, 0f, ref refVelocity, slideRate), rigidBody.velocity.y);
    }

    // 방향 전환
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

    // 해당 애니메이션 실행 여부
    public bool IsPlayingAnimation(string animName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            return true;

        return false;
    }

    // 애니메이션 실행
    public void AnimationSetTrigger(string animName)
    {
        if (!IsPlayingAnimation(animName))
            animator.SetTrigger(animName);
    }

    // 무적 판정
    IEnumerator ResetCollider()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(hitRecovery);

        isInvulnerable = false;
    }

    // 공격 판정
    public void WeaponColliderOnOff()
    {
        weaponCollider.SetActive(!weaponCollider.activeInHierarchy);
    }

    // 데미지 판정
    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvulnerable) return;

        Hp -= damage;
        Hp = Mathf.Max(Hp, 0);

        // 체력 감소
        RemoveHeart(heartImages.Count - Hp);

        // 사망
        if (Hp <= 0)
        {
            Die();
        }

        // 넉백
        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce(-(knockbackDirection+new Vector2(0f, 0.5f)) * 15f, ForceMode2D.Impulse);
        
        StartCoroutine(ResetCollider());
    }

    // 체력 회복
    public void GainHeart(int count)
    {
        Hp += count;

        for(int i = 0; i < count; i++)
        {
            GameObject newHeart = Instantiate(heartImage, heartPanel);
            heartImages.Add(newHeart);
        }
    }

    // 체력 감소
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

    // 사망
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

    // 게임 오버
    public void GameOver()
    {
        isPlaying = false;
        Debug.Log("플레이어 사망 후 씬 이동");
        SceneManager.LoadScene("Result");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 함정과 충돌 시 즉사
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Die();
        }
        // 문이랑 상호작용 가능
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = true;
            Door = collision.gameObject;
        }
        // 피격 판정
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Mob collisionMob = collision.gameObject.GetComponent<Mob>();
            if (collisionMob != null)
            {
                // 넉백 방향
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                TakeDamage(collisionMob.atk, -direction);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 문이랑 상호작용 불가
        if (collision.gameObject.layer == LayerMask.NameToLayer("InteractDoor"))
        {
            canInteract = false;
            Door = null;
        }
    }
}
