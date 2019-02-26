using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicButton : MonoBehaviour
{
    // switches button type
    [SerializeField]
    public bool isHoldButton;
    public bool isReverseButton;

    // Event variables
    ButtonPressedEvent buttonPressedEvent;
    ButtonUnPressedEvent buttonUnPressedEvent;

    bool secondTouchedMe;

    // for music
    AudioSource myAudio;
    bool PlayedAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
        // handles extra problems
        secondTouchedMe = false;

        // creates new event
        buttonPressedEvent = new ButtonPressedEvent();

        // adds button to list of invokers
        EventManager.AddButtonPressedInvokers(gameObject);

        // creates new event
        buttonUnPressedEvent = new ButtonUnPressedEvent();

        // adds button to list of invokers
        EventManager.AddButtonUnPressedInvokers(gameObject);

        // tells door it is an open button to start if so
        if (isReverseButton)
        {
            ButtonPressed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (secondTouchedMe && isHoldButton)
        {
            if (GameObject.FindGameObjectWithTag("SecondState") == null)
            {
                ButtonUnPressed();
                secondTouchedMe = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (!isReverseButton)
        {
            if (other.gameObject.tag == "Cube01")
            {
                ButtonPressed();
            }
            else if (other.gameObject.tag == "Player")
            {
                ButtonPressed();
                Debug.Log("player opening");
            }
            else if (other.gameObject.tag == "SecondState")
            {
                ButtonPressed();
                secondTouchedMe = true;
            }
        }
        else
        {
            if (other.gameObject.tag == "Cube01")
            {
                ButtonUnPressed();
            }
            else if (other.gameObject.tag == "Player")
            {
                ButtonUnPressed();
            }
            else if (other.gameObject.tag == "SecondState")
            {
                ButtonUnPressed();
                secondTouchedMe = true;
            }
        }

    }

    public void ButtonPressed()
    {
        buttonPressedEvent.Invoke(gameObject);
        myAudio.Play();
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
        if (!isReverseButton)
        {
            if (other.gameObject.tag == "Cube01" && isHoldButton)
            {
                ButtonUnPressed();
            }
            else if (other.gameObject.tag == "Player" && isHoldButton)
            {
                ButtonUnPressed();
            }
            else if (other.gameObject.tag == "SecondState" && isHoldButton)
            {
                ButtonUnPressed();
            }
        }
        else
        {
            if (other.gameObject.tag == "Cube01" && isHoldButton)
            {
                ButtonPressed();
            }
            else if (other.gameObject.tag == "Player" && isHoldButton)
            {
                ButtonPressed();
            }
            else if (other.gameObject.tag == "SecondState" && isHoldButton)
            {
                ButtonPressed();
            }
        }
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
        buttonUnPressedEvent.Invoke(this.gameObject);
        myAudio.Play();
    }
}
