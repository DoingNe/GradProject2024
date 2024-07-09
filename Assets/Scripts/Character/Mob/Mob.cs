using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : Character
{
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    GameObject player;

    [SerializeField]
    bool isPatrolling;
    [SerializeField]
    bool isChasing;
    [SerializeField]
    int direction = 0; // -1 = left, 0 = stop, 1 = right
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float detectionDistance = 5f;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        isChasing = false;

        // Start Patrol
        isPatrolling = true;
        Invoke("MobPatrol", Random.Range(3, 6));
    }

    void FixedUpdate()
    {
        // If enemy is chasing the player
        if (isChasing)
        {
            if (transform.position.x < player.transform.position.x) // enemy is on left of player
            {
                direction = 1; // chase to right
            }
            else if (transform.position.x > player.transform.position.x) // enemy is on right of player
            {
                direction = -1; // chase to left
            }
        }

        // Move
        rigidBody.velocity = new Vector2(direction * speed, rigidBody.velocity.y);

        // Player detection
        RaycastHit2D playerRayCast = Physics2D.Raycast(transform.position, new Vector2(direction, 0), detectionDistance, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, new Vector2(direction, 0) * detectionDistance, Color.green);

        if (playerRayCast.collider != null) // Detected player
        {
            // Stop patrol
            isPatrolling = false;
            CancelInvoke("MobPatrol");

            // Chase player
            isChasing = true;
            player = playerRayCast.transform.gameObject;
        }
        else
        {
            isChasing = false; // Stop chasing
            isPatrolling = true; // Start patrol
        }

        // Platform detection
        Vector2 frontVec = new Vector2(direction * 0.5f + rigidBody.position.x, rigidBody.position.y);
        RaycastHit2D platformRayCast = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));

        if (platformRayCast.collider == null) // Detected the end of platform
        {
            direction *= -1; // prevent falling
            if (isPatrolling)
            {
                CancelInvoke("MobPatrol");
                MobPatrol();
            }
        }
    }

    void MobPatrol()
    {
        direction = Random.Range(-1, 2); // -1 ~ 1
        float nextTime = Random.Range(1f, 3f); // repeat every 1~3 seconds
        Invoke("MobPatrol", nextTime); 
    }
}
