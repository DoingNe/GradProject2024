using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttackBehavior : StateMachineBehaviour
{
    private Boss boss;
    private bool isColliderActive;

    private float activationTime = 13f / 19f; // 13번째 프레임 (비율)
    private float deactivationTime = 18f / 19f; // 18번째 프레임 (비율)

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        boss.state = Define.BossState.MeleeAttack;
        isColliderActive = false;
        boss.InitCollider(false, false, false); // 초기화 상태
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float normalizedTime = stateInfo.normalizedTime % 1; // 애니메이션 진행 비율 (0~1)

        // 13번째 프레임에서 콜라이더 활성화
        if (normalizedTime >= activationTime && !isColliderActive)
        {
            boss.InitCollider(false, true, false); // atkCollider 활성화
            isColliderActive = true;
        }

        // 18번째 프레임에서 콜라이더 비활성화
        if (normalizedTime >= deactivationTime && isColliderActive)
        {
            boss.InitCollider(true, false, false); // atkCollider 비활성화
            isColliderActive = false;
        }

        // 애니메이션 종료 시점에 상태 전환
        if (normalizedTime >= 0.95f) // 애니메이션 종료
        {
            boss.InitCollider(true, false, false); // 확실히 비활성화
            isColliderActive = false;

            // 플레이어와의 거리 확인
            float distanceToPlayer = Vector3.Distance(boss.transform.position, GameManager.Instance.Player.transform.position);

            if (distanceToPlayer <= 50f)
            {
                animator.SetTrigger("Chase"); // Chase 상태로 전환
            }
            else
            {
                animator.SetTrigger("Run"); // Idle 상태로 전환
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.InitCollider(true, false, false); // 상태 종료 시 콜라이더 비활성화
        isColliderActive = false;
    }
}