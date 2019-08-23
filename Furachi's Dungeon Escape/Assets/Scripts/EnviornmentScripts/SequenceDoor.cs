using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SequenceDoor : MonoBehaviour
{
    // for door types
    public bool orDoor;
    public bool sequenceDoor = false;
    public bool closeable = true;
    bool inSequence;
    int buttonToCheckUpTo;
    bool checkSequence;

    // special doors
    public float specialDropDistance = 0;

    // Open requirements
    public List<GameObject> myActivators;
    private List<bool> allButtonsReady;

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

        // sequence door vaiables
        inSequence = true;
        buttonToCheckUpTo = 0;
        checkSequence = false;

        // gets audio object
        myAudio = gameObject.GetComponent<AudioSource>();

        // gets current y for door movement
        startingposition = transform.localPosition.y;

        // updates door hight for special doors
        doorHight += specialDropDistance;
        //dropSpeed += specialDropDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sequenceDoor)
        {
            if (checkSequence)
            {
                //Debug.Log("sequenceChecl");
                for (int i = 0; i < myActivators.Capacity; i++)
                {
                    if (allButtonsReady[i])
                    {
                        checkSequence = false;
                        break;
                    }
                    else
                    {
                        if (i >= myActivators.Capacity - 1)
                        {
                            checkSequence = false;
                            inSequence = true;
                            buttonToCheckUpTo = 0;
                        }
                    }
                }
            }
        }

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
        if (closeable && !doorOpened)
        {
            myAudio.Play();
            if (movingDown)
            {
                movingDown = false;
            }
            movingUp = true;
        }
    }

    /// <summary>
    /// uses the passed game object to confirm if the button opens the door
    /// </summary>
    /// <param name="MightBeMyButton"></param>
    private void AreAllMyButtonsPressed(GameObject MightBeMyButton)
    {
        for (int i = 0; i < myActivators.Capacity; i++)
        {
            //if (!allButtonsReady[i])
            //{
            //    break;
            //}
            if (myActivators[i] == MightBeMyButton)
            {
                // sets current button as unlocked 
                allButtonsReady[i] = true;

                if (sequenceDoor)
                {
                    if (i == buttonToCheckUpTo && inSequence)
                    {
                        //Debug.Log("is in sequence");
                        buttonToCheckUpTo += 1;
                        break;
                    }
                    else
                    {
                        inSequence = false;
                        //Debug.Log("no sequence");
                        break;
                    }
                }
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
                //Debug.Log("willSequenceDoorOpen");
                if (sequenceDoor)
                {
                    if (!doorOpened && inSequence)
                    {
                        OpenDoor();
                        doorOpened = true;
                    }
                }
                else
                {
                    if (!doorOpened)
                    {
                        OpenDoor();
                        doorOpened = true;
                    }
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

                if (sequenceDoor)
                {
                    checkSequence = true;
                }

                if (orDoor)
                {
                    // resets for testing
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
