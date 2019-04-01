using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondGrabScript : MonoBehaviour
{
    // handles state changes
    bool amIMovable;

    // bool touch puzzle piece
    bool puzzlepiece;
    bool holding;
    GameObject currentpuzzlepiece;
    //bool letGo;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        // allows for movement
        amIMovable = true;

        // sets early
        puzzlepiece = false;
        //letGo = true;
        active = false;

        // allows for movement changing
        EventManager.AddSecondStateChangeListeners(ChangeMovabillity);
    }

    // Update is called once per frame
    void Update()
    {
        // makes sure it doesn't grab cubes if state loses movabillity
        if (amIMovable)
        {
            // checks to see if we have the puzzle piece or if we are colliding with it
            if (puzzlepiece && active)
            {
                // if the proper key is pressed
                if (Input.GetKeyDown(KeyCode.E) && Time.timeScale != 0)
                {
                    // dependinf if it was holding or not allready
                    // change bool and nullify piece if applicable
                    if (!holding)
                    {
                        holding = true;
                    }
                    else
                    {
                        holding = false;
                        currentpuzzlepiece = null;
                    }
                }
            }

            // moves pieces position to our desired position
            if (holding && currentpuzzlepiece != null)
            {
                currentpuzzlepiece.transform.position = ((transform.position + transform.forward + transform.right));
            }
        }
    }

    /// <summary>
    /// On the event call, this method checks to see if it should switch its view.
    /// </summary>
    /// <param name="enumeration"> the state change enumeration </param>
    void ChangeMovabillity(Enumeration.secondStateTransitions enumeration)
    {
        // checks if views have been switched
        if (enumeration == Enumeration.secondStateTransitions.switchView)
        {
            if (amIMovable)
            {
                amIMovable = false;
            }
            else
            {
                amIMovable = true;
            }
        }
    }

    /// <summary>
    /// checks if we are touching a proper cube for grabbing and handles the bools
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // for either makes sure we are activly touching and there is a puzzle piece
        // additionally gives us the piece for used later
        if (other.gameObject.tag == "Cube01")
        {
            if (!holding)
            {
                puzzlepiece = true;
                active = true;
                currentpuzzlepiece = other.gameObject;
            }
        }

        // for either makes sure we are activly touching and there is a puzzle piece
        // additionally gives us the piece for used later
        if (other.gameObject.tag == "SpecialCube")
        {
            if (!holding)
            {
                puzzlepiece = true;
                active = true;
                currentpuzzlepiece = other.gameObject;
            }
        }
    }

    /// <summary>
    /// checks if we are touching a proper cube for grabbing and handles the bools
    /// important that we are holding a cube first
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        // for either makes sure we are no longer touching and there is no puzzle piece
        // additionally removes taht piece from our variable
        if (other.gameObject.tag == "Cube01")
        {
            if (!holding)
            {
                puzzlepiece = true;
                currentpuzzlepiece = null;
                active = false;
            }
        }

        // for either makes sure we are no longer touching and there is no puzzle piece
        // additionally removes taht piece from our variable
        if (other.gameObject.tag == "SpecialCube")
        {
            if (!holding)
            {
                puzzlepiece = true;
                active = false;
                currentpuzzlepiece = null;
            }
        }
    }
}
