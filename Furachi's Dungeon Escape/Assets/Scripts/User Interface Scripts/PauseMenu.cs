using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // handles open or closed and holds pause menu
    GameObject myPauseMenu;
    bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        // pause menu can't have been opened yet
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
            {
                // opens pause menu
                myPauseMenu = Instantiate(pauseMenu);
                isOpen = true;

                // stops time update
                Time.timeScale = 0;
            }
            else
            {
                // destroys pause menu
                Destroy(myPauseMenu);
                isOpen = false;

                // starts up time again
                Time.timeScale = 1;
            }
        }
    }
}
