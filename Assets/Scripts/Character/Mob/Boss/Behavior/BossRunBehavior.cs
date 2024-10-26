using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRunBehavior : StateMachineBehaviour
{
    public float speed;
    public float minTime;
    public float maxTime;

    // Ÿ�̸� ���� �߰� (�ν����Ϳ��� Ȯ�� ����)
    [SerializeField]
    private float timer;

    private Transform bossTransform;
    private Transform playerTransform;
    private Rigidbody2D rb;

    private Vector2 direction; // �̵� ���� ���� �߰�

    public float wallCheckDistance = 3.7f; // ���� ���� �Ÿ�
    public LayerMask platformLayer; // �÷��� ���̾�

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossTransform = animator.transform;
        rb = bossTransform.GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Instance.Player.transform;

        // �ʱ� ���� ����
        SetInitialDirection();

        // Ÿ�̸� �ʱ�ȭ
        timer = Random.Range(minTime, maxTime);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MoveBoss();

        // �� üũ
        CheckForWall();

        // �÷��̾� ��ġ�� ���� ���� ������Ʈ
        UpdateDirectionBasedOnPlayer();

        // Ÿ�̸� ������Ʈ �� ���� ��ȯ
        if (timer <= 0)
        {
            animator.SetTrigger("Idle"); // Idle ���·� ��ȯ
        }
        else
        {
            timer -= Time.deltaTime; // Ÿ�̸� ����
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
    }

    private void SetInitialDirection()
    {
        // �ʱ� ������ �������� ����
        direction = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        UpdateScale();
    }

    private void UpdateDirectionBasedOnPlayer()
    {
        // �÷��̾���� �Ÿ� �� ��ġ�� ���� ������ ������Ʈ
        if (playerTransform != null && (Mathf.Abs(playerTransform.position.x - bossTransform.position.x) < 15f && Mathf.Abs(playerTransform.position.y - bossTransform.position.y) < 15f))
        {
            direction = (playerTransform.position - bossTransform.position).normalized;
        }
    }

    private void CheckForWall()
    {
        // Raycast�� �� üũ
        Vector2 wallCheckPosition = (Vector2)bossTransform.position + direction * wallCheckDistance;
        RaycastHit2D hit = Physics2D.Raycast(bossTransform.position + new Vector3(0f, 0.5f, 0f), direction, wallCheckDistance, platformLayer);

        // ���� �����Ǹ� ������ ����
        if (hit.collider != null)
        {
            direction = -direction; // ���� ����
            UpdateScale(); // ������ ������Ʈ
        }
    }

    private void UpdateScale()
    {
        // ������ ����
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
        rb.velocity = direction * speed; // ������ �������� �̵�
        UpdateScale(); // �̵� ���⿡ ���� ������ ������Ʈ
    }
}