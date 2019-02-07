using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour
{
    // body for motion
    Rigidbody myBody;

    // force strength
    float movementSpeed = 50.0f;
    float rotationSPeed = 80.0f;

    // limiting speed vector
    Vector3 speedLimit = new Vector3(5, 0/*no limit*/ , 5); // negative is same as positive

    // values for machanics
    float forwardBackward;
    float sideToSide;
    Vector3 directionalAxis;
    // Vector3 rotationalAxis;

    // bool touch puzzle piece
    bool puzzlepiece;
    bool holding;
    GameObject currentpuzzlepiece;

    float puzzlePieceDistance = 4;

    // not holding the cube
    bool letGo;

    // for second state
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody>();

        // sets early
        puzzlepiece = false;
        letGo = true;

        // allows for movement
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        // updates axis data
        forwardBackward = Input.GetAxis("Vertical") * movementSpeed;
        sideToSide = ( Input.GetAxis("Horizontal") * rotationSPeed);
        directionalAxis = new Vector3(0, 0, forwardBackward);
        // rotationalAxis = new Vector3(0, sideToSide, 0);

        directionalAxis *= Time.deltaTime;
        sideToSide *= Time.deltaTime;

        // handles rotation
        this.transform.Rotate(0, sideToSide, 0);

        // adds force to move forward
        myBody.AddRelativeForce(directionalAxis, ForceMode.Impulse);

        // both statements ensure speed limit is maintained.
        if (myBody.velocity.x > speedLimit.x)
        {
            myBody.velocity = new Vector3(speedLimit.x, myBody.velocity.y, myBody.velocity.z);
        } 
        else if ((myBody.velocity.x < -speedLimit.x))
        {
            myBody.velocity = new Vector3(-speedLimit.x, myBody.velocity.y, myBody.velocity.z);
        }

        if (myBody.velocity.z > speedLimit.z)
        {
            myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, speedLimit.z);
        }
        else if (myBody.velocity.z < -speedLimit.z)
        {
            myBody.velocity = new Vector3(myBody.velocity.x, myBody.velocity.y, -speedLimit.z);
        }

        // invalid
        // this.transform.Translate(directionalAxis);

        if (puzzlepiece && active)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!holding)
                {
                    holding = true;
                    //Debug.Log("I am holding.");
                }
                else
                {
                    holding = false;
                    //Debug.Log("I let go.");
                    currentpuzzlepiece = null;
                }
            }
        }

        if (holding && currentpuzzlepiece != null)
        {
            currentpuzzlepiece.transform.position = ((transform.position + transform.forward + transform.right));
            //Debug.Log("I am following.");
        }

        // force based
        //// scales force up
        //directionalAxis *= movementForce;

        //myBody.AddForce(directionalAxis, ForceMode.Impulse);
        //myBody.AddTorque(rotationalAxis, ForceMode.Force);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube01")
        {
            //Debug.Log("we touched");
            puzzlepiece = true;
            currentpuzzlepiece = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Cube01")
        {
            //Debug.Log("we touched");
            if (!holding)
            {
                puzzlepiece = true;
                currentpuzzlepiece = other.gameObject;
            }
        }
    }
}
