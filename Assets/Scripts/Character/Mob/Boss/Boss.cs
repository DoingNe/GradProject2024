using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health;
    public int damage;
    public int dashDamage;
    private float timeBtwDamage = 1.5f;

    private Animator anim;
    public bool isDead;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Die");
        }

        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Result");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isDead == false)
        {
            if (timeBtwDamage <= 0)
            {
                // Dash
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
                {
                    collision.GetComponent<Player>().Hp -= dashDamage;
                }
                else
                {
                    collision.GetComponent<Player>().Hp -= damage;
                }
                
            }
        }
    }
}
