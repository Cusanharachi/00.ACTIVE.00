using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SecondStuckField : MonoBehaviour
{
    // events system variables
    SecondStuckEvent secondStuckEvent;

    // Start is called before the first frame update
    void Start()
    {
        // creates the event
        secondStuckEvent = new SecondStuckEvent();

        // handles event manager needs
        EventManager.AddSecondStateStuckInvokers(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// checks the collision to let people know the second state is touching it
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SecondState")
        {
            secondStuckEvent.Invoke(1);
        }
    }

    /// <summary>
    /// checks the collisionto let people know the second state isn't touching anymore
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SecondState")
        {
            secondStuckEvent.Invoke(-1);
        }
    }

    /// <summary>
    /// adds the listener to the invoker invoked by this script
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddSecondStateStuckListener(UnityAction<int> listener)
    {
        secondStuckEvent.AddListener(listener);
    }
}
