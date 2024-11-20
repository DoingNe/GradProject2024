using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStunBehavior : StateMachineBehaviour
{
    private float stunDuration = 2f; // ���� ���� �ð�
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

        // ���� ���� ����
        boss.state = Define.BossState.Stun;

        // ���� �浹 ���� ����
        boss.InitCollider(true, false, false);

        // ���� Ÿ�̸� �ʱ�ȭ
        stunTimer = stunDuration;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� Ÿ�̸� ����
        stunTimer -= Time.deltaTime;

        // ���� ���� �ð� ���� �� ���� ��ȯ
        if (stunTimer <= 0)
        {
            float distanceToPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);

            if (distanceToPlayer <= 30f)
            {
                animator.SetTrigger("Chase"); // Chase Ʈ���� �ߵ�
            }
            else
            {
                animator.SetTrigger("Idle"); // Idle Ʈ���� �ߵ�
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ���� ���� �� �ӵ��� �ʱ�ȭ
        rb.velocity = Vector2.zero;
    }
}