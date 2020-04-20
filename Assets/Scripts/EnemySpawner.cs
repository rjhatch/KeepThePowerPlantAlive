using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numberOfEnemiesToSpawn;
    public float spawnTime;
    public bool spawnEnemies;
    public GameObject enemy;
    public float timer, gameTimer;

    private List<GameObject> enemies = new List<GameObject>();

    void Update()
    {
        if (spawnEnemies)
            Spawn();

        gameTimer += Time.deltaTime;

        if (gameTimer >= 60f)
            spawnTime = 2;
        if (gameTimer >= 120f)
            spawnTime = .4f;
    }

    private void Spawn()
    {
        if (timer < spawnTime)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            timer = 0f;
        }

        if (enemies.Count >= numberOfEnemiesToSpawn)
            return;

        float deg = Random.Range(0f, 10f);
        deg *= Random.Range(0f, 40f);
        deg += Random.Range(0f, 10f);

        float distance = Random.Range(200f, 400f);


        Vector3 position = new Vector3(Mathf.Cos(deg) * distance, 0f, Mathf.Sin(deg) * distance);
        position += new Vector3(0f, 50f, 0f);

        RaycastHit hit;


        if (Physics.SphereCast(transform.position + position, 2f, -transform.up, out hit))
        {
            if (hit.transform.tag == "Terrain" || hit.transform.tag == "Enemy")
            {
                GameObject temp = Instantiate(enemy, hit.point, Quaternion.identity);
                enemies.Add(temp);
            }
        }

        enemies.RemoveAll(go => go == null);
    }
}
