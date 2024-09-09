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
    float speed = 5f;
    [SerializeField]
    float jump = 10f;
    [SerializeField]
    float atkCoolTime = 3f;
    [SerializeField]
    float atkCoolTimeCalc = 3f;

    protected void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        //StartCoroutine(CalcCoolTime());
        //StartCoroutine(ResetCollider());
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
        return false;
    }
}
