using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRunBehavior : StateMachineBehaviour
{
    Boss boss;

    public float speed = 5;
    public float minTime;
    public float maxTime;

    // 타이머 변수 추가 (인스펙터에서 확인 가능)
    [SerializeField]
    private float timer;

    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private Vector2 direction; // 이동 방향 변수 추가

    public float wallCheckDistance = 3.7f; // 전방 감지 거리
    public LayerMask platformLayer; // 플랫폼 레이어

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        bossTransform = animator.transform;
        boss.state = Define.BossState.Run;
        boss.InitCollider(true, false, false);
        rb = bossTransform.GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Instance.Player.transform;

        // 초기 방향 설정
        SetInitialDirection();

        // 타이머 초기화
        timer = Random.Range(minTime, maxTime);

        animator.ResetTrigger("Run");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveBoss();

        // 벽 체크
        CheckForWall();

        // 플레이어 위치에 따라 방향 업데이트
        UpdateDirectionBasedOnPlayer();

        // 타이머 업데이트 및 상태 전환
        if (timer <= 0)
        {
            animator.SetTrigger("Idle"); // Idle 상태로 전환
        }
        else
        {
            timer -= Time.deltaTime; // 타이머 감소
        }

        if (Vector3.Distance(bossTransform.position, playerTransform.position) <= 50f)
        {
            animator.SetTrigger("Chase");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
    }

    private void SetInitialDirection()
    {
        // 초기 방향을 랜덤으로 설정
        direction = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        UpdateScale();
    }

    private void UpdateDirectionBasedOnPlayer()
    {
        // 플레이어와의 거리 및 위치에 따라 방향을 업데이트
        if (playerTransform != null && (Mathf.Abs(playerTransform.position.x - bossTransform.position.x) < 10f && Mathf.Abs(playerTransform.position.y - bossTransform.position.y) < 15f))
        {
            direction = new Vector2(playerTransform.position.x - bossTransform.position.x, 0f).normalized;
        }
    }

    private void CheckForWall()
    {
        // Raycast로 벽 체크
        Vector2 wallCheckPosition = (Vector2)bossTransform.position + direction * wallCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(bossTransform.position + new Vector3(0f, 0.5f, 0f), direction, wallCheckDistance, platformLayer);

        // 벽이 감지되면 방향을 반전
        if (hit.collider != null)
        {
            direction = -direction; // 방향 반전
            UpdateScale(); // 스케일 업데이트
        }
    }

    private void UpdateScale()
    {
        // 스케일 변경
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
        rb.velocity = direction * speed; // 설정된 방향으로 이동
        UpdateScale(); // 이동 방향에 따라 스케일 업데이트
    }
}