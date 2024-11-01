using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject mobPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnMob();
    }

    void SpawnMob()
    {
        GameObject mob = Instantiate(mobPrefab, transform.position, transform.rotation);
        Mob mobScript = mob.GetComponent<Mob>();

        if (Random.value < 0.5f)
        {
            Vector3 scale = mob.transform.localScale;
            scale.x *= -1;
            mob.transform.localScale = scale;
        }

        mobScript.OnDeath += () => StartCoroutine(RespawnMob());
    }

    IEnumerator RespawnMob()
    {
        float waitTime = Random.Range(3f, 5f);
        float alertTime = waitTime - 1f;

        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(alertTime);

        if (spriteRenderer != null)
        {
            Color origin = spriteRenderer.material.color;

            spriteRenderer.material.color = Color.red;
            yield return new WaitForSeconds(1f);
            spriteRenderer.material.color = origin;
        }

        SpawnMob();
    }
}
