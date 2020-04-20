using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageSpawner : MonoBehaviour
{
    public GameObject village;

    public int numberOfVillages;
    public float minDistance, maxDistance;

    private bool finished;

    void Start()
    {
        for (int i = 0; i < numberOfVillages; i++)
        {
            finished = false;

            do
            {
                float deg = Random.Range(0f, 10f);
                deg *= Random.Range(0f, 40f);
                deg += Random.Range(0f, 10f);

                Vector3 position;

                if (i < numberOfVillages / 3)
                {
                    position = new Vector3(Mathf.Cos(deg) * minDistance, 0f, Mathf.Sin(deg) * minDistance);
                }
                else if (i < numberOfVillages - (numberOfVillages / 3))
                {
                    position = new Vector3(Mathf.Cos(deg) * (minDistance + ((maxDistance - minDistance) / 2)), 0f, Mathf.Sin(deg) * (minDistance + ((maxDistance - minDistance) / 2)));
                }
                else
                {
                    position = new Vector3(Mathf.Cos(deg) * maxDistance, 0f, Mathf.Sin(deg) * maxDistance);
                }

                RaycastHit hit;

                if (Physics.SphereCast(transform.position + position + new Vector3(0f, 50f, 0f), 10f, -transform.up, out hit))
                {
                    if (hit.transform.tag == "Terrain")
                    {
                        GameObject temp = Instantiate(village, position, Quaternion.identity);
                        temp.transform.LookAt(Vector3.zero, Vector3.up);
                        finished = true;
                    }
                }

            } while (!finished) ;

    }
    }
}
