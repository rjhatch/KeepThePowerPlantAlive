using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerStation : MonoBehaviour
{
    public int amountOfCoal = 3;
    public float consumptionRate;
    public bool producingPower = true;

    public TextMeshProUGUI coalText;
    public GameObject powerPlantStatus;

    private bool sentPowerChange;

    private float timer = 0f;

    private void Start()
    {
        ChangeResource(0);
    }

    void Update()
    {
        if (amountOfCoal > 0 && !producingPower)
        {
            sentPowerChange = false;
            producingPower = true;
        }
        else if (amountOfCoal <= 0 && producingPower)
        {
            sentPowerChange = false;
            producingPower = false;
        }

        if (!producingPower && !sentPowerChange)
        {
            GameManager.Instance.PowerOff();
            sentPowerChange = true;
        }
        else if (producingPower && !sentPowerChange)
        {
            GameManager.Instance.PowerOn();
            sentPowerChange = true;
        }

        if (producingPower)
        {
            if (timer >= consumptionRate)
            {
                ChangeResource(-1);
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void ChangeResource(int change)
    {
        amountOfCoal += change;

        coalText.text = amountOfCoal.ToString();

        if (amountOfCoal < 0)
            amountOfCoal = 0;

        if (amountOfCoal == 0)
        {
            powerPlantStatus.GetComponent<Image>().color = Color.red;
        }
        else if (amountOfCoal <= 7)
        {
            powerPlantStatus.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            powerPlantStatus.GetComponent<Image>().color = Color.green;
        }

    }

}
