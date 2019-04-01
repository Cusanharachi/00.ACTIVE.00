using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthModifer : MonoBehaviour
{
    // events system variables
    HealthChangedEvent healthChangedEvent;

    // decides who gets the damage
    public bool damagePlayer = true;
    public bool damageBoth = false;

    // special damage value
    public float specialDamageValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        healthChangedEvent = new HealthChangedEvent();
        EventManager.AddHealthChangedInvokers(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// adds listener to the invoker invoked by this script
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddHealthChangedListener(UnityAction<Enumeration.playerState, float> listener)
    {
        healthChangedEvent.AddListener(listener);
    }

    private void OnTriggerStay(Collider other)

    {
        if ((damagePlayer || damageBoth) && other.gameObject.tag == "Player")
        {
            //Debug.Log("touching player");
            healthChangedEvent.Invoke(Enumeration.playerState.playerState,  specialDamageValue * 10f * Time.deltaTime);
        }
        else if ((!damagePlayer || damageBoth) && other.gameObject.tag == "SecondState")
        {
            //Debug.Log("touching second");
            healthChangedEvent.Invoke(Enumeration.playerState.secondState, specialDamageValue * 10f * Time.deltaTime);
        }
    }
}
