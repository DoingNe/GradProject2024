using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleBehavior : StateMachineBehaviour
{
    Boss boss;

    public float timer;
    public float minTime;
    public float maxTime;

    [SerializeField]
    GameObject player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<Boss>();
        player = GameManager.Instance.Player.gameObject;
        boss.state = Define.BossState.Idle;
        boss.InitCollider(true, false, false);
        timer = Random.Range(minTime, maxTime);

        animator.ResetTrigger("Idle");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            int stateNum = Random.Range(1, 3);

            switch (stateNum)
            {
                case 1:
                    animator.SetTrigger("Run");
                    break;
                case 2:
                    timer = Random.Range(minTime, maxTime);
                    break;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (Vector3.Distance(animator.transform.position, player.transform.position) <= 50f)
        {
            animator.SetTrigger("Chase");
        }
    }
}
