using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // handles open or closed and holds pause menu
    GameObject myPauseMenu;
    bool isOpen;
    bool openable;

    // Start is called before the first frame update
    void Start()
    {
        // pause menu can't have been opened yet
        isOpen = false;

        // pause menu is restricted by something
        openable = true;

        EventManager.AddPlayerDeathListeners(PlayerDied);
    }

    // Update is called once per frame
    void Update()
    {
        if (openable && Input.GetKeyDown(KeyCode.Escape))
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
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //if (Input.GetKeyDown(KeyCode.LeftControl))
            //{
                EventManager.ClearManager();
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(GameManager.Instance.NextScene, LoadSceneMode.Single);
            //}
        }
    }

    /// <summary>
    /// changes variables if player dies
    /// </summary>
    /// <param name="stateDeath"> this is the state that died on the event call</param>
    void PlayerDied(Enumeration.playerState stateDeath)
    {
        if (stateDeath == Enumeration.playerState.playerState)
        {
            openable = false;
        }
    }
}
