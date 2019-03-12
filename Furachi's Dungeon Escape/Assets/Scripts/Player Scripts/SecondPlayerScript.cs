﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SecondPlayerScript : MonoBehaviour
{
    // death menu
    public GameObject deathMenu;

    // secondstate
    [SerializeField]
    public GameObject secondState;

    // second state active
    bool twoBeings;
    bool secondExists;

    // events for state changes and assiting stuff
    Enumeration enumeration = new Enumeration();
    SecondStateTransitionEvent secondStateTransitionEvent;

    AudioSource myAudio;
    public AudioClip born;
    public AudioClip removed;

    // Start is called before the first frame update
    void Start()
    {
        twoBeings = false;
        secondExists = false;

        // creates new event
        secondStateTransitionEvent = new SecondStateTransitionEvent();
        EventManager.AddSecondStateChangeInvokers(this.gameObject);

        // sets up other events
        EventManager.AddPlayerDeathListeners(Die);

        myAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q) && Time.timeScale != 0)
        {
            if (!twoBeings)
            {
                twoBeings = true;

                //Instantiate(secondState, this.transform);
                GameObject.Instantiate(secondState, this.transform.position, this.transform.rotation);

                // handles audio needs
                myAudio.clip = born;
                myAudio.Play();

                secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.addState);
                
            }
            else
            {
                // handles audio needs
                myAudio.clip = removed;
                myAudio.Play();

                twoBeings = false;
                Destroy(GameObject.FindGameObjectWithTag("SecondState"));
                secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && GameObject.FindGameObjectWithTag("SecondState") != null && Time.timeScale != 0)
        {
            secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.switchView);
        }
    }

    public void AddSecondStateChangeListener(UnityAction<Enumeration.secondStateTransitions> listener)
    {
        secondStateTransitionEvent.AddListener(listener);
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
            Instantiate(deathMenu);
        }
        else
        {
            // handles audio needs
            myAudio.clip = removed;
            myAudio.Play();

            twoBeings = false;
            Destroy(GameObject.FindGameObjectWithTag("SecondState"));
            secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
        }
    }
}
