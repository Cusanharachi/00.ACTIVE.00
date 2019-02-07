using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicButton : MonoBehaviour
{
    // switches button type
    [SerializeField]
    public bool isHoldButton;

    // Event variables
    ButtonPressedEvent buttonPressedEvent;
    ButtonUnPressedEvent buttonUnPressedEvent;

    bool secondTouchedMe;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("gone");
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
        }

    }

    public void ButtonPressed()
    {
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
        if (other.gameObject.tag == "Cube01" && isHoldButton)
        {
            ButtonUnPressed();
        }
        else if (other.gameObject.tag == "Player" && isHoldButton)
        {
            ButtonUnPressed();
            Debug.Log("Player gone");
        }
        else if (other.gameObject.tag == "SecondState" && isHoldButton)
        {
            ButtonUnPressed();
            secondTouchedMe = true;
            Debug.Log("second state gone");
        }
    }

    public void ButtonUnPressed()
    {
        buttonUnPressedEvent.Invoke(this.gameObject);
    }
}
