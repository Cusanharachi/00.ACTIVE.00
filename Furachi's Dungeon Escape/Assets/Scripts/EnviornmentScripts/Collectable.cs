using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    /// <summary>
    /// bult like a button but its a one press and destroys itself
    /// </summary>

    // Event variables
    ButtonPressedEvent buttonPressedEvent;

    //bool secondTouchedMe;

    bool playerTouching;
    bool secondTouching;

    // handles extra event calls
    bool pressed;
    bool unPressed;

    // Start is called before the first frame update
    void Start()
    {
        // handles extra problems
        //secondTouchedMe = false;

        // creates new event
        buttonPressedEvent = new ButtonPressedEvent();

        playerTouching = false;
        secondTouching = false;

        // adds button to list of invokers
        EventManager.AddButtonPressedInvokers(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        if (secondTouching || playerTouching)
        {
            ButtonPressed();
        }
    }

    public void ButtonPressed()
    {
        buttonPressedEvent.Invoke(gameObject);
        Destroy(gameObject);
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
    /// this method in this class is intended to make sure the cube works if anyone is touching
    /// </summary>
    /// <param name="other"> collision object </param>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SecondState" && !secondTouching)
        {
            secondTouching = true;
        }
        else if (other.gameObject.tag == "Player" && !playerTouching)
        {
            playerTouching = true;
        }
    }
}
