﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SecondPlayerScript : MonoBehaviour
{
    // death menu
    public GameObject deathMenu;
    GameObject myDeathMenu;

    // secondstate
    [SerializeField]
    public GameObject secondState;
    private GameObject mySecondState;

    // second state active
    bool twoBeings;
    //bool secondExists;

    // events for state changes and assiting stuff
    SecondStateTransitionEvent secondStateTransitionEvent;
    HealthChangedEvent healthChangedEvent;

    AudioSource myAudio;
    public AudioClip born;
    public AudioClip removed;

    // bools to prevent the second state form being removed
    bool SecondStuck;
    int StuckValue;

    Vector3 spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        // makes empty second state
        mySecondState = null;
        twoBeings = false;
        //secondExists = false;

        // value to change the number of affectors for the second state
        StuckValue = 0;
        SecondStuck = false;

        // creates new event
        secondStateTransitionEvent = new SecondStateTransitionEvent();
        healthChangedEvent = new HealthChangedEvent();

        myAudio = gameObject.GetComponent<AudioSource>();

        // sets up other events
        EventManager.AddPlayerDeathListeners(Die);
        EventManager.AddSecondStateChangeInvokers(this.gameObject);
        EventManager.AddSecondStateStuckListeners(ChangeStuckValue);
        EventManager.AddHealthChangedInvokers(gameObject);

        // sets spawn location to current location, since this will most likely be where the 
        // initial spawn lcoation is for the player
        spawnLocation = transform.position;

        myDeathMenu = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && Time.timeScale != 0)
        {
            if (!twoBeings)
            {
                // handles two beings at once code
                twoBeings = true;

                //Instantiate(secondState, this.transform);
                mySecondState = GameObject.Instantiate(secondState, this.transform.position, this.transform.rotation);

                // handles audio needs
                myAudio.clip = born;
                myAudio.Play();

                secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.addState);
            }
            else
            {
                if (!SecondStuck)
                {
                    // handles audio needs
                    myAudio.clip = removed;
                    myAudio.Play();

                    twoBeings = false;
                    Destroy(mySecondState);
                    mySecondState = null;

                    // resets stuck value
                    StuckValue = 0;

                    // handles removal of a state
                    secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && mySecondState != null && Time.timeScale != 0)
        {
            secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.switchView);
        }
    }

    public void AddSecondStateChangeListener(UnityAction<Enumeration.secondStateTransitions> listener)
    {
        secondStateTransitionEvent.AddListener(listener);
    }

    public void AddHealthChangedListener(UnityAction<Enumeration.playerState, float> listener)
    {
        healthChangedEvent.AddListener(listener);
    }

    /// <summary>
    /// used for asserting the spawn location
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            if (other.transform.position != spawnLocation && healthChangedEvent != null)
            {
                spawnLocation = other.transform.position;
                healthChangedEvent.Invoke(Enumeration.playerState.playerState, -400);
            }
        }
    }

    /// <summary>
    /// kills the player or second state if their death is triggered
    /// by the event system
    /// </summary>
    /// <param name="whoDies"></param>
    void Die(Enumeration.playerState whoDies)
    {
        if (whoDies == Enumeration.playerState.playerState)
        {
            if (myDeathMenu == null)
            {
                myDeathMenu = Instantiate(deathMenu);
            }
            transform.position = spawnLocation;
            healthChangedEvent.Invoke(Enumeration.playerState.playerState, -400);
        }
        else
        {
            // handles audio needs
            myAudio.clip = removed;
            myAudio.Play();

            twoBeings = false;
            Destroy(mySecondState);
            mySecondState = null;
            secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
        }
    }

    /// <summary>
    /// the method that changes its stuck value to ensure
    /// that there is soemthing (or isnt) something touching
    /// the second state preventing their changing.
    /// </summary>
    /// <param name="valueToChange"></param>
    void ChangeStuckValue(int valueToChange)
    {
        // adds the value
        StuckValue += valueToChange;

        //Debug.Log(StuckValue);

        // if there aren't any in contanct then we aren't stuck
        if (StuckValue > 0)
        {
            SecondStuck = true;
        }
        else
        {
            SecondStuck = false;
        }

        //Debug.Log(SecondStuck);
    }
}
