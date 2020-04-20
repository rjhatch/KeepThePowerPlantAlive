using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public List<GameObject> points;

    public int cameraPoint = 0;

    public TextMeshProUGUI helpTextGUI;
    public GameObject title;

    private string[] helpText;

    // Start is called before the first frame update
    void Start()
    {
        helpText = new string[] {
            "This is your PowerStation, protect it.",
            "These are power poles; they change color to indicate good/bad placement. Once they are placed, if they are powered, they turn green, if they are not powered, they turn red.",
            "Press [B] to build. You have to start from the power station, but you can build from other poles as well. Poles are limited to 3 other connections.",
            "This is a village, it produces one of three resources, when powered. Powered indicated by the horrid zappy thing on the right.",
            "Coal. Feed this to the PowerStation by pressing [E] and looking at it.",
            "Wood. Needed to build poles. 2 per pole.",
            "Copper. Needed to build poles. 2 per pole.",
            "These are enemies. What can I say? I ran out of time. Also, they only attack the buildings. So, they are like zombies, but only for buildings.",
            "Also, enemies that are touching powered villages get zapped. There is no visual/auditory indicatory, you just have to trust me.",
            "Right click to shoot a cannon thing. Shoots once, per second. One shot enemies.",
            "There’s a HUD in the game and I hope you can figure it out. Also, there aren’t any sounds. So, you aren’t going crazy."            
        };
        ChangePoint();

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            cameraPoint++;
            if (cameraPoint > points.Count - 1)
                cameraPoint = 0;
            ChangePoint();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            cameraPoint--;
            if (cameraPoint < 0)
                cameraPoint = points.Count - 1;

            ChangePoint();
        }

        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(1);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void ChangePoint()
    {
        title.SetActive(false);
        Camera.main.transform.position = points[cameraPoint].transform.position;
        Camera.main.transform.rotation = points[cameraPoint].transform.rotation;

        helpTextGUI.text = "(" + (cameraPoint + 1) + ")" + helpText[cameraPoint];
    }
}
