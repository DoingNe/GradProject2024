using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    protected BoxCollider2D boxCollider;

    public GameObject hitBoxCollider;
    public Animator animator;
    public LayerMask layerMask;

    public int currentHp = 1;

    public bool isHit = false;
    public bool isGround = true;
    public bool canAtk = true;
    public bool mobDirRight;

    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    protected float jump = 10f;
    [SerializeField]
    protected float atkCoolTime = 3f;
    [SerializeField]
    protected float atkCoolTimeCalc = 3f;

    protected void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(CalcCoolTime());
        StartCoroutine(ResetCollider());
    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;

            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(0.5f);
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
            thisScale.x = -Mathf.Abs(thisScale.x);
        else
            thisScale.x = Mathf.Abs(thisScale.x);

        transform.localScale = thisScale;
        rigidbody.velocity = Vector2.zero;
    }

    protected bool IsPlayerDirection()
    {
        if (transform.position.x < GameManager.instance.Player.transform.position.x ? mobDirRight : !mobDirRight)
            return true;

        return false;
    }

    protected void GroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.05f, layerMask))
            isGround = true;
        else isGround = false;
    }
}
