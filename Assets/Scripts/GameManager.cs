using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public GameObject powerStation;

    public Resource[] resources;

    public List<GameObject> buildings = new List<GameObject>();
    public List<GameObject> poweredBuildings = new List<GameObject>();

    private void Awake()
    {
        if (_instance != null && _instance != this.gameObject)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    void Start()
    {
        buildings.Add(powerStation);
    }

    void Update()
    {
        if (powerStation == null)
            SceneManager.LoadScene(0);
    }

    public void AddPoweredObject(GameObject poweredObject)
    {
        if (!buildings.Contains(poweredObject))
            buildings.Add(poweredObject);

        CheckPower();
    }

    public void RemovePoweredObject(GameObject poweredObject)
    {
        if (buildings.Contains(poweredObject))
            buildings.Remove(poweredObject);

        CheckPower();
    }

    public void CheckPower()
    {
        if (powerStation == null)
            return;

        powerStation.GetComponent<Power>().ConnectedList(poweredBuildings);

        foreach (var connection in buildings)
        {
            if (!poweredBuildings.Contains(connection))
                connection.GetComponent<Power>().PowerStatusChange(false);
            else
                connection.GetComponent<Power>().PowerStatusChange(true);
        }

        poweredBuildings.Clear();
    }

    public void PowerOff()
    {
        foreach (var building in buildings)
        {
            building.GetComponent<Power>().PowerStatusChange(false);
        }
    }

    public void PowerOn()
    {
        CheckPower();
    }

    public Resource RequestResourceType()
    {
        return resources[Random.Range(0, resources.Length)];
    }
}
