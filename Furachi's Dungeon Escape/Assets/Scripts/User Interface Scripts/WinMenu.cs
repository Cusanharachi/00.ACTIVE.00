using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour
{
    // holds the winMenuFor  use 
    public GameObject winMenu;
    public GameObject continueMenu;

    // used to prevent multiple menus
    bool menuLaunched;

    // Start is called before the first frame update
    void Start()
    {
        // sets value to a safe assumption
        menuLaunched = false;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!menuLaunched)
            {
                if (GameManager.Instance.LastScene)
                {
                    menuLaunched = true;

                    // launches a menu and stops time
                    Instantiate(winMenu);
                    Time.timeScale = 0;
                }
                else
                {
                    menuLaunched = true;

                    // launches a menu and stops time
                    Instantiate(continueMenu);
                    Time.timeScale = 0;
                }
            }
        }
    }
}
