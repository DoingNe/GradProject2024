using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashBehavior : StateMachineBehaviour
{
    Boss boss;

    private Transform bossTransform;
    private Rigidbody2D rb;
    private Animator animator;

    private Vector2 dashDirection;
    private float dashSpeed = 20f;
    private float dashDuration = 3f;
    private float dashTimer;

    public float wallCheckDistance = 3.7f; // 벽 체크 거리
    public LayerMask platformLayer; // 플랫폼 레이어

    private System.Action checkForWall; // 외부에서 받아오는 벽 체크 함수

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 필요한 컴포넌트 및 초기 설정
        boss = animator.GetComponent<Boss>();
        bossTransform = animator.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();
        this.animator = animator;

        boss.state = Define.BossState.Dash;

        // 대시 방향 설정 (현재 x스케일 값으로 방향 결정)
        dashDirection = bossTransform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // 대시 타이머 초기화
        dashTimer = dashDuration;

        boss.InitCollider(false, false, true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 벽 감지 및 처리
        if (CheckForWall())
        {
            Debug.Log("Wall detected. Transitioning to Stun.");
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Stun");
            return;
        }

        // 대시 진행
        Debug.Log(dashSpeed);
        rb.velocity = dashDirection * dashSpeed;
        dashTimer -= Time.deltaTime;

        if(dashTimer <= 0)
        {
            // 대시 종료 후 상태 전환 (플레이어 위치에 따라 처리)
            rb.velocity = Vector2.zero;
            TransitionAfterDash();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 상태 종료 시 속도를 초기화
        rb.velocity = Vector2.zero;
        boss.InitCollider(false, false, false);
        Debug.Log("Dash State Exited.");
    }

    private bool CheckForWall()
    {
        // 벽 감지 Raycast
        Vector3 rayOrigin = bossTransform.position + new Vector3(0f, 1f, 0f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dashDirection, wallCheckDistance, platformLayer);

        Debug.DrawRay(rayOrigin, dashDirection * wallCheckDistance, Color.red); // 시각적 디버깅
        return hit.collider != null; // 감지 여부 반환
    }

    private void TransitionAfterDash()
    {
        // 플레이어 위치에 따라 트리거 발동 (다른 스크립트 로직 참고)
        GameObject player = GameManager.Instance.Player.gameObject;
        float distanceToPlayer = Vector3.Distance(bossTransform.position, player.transform.position);

        if (distanceToPlayer <= 50f)
        {
            animator.SetTrigger("Chase"); // Chase 상태로 전환
        }
        else
        {
            animator.SetTrigger("Run"); // Run 상태로 전환
        }
    }
}