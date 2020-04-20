using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildInventory : MonoBehaviour
{
    public int coal, copper, wood, buildingSelection;
    public float buildDistance = 30f;
    public float powerPoleDistance = 30f;
    public Building[] buildings;
    public bool isBuilding;
    public LayerMask playerLayer;
    public Material goodPlacement, badPlacement;
    public GameObject buildingWarning;
    public GameObject weapon;
    public GameObject bulletPrefab;

    public TextMeshProUGUI warningText, woodAmount, coalAmount, copperAmount;

    private GameObject previewBuilding;
    private GameObject powerFrom;

    private float shootTimer;

    void Start()
    {
        UpdateResourceUI();
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
        }

        if (previewBuilding == null)
        {
            previewBuilding = Instantiate(buildings[buildingSelection].Preview, Vector3.zero, Quaternion.identity);
            previewBuilding.SetActive(false);
        }

        if (!isBuilding)
        {
            previewBuilding.SetActive(false);
            previewBuilding.transform.position = Vector3.zero;
            powerFrom = null;
            OtherRaycast();
            buildingWarning.SetActive(false);
            weapon.SetActive(true);

            if (Input.GetMouseButtonDown(1))
                Shoot();
        }

        if (isBuilding)
        {
            Building();
            buildingWarning.SetActive(true);
            weapon.SetActive(false);
        }

        if (shootTimer < 1f)
            shootTimer += Time.deltaTime;

        RightMouseButton();

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void Building()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, buildDistance, ~playerLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (powerFrom == null && (hit.transform.root.tag == "PowerPole" || hit.transform.root.tag == "PowerStation"))
                {
                    if (hit.transform.root.GetComponent<Power>().CanConnect())
                        powerFrom = hit.transform.root.gameObject;

                }
                else if (hit.transform.tag != "Terrain" && powerFrom != null && powerFrom != hit.transform.root.gameObject && DistanceIsWithinRange(powerFrom, hit.transform.root.gameObject))
                {
                    previewBuilding.SetActive(false);
                    NewPowerConnection(hit.transform.root.gameObject);
                }
            }

            if (hit.transform.tag == "Terrain")
                previewBuilding.transform.position = hit.point;

            if (hit.transform.tag == "Terrain" && powerFrom != null && DistanceIsWithinRange(powerFrom, previewBuilding))
            {
                previewBuilding.SetActive(true);
                previewBuilding.transform.Find("Mesh").GetComponent<Renderer>().material = goodPlacement;
                if (Input.GetMouseButtonDown(0))
                {
                    NewPowerPole();
                }
            }
            else
            {
                previewBuilding.transform.Find("Mesh").GetComponent<Renderer>().material = badPlacement;
            }

        }
        else
        {
            previewBuilding.SetActive(false);
        }
    }

    private void NewPowerPole()
    {
        if (!HasEnoughResources())
        {
            UserInterface.Instance.DisplayWarning("Not Enough Resources!");
            return;
        }
        else
        {
            wood -= buildings[0].WoodCost;
            copper -= buildings[0].CopperCost;
        }

        GameObject newPowerPole = Instantiate(buildings[buildingSelection].PreFab, previewBuilding.transform.position, Quaternion.identity);
        newPowerPole.GetComponent<Power>().AddConnection(powerFrom);
        powerFrom.GetComponent<Power>().AddConnection(newPowerPole);


        powerFrom = newPowerPole;
    }

    private void NewPowerConnection(GameObject otherPoweredObject)
    {
        if (!otherPoweredObject.GetComponent<Power>().AddConnection(powerFrom))
        {
            UserInterface.Instance.DisplayWarning("Too Many connections!");
            powerFrom = null;
            return;
        }
        powerFrom.GetComponent<Power>().AddConnection(otherPoweredObject);

        if (otherPoweredObject.GetComponent<Power>().CanConnect())
            powerFrom = otherPoweredObject;
        else
            powerFrom = null;
    }

    private bool DistanceIsWithinRange(GameObject pos1, GameObject pos2)
    {
        if (Vector3.Distance(pos1.transform.position, pos2.transform.position) <= powerPoleDistance)
        {
            return true;
        }
        else
        {
            UserInterface.Instance.DisplayWarning("Line is too long!");
            return false;
        }
    }

    private void RightMouseButton()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isBuilding = false;
        }
    }

    private bool HasEnoughResources()
    {
        if (copper >= buildings[0].CopperCost && wood >= buildings[0].WoodCost)
            return true;

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Coal" || other.transform.name == "Wood" || other.transform.name == "Copper")
        {
            CollectResources(other.transform.name);
            Destroy(other.gameObject);
        }
    }

    private bool CollectResources(string name)
    {
        if (name == "Coal")
            coal++;
        if (name == "Wood")
            wood++;
        if (name == "Copper")
            copper++;
        
        UpdateResourceUI();

        return false;
    }

    private void UpdateResourceUI()
    {
        woodAmount.text = wood.ToString();
        coalAmount.text = coal.ToString();
        copperAmount.text = copper.ToString();
    }

    private void OtherRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 15f, ~playerLayer))
        {
            if (hit.transform.root.tag == "PowerStation" && coal > 0)
            {
                UserInterface.Instance.DisplayWarning("Deposit Coal (E)");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    coal--;
                    hit.transform.root.SendMessage("ChangeResource", 1);
                    UpdateResourceUI();
                }
            }
        }
    }

    private void Shoot()
    {
        if (shootTimer < 1f)
            return;

        shootTimer = 0f;

        GameObject bullet = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(weapon.transform.forward * 3000f);
    }
}