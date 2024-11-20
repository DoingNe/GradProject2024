using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseBehavior : StateMachineBehaviour
{
    private Boss boss;

    [SerializeField]
    private GameObject player;

    public float speed = 5; // 이동 속도
    public float dashAttackDistance = 40f;
    public float meleeAttackDistance = 5f; // 플레이어와의 최소 거리

    private float lastDashAttackTime;
    private float dashAttackCooldown = 15f;

    private float lastMeleeAttackTime;
    private float meleeAttackCooldown = 3f;

    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private Vector2 direction;

    public LayerMask platformLayer; // 플랫폼 레이어

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();

        player = GameManager.Instance.Player.gameObject;

        boss.state = Define.BossState.Chase;
        boss.InitCollider(true, false, false);
        bossTransform = animator.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Instance.Player.transform;

        // 초기 방향 설정
        UpdateDirection();

        animator.ResetTrigger("Chase");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);

            // 플레이어와의 거리를 확인하여 공격할지 결정
            if (distanceToPlayer > dashAttackDistance)
            {
                MoveBoss();
            }
            else if (distanceToPlayer > meleeAttackDistance)
            {
                MoveBoss();
                if (Time.time - boss.lastDashAttackTime >= dashAttackCooldown)
                {
                    //rb.velocity = Vector2.zero; // 멈춤
                    animator.SetTrigger("Dash");
                    boss.lastDashAttackTime = Time.time; // Boss 클래스에 기록
                }
            }
            else if (distanceToPlayer <= meleeAttackDistance && boss.state != Define.BossState.Dash)
            {
                MoveBoss();
                if (Time.time - boss.lastMeleeAttackTime >= meleeAttackCooldown)
                {
                    //rb.velocity = Vector2.zero; // 멈춤
                    animator.SetTrigger("MeleeAttack");
                    boss.lastMeleeAttackTime = Time.time; // Boss 클래스에 기록
                }
            }

            // 플레이어 위치에 따라 방향 업데이트
            UpdateDirection();
        }

        // Chase 상태를 유지할지 확인
        if (Vector3.Distance(bossTransform.position, playerTransform.position) > 50f)
        {
            animator.SetTrigger("Run"); // Run 상태로 전환
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void UpdateDirection()
    {
        // 플레이어 방향으로 업데이트
        if (playerTransform != null)
        {
            direction = (playerTransform.position - bossTransform.position).normalized;
            direction.y = 0f;

            // 스케일 업데이트
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        // 방향에 따라 보스 스케일 변경
        if (direction.x < 0 && bossTransform.localScale.x > 0)
        {
            bossTransform.localScale = new Vector3(-3f, bossTransform.localScale.y, bossTransform.localScale.z);
        }
        else if (direction.x > 0 && bossTransform.localScale.x < 0)
        {
            bossTransform.localScale = new Vector3(3f, bossTransform.localScale.y, bossTransform.localScale.z);
        }
    }

    private void MoveBoss()
    {
        Debug.Log(speed);
        // 플레이어 방향으로 이동
        rb.velocity = direction * speed;
    }
}
