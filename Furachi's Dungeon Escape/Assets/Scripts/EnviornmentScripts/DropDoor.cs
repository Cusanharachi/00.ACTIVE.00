using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropDoor : MonoBehaviour
{
    // Open requirements
    public List<GameObject> myActivators;
    private  List<bool> allButtonsReady;

    // Open conditions
    bool doorOpened; // for list analysis
    float dropSpeed;

    // events
    ButtonPressedEvent buttonPressedEvent;
    ButtonUnPressedEvent buttonUnPressedEvent;

    // helps with door opening/closing
    float doorHight = 5;

    // Start is called before the first frame update
    void Start()
    {
        // creates new variables
        allButtonsReady = new List<bool>();
        
        // door mechanic variables
        dropSpeed = 2.0f;
        
        for (int i = 0; i < myActivators.Capacity; i++)
        {
            allButtonsReady.Add(false);
        }

        // creates the event
        buttonPressedEvent = new ButtonPressedEvent();
        buttonUnPressedEvent = new ButtonUnPressedEvent();

        // adds self as listener to button pressed event
        EventManager.AddButtonPressedListeners(AreAllMyButtonsPressed);
        EventManager.AddButtonUnPressedListeners(AnyUnPressedButtons);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenDoor()
    {
        transform.Translate(0, -doorHight, 0, transform);
    }

    void CloseDoor()
    {
        transform.Translate(0, doorHight, 0, transform);
    }

    /// <summary>
    /// uses the passed game object to confirm if the button opens the door
    /// </summary>
    /// <param name="MightBeMyButton"></param>
    private void AreAllMyButtonsPressed(GameObject MightBeMyButton)
    {
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
                if (!doorOpened)
                {
                    OpenDoor();
                    doorOpened = true;
                }
            }
            else
            {
                // does nothing, but here in case of need
            }
        }
    }

    /// <summary>
    /// uses the passed game object to confirm if the button closes the door
    /// </summary>
    /// <param name="MightBeMyButton"></param>
    private void AnyUnPressedButtons(GameObject MightBeMyButton)
    {
        for (int i = 0; i < myActivators.Capacity; i++)
        {
            if (myActivators[i] == MightBeMyButton)
            {
                // sets current button as unlocked 
                allButtonsReady[i] = false;

                // checks if door was opened and closes it if so
                // also marks door as closed
                if (doorOpened)
                {
                    CloseDoor();
                    doorOpened = false;
                }
            }
        }
    }
}
