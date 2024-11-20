using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public Define.BossState state;

    public int health = 50;
    public int damage = 2;
    public int atkDamage = 3;
    public int dashDamage = 5;

    private float timeBtwDamage = 1.5f;

    public float lastDashAttackTime = -Mathf.Infinity;
    public float lastMeleeAttackTime = -Mathf.Infinity;

    private Animator anim;
    public GameObject hitBoxCollider;
    public GameObject atkCollider;
    public GameObject dashCollider;

    public Slider hpBar;

    public bool isDead;

    void Start()
    {
        anim = GetComponent<Animator>();

        health = 50;
        damage = 1;
        dashDamage = 3;

        hpBar.maxValue = health;
        hpBar.minValue = 0;

        InitCollider(true, false, false);
    }

    void Update()
    {
        hpBar.value = health;

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
            Debug.Log("보스 퇴치");
            SceneManager.LoadScene("Result");
        }/*
        Debug.Log("보스 퇴치");
        SceneManager.LoadScene("Result");*/
    }

    public void InitCollider(bool hitbox, bool atk, bool dash)
    {
        hitBoxCollider.SetActive(hitbox);
        atkCollider.SetActive(atk);
        dashCollider.SetActive(dash);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            Debug.Log("플레이어 공격에 피격당함");
            health = Mathf.Clamp(health - GameManager.Instance.Player.Atk, 0, 50);
        }

        /*if(collision.CompareTag("Player") && isDead == false)
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
        }*/
    }
}
