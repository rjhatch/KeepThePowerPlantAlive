using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float searchDistance, speed, attackSpeed, attackDamage;
    public bool colliding;

    public GameObject target;

    private float attackTimer;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (target == null)
            Search();

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if (hit.transform.gameObject == target)
                Attack();
        }
    }

    private void FixedUpdate()
    {
        MoveRB();
    }

    private void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchDistance);
        float closestTarget = 100f;

        foreach (var col in colliders)
        {
            if (col.transform.root.tag == "PowerPole" || col.transform.root.tag == "Village")
            {
                float temp = Vector3.Distance(col.transform.root.position, transform.position);

                if ( temp < closestTarget)
                {
                    closestTarget = temp;
                    target = col.transform.root.gameObject;
                }
            }
        }
    }

    private void Attack()
    {
        if (attackTimer >= attackSpeed)
        {
            attackTimer = 0f;
            target.SendMessage("TakeDamage", attackDamage);
        }
        else
            attackTimer += Time.deltaTime;
    }
    
    private void MoveRB()
    {
        if (target != null)
        {
            var qTo = Quaternion.LookRotation(target.transform.position - transform.position);
            qTo = Quaternion.Slerp(transform.rotation, qTo, speed * Time.fixedDeltaTime);
            rb.MoveRotation(qTo);
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            var qTo = Quaternion.LookRotation(Vector3.zero - transform.position);
            qTo = Quaternion.Slerp(transform.rotation, qTo, speed * Time.fixedDeltaTime);
            rb.MoveRotation(qTo);
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }
}
