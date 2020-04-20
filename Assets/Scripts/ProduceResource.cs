using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceResource : MonoBehaviour
{
    public GameObject resourceSpawner;
    public Resource resource;
    public Power powerScript;
    public GameObject sprite;

    public float timer;

    private void Awake()
    {
        timer = 0f;
    }

    private void Update()
    {
        if (GameManager.Instance != null && resource.Name == "")
        {
            resource = GameManager.Instance.RequestResourceType();
            sprite.GetComponent<SpriteRenderer>().sprite = resource.sprite;
        }

        CheckTime();
    }

    private void CheckTime()
    {
        if (timer >= resource.ProductionTime)
        {
            CreateResource();
            timer = 0f;
        }
        if (powerScript.isPowered)
        {
            timer += Time.deltaTime;
        }
    }

    private void CreateResource()
    {
        GameObject temp = Instantiate(resource.PreFab, resourceSpawner.transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().AddForce(Random.Range(0f, 1f), 0f, 0f);
        temp.name = resource.Name;
    }
}
