using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    #region SingleVersion
    // allows access to game manager
    public static EventManager Instance { get; private set; }

    // makes constructor private
    private EventManager() { }

    private void Awake()
    {
        // if there are no other game managers , allows creation
        // otherwise destroys self if not needed
        if (Instance == null)
        {
            Instance = new EventManager();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
    // vocabulary
    #region VocabularyAssistance
    /*doors*/
    // references to "Doors" can refer to more than just doors,
    // a door can be a bridge or an additional puzzle pieces.

    #endregion

    // all invokers are contained in these lists
    #region InvokerLists
    // lists of invokers
    /*format*/ // static List<T> [NAME] = new List<T>();

    // objects that invoke the button pressed method
    static List<GameObject> buttonPressedInvokers = new List<GameObject>();

    // objects that invoke the button unpressed method
    static List<GameObject> buttonUnPressedInvokers = new List<GameObject>();

    // objects that inform the state change of the second state
    static List<GameObject> secondStateChangeInvokers = new List<GameObject>();

    // objects that change the health or imagination of the player
    static List<GameObject> healthChangedInvokers = new List<GameObject>();

    // objects that only change imagination of the second state
    // NOTE: these are given the helath changed listeners but have their own invoker method
    static List<GameObject> imaginationChangedInvokers = new List<GameObject>();

    // objects that announce a players death appear here
    // NOTE: imagination deaths come from these invokers (hopefuly just the health bar)
    static List<GameObject> playerDeathInvokers = new List<GameObject>();

    // objects that invoke the Second State Stuck event
    static List<GameObject> secondStateStuckInokers = new List<GameObject>();
    #endregion

    // 
    #region ListenerLists
    // lists of listeners
    /*format*/ // static List<UnityAction?<>?> [NAME] = new List<T>();

    // objects that Listen for the button pressed event
    static List<UnityAction<GameObject>> buttonPressedListeners = new List<UnityAction<GameObject>>();

    // objects that listen for the button unpressed event
    static List<UnityAction<GameObject>> buttonUnPressedListeners = new List<UnityAction<GameObject>>();

    // objects inquiring about the second state change
    static List<UnityAction<Enumeration.secondStateTransitions>> secondStateChangeListeners = new List<UnityAction<Enumeration.secondStateTransitions>>();

    // objects listening for health or imagination change events
    static List<UnityAction<Enumeration.playerState, float>> healthChangedListeners = new List<UnityAction<Enumeration.playerState, float>>();

    // objects listening for player death events
    static List<UnityAction<Enumeration.playerState>> playerDeathListeners = new List<UnityAction<Enumeration.playerState>>();

    // objects listening for second state stuck events
    static List<UnityAction<int>> secondStateStuckListeners = new List<UnityAction<int>>();

    #endregion

    // all invoker and listener pairing happens here
    #region InvokerAndListenerPairing
    // each event typically has a listener and invoker list/method
    // formating obviously switch invoker/listern depending
    /*
    \\\ <summary>
    \\\ Describe event pairing (pairs this to this)
    \\\ </summary>
    public static void [NAME](T [NAME])
    {
        // adds invoker to list and all listeners to invoker
        {properInvokerList}.ADD([NAME]);
        foreach (UnityAction listener in {properListenerList})
            [NAME].{MethodForAdding}
    }*/

    /// <summary>
    /// Pairs nutton pressed invokers to button pressed listeners
    /// </summary>
    public static void AddButtonPressedListeners(UnityAction<GameObject> listener)
    {
        // adds listener to list and to all invokers
        buttonPressedListeners.Add(listener);
        foreach (GameObject invoker in buttonPressedInvokers)
        {
            invoker.GetComponent<BasicButton>().AddButtonPressedListener(listener);
            //invoker.GetComponent<HoldButton>().AddButtonPressedListener(listener);
        }
    }

    /// <sumary>
    /// pairs button pressed listeners to button pressed invokers
    /// </sumary>
    public static void AddButtonPressedInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        buttonPressedInvokers.Add(invoker);
        foreach (UnityAction<GameObject> listener in buttonPressedListeners)
        {
            invoker.GetComponent<BasicButton>().AddButtonPressedListener(listener);
            //invoker.GetComponent<HoldButton>().AddButtonPressedListener(listener);
        }
    }

    /// <summary>
    /// Pairs nutton pressed invokers to button pressed listeners
    /// </summary>
    public static void AddButtonUnPressedListeners(UnityAction<GameObject> listener)
    {
        // adds listener to list and to all invokers
        buttonUnPressedListeners.Add(listener);
        foreach (GameObject invoker in buttonUnPressedInvokers)
        {
            invoker.GetComponent<BasicButton>().AddButtonUnPressedListener(listener);
        }
    }

    /// <sumary>
    /// pairs button pressed listeners to button pressed invokers
    /// </sumary>
    public static void AddButtonUnPressedInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        buttonUnPressedInvokers.Add(invoker);
        foreach (UnityAction<GameObject> listener in buttonUnPressedListeners)
        {
            invoker.GetComponent<BasicButton>().AddButtonUnPressedListener(listener);
        }
    }

    /// <summary>
    /// Pairs second state change events to all listeners
    /// </summary>
    public static void AddSecondStateChangeListeners(UnityAction<Enumeration.secondStateTransitions> listener)
    {
        // adds listener to list and to all invokers
        secondStateChangeListeners.Add(listener);
        foreach (GameObject invoker in secondStateChangeInvokers)
        {
            invoker.GetComponent<SecondPlayerScript>().AddSecondStateChangeListener(listener);
        }
    }

    /// <sumary>
    /// pairs all second state invokers to every listener
    /// </sumary>
    public static void AddSecondStateChangeInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        secondStateChangeInvokers.Add(invoker);
        foreach (UnityAction<Enumeration.secondStateTransitions> listener in secondStateChangeListeners)
        {
            invoker.GetComponent<SecondPlayerScript>().AddSecondStateChangeListener(listener);
        }
    }

    /// <summary>
    /// pairs all health/imagination change listeners to invokers
    /// </summary>
    public static void AddHealthChangedListeners(UnityAction<Enumeration.playerState, float> listener)
    {
        // adds listener to list and all invokers
        healthChangedListeners.Add(listener);
        foreach (GameObject invoker in healthChangedInvokers)
        {
            invoker.GetComponent<HealthModifer>().AddHealthChangedListener(listener);
        }

        // adds listeners to all imagination changed invokers
        foreach (GameObject invoker in imaginationChangedInvokers)
        {
            invoker.GetComponent<ImaginationModifier>().AddHealthChangedListener(listener);
        }
    }

    /// <sumary>
    /// pairs all health changed invokers to every listener
    /// </sumary>
    public static void AddHealthChangedInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        healthChangedInvokers.Add(invoker);
        foreach (UnityAction<Enumeration.playerState, float> listener in healthChangedListeners)
        {
            invoker.GetComponent<HealthModifer>().AddHealthChangedListener(listener);
        }

        // adds the imagination change invokers to the list of all listeners
        foreach (UnityAction<Enumeration.playerState, float> listener in healthChangedListeners)
        {
            invoker.GetComponent<ImaginationModifier>().AddHealthChangedListener(listener);
        }
    }

    /// <sumary>
    /// pairs all imagination changed invokers to every listener for healt/imagination change events
    /// </sumary>
    public static void AddImaginationChangedInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        imaginationChangedInvokers.Add(invoker);

        // adds the imagination change invokers to the list of all listeners
        foreach (UnityAction<Enumeration.playerState, float> listener in healthChangedListeners)
        {
            invoker.GetComponent<ImaginationModifier>().AddHealthChangedListener(listener);
        }
    }

    /// <summary>
    /// pairs all playerDeath listeners to invokers
    /// </summary>
    public static void AddPlayerDeathListeners(UnityAction<Enumeration.playerState> listener)
    {
        // adds listener to list and all invokers
        playerDeathListeners.Add(listener);
        foreach (GameObject invoker in playerDeathInvokers)
        {
            HealthManager hm = invoker.GetComponent<HealthManager>();
            invoker.GetComponent<HealthManager>().AddPlayerDeathListener(listener);
        }
    }

    /// <sumary>
    /// pairs all player death invokers to every listener
    /// </sumary>
    public static void AddPlayerDeathInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        playerDeathInvokers.Add(invoker);
        foreach (UnityAction<Enumeration.playerState> listener in playerDeathListeners)
        {
            invoker.GetComponent<HealthManager>().AddPlayerDeathListener(listener);
        }
    }

    /// <summary>
    /// pairs all second stuck listeners to invokers
    /// </summary>
    /// <param name="listener"></param>
    public static void AddSecondStateStuckListeners(UnityAction<int> listener)
    {
        // adds listener to list and to all Invokers
        secondStateStuckListeners.Add(listener);
        foreach (GameObject invoker in secondStateStuckInokers)
        {
            invoker.GetComponent<SecondStuckField>().AddSecondStateStuckListener(listener);
        }
    }

    /// <summary>
    /// pairs all second stuck invokers to every listener
    /// </summary>
    /// <param name="invoker"></param>
    public static void AddSecondStateStuckInvokers(GameObject invoker)
    {
        // adds invoker to list and to all listeners
        secondStateStuckInokers.Add(invoker);
        foreach (UnityAction<int> listener in secondStateStuckListeners)
        {
            invoker.GetComponent<SecondStuckField>().AddSecondStateStuckListener(listener);
        }
    }
    #endregion

    #region specialMethods

    /// <summary>
    /// clears all of the lists in the manager to start fresh
    /// </summary>
    public static void ClearManager()
    {
        #region invokers
        // objects that invoke the button pressed method
        buttonPressedInvokers.Clear();

        // objects that invoke the button unpressed method
        buttonUnPressedInvokers.Clear();

        // objects that inform the state change of the second state
        secondStateChangeInvokers.Clear();

        // objects that change the health or imagination of the player
        healthChangedInvokers.Clear();

        // objects that only change imagination of the second state
        // NOTE: these are given the helath changed listeners but have their own invoker method
        imaginationChangedInvokers.Clear();

        // objects that announce a players death appear here
        // NOTE: imagination deaths come from these invokers (hopefuly just the health bar)
        playerDeathInvokers.Clear();

        // objects that signal second state stuck 
        secondStateStuckInokers.Clear();
        #endregion

        // 
        #region ListenerLists
        // lists of listeners
        /*format*/ // static List<UnityAction?<>?> [NAME] = new List<T>();

        // objects that Listen for the button pressed event
        buttonPressedListeners.Clear();

        // objects that listen for the button unpressed event
        buttonUnPressedListeners.Clear();

        // objects inquiring about the second state change
        secondStateChangeListeners.Clear();

        // objects listening for health or imagination change events
        healthChangedListeners.Clear();

        // objects listening for player death events
        playerDeathListeners.Clear();

        // objects that listen for the second stuck event
        secondStateStuckListeners.Clear();

        #endregion
    }

    void RemoveListener()
    {

    }
    #endregion
}
