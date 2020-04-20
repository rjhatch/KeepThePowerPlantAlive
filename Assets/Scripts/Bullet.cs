using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.root.tag == "Enemy")
            collision.transform.root.SendMessage("TakeDamage", 100);

        Destroy(gameObject);
    }
}
