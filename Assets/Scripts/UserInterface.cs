using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    private static UserInterface _instance;
    public static UserInterface Instance { get { return _instance;  } }

    public float warningTextDuration = 3f;


    private float warningTextTimer;

    private void Awake()
    {
        if (_instance != null && _instance != this.gameObject)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public TextMeshProUGUI warningText;

    void Start()
    {
        
    }

    void Update()
    {
        Timers();
        ResetTimers();
    }

    public void DisplayWarning(string warning)
    {
        warningTextTimer = 0f;

        warningText.text = warning;
    }

    private void Timers()
    {
        warningTextTimer += Time.deltaTime;
    }

    private void ResetTimers()
    {
        if (warningTextTimer >= warningTextDuration)
            warningText.text = "";
    }
}
