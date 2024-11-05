using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    protected BoxCollider2D boxCollider;

    public delegate void DeathAction();
    public event DeathAction OnDeath;

    public Transform[] wallCheck;
    public GameObject hitBoxCollider;
    public GameObject attackCollider;
    public Animator animator;
    public LayerMask layerMask;

    public int Hp;

    public bool isHit = false;
    public bool isGround = true;
    public bool isDead = false;
    public bool canAtk = true;
    public bool mobDirRight = true;

    public int atk;
    public int gold;
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected float jump = 10f;
    protected float atkCoolTime = 3f;
    protected float atkCoolTimeCalc = 3f;

    protected void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        isHit = false;
        isGround = false;
        isDead = false;
        canAtk = true;
        mobDirRight = true;

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());
    }

    protected void Update()
    {
        if (!isHit)
        {
            rigidbody.velocity = new Vector2(transform.localScale.x * speed, rigidbody.velocity.y);

            if (Physics2D.OverlapCircle(wallCheck[0].position, 0.01f, layerMask) || !Physics2D.OverlapCircle(wallCheck[1].position, 0.01f, layerMask))
                MobFlip();
        }
    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;

            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
                AnimationSetTrigger("Walk");
                hitBoxCollider.SetActive(true);
                isHit = false;
            }
        }
    }

    IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;

            if (!canAtk)
            {
                atkCoolTimeCalc -= Time.deltaTime;
                if (atkCoolTimeCalc <= 0)
                {
                    atkCoolTimeCalc = atkCoolTime;
                    canAtk = true;
                }
            }
        }
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

    protected void MobFlip()
    {
        mobDirRight = !mobDirRight;

        Vector3 thisScale = transform.localScale;
        if (mobDirRight)
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
            //thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
        rigidbody.velocity = Vector2.zero;
    }

    protected bool IsPlayerDirection()
    {
        if (transform.position.x < GameManager.Instance.Player.transform.position.x ? mobDirRight : !mobDirRight)
            return true;

        return false;
    }

    protected void GroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.05f, layerMask))
            isGround = true;
        else isGround = false;
    }

    public void HitBoxColliderOnOff()
    {
        hitBoxCollider.SetActive(!hitBoxCollider.activeInHierarchy);
    }

    public void setDeactive()
    {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (isHit) return;

        Hp -= damage;
        isHit = true;
        rigidbody.velocity = Vector2.zero;
        hitBoxCollider.SetActive(false);

        if (Hp <= 0 && !isDead)
        {
            isDead = true;

            GameManager.Instance.Player.Gold += (int)(gold * (1 + (0.1f * GameManager.Instance.playerStat[2])));
            GameManager.Instance.earnGold += (int)(gold * (1 + (0.1f * GameManager.Instance.playerStat[2])));
            GameManager.Instance.kill++;

            AnimationSetTrigger("Die");
        }
        else
        {
            AnimationSetTrigger("Hit");

            if (transform.position.x > GameManager.Instance.Player.transform.position.x)
                rigidbody.velocity = new Vector2(10f, 1f);
            else
                rigidbody.velocity = new Vector2(-10f, 1f);
        }

        StartCoroutine(ResetCollider());
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerAttack"))
        {
            TakeDamage(GameManager.Instance.Player.Atk + GameManager.Instance.playerStat[0]);
        }

        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            if (!GameManager.Instance.Player.isInvulnerable)
            {
                //MobFlip();
            }
        }
    }
}
