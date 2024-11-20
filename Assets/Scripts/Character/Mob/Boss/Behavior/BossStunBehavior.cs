using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunBehavior : StateMachineBehaviour
{
    private float stunDuration = 2f; // 스턴 지속 시간
    private float stunTimer;

    private Boss boss;
    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        bossTransform = animator.transform;
        playerTransform = GameManager.Instance.Player.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();

        // 보스 상태 설정
        boss.state = Define.BossState.Stun;

        // 보스 충돌 판정 설정
        boss.InitCollider(true, false, false);

        // 스턴 타이머 초기화
        stunTimer = stunDuration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 스턴 타이머 감소
        stunTimer -= Time.deltaTime;

        // 스턴 지속 시간 종료 후 상태 전환
        if (stunTimer <= 0)
        {
            float distanceToPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);

            if (distanceToPlayer <= 30f)
            {
                animator.SetTrigger("Chase"); // Chase 트리거 발동
            }
            else
            {
                animator.SetTrigger("Idle"); // Idle 트리거 발동
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 상태 종료 시 속도를 초기화
        rb.velocity = Vector2.zero;
    }
}