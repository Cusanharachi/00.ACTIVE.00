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

    // Start is called before the first frame update
    void Start()
    {
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
        if (other.gameObject.tag == "Cube01")
        {
            ButtonPressed();
        }
        else if (other.gameObject.tag == "Player")
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
        }
    }

    public void ButtonUnPressed()
    {
        buttonUnPressedEvent.Invoke(this.gameObject);
    }
}
