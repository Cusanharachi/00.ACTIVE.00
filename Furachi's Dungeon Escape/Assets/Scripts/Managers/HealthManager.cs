using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    // events
    PlayerDeathEvent playerDeathEvent;

    // health variables
    float playerHealth = 300; // THIS SHOULD REMAIN CONSTANT, IT IS THE HEALTH BAR SIZE
    float imagination = 300; // THIS SHOULD ALSO REMAIN CONSTANT FOR THE SAME REASON

    // removalValues
    Vector2 healthScalingVector;
    Vector3 healthpositionVector;
    float healthScalingValue = 300;
    Vector2 imaginationScalingVector;
    Vector3 imaginationpositionVector;
    float imaginationScalingValue = 300;


    // UI objects that represent health
    public GameObject healthPanel;
    RectTransform healthPanelRect;
    public GameObject imaginationPanel;
    RectTransform imaginationPanelRect;

    // properties

    // testing code
    public float Imagination
    {
        get
        {
            return imaginationScalingValue;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // adds player death event
        playerDeathEvent = new PlayerDeathEvent();

        // gets panels for minipulation
        healthPanelRect = healthPanel.GetComponent<RectTransform>();
        imaginationPanelRect = imaginationPanel.GetComponent<RectTransform>();

        // asistant 
        healthScalingValue = healthPanelRect.sizeDelta.x;
        playerHealth = healthPanelRect.sizeDelta.x;
        imaginationScalingValue = imaginationPanelRect.sizeDelta.x;
        imagination = imaginationPanelRect.sizeDelta.x;

        // safely sets scaling vector
        healthScalingVector = new Vector2(healthScalingValue, 1);
        healthpositionVector = new Vector3(healthPanelRect.localPosition.x, healthPanelRect.localPosition.y, healthPanelRect.localPosition.z);
        imaginationScalingVector = new Vector2(imaginationScalingValue, 1);
        imaginationpositionVector = new Vector3(imaginationPanelRect.localPosition.x, imaginationPanelRect.localPosition.y, imaginationPanelRect.localPosition.z);

        // handles event manager needs
        EventManager.AddHealthChangedListeners(CheckHealthChange);
        EventManager.AddPlayerDeathInvokers(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (imaginationScalingValue != imagination)
        {
            if (GameObject.FindGameObjectWithTag("SecondState") == null && GameObject.FindGameObjectWithTag("SpecialCube") == null)
            {
                ChangeHealth(imagination);
            }
        }
    }

    /// <summary>
    /// checks which state needs to be changes
    /// </summary>
    /// <param name="stateToChange"> indicated which state to change</param>
    /// <param name="changeValue"> indicates how much should be changed</param>
    void CheckHealthChange(Enumeration.playerState stateToChange, float changeValue)
    {
        if (stateToChange == Enumeration.playerState.playerState)
        {
            //Debug.Log("Changing: player");
            ChangeHealth(changeValue);
        }
        else
        {
            //Debug.Log("Changing: Second");
            ChangeImagination(changeValue);
        }
    }

    /// <summary>
    /// This method changes the players health value
    /// </summary>
    /// <param name="value"> value to change health by</param>
    void ChangeHealth(float value)
    {
        //testing code
        if (healthScalingValue - value >= playerHealth)
        {
            //Debug.Log("player health restored");
            value = 0;
            healthScalingValue = playerHealth;
        }
        else if (healthScalingValue - value <= 0)
        {
            //Debug.Log("PlayerHealth Reduced");
            value = healthScalingValue;

            // finished changing values so player can see their empty health after death.
            healthScalingValue -= value;
            //Debug.Log("Changing: health" + healthScalingValue);
            healthScalingVector.x = healthScalingValue / playerHealth;
            healthPanelRect.transform.localScale = healthScalingVector;
            healthpositionVector.x = healthScalingValue / 2 - (playerHealth + 100);
            healthPanelRect.transform.localPosition = healthpositionVector;

            // displays death screen
            playerDeathEvent.Invoke(Enumeration.playerState.playerState);

            // stops time since the player is dead
            Time.timeScale = 0;
        }

        //Debug.Log("healthscaling value: " + healthScalingValue);
        healthScalingValue -= value;
        //Debug.Log("healthscalingavalue: " + healthScalingValue);
        //Debug.Log("value: " + value);
        //Debug.Log("Changing: health" + healthScalingValue);
        healthScalingVector.x = healthScalingValue / playerHealth;
        healthPanelRect.transform.localScale = healthScalingVector;
        healthpositionVector.x = healthScalingValue / 2 - (playerHealth + 100);
        healthPanelRect.transform.localPosition = healthpositionVector;
    }

    /// <summary>
    /// This method changes the imagination value
    /// </summary>
    /// <param name="value"> value to imagination by</param>
    void ChangeImagination(float value)
    {
        //testing code
        if (imaginationScalingValue - value >= imagination)
        {
            value = 0;
            imaginationScalingValue = imagination;

        }
        else if (imaginationScalingValue - value <= 0)
        {
            value = 0;
            playerDeathEvent.Invoke(Enumeration.playerState.secondState);
            imaginationScalingValue = imagination;
        }
        imaginationScalingValue -= value;
        //Debug.Log("Changing: imagination" + imaginationScalingValue);
        imaginationScalingVector.x = imaginationScalingValue / imagination;
        imaginationPanelRect.transform.localScale = imaginationScalingVector;
        imaginationpositionVector.x = imaginationScalingValue / 2 - (imagination + 100);
        imaginationPanelRect.transform.localPosition = imaginationpositionVector;
    }

    /// <summary>
    /// adds listener to the invoker invoked by this script
    /// </summary>
    /// <param name="listener"> listener for the event </param>
    public void AddPlayerDeathListener(UnityAction<Enumeration.playerState> listener)
    {
        playerDeathEvent.AddListener(listener);
    }
}
