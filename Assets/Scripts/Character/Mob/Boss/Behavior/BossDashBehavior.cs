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

    public float wallCheckDistance = 3.7f; // �� üũ �Ÿ�
    public LayerMask platformLayer; // �÷��� ���̾�

    private System.Action checkForWall; // �ܺο��� �޾ƿ��� �� üũ �Լ�

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �ʿ��� ������Ʈ �� �ʱ� ����
        boss = animator.GetComponent<Boss>();
        bossTransform = animator.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();
        this.animator = animator;

        boss.state = Define.BossState.Dash;

        // ��� ���� ���� (���� x������ ������ ���� ����)
        dashDirection = bossTransform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // ��� Ÿ�̸� �ʱ�ȭ
        dashTimer = dashDuration;

        boss.InitCollider(false, false, true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // �� ���� �� ó��
        if (CheckForWall())
        {
            Debug.Log("Wall detected. Transitioning to Stun.");
            rb.velocity = Vector2.zero;
            animator.SetTrigger("Stun");
            return;
        }

        // ��� ����
        Debug.Log(dashSpeed);
        rb.velocity = dashDirection * dashSpeed;
        dashTimer -= Time.deltaTime;

        if(dashTimer <= 0)
        {
            // ��� ���� �� ���� ��ȯ (�÷��̾� ��ġ�� ���� ó��)
            rb.velocity = Vector2.zero;
            TransitionAfterDash();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ���� �� �ӵ��� �ʱ�ȭ
        rb.velocity = Vector2.zero;
        boss.InitCollider(false, false, false);
        Debug.Log("Dash State Exited.");
    }

    private bool CheckForWall()
    {
        // �� ���� Raycast
        Vector3 rayOrigin = bossTransform.position + new Vector3(0f, 1f, 0f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dashDirection, wallCheckDistance, platformLayer);

        Debug.DrawRay(rayOrigin, dashDirection * wallCheckDistance, Color.red); // �ð��� �����
        return hit.collider != null; // ���� ���� ��ȯ
    }

    private void TransitionAfterDash()
    {
        // �÷��̾� ��ġ�� ���� Ʈ���� �ߵ� (�ٸ� ��ũ��Ʈ ���� ����)
        GameObject player = GameManager.Instance.Player.gameObject;
        float distanceToPlayer = Vector3.Distance(bossTransform.position, player.transform.position);

        if (distanceToPlayer <= 50f)
        {
            animator.SetTrigger("Chase"); // Chase ���·� ��ȯ
        }
        else
        {
            animator.SetTrigger("Run"); // Run ���·� ��ȯ
        }
    }
}