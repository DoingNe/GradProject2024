using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleBehavior : StateMachineBehaviour
{
    public float timer;
    public float minTime;
    public float maxTime;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
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
    }
}
