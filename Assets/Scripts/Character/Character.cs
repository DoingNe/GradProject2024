using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int hp = 100;
    public int Hp { get { return hp; } set { hp = value; } }

    protected void Damage(int damage, Vector2 targetPos)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(OnDamaged(targetPos));
        }
    }

    protected virtual void Die()
    {

    }

    // 데미지를 받았을 때 반짝임
    protected virtual IEnumerator OnDamaged(Vector2 targetPos)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        yield return new WaitForSeconds(2f);

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
