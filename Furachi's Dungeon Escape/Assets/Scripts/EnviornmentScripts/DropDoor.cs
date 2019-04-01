using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropDoor : MonoBehaviour
{
    // for door types
    public bool orDoor;

    // special doors
    public float specialDropDistance = 0;

    // Open requirements
    public List<GameObject> myActivators;
    private  List<bool> allButtonsReady;

    // Open conditions
    bool doorOpened; // for list analysis
    float dropSpeed = 0.2f;
    float startingposition;
    //bool anyButtonsOn;
    bool movingDown;
    bool movingUp;

    // events
    ButtonPressedEvent buttonPressedEvent;
    ButtonUnPressedEvent buttonUnPressedEvent;

    // helps with door opening/closing
    float doorHight = 5;

    // holds audio object
    AudioSource myAudio;

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
        buttonPressedEvent = new ButtonPressedEvent();
        buttonUnPressedEvent = new ButtonUnPressedEvent();

        // adds self as listener to button pressed event
        EventManager.AddButtonPressedListeners(AreAllMyButtonsPressed);
        EventManager.AddButtonUnPressedListeners(AnyUnPressedButtons);

        // for those using the or door
        //anyButtonsOn = false;

        // gets audio object
        myAudio = gameObject.GetComponent<AudioSource>();

        // gets current y for door movement
        startingposition = transform.localPosition.y;

        // updates door hight for special doors
        doorHight += specialDropDistance;
        dropSpeed += specialDropDistance / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingDown)
        {
            if (transform.localPosition.y >= startingposition - doorHight)
            {
                transform.Translate(0, -dropSpeed, 0);
            }
            else
            {
                movingDown = false;
            }
        }
        if (movingUp)
        {
            if (transform.localPosition.y <= startingposition)
            {
                transform.Translate(0, dropSpeed, 0);
            }
            else
            {
                movingDown = false;
            }
        }
    }

    void OpenDoor()
    {
        myAudio.Play();
        if (movingUp)
        {
            movingUp = false;
        }
        movingDown = true;
    }

    void CloseDoor()
    {
        myAudio.Play();
        if (movingDown)
        {
            movingDown = false;
        }
        movingUp = true;
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
            if (!allButtonsReady[j] && !orDoor)
            {
                break;
            }
            else if (orDoor && allButtonsReady[j])
            {
                if (!doorOpened)
                {
                    OpenDoor();
                    doorOpened = true;
                }
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
                // sets current button as closed
                allButtonsReady[i] = false;

                if (orDoor)
                {
                    //// resets for testing
                    //anyButtonsOn = false;

                    for (int j = 0; j < myActivators.Capacity; j++)
                    {
                        if (allButtonsReady[j])
                        {
                            //anyButtonsOn = true;
                            break;
                        }
                        else if (j == (myActivators.Capacity - 1))
                        {
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
                else
                {
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
}
