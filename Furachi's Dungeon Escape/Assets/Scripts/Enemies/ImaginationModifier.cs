using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImaginationModifier : MonoBehaviour
{
    // events system variables
    HealthChangedEvent healthChangedEvent;

    // speacial cube values
    public bool specialCube = false;
    float specialCubeCost = 20;
    float specialCubeDistanceCost = 2;
    float accumulatedPieceCost;

    // special damage value
    //public float specialDamageValue = 1;

    // distance that effects the second state imagination (depending on player distance
    public float imaginationDamagerPerMeter = 10f;
    public float imaginationSafeDistance = 5;

    // holds variables for distanc                                                                                          e math
    float distanceToPlayer;
    float determinedDistance;
    Transform playerTransform;

    // holds variables for time math
    float imaginationDamagerPerMS = 1f;

    // used to restore values when new second state appears 
    // MAY BE TEMPORARY
    //float restoreValue = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("HealthBars").GetComponent<HealthManager>().Imagination >= 0)
        {
            if (!specialCube)
            {
                // sets up events
                healthChangedEvent = new HealthChangedEvent();
                EventManager.AddImaginationChangedInvokers(gameObject);

                // initializes variabels
                distanceToPlayer = 0;
                determinedDistance = 0;

                // sets up accumulated cost
                accumulatedPieceCost = 0;

                // gets player transform for math later
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                // sets up events
                healthChangedEvent = new HealthChangedEvent();
                EventManager.AddImaginationChangedInvokers(gameObject);

                // initializes variabels
                distanceToPlayer = 0;
                determinedDistance = 0;

                // gets player transform for math later
                playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                // changes regular cost for special cube cost
                imaginationDamagerPerMeter *= specialCubeDistanceCost;

                // restores value so that it is a fresh imagination
                healthChangedEvent.Invoke(Enumeration.playerState.secondState, specialCubeCost);
                EventManager.AddPlayerDeathListeners(DestroySpecialCube);

                accumulatedPieceCost = 0;
                accumulatedPieceCost -= specialCubeCost;
            }
        }
        else
        {
            //Debug.Log("dissapeard");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        determinedDistance = Vector3.Distance(playerTransform.position, transform.position) - imaginationSafeDistance;

        // determines of the distance is acceptable for damage
        if (determinedDistance <= 0 && !specialCube)
        {
            // keeps track of the distance
            //if (determinedDistance != distanceToPlayer)
            //{
            //    distanceToPlayer = determinedDistance;
            //}

            // increases imagination over time
            healthChangedEvent.Invoke(Enumeration.playerState.secondState, - Time.deltaTime * imaginationDamagerPerMS);

            //if (accumulatedPieceCost < 0)
            //{
            accumulatedPieceCost += Time.deltaTime * imaginationDamagerPerMS;
            //}
        }
        else
        {
            // changes the imagination based on the distance to the player
            if (determinedDistance != distanceToPlayer)
            {
                accumulatedPieceCost -= (imaginationDamagerPerMeter * ((determinedDistance) - (distanceToPlayer)));
                healthChangedEvent.Invoke(Enumeration.playerState.secondState, (imaginationDamagerPerMeter * ((determinedDistance) - (distanceToPlayer))));
                distanceToPlayer = determinedDistance;
            }

            healthChangedEvent.Invoke(Enumeration.playerState.secondState, Time.deltaTime * imaginationDamagerPerMS);
            accumulatedPieceCost -= Time.deltaTime * imaginationDamagerPerMS;
        }
    }

    /// <summary>
    /// adds listener to the invoker invoked by this script
    /// this listener should only care about the change in imagination anyways
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddHealthChangedListener(UnityAction<Enumeration.playerState, float> listener)
    {
        healthChangedEvent.AddListener(listener);
    }

    public void DestroySpecialCube(Enumeration.playerState state)
    {
        if (state == Enumeration.playerState.secondState)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// returns stolen value to system if puzzle piece
    /// </summary>
    private void OnDestroy()
    {
        healthChangedEvent.Invoke(Enumeration.playerState.secondState, (accumulatedPieceCost));
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (damagePlayer && other.gameObject.tag == "Player")
    //    {
    //        //Debug.Log("touching player");
    //        healthChangedEvent.Invoke(Enumeration.playerState.playerState, specialDamageValue * 10f * Time.deltaTime);
    //    }
    //    else if (!damagePlayer && other.gameObject.tag == "SecondState")
    //    {
    //        //Debug.Log("touching second");
    //        healthChangedEvent.Invoke(Enumeration.playerState.secondState, specialDamageValue * 10f * Time.deltaTime);
    //    }
    //}
}
