using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformCollectableRequired : MonoBehaviour
{
    /// <summary>
    /// This script is made from pieces of the drop door script
    /// it has similar mechanics but requires its own setup
    /// </summary>
    // Open requirements
    public List<GameObject> myActivators;
    private List<bool> allButtonsReady;

    // special collectable values
    //bool anyButtonsOn;

    // events
    ButtonPressedEvent buttonPressedEvent;

    // Start is called before the first frame update
    void Start()
    {
        // creates new variables
        allButtonsReady = new List<bool>();

        for (int i = 0; i < myActivators.Capacity; i++)
        {
            allButtonsReady.Add(false);
        }

        // creates the event
        //buttonPressedEvent = new ButtonPressedEvent();

        // adds self as listener to button pressed event
        EventManager.AddButtonPressedListeners(AreMyButtonsPressed);

        // for those using the or door
        //anyButtonsOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// uses the passed game object to confirm if the button opens the door
    /// </summary>
    /// <param name="MightBeMyButton"></param>
    private void AreMyButtonsPressed(GameObject MightBeMyButton)
    {
        //Debug.Log("buttondetected");
        for (int i = 0; i < myActivators.Capacity; i++)
        {
            if (myActivators[i] == MightBeMyButton)
            {
                // sets current button as unlocked 
                allButtonsReady[i] = true;
            }
        }

        // checks if ready to open and if so marks it as open
        for (int j = 0; j < myActivators.Capacity; j++)
        {
            if (!allButtonsReady[j])
            {
                break;
            }
            else if (j == (myActivators.Capacity - 1) && allButtonsReady[j])
            {
                gameObject.GetComponent<PlatformScript>().GetMoving = true;
                //Debug.Log("Platforming");
            }
            else
            {
                // does nothing, but here in case of need
            }
        }
    }
}
