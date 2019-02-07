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
    #endregion
}
