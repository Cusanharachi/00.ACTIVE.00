using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicButton : MonoBehaviour
{
    // switches button type
    [SerializeField]
    public bool isHoldButton;
    //public bool isReverseButton;
    public bool isSpecialButton = false;

    // Event variables
    ButtonPressedEvent buttonPressedEvent;
    ButtonUnPressedEvent buttonUnPressedEvent;

    // for music
    AudioSource myAudio;
    bool PlayedAudio = false;

    // handles button collisions
    bool cubeTouching;
    bool playerTouching;
    bool secondTouching;
    //bool specialTouching;

    // handles extra event calls
    bool pressed;
    bool unPressed;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
        // handles extra problems

        // creates new event
        buttonPressedEvent = new ButtonPressedEvent();

        // creates new event
        buttonUnPressedEvent = new ButtonUnPressedEvent();

        // makes new list
        cubeTouching = false;
        playerTouching = false;
        secondTouching = false;

        // makes sure we aren't spamming
        pressed = false;
        unPressed = true;


        // tells door it is an open button to start if so
        //if (isReverseButton)
        //{
        //    ButtonPressed();
        //}

        // adds button to list of invokers
        EventManager.AddButtonUnPressedInvokers(gameObject);

        // adds button to list of invokers
        EventManager.AddButtonPressedInvokers(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSpecialButton)
        {
            if (isHoldButton && secondTouching)
            {
                if (GameObject.FindGameObjectWithTag("SecondState") == null)
                {
                    secondTouching = false;
                }
            }
        }
        else
        {
            if (isHoldButton && cubeTouching)
            {
                if (GameObject.FindGameObjectWithTag("SpecialCube") == null)
                {
                    cubeTouching = false;
                }
            }
        }

        if (secondTouching || playerTouching || cubeTouching)
        {
            if (unPressed)
            {
                unPressed = false;
                pressed = true;
                ButtonPressed();
            }
        }
        else if (!PlayedAudio)
        {
            if (pressed)
            {
                unPressed = true;
                pressed = false;
                ButtonUnPressed();
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("this works");
    //    if (collision.gameObject.tag == "Cube01")
    //    {
    //        Debug.Log("I was Pressed");
    //        ButtonPressed();
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!isReverseButton)
    //    {
    //        if (other.gameObject.tag == "Cube01")
    //        {
    //            ButtonPressed();
    //        }
    //        else if (other.gameObject.tag == "Player")
    //        {
    //            ButtonPressed();
    //            Debug.Log("player opening");
    //        }
    //        else if (other.gameObject.tag == "SecondState")
    //        {
    //            ButtonPressed();
    //            secondTouchedMe = true;
    //        }
    //    }
    //    else
    //    {
    //        if (other.gameObject.tag == "Cube01")
    //        {
    //            ButtonUnPressed();
    //        }
    //        else if (other.gameObject.tag == "Player")
    //        {
    //            ButtonUnPressed();
    //        }
    //        else if (other.gameObject.tag == "SecondState")
    //        {
    //            ButtonUnPressed();
    //            secondTouchedMe = true;
    //        }
    //    }

    //}

    /// <summary>
    /// this method in this class is intended to make sure the cube works if anyone is touching
    /// </summary>
    /// <param name="other"> collision object </param>
    private void OnTriggerStay(Collider other)
    {
        if (!isSpecialButton)
        {
            if (other.gameObject.tag == "Cube01" & !cubeTouching)
            {
                cubeTouching = true;
            }
            if (other.gameObject.tag == "SecondState" && !secondTouching)
            {
                secondTouching = true;
            }
            if (other.gameObject.tag == "Player" && !playerTouching)
            {
                playerTouching = true;
            }
        }
        else
        {
            if (!cubeTouching && other.gameObject.tag == "SpecialCube")
            {
                cubeTouching = true;
            }
        }
    }

    public void ButtonPressed()
    {
        myAudio.Play();
        buttonPressedEvent.Invoke(gameObject);
    }

    /// <summary>
    /// adds listener to the invoker invoked by this script
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddButtonPressedListener(UnityAction<GameObject> listener)
    {
        buttonPressedEvent.AddListener(listener);
    }

    /// <summary>
    /// adds listener to the invoker invoked by this script
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddButtonUnPressedListener(UnityAction<GameObject> listener)
    {
        buttonUnPressedEvent.AddListener(listener);
    }

    private void OnTriggerExit(Collider other)
    {
        //if (!isReverseButton)
        //{
        if (!isSpecialButton)
        {
            if (other.gameObject.tag == "Cube01" && isHoldButton)
            {
                cubeTouching = false;
            }
            if (other.gameObject.tag == "Player" && isHoldButton)
            {
                playerTouching = false;
            }
            if (other.gameObject.tag == "SecondState" && isHoldButton)
            {
                secondTouching = false;
            }
        }
        else
        {
            if (isHoldButton && other.gameObject.tag == "SpecialCube")
            {
                cubeTouching = false;
            }
        }
        //}
        //else
        //{
        //    if (other.gameObject.tag == "Cube01" && isHoldButton)
        //    {
        //        ButtonPressed();
        //    }
        //    else if (other.gameObject.tag == "Player" && isHoldButton)
        //    {
        //        ButtonPressed();
        //    }
        //    else if (other.gameObject.tag == "SecondState" && isHoldButton)
        //    {
        //        ButtonPressed();
        //    }
        //}
    }

    //void OnCollisionStay(Collision collisionInfo)
    //{
    //    // Debug-draw all contact points and normals
    //    foreach (ContactPoint contact in collisionInfo.contacts)
    //    {
    //        if (contact.otherCollider.gameObject.tag == "SecondState")
    //        {

    //        }
    //    }
    //}

    public void ButtonUnPressed()
    {
        myAudio.Play();
        buttonUnPressedEvent.Invoke(this.gameObject);
    }
}
