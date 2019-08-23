using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerGrabScript : MonoBehaviour
{
    // handles state changes
    bool amIMovable;

    // bool touch puzzle piece
    bool puzzlepiece;
    bool holding;
    GameObject currentpuzzlepiece;
    //bool letGo;
    bool active;

    // player puzzle piece variables (the imaginary one)
    bool placedPiece;
    public GameObject puzzlePiece;
    GameObject instantiatedPiece;

    // for moving cubes
    //public bool useTargetY = true;
    //public float relativeX = 0;
    //public float relativeY = 0;
    //public float relativeZ = 0;
    public float specialSpeed = 1;
    static float followSpeed = 0.1f;
    Vector3 toVectorAngle;

    public float dropDistance = 5;

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
        EventManager.AddPlayerDeathListeners(CheckPlacedPiece);

        // allows for movement changing
        EventManager.AddSecondStateChangeListeners(ChangeMovabillity);

        // sets place peace
        placedPiece = false;

        toVectorAngle = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // makes sure it doesn't grab cubes if state loses movabillity
        if (amIMovable)
        {
            // if this key is pressed
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.timeScale != 0 && !placedPiece)
            {
                // places piece and changes bool
                instantiatedPiece = Instantiate(puzzlePiece, transform.position + puzzlePiece.transform.position, puzzlePiece.transform.rotation);
                placedPiece = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift) && Time.timeScale != 0 && placedPiece)
            {
                // removes piece and changes bool
                Destroy(instantiatedPiece);
                placedPiece = false;
            }

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
                        currentpuzzlepiece.GetComponent<Rigidbody>().useGravity = true;
                        currentpuzzlepiece = null;
                    }
                }
            }

            // moves pieces position to our desired position
            if (holding && currentpuzzlepiece != null)
            {
                //Debug.Log(Vector3.Distance(currentpuzzlepiece.transform.position, transform.position + transform.forward + transform.right + transform.forward));
                if (Vector3.Distance(currentpuzzlepiece.transform.position, transform.position + transform.forward + transform.right + transform.forward) > 1)
                {
                    //currentpuzzlepiece.transform.position = ((transform.position + transform.forward + transform.right));
                    Vector3 toPlayerVector = transform.position + transform.forward + transform.right + transform.forward;
                    toPlayerVector.y += 0.25f;
                    toVectorAngle = toPlayerVector - currentpuzzlepiece.transform.position;
                    //toVectorAngle = transform.position + transform.forward + transform.right;

                    toVectorAngle.Normalize();

                    toVectorAngle.y *= followSpeed * specialSpeed;
                    toVectorAngle.x *= followSpeed * specialSpeed;
                    toVectorAngle.z *= followSpeed * specialSpeed;

                    currentpuzzlepiece.GetComponent<Rigidbody>().AddForce(toVectorAngle, ForceMode.Impulse);
                }
                else
                {
                    currentpuzzlepiece.GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward + transform.right + transform.forward);
                    currentpuzzlepiece.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }

        if (currentpuzzlepiece != null && Vector3.Distance(currentpuzzlepiece.transform.position, transform.position) > dropDistance)
        {
            currentpuzzlepiece.GetComponent<Rigidbody>().useGravity = true;
            holding = false;
            currentpuzzlepiece = null;
            active = false;
        }
    }

    /// <summary>
    /// looks to see if a puzzle piece was placed, uses the death of second state for check
    /// </summary>
    /// <param name="state"></param>
    void CheckPlacedPiece(Enumeration.playerState state)
    {
        // if the second state died, then we want to clear both
        if (state == Enumeration.playerState.secondState && placedPiece)
        {
            placedPiece = false;
        }
    }

    /// <summary>
    /// On the event call, this method checks to see if it should switch its view.
    /// </summary>
    /// <param name="enumeration"> the state change enumeration </param>
    void ChangeMovabillity(Enumeration.secondStateTransitions enumeration)
    {
        if (enumeration == Enumeration.secondStateTransitions.removeState)
        {
            amIMovable = true;
        }
        else if (enumeration == Enumeration.secondStateTransitions.addState)
        {
            amIMovable = false;
        }
        else if (enumeration == Enumeration.secondStateTransitions.switchView)
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
                currentpuzzlepiece =null;
            }
        }
    }
}
