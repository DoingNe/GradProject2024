using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseBehavior : StateMachineBehaviour
{
    private Boss boss;

    [SerializeField]
    private GameObject player;

    public float speed = 5; // �̵� �ӵ�
    public float dashAttackDistance = 40f;
    public float meleeAttackDistance = 5f; // �÷��̾���� �ּ� �Ÿ�

    private float lastDashAttackTime;
    private float dashAttackCooldown = 15f;

    private float lastMeleeAttackTime;
    private float meleeAttackCooldown = 3f;

    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private Vector2 direction;

    public LayerMask platformLayer; // �÷��� ���̾�

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();

        player = GameManager.Instance.Player.gameObject;

        boss.state = Define.BossState.Chase;
        boss.InitCollider(true, false, false);
        bossTransform = animator.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Instance.Player.transform;

        // �ʱ� ���� ����
        UpdateDirection();

        animator.ResetTrigger("Chase");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(bossTransform.position, playerTransform.position);

            // �÷��̾���� �Ÿ��� Ȯ���Ͽ� �������� ����
            if (distanceToPlayer > dashAttackDistance)
            {
                MoveBoss();
            }
            else if (distanceToPlayer > meleeAttackDistance)
            {
                MoveBoss();
                if (Time.time - boss.lastDashAttackTime >= dashAttackCooldown)
                {
                    //rb.velocity = Vector2.zero; // ����
                    animator.SetTrigger("Dash");
                    boss.lastDashAttackTime = Time.time; // Boss Ŭ������ ���
                }
            }
            else if (distanceToPlayer <= meleeAttackDistance && boss.state != Define.BossState.Dash)
            {
                MoveBoss();
                if (Time.time - boss.lastMeleeAttackTime >= meleeAttackCooldown)
                {
                    //rb.velocity = Vector2.zero; // ����
                    animator.SetTrigger("MeleeAttack");
                    boss.lastMeleeAttackTime = Time.time; // Boss Ŭ������ ���
                }
            }

            // �÷��̾� ��ġ�� ���� ���� ������Ʈ
            UpdateDirection();
        }

        // Chase ���¸� �������� Ȯ��
        if (Vector3.Distance(bossTransform.position, playerTransform.position) > 50f)
        {
            animator.SetTrigger("Run"); // Run ���·� ��ȯ
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void UpdateDirection()
    {
        // �÷��̾� �������� ������Ʈ
        if (playerTransform != null)
        {
            direction = (playerTransform.position - bossTransform.position).normalized;
            direction.y = 0f;

            // ������ ������Ʈ
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        // ���⿡ ���� ���� ������ ����
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
        // �÷��̾� �������� �̵�
        rb.velocity = direction * speed;
    }
}
