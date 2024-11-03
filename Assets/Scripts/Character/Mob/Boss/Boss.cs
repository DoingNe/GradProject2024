using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public int health = 50;
    int damage = 1;
    int dashDamage = 3;

    private float timeBtwDamage = 1.5f;

    private Animator anim;
    public bool isDead;

    void Start()
    {
        anim = GetComponent<Animator>();

        health = 50;
        damage = 1;
        dashDamage = 3;
    }

    void Update()
    {
        if (health <= 0)
        {
            isDead = true;
            anim.SetTrigger("Die");
        }

        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }
    }

    public void GameOver()
    {
        if (isDead)
        {
            GameManager.Instance.Player.isPlaying = false;
            Debug.Log("º¸½º ÅðÄ¡");
            SceneManager.LoadScene("Result");
        }/*
        Debug.Log("º¸½º ÅðÄ¡");
        SceneManager.LoadScene("Result");*/
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
