using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttackBehavior : StateMachineBehaviour
{
    private Boss boss;
    private bool isColliderActive;

    private float activationTime = 13f / 19f; // 13��° ������ (����)
    private float deactivationTime = 18f / 19f; // 18��° ������ (����)

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        boss.state = Define.BossState.MeleeAttack;
        isColliderActive = false;
        boss.InitCollider(false, false, false); // �ʱ�ȭ ����
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float normalizedTime = stateInfo.normalizedTime % 1; // �ִϸ��̼� ���� ���� (0~1)

        // 13��° �����ӿ��� �ݶ��̴� Ȱ��ȭ
        if (normalizedTime >= activationTime && !isColliderActive)
        {
            boss.InitCollider(false, true, false); // atkCollider Ȱ��ȭ
            isColliderActive = true;
        }

        // 18��° �����ӿ��� �ݶ��̴� ��Ȱ��ȭ
        if (normalizedTime >= deactivationTime && isColliderActive)
        {
            boss.InitCollider(true, false, false); // atkCollider ��Ȱ��ȭ
            isColliderActive = false;
        }

        // �ִϸ��̼� ���� ������ ���� ��ȯ
        if (normalizedTime >= 0.95f) // �ִϸ��̼� ����
        {
            boss.InitCollider(true, false, false); // Ȯ���� ��Ȱ��ȭ
            isColliderActive = false;

            // �÷��̾���� �Ÿ� Ȯ��
            float distanceToPlayer = Vector3.Distance(boss.transform.position, GameManager.Instance.Player.transform.position);

            if (distanceToPlayer <= 50f)
            {
                animator.SetTrigger("Chase"); // Chase ���·� ��ȯ
            }
            else
            {
                animator.SetTrigger("Run"); // Idle ���·� ��ȯ
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.InitCollider(true, false, false); // ���� ���� �� �ݶ��̴� ��Ȱ��ȭ
        isColliderActive = false;
    }
}