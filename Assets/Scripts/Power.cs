using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public bool isPowerPole;
    public bool isPowered;
    public bool isDestroyed;
    public bool changePowerStatus;
    public int connectionLimit;

    public GameObject cable;
    public Transform powerAttachPoint;

    public List<GameObject> connectedPoles = new List<GameObject>();
    public List<Cable> connectedCables = new List<Cable>();

    private List<Cable> cablesToDestroy = new List<Cable>();

    private bool registered;

    private void Update()
    {
        if (GameManager.Instance != null && !registered)
        {
            GameManager.Instance.AddPoweredObject(this.gameObject);
            registered = true;
        }

        if (isDestroyed)
            Destroy(this.gameObject);

        foreach (var cable in connectedCables)
        {
            if (cable.OtherPoweredObject == null)
            {
                cablesToDestroy.Add(cable);
            }
        }

        if (cablesToDestroy.Count > 0)
            DestroyCables();
    }

    public bool AddConnection(GameObject otherPoweredObject)
    {
        if (!CanConnect())
            return false;

        GameObject tempCable = Instantiate(cable, powerAttachPoint.transform.position, Quaternion.identity);
        tempCable.transform.LookAt(otherPoweredObject.transform.Find("PowerAttachPoint").transform);
        tempCable.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(otherPoweredObject.transform.Find("PowerAttachPoint").position, powerAttachPoint.position));
        tempCable.transform.parent = gameObject.transform;
        connectedCables.Add(new Cable(tempCable, otherPoweredObject));

        connectedPoles.Add(otherPoweredObject);

        GameManager.Instance.CheckPower();

        return true;
    }

    public void RemoveConnection(GameObject otherPoweredObject)
    {
        if (connectedPoles.Contains(otherPoweredObject))
            connectedPoles.Remove(otherPoweredObject);

        GameManager.Instance.CheckPower();
    }

    private void OnDestroy()
    {
        if (connectedPoles.Count > 0)
        {
            foreach (var poweredObject in connectedPoles)
            {
                if (poweredObject != null)
                    poweredObject.GetComponent<Power>().RemoveConnection(this.gameObject);
            }
        }

        GameManager.Instance.RemovePoweredObject(this.gameObject);
    }

    private void DestroyCables()
    {
        foreach (var cable in cablesToDestroy)
        {
            if (connectedCables.Contains(cable))
            {
                connectedCables.Remove(cable);
                Destroy(cable.CableObject);
            }
        }

        cablesToDestroy.Clear();
    }

    public bool CanConnect()
    {
        if (connectedPoles.Count < connectionLimit)
        {
            return true;
        }
        return false;
    }

    public void PowerStatusChange(bool powered)
    {

        if (transform.tag == "PowerStation")
            return;

        isPowered = powered;

        if (transform.tag == "Village")
        {
            transform.Find("Powered").gameObject.SetActive(isPowered);
        }

        if (transform.tag != "PowerPole")
            return;

        if (isPowered == false)
        {
            transform.Find("Mesh").GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            transform.Find("Mesh").GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public List<GameObject> ConnectedList(List<GameObject> connectedList)
    {
        if (!connectedList.Contains(gameObject))
        {
            connectedList.Add(gameObject);

            foreach (var connection in connectedPoles)
            {
                connection.GetComponent<Power>().ConnectedList(connectedList);
            }
        }
        
        return connectedList;
    }
}
