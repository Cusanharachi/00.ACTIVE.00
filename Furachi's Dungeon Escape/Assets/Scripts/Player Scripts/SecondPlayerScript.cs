using System.Collections;
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
    private GameObject mySecondState;

    // second state active
    bool twoBeings;
    bool secondExists;

    // events for state changes and assiting stuff
    SecondStateTransitionEvent secondStateTransitionEvent;

    AudioSource myAudio;
    public AudioClip born;
    public AudioClip removed;

    // Start is called before the first frame update
    void Start()
    {
        // makes empty second state
        mySecondState = null;
        twoBeings = false;
        secondExists = false;

        // creates new event
        secondStateTransitionEvent = new SecondStateTransitionEvent();

        myAudio = gameObject.GetComponent<AudioSource>();

        // sets up other events
        EventManager.AddPlayerDeathListeners(Die);
        EventManager.AddSecondStateChangeInvokers(this.gameObject);
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
                // handles audio needs
                myAudio.clip = removed;
                myAudio.Play();

                twoBeings = false;
                Destroy(mySecondState);
                mySecondState = null;

                // handles removal of a state
                secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
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
            Destroy(mySecondState);
            mySecondState = null;
            secondStateTransitionEvent.Invoke(Enumeration.secondStateTransitions.removeState);
        }
    }
}
