using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapEnemies : MonoBehaviour
{
    public float damage, attackTime, timer;

    private Power powerScript;

    public List<GameObject> enemiesList = new List<GameObject>();

    private void Start()
    {
        powerScript = GetComponent<Power>();
    }

    private void Update()
    {
        if (timer >= attackTime)
        {
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (powerScript.isPowered)
            AttackList();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (powerScript.isPowered && collision.gameObject.tag == "Enemy" && !enemiesList.Contains(collision.gameObject))
        {
            enemiesList.Add(collision.gameObject);
        }
    }

    private void AttackList()
    {
        enemiesList.RemoveAll(go => go == null);

        foreach (var enemy in enemiesList)
        {
            if (enemy != null && timer >= attackTime)
            {
                enemy.SendMessage("TakeDamage", damage);
            }
        }
    }
}
